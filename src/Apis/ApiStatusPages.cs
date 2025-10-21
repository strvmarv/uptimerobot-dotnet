using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UptimeRobotDotnet.Exceptions;
using UptimeRobotDotnet.Models;

namespace UptimeRobotDotnet
{
    /// <summary>
    /// Status Page (Public Status Pages) API endpoints.
    /// </summary>
    public partial class UptimeRobotClient
    {
        /// <summary>
        /// API path for getting status pages.
        /// </summary>
        public const string StatusPagesGetPath = "getPSPs";

        /// <summary>
        /// API path for creating status pages.
        /// </summary>
        public const string StatusPagesCreatePath = "newPSP";

        /// <summary>
        /// API path for updating status pages.
        /// </summary>
        public const string StatusPagesUpdatePath = "editPSP";

        /// <summary>
        /// API path for deleting status pages.
        /// </summary>
        public const string StatusPagesDeletePath = "deletePSP";

        /// <summary>
        /// Gets a list of status pages with optional filtering.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the list of status pages.</returns>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> GetStatusPagesAsync(StatusPageSearchParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            parameters ??= new StatusPageSearchParameters { ApiKey = _apiKey };

            if (parameters.Limit > 50)
                throw new UptimeRobotValidationException("Limit must be less than or equal to 50.", nameof(parameters.Limit));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(StatusPagesGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets all status pages with automatic pagination.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>An async enumerable of status pages.</returns>
        public async IAsyncEnumerable<Models.StatusPage> GetAllStatusPagesAsync(
            StatusPageSearchParameters? parameters = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            parameters ??= new StatusPageSearchParameters { ApiKey = _apiKey };
            parameters.Limit ??= 50;
            parameters.Offset ??= 0;

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            while (true)
            {
                var response = await GetStatusPagesAsync(parameters, cancellationToken).ConfigureAwait(false);

                if (response.StatusPages != null)
                {
                    foreach (var page in response.StatusPages)
                    {
                        yield return page;
                    }
                }

                // Check if there are more results
                if (response.Pagination != null &&
                    response.Pagination.Offset + response.Pagination.Limit < response.Pagination.Total)
                {
                    parameters.Offset = response.Pagination.Offset + response.Pagination.Limit;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Gets a specific status page by ID.
        /// </summary>
        /// <param name="parameters">Search parameters including the status page ID.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the status page details.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> GetStatusPageAsync(StatusPageSearchParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(StatusPagesGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Creates a new status page.
        /// </summary>
        /// <param name="parameters">Parameters for creating the status page.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the created status page information.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> CreateStatusPageAsync(StatusPageCreateParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(StatusPagesCreatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Updates an existing status page.
        /// </summary>
        /// <param name="parameters">Parameters for updating the status page.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the update.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> UpdateStatusPageAsync(StatusPageUpdateParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(StatusPagesUpdatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes a status page.
        /// </summary>
        /// <param name="parameters">Parameters for deleting the status page.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> DeleteStatusPageAsync(StatusPageDeleteParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(StatusPagesDeletePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }
    }
}

