using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UptimeRobotDotnet.Models;

namespace UptimeRobotDotnet
{
    public class UptimeRobotClientBase
    {
        public const string DefaultApiUrl = "https://api.uptimerobot.com";
        public const string DefaultApiVersion = "v2";

        protected readonly HttpClient HttpClient;
        private readonly string _apiVersion;

        public UptimeRobotClientBase(HttpClient httpClient, string apiVersion)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(apiVersion)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiVersion));
            _apiVersion = apiVersion;
        }

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static Uri GetDefaultBaseApiUri()
        {
            return new Uri(DefaultApiUrl);
        }

        public Uri GetRelativePathWithVersion(string path)
        {
            return new Uri($"{_apiVersion}/{path}", UriKind.Relative);
        }

        public static Uri GetRelativePathWithDefaultVersion(string path)
        {
            return new Uri($"{DefaultApiVersion}/{path}", UriKind.Relative);
        }

        protected async Task<T> PostAsync<T>(Uri path, object content)
        {
            var reqContent = JsonSerializer.Serialize(content, JsonOptions);
            var resp = await HttpClient.PostAsync(path, new StringContent(reqContent, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var respContent = await resp.Content.ReadAsStringAsync().ConfigureAwait(false); // Not using stream here due to "stat: fail" scenario

            // API returns non-Http errors as a dynamic payload, detect and throw for consumer to manage
            if (respContent.Contains("stat") && respContent.Contains("fail"))
            {
                var error = JsonSerializer.Deserialize<UtrResponse>(respContent, JsonOptions);
                throw new HttpRequestException($"Request Failed: {respContent}");
            }

            resp.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<T>(respContent, JsonOptions);
            return result;
        }
    }
}