using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Standard response from UptimeRobot API.
    /// </summary>
    public class UtrResponse
    {
        /// <summary>
        /// Gets or sets the status of the response ("ok" or "fail").
        /// </summary>
        [JsonPropertyName("stat")]
        public string? Stat { get; set; }

        /// <summary>
        /// Gets or sets pagination information for list responses.
        /// </summary>
        [JsonPropertyName("pagination")]
        public UtrPagination? Pagination { get; set; }

        /// <summary>
        /// Gets or sets error information when stat is "fail".
        /// </summary>
        [JsonPropertyName("error")]
        public UtrError? Error { get; set; }

        /// <summary>
        /// Gets or sets monitor information for action responses (create, update, delete).
        /// </summary>
        [JsonPropertyName("monitor")]
        public UtrResponseActionMonitor? Monitor { get; set; }

        /// <summary>
        /// Gets or sets the list of monitors returned by the API.
        /// </summary>
        [JsonPropertyName("monitors")]
        public List<Monitor>? Monitors { get; set; }

        /// <summary>
        /// Gets or sets alert contact information for action responses.
        /// </summary>
        [JsonPropertyName("alert_contact")]
        public UtrResponseActionAlertContact? AlertContact { get; set; }

        /// <summary>
        /// Gets or sets the list of alert contacts returned by the API.
        /// </summary>
        [JsonPropertyName("alert_contacts")]
        public List<AlertContact>? AlertContacts { get; set; }

        /// <summary>
        /// Gets or sets maintenance window information for action responses.
        /// </summary>
        [JsonPropertyName("mwindow")]
        public UtrResponseActionMaintenanceWindow? MaintenanceWindow { get; set; }

        /// <summary>
        /// Gets or sets the list of maintenance windows returned by the API.
        /// </summary>
        [JsonPropertyName("mwindows")]
        public List<MaintenanceWindow>? MaintenanceWindows { get; set; }

        /// <summary>
        /// Gets or sets status page information for action responses.
        /// </summary>
        [JsonPropertyName("psp")]
        public UtrResponseActionStatusPage? StatusPage { get; set; }

        /// <summary>
        /// Gets or sets the list of status pages returned by the API.
        /// </summary>
        [JsonPropertyName("psps")]
        public List<StatusPage>? StatusPages { get; set; }
    }

    /// <summary>
    /// Pagination information in API responses.
    /// </summary>
    public class UtrPagination
    {
        /// <summary>
        /// Gets or sets the offset used in the request.
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the limit used in the request.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the total number of items available.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    /// <summary>
    /// Error information in API responses.
    /// </summary>
    public class UtrError
    {
        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the parameter name that caused the error.
        /// </summary>
        [JsonPropertyName("parameter_name")]
        public string? ParameterName { get; set; }
    }

    /// <summary>
    /// Monitor information in action responses (create, update, delete).
    /// </summary>
    public class UtrResponseActionMonitor
    {
        /// <summary>
        /// Gets or sets the monitor ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the monitor status.
        /// </summary>
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }

    /// <summary>
    /// Alert contact information in action responses.
    /// </summary>
    public class UtrResponseActionAlertContact
    {
        /// <summary>
        /// Gets or sets the alert contact ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the alert contact status.
        /// </summary>
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }

    /// <summary>
    /// Maintenance window information in action responses.
    /// </summary>
    public class UtrResponseActionMaintenanceWindow
    {
        /// <summary>
        /// Gets or sets the maintenance window ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the maintenance window status.
        /// </summary>
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }

    /// <summary>
    /// Status page information in action responses.
    /// </summary>
    public class UtrResponseActionStatusPage
    {
        /// <summary>
        /// Gets or sets the status page ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the status page status.
        /// </summary>
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }
}
