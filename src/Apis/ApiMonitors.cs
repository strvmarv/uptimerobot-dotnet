using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UptimeRobotDotnet.Models;

// ReSharper disable CheckNamespace
// ReSharper disable AsyncApostle.AsyncMethodNamingHighlighting

namespace UptimeRobotDotnet
{
    public partial class UptimeRobotClient
    {
        public const string MonitorsCreatePath = "newMonitor";
        public const string MonitorsDeletePath = "deleteMonitor";
        public const string MonitorsGetPath = "getMonitors";
        public const string MonitorsUpdatePath = "editMonitor";

        public async Task<UtrResponse> Monitors(MonitorSearchParameters parameters = null)
        {
            if (parameters == null) parameters = new MonitorSearchParameters() { api_key = _apiKey };
            if (parameters.Limit > 50) throw new ArgumentException("Limit must be less than or equal to 50");
            if (string.IsNullOrWhiteSpace(parameters.api_key)) parameters.api_key = _apiKey;
            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters).ConfigureAwait(false);
            return result;
        }

        public async Task<UtrResponse> Monitor(MonitorSearchParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.api_key)) parameters.api_key = _apiKey;
            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters).ConfigureAwait(false);
            return result;
        }

        public async Task<UtrResponse> MonitorCreate(MonitorCreateParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.api_key)) parameters.api_key = _apiKey;
            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsCreatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters).ConfigureAwait(false);
            return result;
        }

        public async Task<UtrResponse> MonitorDelete(MonitorDeleteParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.api_key)) parameters.api_key = _apiKey;
            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsDeletePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters).ConfigureAwait(false);
            return result;
        }

        public async Task<UtrResponse> MonitorUpdate(MonitorUpdateParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.api_key)) parameters.api_key = _apiKey;
            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsUpdatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters).ConfigureAwait(false);
            return result;
        }
    }
}
