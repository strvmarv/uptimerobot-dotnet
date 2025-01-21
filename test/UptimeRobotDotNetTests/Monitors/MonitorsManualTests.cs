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
    public class MonitorsManualTests : BaseTest
    {
        private const string ApiKey = "YOUR-API-KEY-HERE";
        private const string TestHeaderKey = "X-Test-";
        private const string TestHeaderValue = "TestHeaderValue";
        private const string TestUrl = "https://github.com/strvmarv";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public MonitorsManualTests()
        {
            _logger = LoggerFactory.CreateLogger<MonitorsTests>();
        }

        [TestCase(ApiKey), Explicit]
        public void Monitors(string apiKey)
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
            var first = results?.First();

            // You can't ask for additional data attributes without filtering on monitors
            parameters.Monitors = first?.Id.ToString();
            //parameters.Include_AlertContacts = 1;
            parameters.Custom_Http_Headers = 1;
            //parameters.Include_Custom_Http_Statuses = 1;
            parameters.Http_Request_Details = true;
            //parameters.Include_Maintenance_Windows = 1;

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
            var searchParameters = new MonitorSearchParameters()
            {
                Custom_Http_Headers = 1, 
                Http_Request_Details = true,
                Search = TestUrl
            };

            var client = UptimeRobotClientFactory.Create(apiKey);

            // cleanup
            var results = client.Monitor(searchParameters).GetAwaiter().GetResult()?.Monitors;
            var exists = results?.SingleOrDefault();
            var deleteParameters = new MonitorDeleteParameters();
            if (exists != null)
            {
                _logger.LogDebug(JsonSerializer.Serialize(exists, JsonOptions));

                deleteParameters.Id = exists.Id;
                await client.MonitorDelete(deleteParameters);
            }

            // create
            var createParameters = new MonitorCreateParameters
            {
                Custom_Http_Headers = new Dictionary<string, string>() { { TestHeaderKey + "1", TestUrl } },
                Friendly_Name = TestUrl.Replace("https://", ""),
                Interval = 900,
                Type = (int)MonitorType.HTTP,
                Url = TestUrl,
            };

            var result = await client.MonitorCreate(createParameters);

            _logger.LogDebug(JsonSerializer.Serialize(result, JsonOptions));

            results = client.Monitor(searchParameters).GetAwaiter().GetResult()?.Monitors;
            exists = results?.SingleOrDefault();

            _logger.LogDebug(JsonSerializer.Serialize(exists, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(result?.Monitor, Is.Not.Null);
                Assert.That(result.Monitor.Id, Is.Not.Default);
                Assert.That(exists?.Interval, Is.EqualTo(900));
                Assert.That(exists?.Type, Is.EqualTo((int)MonitorType.HTTP));
                Assert.That(exists?.Custom_Http_Headers?.Count, Is.EqualTo(1));
            });

            // update
            var updateParameters = new MonitorUpdateParameters
            {
                Id = result.Monitor.Id,
                Interval = 300,

                Custom_Http_Headers = exists?.Custom_Http_Headers
            };
            updateParameters.Custom_Http_Headers?.Add(TestHeaderKey + "2", TestHeaderValue);
            var update = await client.MonitorUpdate(updateParameters);

            _logger.LogDebug(JsonSerializer.Serialize(update, JsonOptions));

            results = client.Monitor(searchParameters).GetAwaiter().GetResult()?.Monitors;
            exists = results?.SingleOrDefault();

            _logger.LogDebug(JsonSerializer.Serialize(exists, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(update?.Monitor, Is.Not.Null);
                Assert.That(exists, Is.Not.Null);
                Assert.That(exists?.Interval, Is.EqualTo(300));
                Assert.That(result.Monitor.Id, Is.EqualTo(update.Monitor.Id));
                Assert.That(exists?.Custom_Http_Headers, Is.Not.Null);
                Assert.That(exists?.Custom_Http_Headers?.Count, Is.EqualTo(2));
            });

            // delete
            deleteParameters.Id = update.Monitor.Id;
            var delete = await client.MonitorDelete(deleteParameters);

            _logger.LogDebug(JsonSerializer.Serialize(delete, JsonOptions));

            Assert.Multiple(() =>
            {
                Assert.That(delete, Is.Not.Null);
                Assert.That(UptimeRobotClientBase.OkResponse.Equals(delete.Stat), Is.True);
            });
        }
    }
}
