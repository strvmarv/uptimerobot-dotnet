using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UptimeRobotDotnet;
using UptimeRobotDotnet.Exceptions;
using UptimeRobotDotnet.Models;
using WireMock.RequestBuilders;
using Response = WireMock.ResponseBuilders.Response;

namespace UptimeRobotDotNetTests.Monitors
{
    /// <summary>
    /// Tests for Monitor API endpoints.
    /// </summary>
    public class MonitorsTests : BaseHttpClientTest
    {
        private const string UserAgentHeaderKey = "User-Agent";
        private const string UserAgentHeaderValue = "curl";
        private const string TestApiKey = "a-p-i-k-e-y";
        private const int TestId = 123456789;
        private const string TestUrl = "https://i-am-a-test.com";

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public MonitorsTests()
        {
            _logger = LoggerFactory.CreateLogger<MonitorsTests>();
        }

        [Test]
        public void GetMonitors()
        {
            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        monitors = new List<object> { new { url = TestUrl } }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var results = client.GetMonitorsAsync().GetAwaiter().GetResult()?.Monitors;

            Assert.That(results?.Count, Is.EqualTo(1));

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task GetMonitor()
        {
            var mockResult = new
            {
                stat = "ok",
                monitors = new List<object>
                {
                    new { id = TestId, url = TestUrl }
                }
            };
            var mockParameters = new MonitorSearchParameters { Monitors = TestId.ToString() };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.GetMonitorAsync(mockParameters);

            Assert.That(result, Is.Not.Null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void GetMonitorNotFound()
        {
            var mockParameters = new MonitorSearchParameters { Monitors = TestId.ToString() };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotException>(() => client.GetMonitorAsync(mockParameters));
        }

        [Test]
        public async Task CreateMonitor()
        {
            var mockParameters = new MonitorCreateParameters
            {
                FriendlyName = "Test Monitor",
                Url = TestUrl,
                Type = MonitorType.HTTP,
                CustomHttpHeaders = new Dictionary<string, string>() { { UserAgentHeaderKey, UserAgentHeaderValue } }
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsCreatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        monitor = new { id = TestId }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.CreateMonitorAsync(mockParameters);

            Assert.That(result, Is.Not.Null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CreateMonitorNotFound()
        {
            var mockParameters = new MonitorCreateParameters
            {
                FriendlyName = "Test Monitor",
                Url = TestUrl,
                Type = MonitorType.HTTP
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsCreatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            Assert.ThrowsAsync<UptimeRobotException>(() => client.CreateMonitorAsync(mockParameters));
        }

        [Test]
        public async Task DeleteMonitor()
        {
            var mockParameters = new MonitorDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { stat = UptimeRobotClientBase.OkResponse }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.DeleteMonitorAsync(mockParameters);

            Assert.That(UptimeRobotClientBase.OkResponse.Equals(result.Stat), Is.True);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void DeleteMonitorNotFound()
        {
            var mockParameters = new MonitorDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            Assert.ThrowsAsync<UptimeRobotException>(() => client.DeleteMonitorAsync(mockParameters));
        }

        [Test]
        public async Task UpdateMonitor()
        {
            var mockParameters = new MonitorUpdateParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsUpdatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { stat = UptimeRobotClientBase.OkResponse }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.UpdateMonitorAsync(mockParameters);

            Assert.That(result, Is.Not.Null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void UpdateMonitorNotFound()
        {
            var mockParameters = new MonitorUpdateParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsUpdatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotException>(() => client.UpdateMonitorAsync(mockParameters));
        }

        [Test]
        public async Task GetAllMonitorsWithPagination()
        {
            // Mock first page
            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        pagination = new { offset = 0, limit = 2, total = 3 },
                        monitors = new List<object>
                        {
                            new { id = 1, url = "https://test1.com" },
                            new { id = 2, url = "https://test2.com" }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var monitors = new List<Monitor>();

            await foreach (var monitor in client.GetAllMonitorsAsync())
            {
                monitors.Add(monitor);
                // Break after first page for this test
                if (monitors.Count >= 2)
                    break;
            }

            Assert.That(monitors.Count, Is.GreaterThanOrEqualTo(2));

            _logger.LogDebug("Retrieved {Count} monitors with pagination", monitors.Count);
        }
    }
}
