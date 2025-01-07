using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using UptimeRobotDotnet;
using UptimeRobotDotnet.Models;
using WireMock.RequestBuilders;
using Response = WireMock.ResponseBuilders.Response;

namespace UptimeRobotDotNetTests.Monitors
{
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
        public void Monitors()
        {
            //var mockResult = new List<Monitor>
            //{
            //    new Monitor
            //    {
            //        Url = TestUrl,
            //        Custom_Http_Headers = new Dictionary<string, string>() { { UserAgentHeaderKey, UserAgentHeaderValue }}
            //    }
            //};

            var mockResult = new MonitorsList()
            {
                Monitors = new List<MonitorListMonitor>()
                {
                    new MonitorListMonitor()
                    {
                        Url = TestUrl
                    }
                }
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        monitors = new List<object> { new { url = TestUrl } }
                    }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var results = client.Monitors().GetAwaiter().GetResult()?.Monitors;

            Assert.That(() => results?.Count == 1);

            _logger.LogDebug(JsonSerializer.Serialize(results, _jsonSerializerOptions));
        }

        [Test]
        public async Task Monitor()
        {
            var mockResult = new Monitor { Id = TestId, Url = TestUrl };
            var mockParameters = new MonitorSearchParameters { Monitors = TestId.ToString(), Search = TestUrl };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(mockResult));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.Monitor(mockParameters);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void MonitorNotFound()
        {
            var mockParameters = new MonitorSearchParameters { Monitors = TestId.ToString(), Search = TestUrl };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsGetPath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<HttpRequestException>(() => client.Monitor(mockParameters));
        }

        [Test]
        public async Task MonitorCreate()
        {
            var mockParameters = new MonitorCreateParameters
            {
                Url = TestUrl,
                Custom_Http_Headers = new Dictionary<string, string>() { { UserAgentHeaderKey, UserAgentHeaderValue } }
            };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsCreatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(201)
                    .WithBodyAsJson(new Monitor(mockParameters)));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.MonitorCreate(mockParameters);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void MonitorCreateNotFound()
        {
            var mockParameters = new MonitorCreateParameters { Url = TestUrl };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsCreatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            Assert.ThrowsAsync<HttpRequestException>(() => client.MonitorCreate(mockParameters));
        }

        [Test]
        public async Task MonitorDelete()
        {
            var mockParameters = new MonitorDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { stat = OkResponse }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.MonitorDelete(mockParameters);

            Assert.That(OkResponse.Equals(result.Stat), Is.True);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void MonitorDeleteNotFound()
        {
            var mockParameters = new MonitorDeleteParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsDeletePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            Assert.ThrowsAsync<HttpRequestException>(() => client.MonitorDelete(mockParameters));
        }

        [Test]
        public async Task MonitorUpdate()
        {
            var mockParameters = new MonitorUpdateParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsUpdatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new { stat = OkResponse }));

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
            var result = await client.MonitorUpdate(mockParameters);

            Assert.That(() => result != null);

            _logger.LogDebug(JsonSerializer.Serialize(result, _jsonSerializerOptions));
        }

        [Test]
        public void MonitorUpdateNotFound()
        {
            var mockParameters = new MonitorUpdateParameters { Id = TestId };

            Server.Given(Request.Create()
                    .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsUpdatePath)}")
                    .UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(404)
                    .WithNotFound());

            var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);

            Assert.ThrowsAsync<HttpRequestException>(() => client.MonitorUpdate(mockParameters));
        }
    }
}
