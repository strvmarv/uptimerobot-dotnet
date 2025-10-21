using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UptimeRobotDotnet;
using UptimeRobotDotnet.Exceptions;
using UptimeRobotDotnet.Models;
using WireMock.RequestBuilders;
using Response = WireMock.ResponseBuilders.Response;

namespace UptimeRobotDotNetTests.StatusPages
{
    /// <summary>
    /// Tests for Status Page API endpoints.
    /// </summary>
    public class StatusPagesTests : BaseHttpClientTest
    {
        private const string TestApiKey = "a-p-i-k-e-y";
        private const int TestId = 123456;
        private const string TestMonitorIds = "111111-222222";

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public StatusPagesTests()
        {
            _logger = LoggerFactory.CreateLogger<StatusPagesTests>();
        }

        [Test]
        public async Task GetStatusPages()
        {
            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.StatusPagesGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        psps = new List<object>
                        {
                            new
                            {
                                id = TestId,
                                friendly_name = "Public Status",
                                monitors = TestMonitorIds,
                                standard_url = "https://status.example.com"
                            }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var results = await client.GetStatusPagesAsync();

            Assert.That(results?.StatusPages?.Count, Is.EqualTo(1));
            Assert.That(results.StatusPages[0].Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task GetStatusPage()
        {
            var mockParameters = new StatusPageSearchParameters { Psps = TestId.ToString() };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.StatusPagesGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        psps = new List<object>
                        {
                            new
                            {
                                id = TestId,
                                friendly_name = "Test Page",
                                monitors = TestMonitorIds
                            }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.GetStatusPageAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusPages?.Count, Is.GreaterThan(0));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void GetStatusPageNotFound()
        {
            var mockParameters = new StatusPageSearchParameters { Psps = TestId.ToString() };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.StatusPagesGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotException>(() => client.GetStatusPageAsync(mockParameters));
        }

        [Test]
        public async Task CreateStatusPage()
        {
            var mockParameters = new StatusPageCreateParameters
            {
                FriendlyName = "Public Status Page",
                Monitors = TestMonitorIds,
                CustomMessage = "All systems operational",
                Sort = StatusPageSort.FriendlyNameAZ
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.StatusPagesCreatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        psp = new { id = TestId }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.CreateStatusPageAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusPage?.Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CreateStatusPageWithInvalidParameters()
        {
            var mockParameters = new StatusPageCreateParameters
            {
                FriendlyName = "Test Page"
                // Missing required Monitors
            };

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotValidationException>(() => client.CreateStatusPageAsync(mockParameters));
        }

        [Test]
        public async Task UpdateStatusPage()
        {
            var mockParameters = new StatusPageUpdateParameters
            {
                Id = TestId,
                FriendlyName = "Updated Status Page",
                Status = StatusPageStatus.Active
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.StatusPagesUpdatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        psp = new { id = TestId }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.UpdateStatusPageAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusPage?.Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public async Task DeleteStatusPage()
        {
            var mockParameters = new StatusPageDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.StatusPagesDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { stat = UptimeRobotClientBase.OkResponse }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.DeleteStatusPageAsync(mockParameters);

            Assert.That(UptimeRobotClientBase.OkResponse.Equals(result.Stat), Is.True);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void DeleteStatusPageNotFound()
        {
            var mockParameters = new StatusPageDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.StatusPagesDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotException>(() => client.DeleteStatusPageAsync(mockParameters));
        }

        [Test]
        public async Task GetAllStatusPagesWithPagination()
        {
            // Mock first page
            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.StatusPagesGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        pagination = new { offset = 0, limit = 2, total = 3 },
                        psps = new List<object>
                        {
                            new { id = 1, friendly_name = "Page 1", monitors = "111" },
                            new { id = 2, friendly_name = "Page 2", monitors = "222" }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var pages = new List<StatusPage>();

            await foreach (var page in client.GetAllStatusPagesAsync())
            {
                pages.Add(page);
                // Break after first page for this test
                if (pages.Count >= 2)
                    break;
            }

            Assert.That(pages.Count, Is.GreaterThanOrEqualTo(2));

            _logger.LogDebug("Retrieved {Count} status pages with pagination", pages.Count);
        }
    }
}

