﻿using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using WikiClientLibrary.Client;
using WikiClientLibrary.Infrastructures;
using WikiClientLibrary.Pages;
using WikiClientLibrary.Sites;
using WikiClientLibrary.Infrastructures.Logging;
using WikiClientLibrary.Pages.Queries;

namespace WikiClientLibrary;

/// <summary>
/// Provides static methods for API queries.
/// </summary>
internal static class RequestHelper
{

    public const int CONTINUATION_DONE = 0;
    public const int CONTINUATION_AVAILABLE = 1;
    public const int CONTINUATION_LOOP = 2;

    public static JsonObject? FindQueryContinuationParameterRoot(JsonNode jresult)
    {
        return (JsonObject?)(jresult["continue"]
                             ?? jresult["query-continue"]?.AsObject().FirstOrDefault().Value);
    }

    public static int ParseContinuationParameters(JsonNode jresult, IDictionary<string, object> queryParams,
        IDictionary<string, string>? continuationParams)
    {
        var continuation = FindQueryContinuationParameterRoot(jresult);
        // No more results.
        if (continuation == null || continuation.Count == 0)
            return CONTINUATION_DONE;

        var anyNewValue = false;
        continuationParams?.Clear();
        foreach (var p in continuation)
        {
            if (p.Value == null) continue;

            var parsed = p.Value.GetValueKind() switch
            {
                // Trivial: unwrap strings.
                JsonValueKind.String => p.Value.GetValue<string>(),
                // Retrieve JSON representation so we won't need to consider which int/float type to use.
                JsonValueKind.Number => p.Value.ToJsonString(),
                // Ignore nulls.
                JsonValueKind.Null or JsonValueKind.Undefined => null,
                // We cannot help -- this is the best we can do.
                _ => p.Value.ToJsonString(),
            };
            if (parsed == null) continue;

            if (!queryParams.TryGetValue(p.Key, out var existing) || !Equals(existing, parsed))
                anyNewValue = true;

            continuationParams?.Add(new(p.Key, parsed));
        }
        return anyNewValue ? CONTINUATION_AVAILABLE : CONTINUATION_LOOP;
    }

    public static JsonNode? FindQueryResponseItemsRoot(JsonNode jresult, string actionName)
    {
        if (actionName == "watchlistraw")
        {
            // "watchlistraw" list isn't nested inside an object with "query" key, it has the following structure
            /*
                {
                    "batchcomplete": true,
                    "continue": {
                        "wrcontinue": "0|3Р41",
                        "continue": "-||tokens"
                    },
                    "watchlistraw": [{"ns": 0,"title": "page title","changed": "2021-03-01T14:59:52Z"}]
                }
             */
            return (JsonArray?)jresult[actionName];
        }
        // If there's no result, "query" node will not exist.
        var queryNode = (JsonObject?)jresult["query"];
        if (queryNode != null && queryNode.Count > 0)
        {
            var listNode = queryNode[actionName];
            if (listNode == null)
            {
                if (queryNode.Count > 1)
                    throw new UnexpectedDataException(Prompts.ExceptionWikiListCannotFindResultRoot);
                listNode = queryNode.First().Value;
            }
            return listNode;
        }
        return null;
    }

    #region Page/Revision query

    public static async IAsyncEnumerable<JsonObject> QueryWithContinuation(WikiSite site,
        IEnumerable<KeyValuePair<string, object?>> parameters,
        Func<IDisposable>? beginActionScope,
        bool distinctPages = false,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var retrievedPageIds = distinctPages ? new HashSet<int>() : null;
        using var actionScopeDisposable = beginActionScope?.Invoke();
        // Defensive copy.
        var baseQueryParams = new Dictionary<string, object?>(parameters);
        Debug.Assert("query".Equals(baseQueryParams["action"]));
        var continuationParams = new Dictionary<string, string>();
        while (true)
        {
            var queryParams = new Dictionary<string, object?>(baseQueryParams);
            queryParams.MergeFrom(continuationParams);
            var jresult = await site.InvokeMediaWikiApiAsync(new MediaWikiFormRequestMessage(queryParams), cancellationToken);
            var jpages = (JsonObject?)FindQueryResponseItemsRoot(jresult, "pages");
            if (jpages != null)
            {
                if (retrievedPageIds != null)
                {
                    // Remove duplicate results
                    var duplicateKeys = new List<string>(jpages.Count);
                    foreach (var jpage in jpages)
                    {
                        if (!retrievedPageIds.Add(Convert.ToInt32(jpage.Key)))
                        {
                            // The page has been retrieved before.
                            duplicateKeys.Add(jpage.Key);
                        }
                    }
                    var originalPageCount = jpages.Count;
                    foreach (var k in duplicateKeys) jpages.Remove(k);
                    if (originalPageCount != jpages.Count)
                    {
                        site.Logger.LogWarning(
                            "Received {Count} results on {Site}, {DistinctCount} distinct results.",
                            originalPageCount, site, jpages.Count);
                    }
                }
                using (ExecutionContextStash.Capture())
                    yield return jpages;
            }
            switch (ParseContinuationParameters(jresult, queryParams, continuationParams))
            {
                case CONTINUATION_DONE:
                    yield break;
                case CONTINUATION_AVAILABLE:
                    if (jpages == null)
                        site.Logger.LogWarning("Empty query page with continuation received on {Site}.", site);
                    // Continue the loop and fetch for the next page of query.
                    break;
                case CONTINUATION_LOOP:
                    throw new UnexpectedContinuationLoopException();
            }
        }
    }

    private static (WikiSite Site, bool HasTitle) BuildWikiPageGroupKey(WikiPage page)
    {
        if (page == null) throw new ArgumentNullException(nameof(page));
        return (page.Site, page.PageStub.HasTitle);
    }

    /// <summary>
    /// Refresh a sequence of pages.
    /// </summary>
    public static async Task RefreshPagesAsync(IEnumerable<WikiPage> pages, IWikiPageQueryProvider options,
        CancellationToken cancellationToken)
    {
        if (pages == null) throw new ArgumentNullException(nameof(pages));
        // You can even fetch pages from different sites.
        foreach (var sitePages in pages.GroupBy(BuildWikiPageGroupKey))
        {
            var site = sitePages.Key.Site;
            var queryParams = options.EnumParameters(site.SiteInfo.Version).ToDictionary();
            var titleLimit = options.GetMaxPaginationSize(site.SiteInfo.Version, site.AccountInfo.HasRight(UserRights.ApiHighLimits));
            // Workaround https://github.com/CXuesong/WikiClientLibrary/issues/118
            // Currently, `prop=revisions` itself does not trigger continuation unless `rvlimit` has been specified. Good.
            // However, `prop=imageinfo` insists it return all the items eventually
            // meaning it will trigger continuation even if WCL consumer is only refreshing 1 page and
            // might only be interested in the latest n versions.
            var needPropImageInfoContinueHack = queryParams.GetValueOrDefault("prop")?.ToString()
                ?.Contains("imageinfo", StringComparison.Ordinal) ?? false;
            using (site.BeginActionScope(sitePages, options))
            {
                foreach (var partition in sitePages.Partition(titleLimit))
                {
                    if (sitePages.Key.HasTitle)
                    {
                        // If a page has both title and ID information,
                        // we will use title anyway.
                        site.Logger.LogDebug("Fetching {Count} pages by title.", partition.Count);
                        queryParams["titles"] = MediaWikiHelper.JoinValues(partition.Select(p => p.Title));
                    }
                    else
                    {
                        site.Logger.LogDebug("Fetching {Count} pages by ID.", partition.Count);
                        Debug.Assert(sitePages.All(p => p.PageStub.HasId));
                        queryParams["pageids"] = MediaWikiHelper.JoinValues(partition.Select(p => p.Id));
                    }
                    var jobj = await site.InvokeMediaWikiApiAsync(new MediaWikiFormRequestMessage(queryParams), cancellationToken);
                    var jquery = jobj["query"]?.AsObject();
                    if (jquery == null) throw new UnexpectedDataException("Missing $.query node.");
                    var continuationStatus = ParseContinuationParameters(jobj, queryParams, null);
                    // Process continuation caused by props (e.g. langlinks) that contain a list that is too long.
                    if (continuationStatus != CONTINUATION_DONE)
                    {
                        var queryParams1 = new Dictionary<string, object?>();
                        var continuationParams = new Dictionary<string, string>();
                        var jobj1 = jobj;
                        ParseContinuationParameters(jobj1, queryParams1, continuationParams);
                        while (continuationStatus != CONTINUATION_DONE)
                        {
                            if (needPropImageInfoContinueHack
                                && partition.Count == 1
                                && continuationParams.ContainsKey("iistart"))
                            {
                                var continueValue = continuationParams.GetValueOrDefault("continue");
                                if (continueValue != null && !continueValue.Contains("imageinfo", StringComparison.Ordinal))
                                {
                                    // Workaround https://github.com/CXuesong/WikiClientLibrary/issues/118
                                    // Fiddle with `continue` so it won't return old file revisions.
                                    continuationParams["continue"] = string.IsNullOrEmpty(continueValue)
                                        ? "||imageinfo"
                                        : continueValue + "|imageinfo";
                                }
                            }
                            if (continuationStatus == CONTINUATION_LOOP)
                                throw new UnexpectedContinuationLoopException();
                            Debug.Assert(continuationStatus == CONTINUATION_AVAILABLE);
                            site.Logger.LogDebug("Detected query continuation. PartitionCount={PartitionCount}.", partition.Count);
                            queryParams1.Clear();
                            queryParams1.MergeFrom(queryParams);
                            queryParams1.MergeFrom(continuationParams);
                            jobj1 = await site.InvokeMediaWikiApiAsync(new MediaWikiFormRequestMessage(queryParams1), cancellationToken);
                            var jquery1 = jobj1["query"]?.AsObject();
                            if (jquery1 != null)
                            {
                                // https://github.com/dotnet/runtime/issues/31433
                                // Merge JSON response
                                JsonHelper.InplaceMerge(jquery, jquery1);
                            }
                            continuationStatus = ParseContinuationParameters(jobj1, queryParams1, continuationParams);
                        }
                    }
                    if (sitePages.Key.HasTitle)
                    {
                        // Process title normalization.
                        var normalized = jquery["normalized"]?.AsArray().ToDictionary(n => (string)n["from"], n => (string)n["to"]);
                        // Process redirects.
                        var redirects = jquery["redirects"]?.AsArray().ToDictionary(n => (string)n["from"], n => (string)n["to"]);
                        var pageInfoDict = jquery["pages"]?.AsObject()
                            .ToDictionary(p => (string)p.Value["title"]);
                        foreach (var page in partition)
                        {
                            var title = page.Title;
                            Debug.Assert(title != null);
                            // Normalize the title first.
                            if (normalized?.ContainsKey(title) ?? false)
                                title = normalized[title];
                            // Then process the redirects.
                            var redirectTrace = new List<string>();
                            while (redirects?.ContainsKey(title) ?? false)
                            {
                                redirectTrace.Add(title); // Adds the last title
                                var next = redirects[title];
                                if (redirectTrace.Contains(next))
                                    throw new InvalidOperationException(string.Format(Prompts.ExceptionWikiPageResolveCircularRedirect1,
                                        string.Join("->", redirectTrace)));
                                title = next;
                            }
                            // Finally, get the page.
                            var pageInfo = pageInfoDict[title];
                            if (redirectTrace.Count > 0)
                                page.RedirectPath = redirectTrace;
                            MediaWikiHelper.PopulatePageFromJson(page, pageInfo.Value.AsObject(), options);
                        }
                    }
                    else
                    {
                        foreach (var page in partition)
                        {
                            var jPage = (JsonObject)jquery["pages"][page.Id.ToString(CultureInfo.InvariantCulture)];
                            MediaWikiHelper.PopulatePageFromJson(page, jPage, options);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Retrieve a sequence of revisions by revid, along with their owner pages.
    /// </summary>
    /// <remarks>
    /// <para>If there's invalid revision id in <paramref name="revIds"/>, an <see cref="ArgumentException"/> will be thrown while enumerating.</para>
    /// </remarks>
    public static async IAsyncEnumerable<Revision?> FetchRevisionsAsync(WikiSite site, IEnumerable<long> revIds,
        IWikiPageQueryProvider options, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (revIds == null) throw new ArgumentNullException(nameof(revIds));
        var queryParams = options.EnumParameters(site.SiteInfo.Version).ToDictionary();
        // Remove any rvlimit magic word generated by RevisionsPropertyProvider.
        // We are only fetching by revisions.
        queryParams.Remove("rvlimit");
        var titleLimit = options.GetMaxPaginationSize(site.SiteInfo.Version, site.AccountInfo.HasRight(UserRights.ApiHighLimits));
        // Page ID --> Page Stub
        var stubDict = new Dictionary<long, WikiPageStub>();
        var revDict = new Dictionary<long, Revision>();
        using (site.BeginActionScope(null, (object)revIds))
        {
            foreach (var partition in revIds.Partition(titleLimit))
            {
                site.Logger.LogDebug("Fetching {Count} revisions from {Site}.", partition.Count, site);
                queryParams["revids"] = MediaWikiHelper.JoinValues(partition);
                var jobj = await site.InvokeMediaWikiApiAsync(new MediaWikiFormRequestMessage(queryParams), cancellationToken);
                var jpages = jobj["query"]["pages"].AsObject();
                // Generate stubs first
                foreach (var p in jpages)
                {
                    var jrevs = p.Value["revisions"]?.AsArray();
                    if (jrevs == null || jrevs.Count == 0) continue;
                    var id = Convert.ToInt32(p.Key);
                    if (!stubDict.TryGetValue(id, out var stub))
                    {
                        stub = new WikiPageStub(id, (string)p.Value["title"], (int)p.Value["ns"]);
                        stubDict.Add(id, stub);
                    }
                    foreach (var jrev in jrevs)
                    {
                        var rev = jrev.Deserialize<Revision>(MediaWikiHelper.WikiJsonSerializerOptions);
                        rev.Page = stub;
                        revDict.Add(rev.Id, rev);
                    }
                }
                using (ExecutionContextStash.Capture())
                    foreach (var id in partition)
                        yield return revDict.GetValueOrDefault(id);
            }
        }
    }

    #endregion

    /// <summary>
    /// Asynchronously purges the pages.
    /// </summary>
    /// <returns>A collection of pages that haven't been successfully purged, because of either missing or invalid titles.</returns>
    public static async Task<IReadOnlyCollection<PurgeFailureInfo>> PurgePagesAsync(IEnumerable<WikiPage> pages, PagePurgeOptions options,
        CancellationToken cancellationToken)
    {
        if (pages == null) throw new ArgumentNullException(nameof(pages));
        List<PurgeFailureInfo>? failedPages = null;
        // You can even purge pages from different sites.
        foreach (var sitePages in pages.GroupBy(BuildWikiPageGroupKey))
        {
            var site = sitePages.Key.Site;
            var titleLimit = site.AccountInfo.HasRight(UserRights.ApiHighLimits)
                ? 500
                : 50;
            using (site.BeginActionScope(sitePages, options))
            {
                foreach (var partition in sitePages.Partition(titleLimit).Select(partition => partition.ToList()))
                {
                    string? titles;
                    string? ids;
                    if (sitePages.Key.HasTitle)
                    {
                        // If a page has both title and ID information,
                        // we will use title anyway.
                        site.Logger.LogDebug("Purging {Count} pages by title.", partition.Count);
                        titles = MediaWikiHelper.JoinValues(partition.Select(p => p.Title));
                        ids = null;
                    }
                    else
                    {
                        site.Logger.LogDebug("Purging {Count} pages by ID.", partition.Count);
                        Debug.Assert(sitePages.All(p => p.PageStub.HasId));
                        titles = null;
                        ids = MediaWikiHelper.JoinValues(partition.Select(p => p.Id));
                    }
                    try
                    {
                        var jresult = await site.InvokeMediaWikiApiAsync(new MediaWikiFormRequestMessage(new
                        {
                            action = "purge",
                            titles = titles,
                            pageids = ids,
                            forcelinkupdate = (options & PagePurgeOptions.ForceLinkUpdate) == PagePurgeOptions.ForceLinkUpdate,
                            forcerecursivelinkupdate = (options & PagePurgeOptions.ForceRecursiveLinkUpdate) ==
                                                       PagePurgeOptions.ForceRecursiveLinkUpdate,
                        }), cancellationToken);
                        // Now check whether the pages have been purged successfully.
                        foreach (JsonObject jitem in jresult["purge"].AsArray())
                        {
                            if (jitem["missing"] != null || jitem["invalid"] != null)
                            {
                                if (failedPages == null) failedPages = new List<PurgeFailureInfo>();
                                failedPages.Add(new PurgeFailureInfo(MediaWikiHelper.PageStubFromJson(jitem),
                                    (string?)jitem["invalidreason"]));
                            }
                        }
                    }
                    catch (OperationFailedException ex)
                    {
                        if (ex.ErrorCode == "cantpurge") throw new UnauthorizedOperationException(ex);
                        throw;
                    }
                }
            }
        }
        return failedPages ?? (IReadOnlyCollection<PurgeFailureInfo>)Array.Empty<PurgeFailureInfo>();
    }

    public static async Task PatrolAsync(WikiSite site, long? recentChangeId, long? revisionId, CancellationToken cancellationToken)
    {
        if (site == null) throw new ArgumentNullException(nameof(site));
        if (recentChangeId == null && revisionId == null)
            throw new ArgumentNullException(nameof(recentChangeId), "Either recentChangeId or revisionId should be set.");
        //if (recentChangeId != null && revisionId != null)
        //    throw new ArgumentException("Either recentChangeId or revisionId should be set, not both.");
        if (revisionId != null && site.SiteInfo.Version < new MediaWikiVersion(1, 22))
            throw new InvalidOperationException(Prompts.ExceptionPatrolledByRevisionNotSupported);
        try
        {
            var jresult = await site.InvokeMediaWikiApiAsync(new MediaWikiFormRequestMessage(new
            {
                action = "patrol",
                rcid = recentChangeId,
                revid = revisionId,
                token = WikiSiteToken.Patrol,
            }), cancellationToken);
            if (recentChangeId != null) Debug.Assert((int)jresult["patrol"]["rcid"] == recentChangeId.Value);
        }
        catch (OperationFailedException ex)
        {
            switch (ex.ErrorCode)
            {
                case "nosuchrcid":
                    throw new ArgumentException(string.Format(Prompts.ExceptionPatrolNoSuchRcid1, recentChangeId), ex);
                case "patroldisabled":
                    throw new NotSupportedException(Prompts.ExceptionPatrolDisabled, ex);
                case "noautopatrol":
                    throw new UnauthorizedOperationException(Prompts.ExceptionPatrolNoAutoPatrol, ex);
            }
            throw;
        }
    }

    /// <summary>
    /// Queries parameter information for one module.
    /// </summary>
    /// <param name="site"></param>
    /// <param name="moduleName">Name of the module.</param>
    /// <returns>The paraminfo.modules[0] item.</returns>
    public static async Task<JsonObject> QueryParameterInformationAsync(WikiSite site, string moduleName)
    {
        if (site == null) throw new ArgumentNullException(nameof(site));
        var pa = new Dictionary<string, object> { { "action", "paraminfo" } };
        if (site.SiteInfo.Version < new MediaWikiVersion(1, 25))
        {
            var parts = moduleName.Split('+');
            switch (parts[0])
            {
                case "main":
                    pa["mainmodule"] = true;
                    break;
                case "query":
                    if (parts.Length == 1)
                        pa["pagesetmodule"] = true;
                    else
                        pa["querymodules"] = parts[1];
                    break;
                case "format":
                    pa["formatmodules"] = true;
                    break;
                default:
                    pa["modules"] = moduleName;
                    break;
            }
        }
        else
        {
            pa["modules"] = moduleName;
        }
        var jresult = await site.InvokeMediaWikiApiAsync(new MediaWikiFormRequestMessage(pa), CancellationToken.None);
        var jmodules = jresult["paraminfo"]?.AsObject().FirstOrDefault(p => p.Key.EndsWith("modules")).Value;
        // For now we use the method internally.
        Debug.Assert(jmodules != null);
        return jmodules.AsArray().First().AsObject();
    }

    /// <summary>
    /// Enumerate links from the page.
    /// </summary>
    public static IAsyncEnumerable<string> EnumLinksAsync(WikiSite site, string titlesExpr, /* optional */ IEnumerable<int> namespaces)
    {
        var pa = new Dictionary<string, object?>
        {
            { "action", "query" },
            { "prop", "links" },
            { "pllimit", site.ListingPagingSize },
            { "plnamespace", namespaces == null ? null : MediaWikiHelper.JoinValues(namespaces) },
        };
        pa["titles"] = titlesExpr;
        var resultCounter = 0;
        return QueryWithContinuation(site, pa, null)
            .SelectMany(jpages =>
            {
                var page = jpages.First().Value;
                var links = (JsonArray?)page?["links"];
                if (links != null)
                {
                    resultCounter += links.Count;
                    site.Logger.LogDebug("Loaded {Count} items linking to [[{Title}]] on {Site}.", resultCounter, titlesExpr, site);
                    return links.Select(l => (string)l["title"]).ToAsyncEnumerable();
                }
                return AsyncEnumerable.Empty<string>();
            });
    }

    /// <summary>
    /// Enumerate transcluded pages trans from the page.
    /// </summary>
    public static IAsyncEnumerable<string> EnumTransclusionsAsync(WikiSite site, string titlesExpr, IEnumerable<int>? namespaces = null,
        IEnumerable<string>? transcludedTitlesExpr = null, int limit = -1)
    {
        // transcludedTitlesExpr should be full titles with ns prefix.
        var pa = new Dictionary<string, object?>
        {
            { "action", "query" },
            { "prop", "templates" },
            { "tllimit", limit > 0 ? limit : site.ListingPagingSize },
            { "tlnamespace", namespaces == null ? null : MediaWikiHelper.JoinValues(namespaces) },
            { "tltemplates", transcludedTitlesExpr == null ? null : MediaWikiHelper.JoinValues(transcludedTitlesExpr) },
        };
        pa["titles"] = titlesExpr;
        var resultCounter = 0;
        return QueryWithContinuation(site, pa, null)
            .SelectMany(jpages =>
            {
                var page = jpages.First().Value;
                var links = (JsonArray?)page?["templates"];
                if (links != null)
                {
                    resultCounter += links.Count;
                    site.Logger.LogDebug("Loaded {Count} items transcluded by [[{Title}]] on {Site}.",
                        resultCounter, titlesExpr, site);
                    return links.Select(l => (string)l["title"]).ToAsyncEnumerable();
                }
                return AsyncEnumerable.Empty<string>();
            });
    }

}
