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

namespace UptimeRobotDotNetTests.AlertContacts
{
    /// <summary>
    /// Tests for Alert Contact API endpoints.
    /// </summary>
    public class AlertContactsTests : BaseHttpClientTest
    {
        private const string TestApiKey = "a-p-i-k-e-y";
        private const string TestId = "12345";
        private const string TestEmail = "test@example.com";

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly ILogger _logger;

        public AlertContactsTests()
        {
            _logger = LoggerFactory.CreateLogger<AlertContactsTests>();
        }

        [Test]
        public async Task GetAlertContacts()
        {
            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.AlertContactsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        alert_contacts = new List<object>
                        {
                            new { id = TestId, friendly_name = "Test Contact", type = 2, value = TestEmail }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var results = await client.GetAlertContactsAsync();

            Assert.That(results?.AlertContacts?.Count, Is.EqualTo(1));
            Assert.That(results.AlertContacts[0].Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task GetAlertContact()
        {
            var mockParameters = new AlertContactSearchParameters { AlertContacts = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.AlertContactsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        alert_contacts = new List<object>
                        {
                            new { id = TestId, friendly_name = "Test Contact", type = 2, value = TestEmail }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.GetAlertContactAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AlertContacts?.Count, Is.GreaterThan(0));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void GetAlertContactNotFound()
        {
            var mockParameters = new AlertContactSearchParameters { AlertContacts = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.AlertContactsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotException>(() => client.GetAlertContactAsync(mockParameters));
        }

        [Test]
        public async Task CreateAlertContact()
        {
            var mockParameters = new AlertContactCreateParameters
            {
                Type = AlertContactType.Email,
                FriendlyName = "Test Contact",
                Value = TestEmail
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.AlertContactsCreatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        alert_contact = new { id = TestId, status = 2 }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.CreateAlertContactAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AlertContact?.Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void CreateAlertContactWithInvalidParameters()
        {
            var mockParameters = new AlertContactCreateParameters
            {
                Type = AlertContactType.Email,
                FriendlyName = "Test Contact"
                // Missing required Value
            };

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotValidationException>(() => client.CreateAlertContactAsync(mockParameters));
        }

        [Test]
        public async Task UpdateAlertContact()
        {
            var mockParameters = new AlertContactUpdateParameters
            {
                Id = TestId,
                FriendlyName = "Updated Contact"
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.AlertContactsUpdatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        alert_contact = new { id = TestId }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.UpdateAlertContactAsync(mockParameters);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AlertContact?.Id, Is.EqualTo(TestId));

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public async Task DeleteAlertContact()
        {
            var mockParameters = new AlertContactDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.AlertContactsDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { stat = UptimeRobotClientBase.OkResponse }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.DeleteAlertContactAsync(mockParameters);

            Assert.That(UptimeRobotClientBase.OkResponse.Equals(result.Stat), Is.True);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void DeleteAlertContactNotFound()
        {
            var mockParameters = new AlertContactDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.AlertContactsDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<UptimeRobotException>(() => client.DeleteAlertContactAsync(mockParameters));
        }

        [Test]
        public async Task GetAllAlertContactsWithPagination()
        {
            // Mock first page
            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.AlertContactsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        stat = "ok",
                        pagination = new { offset = 0, limit = 2, total = 3 },
                        alert_contacts = new List<object>
                        {
                            new { id = "1", friendly_name = "Contact 1", type = 2, value = "test1@example.com" },
                            new { id = "2", friendly_name = "Contact 2", type = 2, value = "test2@example.com" }
                        }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var contacts = new List<AlertContact>();

            await foreach (var contact in client.GetAllAlertContactsAsync())
            {
                contacts.Add(contact);
                // Break after first page for this test
                if (contacts.Count >= 2)
                    break;
            }

            Assert.That(contacts.Count, Is.GreaterThanOrEqualTo(2));

            _logger.LogDebug("Retrieved {Count} alert contacts with pagination", contacts.Count);
        }
    }
}

