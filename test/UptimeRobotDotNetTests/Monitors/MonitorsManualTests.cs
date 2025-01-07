using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UptimeRobotDotnet;
using UptimeRobotDotnet.Models;

namespace UptimeRobotDotNetTests.Monitors
{
    public class MonitorsManualTests : BaseTest
    {
        private const string ApiKey = "YOUR-API-KEY-HERE";
        private const string TestHeaderKey = "X-Test-1";
        private const string TestHeaderValue = "uptimerobot.com";
        private const string TestUrl = "https://uptimerobot.com";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public MonitorsManualTests()
        {
            _logger = LoggerFactory.CreateLogger<MonitorsTests>();
        }

        [TestCase(ApiKey), Explicit]
        public async Task Monitors(string apiKey)
        {
            var client = UptimeRobotClientFactory.Create(apiKey);
            var parameters = new MonitorSearchParameters();
            var results = client.Monitors(parameters).GetAwaiter().GetResult()?.Monitors;
            var result = results?.FirstOrDefault();

            _logger.LogDebug($"{results?.Count} returned");
            _logger.LogDebug(JsonSerializer.Serialize(results, JsonOptions));

            Assert.That(results, Is.Not.Null);
            Assert.That(result, Is.Not.Null);
        }

        [TestCase(ApiKey), Explicit]
        public async Task Monitor(string apiKey)
        {
            var client = UptimeRobotClientFactory.Create(apiKey);
            var parameters = new MonitorSearchParameters();
            var results = client.Monitors(parameters).GetAwaiter().GetResult()?.Monitors;
            var random = new Random();
            var first = results?.Skip(random.Next(0, results.Count)).First();
            parameters.Monitors = first?.Id.ToString();
            var find = await client.Monitor(parameters);
            var result = find?.Monitors.FirstOrDefault();

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(find, Is.Not.Null);
                Assert.That(result?.Url, Is.Not.Null);
            });
        }

        [TestCase(ApiKey), Explicit]
        public async Task MonitorCreateUpdateDelete(string apiKey)
        {
            var parameters = new MonitorCreateParameters
            {
                Url = "https://www.radancy.com",
                Custom_Http_Headers = new Dictionary<string, string>()
            };
            parameters.Custom_Http_Headers.Add(TestHeaderKey, TestHeaderValue);
            var client = UptimeRobotClientFactory.Create(apiKey);

            // cleanup

            var results = client.Monitors().GetAwaiter().GetResult()?.Monitors;
            var exists = results?.FirstOrDefault(c => parameters.Url.Equals(c.Url));
            if (exists != null)
            {
                await client.MonitorDelete(null);
            }

            // create
            var result = await client.MonitorCreate(parameters);

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.Not.Null);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Custom_Http_Headers.Count, Is.EqualTo(1));
            });

            // update

            var updateParameters = new MonitorUpdateParameters
            {
                Interval = 300,
                Custom_Http_Headers = result.Custom_Http_Headers
            };
            updateParameters.Custom_Http_Headers.Add(TestHeaderKey, TestHeaderValue);
            var update = await client.MonitorUpdate(updateParameters);

            _logger.LogDebug(JsonSerializer.Serialize(update, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(update, Is.Not.Null);
                Assert.That(update.Interval, Is.EqualTo(300));
                Assert.That(result.Id, Is.EqualTo(update.Id));
                Assert.That(result.Custom_Http_Headers, Is.Not.Null);
                Assert.That(result.Custom_Http_Headers.Count, Is.EqualTo(2));
            });

            // delete

            var delete = await client.MonitorDelete(null);

            _logger.LogDebug(JsonSerializer.Serialize(delete, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(delete, Is.Not.Null);
                Assert.That(OkResponse.Equals(delete.Stat), Is.True);
            });
        }
    }
}
