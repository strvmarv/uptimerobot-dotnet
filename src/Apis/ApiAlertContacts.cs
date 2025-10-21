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
    /// Alert Contact API endpoints.
    /// </summary>
    public partial class UptimeRobotClient
    {
        /// <summary>
        /// API path for getting alert contacts.
        /// </summary>
        public const string AlertContactsGetPath = "getAlertContacts";

        /// <summary>
        /// API path for creating alert contacts.
        /// </summary>
        public const string AlertContactsCreatePath = "newAlertContact";

        /// <summary>
        /// API path for updating alert contacts.
        /// </summary>
        public const string AlertContactsUpdatePath = "editAlertContact";

        /// <summary>
        /// API path for deleting alert contacts.
        /// </summary>
        public const string AlertContactsDeletePath = "deleteAlertContact";

        /// <summary>
        /// Gets a list of alert contacts with optional filtering.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the list of alert contacts.</returns>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> GetAlertContactsAsync(AlertContactSearchParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            parameters ??= new AlertContactSearchParameters { ApiKey = _apiKey };

            if (parameters.Limit > 50)
                throw new UptimeRobotValidationException("Limit must be less than or equal to 50.", nameof(parameters.Limit));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(AlertContactsGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets all alert contacts with automatic pagination.
        /// </summary>
        /// <param name="parameters">Search and filter parameters.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>An async enumerable of alert contacts.</returns>
        public async IAsyncEnumerable<Models.AlertContact> GetAllAlertContactsAsync(
            AlertContactSearchParameters? parameters = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            parameters ??= new AlertContactSearchParameters { ApiKey = _apiKey };
            parameters.Limit ??= 50;
            parameters.Offset ??= 0;

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            while (true)
            {
                var response = await GetAlertContactsAsync(parameters, cancellationToken).ConfigureAwait(false);

                if (response.AlertContacts != null)
                {
                    foreach (var contact in response.AlertContacts)
                    {
                        yield return contact;
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
        /// Gets a specific alert contact by ID.
        /// </summary>
        /// <param name="parameters">Search parameters including the alert contact ID.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the alert contact details.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> GetAlertContactAsync(AlertContactSearchParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(AlertContactsGetPath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Creates a new alert contact.
        /// </summary>
        /// <param name="parameters">Parameters for creating the alert contact.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response containing the created alert contact information.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> CreateAlertContactAsync(AlertContactCreateParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(AlertContactsCreatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Updates an existing alert contact.
        /// </summary>
        /// <param name="parameters">Parameters for updating the alert contact.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the update.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> UpdateAlertContactAsync(AlertContactUpdateParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(AlertContactsUpdatePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Deletes an alert contact.
        /// </summary>
        /// <param name="parameters">Parameters for deleting the alert contact.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        /// <exception cref="ArgumentNullException">Thrown when parameters is null.</exception>
        /// <exception cref="UptimeRobotValidationException">Thrown when validation fails.</exception>
        /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
        public async Task<UtrResponse> DeleteAlertContactAsync(AlertContactDeleteParameters parameters, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            ValidateModel(parameters);

            if (string.IsNullOrWhiteSpace(parameters.ApiKey))
                parameters.ApiKey = _apiKey;

            var uri = new Uri($"{GetRelativePathWithVersion(AlertContactsDeletePath)}", UriKind.Relative);
            var result = await PostAsync<UtrResponse>(uri, parameters, cancellationToken).ConfigureAwait(false);
            return result;
        }
    }
}

