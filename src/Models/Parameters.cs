using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Parameters for creating a monitor.
    /// </summary>
    public class MonitorCreateParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the friendly name of the monitor (required).
        /// </summary>
        [Required]
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the URL to monitor (required).
        /// </summary>
        [Required]
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the type of monitor (required).
        /// </summary>
        [Required]
        [JsonPropertyName("type")]
        public MonitorType Type { get; set; }

        /// <summary>
        /// Gets or sets the sub-type for port monitors.
        /// </summary>
        [JsonPropertyName("sub_type")]
        public MonitorSubType? SubType { get; set; }

        /// <summary>
        /// Gets or sets the port number (for port monitors).
        /// </summary>
        [JsonPropertyName("port")]
        public string? Port { get; set; }

        /// <summary>
        /// Gets or sets the keyword type (for keyword monitors).
        /// </summary>
        [JsonPropertyName("keyword_type")]
        public KeywordType? KeywordType { get; set; }

        /// <summary>
        /// Gets or sets the keyword case sensitivity (for keyword monitors).
        /// </summary>
        [JsonPropertyName("keyword_case_type")]
        public KeywordCaseType? KeywordCaseType { get; set; }

        /// <summary>
        /// Gets or sets the keyword value (for keyword monitors).
        /// </summary>
        [JsonPropertyName("keyword_value")]
        public string? KeywordValue { get; set; }

        /// <summary>
        /// Gets or sets the check interval in seconds (60-3600, default 300).
        /// </summary>
        [Range(60, 3600)]
        [JsonPropertyName("interval")]
        public int? Interval { get; set; }

        /// <summary>
        /// Gets or sets the timeout in seconds (default 30).
        /// </summary>
        [Range(1, 300)]
        [JsonPropertyName("timeout")]
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the initial status of the monitor.
        /// </summary>
        [JsonPropertyName("status")]
        public MonitorStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the HTTP basic authentication username.
        /// </summary>
        [JsonPropertyName("http_username")]
        public string? HttpUsername { get; set; }

        /// <summary>
        /// Gets or sets the HTTP basic authentication password.
        /// </summary>
        [JsonPropertyName("http_password")]
        public string? HttpPassword { get; set; }

        /// <summary>
        /// Gets or sets the HTTP authentication type.
        /// </summary>
        [JsonPropertyName("http_auth_type")]
        public HttpAuthType? HttpAuthType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        [JsonPropertyName("http_method")]
        public HttpMethod? HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the POST data type.
        /// </summary>
        [JsonPropertyName("post_type")]
        public PostType? PostType { get; set; }

        /// <summary>
        /// Gets or sets the POST data value.
        /// </summary>
        [JsonPropertyName("post_value")]
        public string? PostValue { get; set; }

        /// <summary>
        /// Gets or sets the POST content type.
        /// </summary>
        [JsonPropertyName("post_content_type")]
        public PostContentType? PostContentType { get; set; }

        /// <summary>
        /// Gets or sets the alert contact IDs.
        /// </summary>
        [JsonPropertyName("alert_contacts")]
        public List<string>? AlertContacts { get; set; }

        /// <summary>
        /// Gets or sets custom HTTP headers.
        /// </summary>
        [JsonPropertyName("custom_http_headers")]
        public Dictionary<string, string>? CustomHttpHeaders { get; set; }

        /// <summary>
        /// Gets or sets custom HTTP status codes to treat as down.
        /// </summary>
        [JsonPropertyName("custom_http_statuses")]
        public string? CustomHttpStatuses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore SSL errors.
        /// </summary>
        [JsonPropertyName("ignore_ssl_errors")]
        public bool? IgnoreSslErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable domain expiration notifications.
        /// </summary>
        [JsonPropertyName("disable_domain_expire_notifications")]
        public bool? DisableDomainExpireNotifications { get; set; }
    }

    /// <summary>
    /// Parameters for deleting a monitor.
    /// </summary>
    public class MonitorDeleteParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the ID of the monitor to delete (required).
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Parameters for searching/filtering monitors.
    /// </summary>
    public class MonitorSearchParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the maximum number of results to return (max 50).
        /// </summary>
        [Range(1, 50)]
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets the offset for pagination.
        /// </summary>
        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        /// <summary>
        /// Gets or sets specific monitor IDs to retrieve (comma-separated or array).
        /// </summary>
        [JsonPropertyName("monitors")]
        public string? Monitors { get; set; }

        /// <summary>
        /// Gets or sets the search term to filter monitors.
        /// </summary>
        [JsonPropertyName("search")]
        public string? Search { get; set; }

        /// <summary>
        /// Gets or sets whether to include alert contacts (0 or 1).
        /// </summary>
        [JsonPropertyName("alert_contacts")]
        public int AlertContacts { get; set; }

        /// <summary>
        /// Gets or sets whether to include custom HTTP headers (0 or 1).
        /// </summary>
        [JsonPropertyName("custom_http_headers")]
        public int CustomHttpHeaders { get; set; }

        /// <summary>
        /// Gets or sets whether to include HTTP request details.
        /// </summary>
        [JsonPropertyName("http_request_details")]
        public bool HttpRequestDetails { get; set; }

        /// <summary>
        /// Gets or sets whether to include custom HTTP statuses (0 or 1).
        /// </summary>
        [JsonPropertyName("custom_http_statuses")]
        public int CustomHttpStatuses { get; set; }

        /// <summary>
        /// Gets or sets whether to include maintenance windows (0 or 1).
        /// </summary>
        [JsonPropertyName("mwindows")]
        public int Mwindows { get; set; }
    }

    /// <summary>
    /// Parameters for updating a monitor.
    /// </summary>
    public class MonitorUpdateParameters : Monitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorUpdateParameters"/> class.
        /// </summary>
        public MonitorUpdateParameters()
        {
        }
    }

    /// <summary>
    /// Parameters for searching alert contacts.
    /// </summary>
    public class AlertContactSearchParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the maximum number of results to return (max 50).
        /// </summary>
        [Range(1, 50)]
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets the offset for pagination.
        /// </summary>
        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        /// <summary>
        /// Gets or sets specific alert contact IDs to retrieve.
        /// </summary>
        [JsonPropertyName("alert_contacts")]
        public string? AlertContacts { get; set; }
    }

    /// <summary>
    /// Parameters for creating an alert contact.
    /// </summary>
    public class AlertContactCreateParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the type of alert contact (required).
        /// </summary>
        [Required]
        [JsonPropertyName("type")]
        public AlertContactType Type { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the alert contact (required).
        /// </summary>
        [Required]
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the value for the alert contact (e.g., email address, phone number).
        /// </summary>
        [Required]
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    /// <summary>
    /// Parameters for updating an alert contact.
    /// </summary>
    public class AlertContactUpdateParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the ID of the alert contact to update (required).
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the alert contact.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the value for the alert contact.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the status of the alert contact.
        /// </summary>
        [JsonPropertyName("status")]
        public AlertContactStatus? Status { get; set; }
    }

    /// <summary>
    /// Parameters for deleting an alert contact.
    /// </summary>
    public class AlertContactDeleteParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the ID of the alert contact to delete (required).
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }

    /// <summary>
    /// Parameters for searching maintenance windows.
    /// </summary>
    public class MaintenanceWindowSearchParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the maximum number of results to return (max 50).
        /// </summary>
        [Range(1, 50)]
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets the offset for pagination.
        /// </summary>
        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        /// <summary>
        /// Gets or sets specific maintenance window IDs to retrieve.
        /// </summary>
        [JsonPropertyName("mwindows")]
        public string? Mwindows { get; set; }
    }

    /// <summary>
    /// Parameters for creating a maintenance window.
    /// </summary>
    public class MaintenanceWindowCreateParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the friendly name of the maintenance window (required).
        /// </summary>
        [Required]
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the type of maintenance window (required).
        /// </summary>
        [Required]
        [JsonPropertyName("type")]
        public MaintenanceWindowType Type { get; set; }

        /// <summary>
        /// Gets or sets the start time (Unix timestamp, required).
        /// </summary>
        [Required]
        [JsonPropertyName("start_time")]
        public int StartTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds (required).
        /// </summary>
        [Required]
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the value for weekly/monthly windows (e.g., day of week).
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    /// <summary>
    /// Parameters for updating a maintenance window.
    /// </summary>
    public class MaintenanceWindowUpdateParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the ID of the maintenance window to update (required).
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the maintenance window.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the start time (Unix timestamp).
        /// </summary>
        [JsonPropertyName("start_time")]
        public int? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        /// <summary>
        /// Gets or sets the value for weekly/monthly windows.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the status of the maintenance window.
        /// </summary>
        [JsonPropertyName("status")]
        public MaintenanceWindowStatus? Status { get; set; }
    }

    /// <summary>
    /// Parameters for deleting a maintenance window.
    /// </summary>
    public class MaintenanceWindowDeleteParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the ID of the maintenance window to delete (required).
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Parameters for searching status pages.
    /// </summary>
    public class StatusPageSearchParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the maximum number of results to return (max 50).
        /// </summary>
        [Range(1, 50)]
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets the offset for pagination.
        /// </summary>
        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        /// <summary>
        /// Gets or sets specific status page IDs to retrieve.
        /// </summary>
        [JsonPropertyName("psps")]
        public string? Psps { get; set; }
    }

    /// <summary>
    /// Parameters for creating a status page.
    /// </summary>
    public class StatusPageCreateParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the friendly name of the status page (required).
        /// </summary>
        [Required]
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the monitors to include (comma-separated IDs, required).
        /// </summary>
        [Required]
        [JsonPropertyName("monitors")]
        public string? Monitors { get; set; }

        /// <summary>
        /// Gets or sets custom message for the status page.
        /// </summary>
        [JsonPropertyName("custom_message")]
        public string? CustomMessage { get; set; }

        /// <summary>
        /// Gets or sets the sort order for monitors.
        /// </summary>
        [JsonPropertyName("sort")]
        public StatusPageSort? Sort { get; set; }

        /// <summary>
        /// Gets or sets the status of the status page.
        /// </summary>
        [JsonPropertyName("status")]
        public StatusPageStatus? Status { get; set; }
    }

    /// <summary>
    /// Parameters for updating a status page.
    /// </summary>
    public class StatusPageUpdateParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the ID of the status page to update (required).
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the status page.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the monitors to include (comma-separated IDs).
        /// </summary>
        [JsonPropertyName("monitors")]
        public string? Monitors { get; set; }

        /// <summary>
        /// Gets or sets custom message for the status page.
        /// </summary>
        [JsonPropertyName("custom_message")]
        public string? CustomMessage { get; set; }

        /// <summary>
        /// Gets or sets the sort order for monitors.
        /// </summary>
        [JsonPropertyName("sort")]
        public StatusPageSort? Sort { get; set; }

        /// <summary>
        /// Gets or sets the status of the status page.
        /// </summary>
        [JsonPropertyName("status")]
        public StatusPageStatus? Status { get; set; }
    }

    /// <summary>
    /// Parameters for deleting a status page.
    /// </summary>
    public class StatusPageDeleteParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the ID of the status page to delete (required).
        /// </summary>
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
