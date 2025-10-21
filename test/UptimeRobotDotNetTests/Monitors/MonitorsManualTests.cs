using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UptimeRobotDotnet;
using UptimeRobotDotnet.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UptimeRobotDotNetTests.Monitors
{
    /// <summary>
    /// Manual tests for Monitor API endpoints. These tests require a valid API key.
    ///
    /// ⚠️ CONFIGURATION REQUIRED:
    /// These tests are marked [Explicit] and will NOT run automatically.
    ///
    /// To run these tests, set environment variable:
    ///   UPTIMEROBOT_API_KEY=your-api-key-here
    ///
    /// Optional: Set a custom test URL:
    ///   UPTIMEROBOT_TEST_URL=https://your-test-site.com
    ///
    /// See SECURITY.md for more information about safe API key handling.
    /// </summary>
    public class MonitorsManualTests : BaseTest
    {
        private const string ApiKeyPlaceholder = "YOUR-API-KEY-HERE";
        private const string TestHeaderKey = "X-Test-";
        private const string TestHeaderValue = "TestHeaderValue";
        private const string DefaultTestUrl = "https://github.com/strvmarv";

        private static string GetApiKey() =>
            Environment.GetEnvironmentVariable("UPTIMEROBOT_API_KEY") ?? ApiKeyPlaceholder;

        private static string GetTestUrl() =>
            Environment.GetEnvironmentVariable("UPTIMEROBOT_TEST_URL") ?? DefaultTestUrl;

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public MonitorsManualTests()
        {
            _logger = LoggerFactory.CreateLogger<MonitorsTests>();
        }

        [Test, Explicit]
        public void GetMonitors()
        {
            var apiKey = GetApiKey();
            if (apiKey == ApiKeyPlaceholder)
            {
                Assert.Inconclusive("API key not configured. Set UPTIMEROBOT_API_KEY environment variable.");
                return;
            }

            var client = UptimeRobotClientFactory.Create(apiKey);
            var parameters = new MonitorSearchParameters();
            var results = client.GetMonitorsAsync(parameters).GetAwaiter().GetResult()?.Monitors;
            var result = results?.FirstOrDefault();

            _logger.LogDebug($"{results?.Count} returned");
            _logger.LogDebug(JsonSerializer.Serialize(results, JsonOptions));

            Assert.That(results, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
        }

        [Test, Explicit]
        public async Task GetMonitor()
        {
            var apiKey = GetApiKey();
            if (apiKey == ApiKeyPlaceholder)
            {
                Assert.Inconclusive("API key not configured. Set UPTIMEROBOT_API_KEY environment variable.");
                return;
            }

            var client = UptimeRobotClientFactory.Create(apiKey);
            var parameters = new MonitorSearchParameters();
            var results = client.GetMonitorsAsync(parameters).GetAwaiter().GetResult()?.Monitors;
            var first = results?.First();

            if (first == null)
            {
                Assert.Inconclusive("No monitors available to test");
                return;
            }

            // You can't ask for additional data attributes without filtering on monitors
            parameters.Monitors = first.Id.ToString();
            parameters.AlertContacts = 1;
            parameters.CustomHttpHeaders = 1;
            parameters.CustomHttpStatuses = 1;
            parameters.HttpRequestDetails = true;
            parameters.Mwindows = 1;

            var find = await client.GetMonitorAsync(parameters);
            var result = find?.Monitors?.FirstOrDefault();

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(find, Is.Not.Null);
                Assert.That(result?.Url, Is.Not.Null);
            });
        }

        [Test, Explicit]
        public async Task MonitorCreateUpdateDelete()
        {
            var apiKey = GetApiKey();
            if (apiKey == ApiKeyPlaceholder)
            {
                Assert.Inconclusive("API key not configured. Set UPTIMEROBOT_API_KEY environment variable.");
                return;
            }

            var testUrl = GetTestUrl();
            var searchParameters = new MonitorSearchParameters()
            {
                CustomHttpHeaders = 1,
                HttpRequestDetails = true,
                Search = testUrl
            };

            var client = UptimeRobotClientFactory.Create(apiKey);

            // cleanup
            var results = client.GetMonitorAsync(searchParameters).GetAwaiter().GetResult()?.Monitors;
            var exists = results?.SingleOrDefault();
            var deleteParameters = new MonitorDeleteParameters();
            if (exists != null)
            {
                _logger.LogDebug(JsonSerializer.Serialize(exists, JsonOptions));

                deleteParameters.Id = exists.Id;
                await client.DeleteMonitorAsync(deleteParameters);
            }

            // create
            var createParameters = new MonitorCreateParameters
            {
                CustomHttpHeaders = new Dictionary<string, string>() { { TestHeaderKey + "1", testUrl } },
                FriendlyName = testUrl.Replace("https://", ""),
                Interval = 900,
                Type = MonitorType.HTTP,
                Url = testUrl,
            };

            var result = await client.CreateMonitorAsync(createParameters);

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            results = client.GetMonitorAsync(searchParameters).GetAwaiter().GetResult()?.Monitors;
            exists = results?.SingleOrDefault();

            _logger.LogDebug(JsonSerializer.Serialize(exists, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(result?.Monitor, Is.Not.Null);
                Assert.That(result.Monitor!.Id, Is.Not.Default);
                Assert.That(exists?.Interval, Is.EqualTo(900));
                Assert.That(exists?.Type, Is.EqualTo(MonitorType.HTTP));
                Assert.That(exists?.CustomHttpHeaders?.Count, Is.EqualTo(1));
            });

            // update
            var updateParameters = new MonitorUpdateParameters
            {
                Id = result.Monitor!.Id,
                Interval = 300,
                CustomHttpHeaders = exists?.CustomHttpHeaders
            };
            updateParameters.CustomHttpHeaders?.Add(TestHeaderKey + "2", TestHeaderValue);
            var update = await client.UpdateMonitorAsync(updateParameters);

            _logger.LogDebug(JsonSerializer.Serialize(update, JsonOptions));

            results = client.GetMonitorAsync(searchParameters).GetAwaiter().GetResult()?.Monitors;
            exists = results?.SingleOrDefault();

            _logger.LogDebug(JsonSerializer.Serialize(exists, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(update?.Monitor, Is.Not.Null);
                Assert.That(exists, Is.Not.Null);
                Assert.That(exists?.Interval, Is.EqualTo(300));
                Assert.That(result.Monitor.Id, Is.EqualTo(update.Monitor!.Id));
                Assert.That(exists?.CustomHttpHeaders, Is.Not.Null);
                Assert.That(exists?.CustomHttpHeaders?.Count, Is.EqualTo(2));
            });

            // delete
            deleteParameters.Id = update.Monitor!.Id;
            var delete = await client.DeleteMonitorAsync(deleteParameters);

            _logger.LogDebug(JsonSerializer.Serialize(delete, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(delete, Is.Not.Null);
                Assert.That(UptimeRobotClientBase.OkResponse.Equals(delete.Stat), Is.True);
            });
        }
    }
}
