using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using UptimeRobotDotnet.Exceptions;
using UptimeRobotDotnet.Models;

namespace UptimeRobotDotnet
{
    /// <summary>
    /// Base class for UptimeRobot API client.
    /// </summary>
    public class UptimeRobotClientBase
    {
        /// <summary>
        /// Default API base URL.
        /// </summary>
        public const string DefaultApiUrl = "https://api.uptimerobot.com";

        /// <summary>
        /// Default API version.
        /// </summary>
        public const string DefaultApiVersion = "v2";

        /// <summary>
        /// Response status indicating success.
        /// </summary>
        public const string OkResponse = "ok";

        /// <summary>
        /// Response status indicating failure.
        /// </summary>
        public const string FailResponse = "fail";

        /// <summary>
        /// Gets the HTTP client used for API requests.
        /// </summary>
        protected readonly HttpClient HttpClient;

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        protected readonly ILogger Logger;

        private readonly string _apiVersion;

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            Converters =
            {
                new Converters.NullableEnumConverter<Models.MonitorSubType>(),
                new Converters.NullableEnumConverter<Models.KeywordType>(),
                new Converters.NullableEnumConverter<Models.KeywordCaseType>(),
                new Converters.NullableEnumConverter<Models.HttpAuthType>(),
                new Converters.NullableEnumConverter<Models.HttpMethod>(),
                new Converters.NullableEnumConverter<Models.PostType>(),
                new Converters.NullableEnumConverter<Models.PostContentType>(),
                new Converters.NullableEnumConverter<Models.MonitorStatus>(),
                new Converters.NullableEnumConverter<Models.AlertContactType>(),
                new Converters.NullableEnumConverter<Models.AlertContactStatus>(),
                new Converters.NullableEnumConverter<Models.MaintenanceWindowType>(),
                new Converters.NullableEnumConverter<Models.MaintenanceWindowStatus>(),
                new Converters.NullableEnumConverter<Models.StatusPageStatus>(),
                new Converters.NullableEnumConverter<Models.StatusPageSort>()
            }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="UptimeRobotClientBase"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="apiVersion">The API version to use.</param>
        /// <param name="logger">Optional logger for diagnostics.</param>
        /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
        /// <exception cref="ArgumentException">Thrown when apiVersion is null or whitespace.</exception>
        public UptimeRobotClientBase(HttpClient httpClient, string apiVersion, ILogger? logger = null)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(apiVersion))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiVersion));
            _apiVersion = apiVersion;
            Logger = logger ?? NullLogger.Instance;
        }

        /// <summary>
        /// Gets the default base API URI.
        /// </summary>
        /// <returns>The default API base URI.</returns>
        public static Uri GetDefaultBaseApiUri()
        {
            return new Uri(DefaultApiUrl);
        }

        /// <summary>
        /// Gets the relative path with the configured API version.
        /// </summary>
        /// <param name="path">The API endpoint path.</param>
        /// <returns>The full relative URI with version.</returns>
        public Uri GetRelativePathWithVersion(string path)
        {
            return new Uri($"{_apiVersion}/{path}", UriKind.Relative);
        }

        /// <summary>
        /// Gets the relative path with the default API version.
        /// </summary>
        /// <param name="path">The API endpoint path.</param>
        /// <returns>The full relative URI with default version.</returns>
        public static Uri GetRelativePathWithDefaultVersion(string path)
        {
            return new Uri($"{DefaultApiVersion}/{path}", UriKind.Relative);
        }

        /// <summary>
        /// Posts a request to the API and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="path">The relative API endpoint path.</param>
        /// <param name="content">The request content model.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>The deserialized response.</returns>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error response.</exception>
        /// <exception cref="UptimeRobotException">Thrown when the request fails for other reasons.</exception>
        protected async Task<T> PostAsync<T>(Uri path, IContentModel content, CancellationToken cancellationToken = default)
        {
            try
            {
                var parsedContent = content.GetContentForRequest();
                var serializedContent = new UtrFormUrlEncodedContent(parsedContent);

                Logger.LogDebug("Posting to {Path}", path);

                var resp = await HttpClient.PostAsync(path, serializedContent, cancellationToken).ConfigureAwait(false);
                var respContent = await resp.Content.ReadAsStringAsync(
#if NET5_0_OR_GREATER
                    cancellationToken
#endif
                ).ConfigureAwait(false);

                Logger.LogDebug("Received response with status code {StatusCode}", resp.StatusCode);

                // API returns non-HTTP errors as a dynamic payload with "stat": "fail"
                if (respContent.Contains("\"stat\"") && respContent.Contains("\"fail\""))
                {
                    var errorResponse = JsonSerializer.Deserialize<UtrResponse>(respContent, JsonOptions);
                    var error = errorResponse?.Error;

                    var errorMessage = error?.Message ?? "Unknown API error";
                    var errorType = error?.Type;
                    var parameterName = error?.ParameterName;

                    Logger.LogError("API returned error: {ErrorType} - {ErrorMessage}", errorType, errorMessage);

                    throw new UptimeRobotApiException(
                        $"API request failed: {errorMessage}",
                        errorType,
                        errorMessage,
                        parameterName,
                        error);
                }

                resp.EnsureSuccessStatusCode();

                var result = JsonSerializer.Deserialize<T>(respContent, JsonOptions);
                if (result == null)
                {
                    throw new UptimeRobotException("Failed to deserialize API response");
                }

                return result;
            }
            catch (UptimeRobotException)
            {
                throw;
            }
            catch (HttpRequestException ex)
            {
                Logger.LogError(ex, "HTTP request failed");
                throw new UptimeRobotException("HTTP request to UptimeRobot API failed", ex);
            }
            catch (TaskCanceledException ex) when (ex.CancellationToken == cancellationToken)
            {
                Logger.LogWarning("Request was cancelled");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                Logger.LogError(ex, "Request timed out");
                throw new UptimeRobotException("Request to UptimeRobot API timed out", ex);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected error during API request");
                throw new UptimeRobotException("Unexpected error during API request", ex);
            }
        }
    }
}
