using System;
using Microsoft.Extensions.Logging;
using WireMock.Server;

namespace UptimeRobotDotNetTests
{
    /// <summary>
    /// Base test class with HTTP client mocking support.
    /// </summary>
    public class BaseHttpClientTest : BaseTest, IDisposable
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Gets the WireMock server for HTTP mocking.
        /// </summary>
        protected readonly WireMockServer Server;

        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseHttpClientTest"/> class.
        /// </summary>
        public BaseHttpClientTest()
        {
            _logger = LoggerFactory.CreateLogger<BaseHttpClientTest>();
            _logger.LogDebug("Server Start");
            Server = WireMockServer.Start();
        }

        /// <summary>
        /// Disposes the resources used by this test class.
        /// </summary>
        /// <param name="disposing">True if disposing managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            _logger.LogDebug("Server Stop");
            Server.Stop();

            if (!_disposedValue)
            {
                if (disposing)
                {
                    Server.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes the test class.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
