using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace UptimeRobotDotnet
{
    /// <summary>
    /// Factory for creating UptimeRobot client instances.
    /// </summary>
    public class UptimeRobotClientFactory
    {
        /// <summary>
        /// User agent string used by the default HTTP client.
        /// </summary>
        public const string UserAgentValue = "uptimerobot-dotnet/2.0";

#if NET5_0_OR_GREATER
        private static readonly HttpClient DefaultHttpClient = new HttpClient(new SocketsHttpHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });
#else
        private static readonly HttpClient DefaultHttpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });
#endif

        static UptimeRobotClientFactory()
        {
            DefaultHttpClient.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip");
            DefaultHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultHttpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            DefaultHttpClient.DefaultRequestHeaders.Add("user-agent", UserAgentValue);
            DefaultHttpClient.BaseAddress = UptimeRobotClientBase.GetDefaultBaseApiUri();
        }

        /// <summary>
        /// Creates a new UptimeRobot client using the default HTTP client.
        /// </summary>
        /// <param name="apiKey">The UptimeRobot API key for authentication.</param>
        /// <param name="apiVersion">The API version to use (defaults to v2).</param>
        /// <param name="logger">Optional logger for diagnostics.</param>
        /// <returns>A new <see cref="UptimeRobotClient"/> instance.</returns>
        public static UptimeRobotClient Create(string apiKey, string apiVersion = UptimeRobotClientBase.DefaultApiVersion, ILogger? logger = null)
        {
            return new UptimeRobotClient(DefaultHttpClient, apiKey, apiVersion, logger);
        }

        /// <summary>
        /// Creates a new UptimeRobot client using a custom HTTP client.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="apiKey">The UptimeRobot API key for authentication.</param>
        /// <param name="apiVersion">The API version to use (defaults to v2).</param>
        /// <param name="logger">Optional logger for diagnostics.</param>
        /// <returns>A new <see cref="UptimeRobotClient"/> instance.</returns>
        public static UptimeRobotClient Create(HttpClient httpClient, string apiKey, string apiVersion = UptimeRobotClientBase.DefaultApiVersion, ILogger? logger = null)
        {
            return new UptimeRobotClient(httpClient, apiKey, apiVersion, logger);
        }
    }
}
