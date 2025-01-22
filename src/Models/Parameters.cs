using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Parameters for creating a monitor
    ///
    /// https://uptimerobot.com/api/#newMonitorWrap
    /// </summary>
    public class MonitorCreateParameters : BaseModel, IContentModel
    {
        /// <summary>
        /// Friendly name of the monitor (REQUIRED)
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string Friendly_Name { get; set; }

        /// <summary>
        /// URL to monitor (REQUIRED)
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Type of monitor (REQUIRED)
        /// </summary>
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("sub_type")]
        public int? Sub_Type { get; set; }
        [JsonPropertyName("port")]
        public string Port { get; set; }
        [JsonPropertyName("keyword_type")]
        public KeywordType? Keyword_Type { get; set; }
        [JsonPropertyName("keyword_case_type")]
        public KeywordCaseType? Keyword_Case_Type { get; set; }
        [JsonPropertyName("keyword_value")]
        public string Keyword_Value { get; set; }
        [JsonPropertyName("interval")]
        public int? Interval { get; set; }
        [JsonPropertyName("timeout")]
        public int? Timeout { get; set; }
        [JsonPropertyName("status")]
        public Status? Status { get; set; }
        [JsonPropertyName("http_username")]
        public string Http_Username { get; set; }
        [JsonPropertyName("http_password")]
        public string Http_Password { get; set; }
        [JsonPropertyName("http_auth_type")]
        public HttpAuthType? Http_Auth_Type { get; set; }
        [JsonPropertyName("http_method")]
        public HttpMethod? Http_Method { get; set; }
        [JsonPropertyName("post_type")]
        public PostType? Post_Type { get; set; }
        [JsonPropertyName("post_value")]
        public string Post_Value { get; set; }
        [JsonPropertyName("post_content_type")]
        public PostContentType? Post_Content_Type { get; set; }
        [JsonPropertyName("alert_contacts")]
        public List<string> Alert_Contacts { get; set; }
        [JsonPropertyName("custom_http_headers")]
        public Dictionary<string, string> Custom_Http_Headers { get; set; }
        [JsonPropertyName("custom_http_statuses")]
        public string Custom_Http_Statuses { get; set; }
        [JsonPropertyName("ignore_ssl_errors")]
        public bool? Ignore_Ssl_Errors { get; set; }
        [JsonPropertyName("disable_domain_expire_notifications")]
        public bool? Disable_Domain_Expire_Notifications { get; set; }
    }

    public class MonitorDeleteParameters : BaseModel, IContentModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class MonitorSearchParameters : BaseModel, IContentModel
    {
        // Pagination properties

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        // Filter properties

        [JsonPropertyName("monitors")]
        public string Monitors { get; set; }

        [JsonPropertyName("search")]
        public string Search { get; set; }

        // Optional data includes

        [JsonPropertyName("alert_contacts")]
        public int Alert_Contacts { get; set; }

        [JsonPropertyName("custom_http_headers")]
        public int Custom_Http_Headers { get; set; }

        [JsonPropertyName("http_request_details")]
        public bool Http_Request_Details { get; set; }

        [JsonPropertyName("custom_http_statuses")]
        public int Custom_Http_Statuses { get; set; }

        [JsonPropertyName("mwindows")]
        public int Mwindows { get; set; }
    }

    public class MonitorUpdateParameters : Monitor
    {
    }
}
