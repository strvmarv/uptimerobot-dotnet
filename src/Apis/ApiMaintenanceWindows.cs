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
    /// Maintenance Window API endpoints.
    /// </summary>
    public partial class UptimeRobotClient
    {
        /// <summary>
        /// API path for getting maintenance windows.
        /// </summary>
        public const string MaintenanceWindowsGetPath = "getMWindows";

        /// <summary>
        /// API path for creating maintenance windows.
        /// </summary>
        public const string MaintenanceWindowsCreatePath = "newMWindow";

        /// <summary>
        /// API path for updating maintenance windows.
        /// </summary>
        public const string MaintenanceWindowsUpdatePath = "editMWindow";

        /// <summary>
        /// API path for deleting maintenance windows.
        /// </summary>
        public const string MaintenanceWindowsDeletePath = "deleteMWindow";

        /// <summary>
        /// Gets a list of maintenance windows with optional filtering.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the list of maintenance windows.</returns>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> GetMaintenanceWindowsAsync(MaintenanceWindowSearchParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            parameters ??= new MaintenanceWindowSearchParameters { ApiKey = _apiKey };

            if (parameters.Limit > 50)
                throw new UptimeRobotValidationException("Limit must be less than or equal to 50.", nameof(parameters.Limit));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MaintenanceWindowsGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets all maintenance windows with automatic pagination.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>An async enumerable of maintenance windows.</returns>
        public async IAsyncEnumerable<Models.MaintenanceWindow> GetAllMaintenanceWindowsAsync(
            MaintenanceWindowSearchParameters? parameters = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            parameters ??= new MaintenanceWindowSearchParameters { ApiKey = _apiKey };
            parameters.Limit ??= 50;
            parameters.Offset ??= 0;

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            while (true)
            {
                var response = await GetMaintenanceWindowsAsync(parameters, cancellationToken).ConfigureAwait(false);

                if (response.MaintenanceWindows != null)
                {
                    foreach (var window in response.MaintenanceWindows)
                    {
                        yield return window;
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
        /// Gets a specific maintenance window by ID.
        /// </summary>
        /// <param name="parameters">Search parameters including the maintenance window ID.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the maintenance window details.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> GetMaintenanceWindowAsync(MaintenanceWindowSearchParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MaintenanceWindowsGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Creates a new maintenance window.
        /// </summary>
        /// <param name="parameters">Parameters for creating the maintenance window.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the created maintenance window information.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> CreateMaintenanceWindowAsync(MaintenanceWindowCreateParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MaintenanceWindowsCreatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Updates an existing maintenance window.
        /// </summary>
        /// <param name="parameters">Parameters for updating the maintenance window.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the update.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> UpdateMaintenanceWindowAsync(MaintenanceWindowUpdateParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MaintenanceWindowsUpdatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes a maintenance window.
        /// </summary>
        /// <param name="parameters">Parameters for deleting the maintenance window.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> DeleteMaintenanceWindowAsync(MaintenanceWindowDeleteParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MaintenanceWindowsDeletePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }
    }
}

