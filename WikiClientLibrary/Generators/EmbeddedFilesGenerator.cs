using System.Collections.Generic;
using WikiClientLibrary.Generators.Primitive;
using WikiClientLibrary.Infrastructures;
using WikiClientLibrary.Pages;
using WikiClientLibrary.Sites;

namespace WikiClientLibrary.Generators
{
    /// <summary>
    /// Generates pages from all media files embedded in the provided page.
    /// (<a href="https://www.mediawiki.org/wiki/API:Images">mw:API:Images</a>, MediaWiki 1.11+)
    /// </summary>
    /// <seealso cref="LinksGenerator"/>
    /// <seealso cref="FileUsageGenerator"/>
    /// <seealso cref="TranscludedInGenerator"/>
    /// <seealso cref="TransclusionsGenerator"/>
    public class EmbeddedFilesGenerator : WikiPagePropertyGenerator
    {
        /// <inheritdoc />
        public EmbeddedFilesGenerator(WikiSite site) : base(site)
        {
        }

        /// <inheritdoc />
        public EmbeddedFilesGenerator(WikiSite site, WikiPageStub pageStub) : base(site, pageStub)
        {
        }

        /// <summary>
        /// Only list these files. Useful for checking whether a certain page has a certain file.
        /// (MediaWiki 1.18+)
        /// </summary>
        /// <value>A sequence of file page titles, or <c>null</c> to list all the embedded files.</value>
        public IEnumerable<string>? MatchingTitles { get; set; }

        /// <summary>
        /// Gets/sets a value that indicates whether the list should be in
        /// the descending order. (MediaWiki 1.19+)
        /// </summary>
        public bool OrderDescending { get; set; }

        /// <inheritdoc />
        public override string PropertyName => "images";

        /// <inheritdoc />
        public override IEnumerable<KeyValuePair<string, object?>> EnumListParameters()
        {
            return new Dictionary<string, object?>
            {
                { "imlimit", PaginationSize },
                { "imimages", MatchingTitles == null ? null : MediaWikiHelper.JoinValues(MatchingTitles) },
                { "imdir", OrderDescending ? "descending" : "ascending" }
            };
        }
    }
}
