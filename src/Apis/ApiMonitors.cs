using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UptimeRobotDotnet.Exceptions;
using UptimeRobotDotnet.Models;

namespace UptimeRobotDotnet
{
    /// <summary>
    /// Monitor API endpoints.
    /// </summary>
    public partial class UptimeRobotClient
    {
        /// <summary>
        /// API path for creating monitors.
        /// </summary>
        public const string MonitorsCreatePath = "newMonitor";

        /// <summary>
        /// API path for deleting monitors.
        /// </summary>
        public const string MonitorsDeletePath = "deleteMonitor";

        /// <summary>
        /// API path for getting monitors.
        /// </summary>
        public const string MonitorsGetPath = "getMonitors";

        /// <summary>
        /// API path for updating monitors.
        /// </summary>
        public const string MonitorsUpdatePath = "editMonitor";

        /// <summary>
        /// Gets a list of monitors with optional filtering.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the list of monitors.</returns>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> GetMonitorsAsync(MonitorSearchParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            parameters ??= new MonitorSearchParameters { ApiKey = _apiKey };

            if (parameters.Limit > 50)
                throw new UptimeRobotValidationException("Limit must be less than or equal to 50.", nameof(parameters.Limit));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets all monitors with automatic pagination.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>An async enumerable of monitors.</returns>
        public async IAsyncEnumerable<Models.Monitor> GetAllMonitorsAsync(
            MonitorSearchParameters? parameters = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            parameters ??= new MonitorSearchParameters { ApiKey = _apiKey };
            parameters.Limit ??= 50;
            parameters.Offset ??= 0;

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            while (true)
            {
                var response = await GetMonitorsAsync(parameters, cancellationToken).ConfigureAwait(false);

                if (response.Monitors != null)
                {
                    foreach (var monitor in response.Monitors)
                    {
                        yield return monitor;
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
        /// Gets a specific monitor by ID.
        /// </summary>
        /// <param name="parameters">Search parameters including the monitor ID.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the monitor details.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> GetMonitorAsync(MonitorSearchParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Creates a new monitor.
        /// </summary>
        /// <param name="parameters">Parameters for creating the monitor.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the created monitor information.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> CreateMonitorAsync(MonitorCreateParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsCreatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes a monitor.
        /// </summary>
        /// <param name="parameters">Parameters for deleting the monitor.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> DeleteMonitorAsync(MonitorDeleteParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsDeletePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Updates an existing monitor.
        /// </summary>
        /// <param name="parameters">Parameters for updating the monitor.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the update.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> UpdateMonitorAsync(MonitorUpdateParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (parameters.Id == 0)
                throw new UptimeRobotValidationException("Monitor ID is required for update operations.", nameof(parameters.Id));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(MonitorsUpdatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Validates a model using data annotations.
        /// </summary>
        /// <param name="model">The model to validate.</param>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        private void ValidateModel(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                var errors = string.Join("; ", validationResults);
                throw new UptimeRobotValidationException($"Validation failed: {errors}");
            }
        }

        // Backward compatibility methods (marked as obsolete)

        /// <summary>
        /// Gets a list of monitors. Use GetMonitorsAsync instead.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <returns>A response containing the list of monitors.</returns>
        [Obsolete("Use GetMonitorsAsync with cancellation token support instead.")]
        public Task<UtrResponse> Monitors(MonitorSearchParameters? parameters = null)
            => GetMonitorsAsync(parameters);

        /// <summary>
        /// Gets a specific monitor. Use GetMonitorAsync instead.
        /// </summary>
        /// <param name="parameters">Search parameters.</param>
        /// <returns>A response containing the monitor details.</returns>
        [Obsolete("Use GetMonitorAsync with cancellation token support instead.")]
        public Task<UtrResponse> Monitor(MonitorSearchParameters parameters)
            => GetMonitorAsync(parameters);

        /// <summary>
        /// Creates a new monitor. Use CreateMonitorAsync instead.
        /// </summary>
        /// <param name="parameters">Parameters for creating the monitor.</param>
        /// <returns>A response containing the created monitor information.</returns>
        [Obsolete("Use CreateMonitorAsync with cancellation token support instead.")]
        public Task<UtrResponse> MonitorCreate(MonitorCreateParameters parameters)
            => CreateMonitorAsync(parameters);

        /// <summary>
        /// Deletes a monitor. Use DeleteMonitorAsync instead.
        /// </summary>
        /// <param name="parameters">Parameters for deleting the monitor.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        [Obsolete("Use DeleteMonitorAsync with cancellation token support instead.")]
        public Task<UtrResponse> MonitorDelete(MonitorDeleteParameters parameters)
            => DeleteMonitorAsync(parameters);

        /// <summary>
        /// Updates a monitor. Use UpdateMonitorAsync instead.
        /// </summary>
        /// <param name="parameters">Parameters for updating the monitor.</param>
        /// <returns>A response indicating the result of the update.</returns>
        [Obsolete("Use UpdateMonitorAsync with cancellation token support instead.")]
        public Task<UtrResponse> MonitorUpdate(MonitorUpdateParameters parameters)
            => UpdateMonitorAsync(parameters);
    }
}
