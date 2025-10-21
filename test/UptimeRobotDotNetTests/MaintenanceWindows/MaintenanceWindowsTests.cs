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

namespace UptimeRobotDotNetTests.MaintenanceWindows
{
    /// <summary>
    /// Tests for Maintenance Window API endpoints.
    /// </summary>
    public class MaintenanceWindowsTests : BaseHttpClientTest
    {
        private const string TestApiKey = "a-p-i-k-e-y";
        private const int TestId = 123456;
        private readonly int TestStartTime = (int)DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public MaintenanceWindowsTests()
        {
            _logger = LoggerFactory.CreateLogger<MaintenanceWindowsTests>();
        }

        [Test]
        public async Task GetMaintenanceWindows()
        {
            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MaintenanceWindowsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        mwindows = new List<object>
                        {
                            new { id = TestId, friendly_name = "Weekly Maintenance", type = 3 }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var results = await client.GetMaintenanceWindowsAsync();

            Assert.That(results?.MaintenanceWindows?.Count, Is.EqualTo(1));
            Assert.That(results.MaintenanceWindows[0].Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task GetMaintenanceWindow()
        {
            var mockParameters = new MaintenanceWindowSearchParameters { Mwindows = TestId.ToString() };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MaintenanceWindowsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        mwindows = new List<object>
                        {
                            new { id = TestId, friendly_name = "Test Window", type = 2 }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.GetMaintenanceWindowAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.MaintenanceWindows?.Count, Is.GreaterThan(0));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void GetMaintenanceWindowNotFound()
        {
            var mockParameters = new MaintenanceWindowSearchParameters { Mwindows = TestId.ToString() };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MaintenanceWindowsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotException>(() => client.GetMaintenanceWindowAsync(mockParameters));
        }

        [Test]
        public async Task CreateMaintenanceWindow()
        {
            var mockParameters = new MaintenanceWindowCreateParameters
            {
                FriendlyName = "Daily Maintenance",
                Type = MaintenanceWindowType.Daily,
                StartTime = TestStartTime,
                Duration = 3600
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MaintenanceWindowsCreatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        mwindow = new { id = TestId }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.CreateMaintenanceWindowAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.MaintenanceWindow?.Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CreateMaintenanceWindowWithMissingFriendlyName()
        {
            var mockParameters = new MaintenanceWindowCreateParameters
            {
                // Missing required FriendlyName
                Type = MaintenanceWindowType.Daily,
                StartTime = TestStartTime,
                Duration = 3600
            };

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotValidationException>(() => client.CreateMaintenanceWindowAsync(mockParameters));
        }

        [Test]
        public async Task UpdateMaintenanceWindow()
        {
            var mockParameters = new MaintenanceWindowUpdateParameters
            {
                Id = TestId,
                Duration = 7200, // 2 hours
                Status = MaintenanceWindowStatus.Active
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MaintenanceWindowsUpdatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        mwindow = new { id = TestId }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.UpdateMaintenanceWindowAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.MaintenanceWindow?.Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public async Task DeleteMaintenanceWindow()
        {
            var mockParameters = new MaintenanceWindowDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MaintenanceWindowsDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { stat = UptimeRobotClientBase.OkResponse }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.DeleteMaintenanceWindowAsync(mockParameters);

            Assert.That(UptimeRobotClientBase.OkResponse.Equals(result.Stat), Is.True);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void DeleteMaintenanceWindowNotFound()
        {
            var mockParameters = new MaintenanceWindowDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MaintenanceWindowsDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotException>(() => client.DeleteMaintenanceWindowAsync(mockParameters));
        }

        [Test]
        public async Task GetAllMaintenanceWindowsWithPagination()
        {
            // Mock first page
            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MaintenanceWindowsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        pagination = new { offset = 0, limit = 2, total = 3 },
                        mwindows = new List<object>
                        {
                            new { id = 1, friendly_name = "Window 1", type = 2 },
                            new { id = 2, friendly_name = "Window 2", type = 3 }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var windows = new List<MaintenanceWindow>();

            await foreach (var window in client.GetAllMaintenanceWindowsAsync())
            {
                windows.Add(window);
                // Break after first page for this test
                if (windows.Count >= 2)
                    break;
            }

            Assert.That(windows.Count, Is.GreaterThanOrEqualTo(2));

            _logger.LogDebug("Retrieved {Count} maintenance windows with pagination", windows.Count);
        }
    }
}

