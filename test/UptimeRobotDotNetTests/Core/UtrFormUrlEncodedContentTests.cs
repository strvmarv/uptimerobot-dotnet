using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UptimeRobotDotnet;

namespace UptimeRobotDotNetTests.Core
{
    /// <summary>
    /// Tests for UtrFormUrlEncodedContent functionality.
    /// </summary>
    public class UtrFormUrlEncodedContentTests
    {
        [Test]
        public async System.Threading.Tasks.Task Constructor_CreatesValidContent()
        {
            // Arrange
            var data = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("key1", "value1"),
                new KeyValuePair<string, object>("key2", 123)
            };

            // Act
            var content = new UtrFormUrlEncodedContent(data);
            var result = await content.ReadAsStringAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Contain("key1=value1"));
            Assert.That(result, Does.Contain("key2=123"));
        }

        [Test]
        public async System.Threading.Tasks.Task Constructor_HandlesSpecialCharacters()
        {
            // Arrange
            var data = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("email", "test@example.com"),
                new KeyValuePair<string, object>("url", "https://example.com/path?query=value")
            };

            // Act
            var content = new UtrFormUrlEncodedContent(data);
            var result = await content.ReadAsStringAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Contain("email="));
            Assert.That(result, Does.Contain("url="));
            // Special characters should be encoded
            Assert.That(result, Does.Not.Contain("@"));
            Assert.That(result, Does.Not.Contain("?"));
        }

        [Test]
        public async System.Threading.Tasks.Task Constructor_EncodesSpacesAsPlus()
        {
            // Arrange
            var data = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", "Test Monitor")
            };

            // Act
            var content = new UtrFormUrlEncodedContent(data);
            var result = await content.ReadAsStringAsync();

            // Assert
            Assert.That(result, Does.Contain("name=Test+Monitor"));
        }

        [Test]
        public async System.Threading.Tasks.Task Constructor_HandlesEmptyString()
        {
            // Arrange
            var data = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("key", "")
            };

            // Act
            var content = new UtrFormUrlEncodedContent(data);
            var result = await content.ReadAsStringAsync();

            // Assert
            Assert.That(result, Does.Contain("key="));
        }

        [Test]
        public async System.Threading.Tasks.Task Constructor_HandlesMultipleValues()
        {
            // Arrange
            var data = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("api_key", "test-key"),
                new KeyValuePair<string, object>("friendly_name", "My Monitor"),
                new KeyValuePair<string, object>("url", "https://example.com"),
                new KeyValuePair<string, object>("type", 1)
            };

            // Act
            var content = new UtrFormUrlEncodedContent(data);
            var result = await content.ReadAsStringAsync();

            // Assert
            Assert.That(result, Does.Contain("api_key="));
            Assert.That(result, Does.Contain("friendly_name="));
            Assert.That(result, Does.Contain("url="));
            Assert.That(result, Does.Contain("type=1"));
            // Should be separated by &
            Assert.That(result.Count(c => c == '&'), Is.EqualTo(3));
        }

        [Test]
        public void Constructor_ThrowsOnNullInput()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new UtrFormUrlEncodedContent(null!));
        }

        [Test]
        public async System.Threading.Tasks.Task Constructor_ConvertsKeysToLowerCase()
        {
            // Arrange
            var data = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("API_KEY", "test"),
                new KeyValuePair<string, object>("FriendlyName", "Test")
            };

            // Act
            var content = new UtrFormUrlEncodedContent(data);
            var result = await content.ReadAsStringAsync();

            // Assert
            Assert.That(result, Does.Contain("api_key="));
            Assert.That(result, Does.Contain("friendlyname="));
        }

        [Test]
        public void ContentType_IsFormUrlEncoded()
        {
            // Arrange
            var data = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("key", "value")
            };

            // Act
            var content = new UtrFormUrlEncodedContent(data);

            // Assert
            Assert.That(content.Headers.ContentType?.MediaType, Is.EqualTo("application/x-www-form-urlencoded"));
        }
    }
}

