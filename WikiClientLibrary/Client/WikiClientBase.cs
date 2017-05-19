using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WikiClientLibrary.Client
{
    /// <summary>
    /// Provides basic operations for MediaWiki API.
    /// </summary>
    public abstract class WikiClientBase : IDisposable
    {
        private TimeSpan _ThrottleTime = TimeSpan.FromSeconds(5);
        private int _MaxRetries = 3;

        public ILogger Logger { get; set; }

        /// <summary>
        /// Time to wait before any modification operations.
        /// </summary>
        /// <remarks>Note that the delay is simply inserted before every modification operations, without queuing.
        /// This won't work as you expect when you attempt to perform multi-threaded operations.</remarks>
        public TimeSpan ThrottleTime
        {
            get { return _ThrottleTime; }
            set
            {
                if (value < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(value));
                _ThrottleTime = value;
            }
        }

        /// <summary>
        /// Timeout for each query.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Delay before each retry.
        /// </summary>
        public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Max retries count.
        /// </summary>
        public int MaxRetries
        {
            get { return _MaxRetries; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                _MaxRetries = value;
            }
        }

        /// <summary>
        /// Invokes API and gets JSON result.
        /// </summary>
        /// <param name="endPointUrl">The API endpoint URL.</param>
        /// <param name="queryParams">The parameters of the query.</param>
        /// <param name="cancellationToken">The cancellation token that will be checked prior to completing the returned task.</param>
        /// <exception cref="InvalidActionException">Specified action is not supported.</exception>
        /// <exception cref="UnauthorizedOperationException">Permission denied.</exception>
        /// <exception cref="OperationFailedException">There's "error" node in returned JSON.</exception>
        public abstract Task<JToken> GetJsonAsync(string endPointUrl,
            IEnumerable<KeyValuePair<string, string>> queryParams,
            CancellationToken cancellationToken);

        /// <summary>
        /// Invokes API and gets JSON result. This method will not attmpt to retry
        /// </summary>
        /// <param name="endPointUrl">The API endpoint URL.</param>
        /// <param name="postContent">The content of the query.</param>
        /// <param name="cancellationToken">The cancellation token that will be checked prior to completing the returned task.</param>
        /// <exception cref="InvalidActionException">Specified action is not supported.</exception>
        /// <exception cref="UnauthorizedOperationException">Permission denied.</exception>
        /// <exception cref="OperationFailedException">There's "error" node in returned JSON.</exception>
        /// <remarks><para>"Get" means the returned value is JSON, though the request is sent via HTTP POST.</para>
        /// <para>The implementation should not retry, because <paramref name="postContent"/> is probably disposed after one request.</para>
        /// </remarks>
        public abstract Task<JToken> GetJsonAsync(string endPointUrl, HttpContent postContent,
            CancellationToken cancellationToken);

        /// <summary>
        /// Invokes API and gets JSON result.
        /// </summary>
        /// <param name="endPointUrl">The API endpoint URL.</param>
        /// <param name="queryParams">The parameters of the query, which will be converted into key-value pairs.</param>
        /// <param name="cancellationToken">The cancellation token that will be checked prior to completing the returned task.</param>
        /// <exception cref="InvalidActionException">Specified action is not supported.</exception>
        /// <exception cref="OperationFailedException">There's "error" node in returned JSON.</exception>
        public virtual Task<JToken> GetJsonAsync(string endPointUrl, object queryParams, CancellationToken cancellationToken)
        {
            return GetJsonAsync(endPointUrl, Utility.ToWikiStringValuePairs(queryParams), cancellationToken);
        }

        /// <summary>
        /// Returns a task which finishes after the time specified in <see cref="ThrottleTime"/> .
        /// </summary>
        public virtual Task WaitForThrottleAsync(CancellationToken cancellationToken)
        {
            return Task.Delay(ThrottleTime, cancellationToken);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            // release unmanaged resources here
            if (disposing)
            {
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        ~WikiClientBase()
        {
            Dispose(false);
        }
    }
}