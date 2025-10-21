using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UptimeRobotDotnet.Models;

namespace UptimeRobotDotNetTests.Core
{
    /// <summary>
    /// Tests for BaseModel functionality.
    /// </summary>
    public class BaseModelTests
    {
        [Test]
        public void GetContentForRequest_ReturnsCorrectDictionary()
        {
            // Arrange
            var parameters = new MonitorSearchParameters
            {
                ApiKey = "test-key",
                Limit = 50,
                Offset = 10,
                Search = "example.com"
            };

            // Act
            var result = parameters.GetContentForRequest();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ContainsKey("api_key"), Is.True);
            Assert.That(result["api_key"], Is.EqualTo("test-key"));
            Assert.That(result.ContainsKey("limit"), Is.True);
            Assert.That(result["limit"], Is.EqualTo(50));
            Assert.That(result.ContainsKey("offset"), Is.True);
            Assert.That(result["offset"], Is.EqualTo(10));
            Assert.That(result.ContainsKey("search"), Is.True);
            Assert.That(result["search"], Is.EqualTo("example.com"));
        }

        [Test]
        public void GetContentForRequest_IgnoresNullValues()
        {
            // Arrange
            var parameters = new MonitorSearchParameters
            {
                ApiKey = "test-key",
                Limit = null,
                Search = null
            };

            // Act
            var result = parameters.GetContentForRequest();

            // Assert
            Assert.That(result.ContainsKey("api_key"), Is.True);
            Assert.That(result.ContainsKey("limit"), Is.False);
            Assert.That(result.ContainsKey("search"), Is.False);
        }

        [Test]
        public void GetContentForRequest_SerializesComplexTypes()
        {
            // Arrange
            var parameters = new MonitorCreateParameters
            {
                ApiKey = "test-key",
                FriendlyName = "Test",
                Type = MonitorType.HTTP,
                Url = "https://example.com",
                CustomHttpHeaders = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer token" },
                    { "User-Agent", "Test" }
                }
            };

            // Act
            var result = parameters.GetContentForRequest();

            // Assert
            Assert.That(result.ContainsKey("custom_http_headers"), Is.True);
            Assert.That(result["custom_http_headers"], Is.TypeOf<string>());
            var serialized = (string)result["custom_http_headers"];
            Assert.That(serialized, Does.Contain("Authorization"));
            Assert.That(serialized, Does.Contain("Bearer token"));
        }

        [Test]
        public void GetContentForRequest_UsesJsonPropertyNameAttribute()
        {
            // Arrange
            var parameters = new MonitorCreateParameters
            {
                ApiKey = "test-key",
                FriendlyName = "Test Monitor",
                Type = MonitorType.HTTP,
                Url = "https://example.com"
            };

            // Act
            var result = parameters.GetContentForRequest();

            // Assert
            // Property is FriendlyName but JSON name is friendly_name
            Assert.That(result.ContainsKey("friendly_name"), Is.True);
            Assert.That(result["friendly_name"], Is.EqualTo("Test Monitor"));
        }

        [Test]
        public void GetContentForRequest_HandlesBooleanValues()
        {
            // Arrange
            var parameters = new MonitorCreateParameters
            {
                ApiKey = "test-key",
                FriendlyName = "Test",
                Type = MonitorType.HTTP,
                Url = "https://example.com",
                IgnoreSslErrors = true
            };

            // Act
            var result = parameters.GetContentForRequest();

            // Assert
            Assert.That(result.ContainsKey("ignore_ssl_errors"), Is.True);
            Assert.That(result["ignore_ssl_errors"], Is.True);
        }

        [Test]
        public void GetContentForRequest_CachesReflectionResults()
        {
            // Arrange
            var parameters1 = new MonitorSearchParameters { ApiKey = "test1" };
            var parameters2 = new MonitorSearchParameters { ApiKey = "test2" };

            // Act
            var result1 = parameters1.GetContentForRequest();
            var result2 = parameters2.GetContentForRequest();

            // Assert - Both should work correctly (cache should be transparent)
            Assert.That(result1["api_key"], Is.EqualTo("test1"));
            Assert.That(result2["api_key"], Is.EqualTo("test2"));
        }

        [Test]
        public void GetContentForRequest_HandlesEnumValues()
        {
            // Arrange
            var parameters = new MonitorCreateParameters
            {
                ApiKey = "test-key",
                FriendlyName = "Test",
                Type = MonitorType.HTTP,
                Url = "https://example.com",
                KeywordType = KeywordType.Exists,
                HttpMethod = HttpMethod.POST
            };

            // Act
            var result = parameters.GetContentForRequest();

            // Assert
            Assert.That(result.ContainsKey("type"), Is.True);
            Assert.That(result.ContainsKey("keyword_type"), Is.True);
            Assert.That(result.ContainsKey("http_method"), Is.True);
        }
    }
}

