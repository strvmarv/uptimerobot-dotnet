using System;
using System.Net.Http;

namespace UptimeRobotDotnet
{
    public partial class UptimeRobotClient : UptimeRobotClientBase
    {
        private readonly string _apiKey;

        public UptimeRobotClient(HttpClient httpClient, string apiKey, string apiVersion) : base(httpClient, apiVersion)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }
    }
}
