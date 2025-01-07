using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UptimeRobotDotnet
{
    public class UptimeRobotClientFactory
    {

#if  NET5_0_OR_GREATER
        private static readonly HttpClient DefaultHttpClient = new HttpClient(new SocketsHttpHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });
#else
        private static readonly HttpClient DefaultHttpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });
#endif

        static UptimeRobotClientFactory()
        {
            DefaultHttpClient.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip");
            DefaultHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultHttpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            DefaultHttpClient.BaseAddress = UptimeRobotClientBase.GetDefaultBaseApiUri();
        }

        public static UptimeRobotClient Create(string apiKey, string apiVersion = UptimeRobotClientBase.DefaultApiVersion)
        {
            return new UptimeRobotClient(DefaultHttpClient, apiKey, apiVersion);
        }

        public static UptimeRobotClient Create(HttpClient httpClient, string apiKey, string apiVersion = UptimeRobotClientBase.DefaultApiVersion)
        {
            return new UptimeRobotClient(httpClient, apiKey, apiVersion);
        }
    }
}
