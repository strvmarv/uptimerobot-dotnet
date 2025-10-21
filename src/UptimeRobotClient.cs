using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace UptimeRobotDotnet
{
    /// <summary>
    /// Main client for interacting with the UptimeRobot API.
    /// </summary>
    public partial class UptimeRobotClient : UptimeRobotClientBase
    {
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="apiKey">The UptimeRobot API key for authentication.</param>
        /// <param name="apiVersion">The API version to use.</param>
        /// <param name="logger">Optional logger for diagnostics.</param>
        /// <exception cref="ArgumentNullException">Thrown when apiKey is null.</exception>
        public UptimeRobotClient(HttpClient httpClient, string apiKey, string apiVersion, ILogger? logger = null)
            : base(httpClient, apiVersion, logger)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }
    }
}
