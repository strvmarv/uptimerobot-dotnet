using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UptimeRobotDotNetTests
{
    public class BaseTest
    {
        public const string OkResponse = "ok";

        protected ILogger Logger;
        protected ILoggerFactory LoggerFactory;

        protected BaseTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(c =>
                {
                    c.SetMinimumLevel(LogLevel.Trace);
                    c.AddConsole();
                    c.AddDebug();
                })
                .BuildServiceProvider();

            LoggerFactory = serviceProvider.GetService<ILoggerFactory>();

            Logger = LoggerFactory.CreateLogger<BaseTest>();
        }
    }
}
