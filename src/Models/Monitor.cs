using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Represents a monitor in UptimeRobot.
    /// </summary>
    public class Monitor : BaseModel, IContentModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Monitor"/> class.
        /// </summary>
        public Monitor() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Monitor"/> class from create parameters.
        /// </summary>
        /// <param name="createParameters">The parameters used to create the monitor.</param>
        public Monitor(MonitorCreateParameters createParameters)
        {
            if (createParameters == null)
                throw new ArgumentNullException(nameof(createParameters));

            FriendlyName = createParameters.FriendlyName;
            Url = createParameters.Url;
            Type = createParameters.Type;
            SubType = createParameters.SubType;
            Port = createParameters.Port;
            KeywordType = createParameters.KeywordType;
            KeywordCaseType = createParameters.KeywordCaseType;
            KeywordValue = createParameters.KeywordValue;
            Interval = createParameters.Interval;
            Timeout = createParameters.Timeout;
            Status = createParameters.Status;
            HttpUsername = createParameters.HttpUsername;
            HttpPassword = createParameters.HttpPassword;
            HttpAuthType = createParameters.HttpAuthType;
            HttpMethod = createParameters.HttpMethod;
            PostType = createParameters.PostType;
            PostValue = createParameters.PostValue;
            PostContentType = createParameters.PostContentType;
            AlertContacts = createParameters.AlertContacts;
            CustomHttpHeaders = createParameters.CustomHttpHeaders;
            CustomHttpStatuses = createParameters.CustomHttpStatuses;
            IgnoreSslErrors = createParameters.IgnoreSslErrors;
            DisableDomainExpireNotifications = createParameters.DisableDomainExpireNotifications;
        }

        /// <summary>
        /// Gets or sets the monitor ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the monitor.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the URL being monitored.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the type of the monitor.
        /// </summary>
        [JsonPropertyName("type")]
        public MonitorType Type { get; set; }

        /// <summary>
        /// Gets or sets the sub-type of the monitor (for port monitors).
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
        /// Gets or sets the check interval in seconds (60-3600).
        /// </summary>
        [JsonPropertyName("interval")]
        public int? Interval { get; set; }

        /// <summary>
        /// Gets or sets the timeout in seconds (default is 30).
        /// </summary>
        [JsonPropertyName("timeout")]
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets the status of the monitor.
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
        /// Gets or sets the HTTP method used for the request.
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
        public object? PostValue { get; set; }

        /// <summary>
        /// Gets or sets the POST content type.
        /// </summary>
        [JsonPropertyName("post_content_type")]
        public PostContentType? PostContentType { get; set; }

        /// <summary>
        /// Gets or sets the alert contacts for this monitor.
        /// </summary>
        [JsonPropertyName("alert_contacts")]
        public object? AlertContacts { get; set; }

        /// <summary>
        /// Gets or sets the maintenance windows for this monitor.
        /// </summary>
        [JsonPropertyName("mwindows")]
        public object? Mwindows { get; set; }

        /// <summary>
        /// Gets or sets custom HTTP headers.
        /// </summary>
        [JsonPropertyName("custom_http_headers")]
        public Dictionary<string, string>? CustomHttpHeaders { get; set; }

        /// <summary>
        /// Gets or sets custom HTTP status codes to consider as down.
        /// </summary>
        [JsonPropertyName("custom_http_statuses")]
        public object? CustomHttpStatuses { get; set; }

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
}
