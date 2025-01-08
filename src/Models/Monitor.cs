using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UptimeRobotDotnet.Models
{
    public class Monitor : BaseModel
    {
        public Monitor() { }

        public Monitor(MonitorCreateParameters createParameters)
        {
            Friendly_Name = createParameters.Friendly_Name;
            Url = createParameters.Url;
            Sub_Type = createParameters.Sub_Type;
            Port = createParameters.Port;
            Keyword_Type = createParameters.Keyword_Type;
            Keyword_Case_Type = createParameters.Keyword_Case_Type;
            Keyword_Value = createParameters.Keyword_Value;
            Interval = createParameters.Interval;
            Timeout = createParameters.Timeout;
            Status = createParameters.Status;
            Http_Username = createParameters.Http_Username;
            Http_Password = createParameters.Http_Password;
            Http_Auth_Type = createParameters.Http_Auth_Type;
            Http_Method = createParameters.Http_Method;
            Post_Type = createParameters.Post_Type;
            Post_Value = createParameters.Post_Value;
            Post_Content_Type = createParameters.Post_Content_Type;
            Alert_Contacts = createParameters.Alert_Contacts;
            Custom_Http_Headers = createParameters.Custom_Http_Headers;
            Custom_Http_Statuses = createParameters.Custom_Http_Statuses;
            Ignore_Ssl_Errors = createParameters.Ignore_Ssl_Errors;
            Disable_Domain_Expire_Notifications = createParameters.Disable_Domain_Expire_Notifications;
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("friendly_name")]
        public string Friendly_Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("sub_type")]
        public string Sub_Type { get; set; }
        [JsonProperty("port")]
        public string Port { get; set; }
        [JsonProperty("keyword_type")]
        public int? Keyword_Type { get; set; }
        [JsonProperty("keyword_case_type")]
        public int? Keyword_Case_Type { get; set; }
        [JsonProperty("keyword_value")]
        public string Keyword_Value { get; set; }
        [JsonProperty("interval")]
        public int? Interval { get; set; }
        [JsonProperty("timeout")]
        public int? Timeout { get; set; }
        [JsonProperty("status")]
        public int? Status { get; set; }
        [JsonProperty("http_username")]
        public string Http_Username { get; set; }
        [JsonProperty("http_password")]
        public string Http_Password { get; set; }
        [JsonProperty("http_auth_type")]
        public string Http_Auth_Type { get; set; }
        [JsonProperty("http_method")]
        public string Http_Method { get; set; }
        [JsonProperty("post_type")]
        public string Post_Type { get; set; }
        [JsonProperty("post_value")]
        public string Post_Value { get; set; }
        [JsonProperty("post_content_type")]
        public string Post_Content_Type { get; set; }
        [JsonProperty("alert_contacts")]
        public List<string> Alert_Contacts { get; set; }
        [JsonProperty("mwindows")]
        public List<string> Maintenance_Windows { get; set; }
        [JsonProperty("custom_http_headers")]
        public Dictionary<string, string> Custom_Http_Headers { get; set; }
        [JsonProperty("custom_http_statuses")]
        public string Custom_Http_Statuses { get; set; }
        [JsonProperty("ignore_ssl_errors")]
        public bool? Ignore_Ssl_Errors { get; set; }
        [JsonProperty("disable_domain_expire_notifications")]
        public bool? Disable_Domain_Expire_Notifications { get; set; }
    }

    /// <summary>
    /// Parameters for creating a monitor
    ///
    /// https://uptimerobot.com/api/#newMonitorWrap
    /// </summary>
    public class MonitorCreateParameters : BaseModel
    {
        /// <summary>
        /// Friendly name of the monitor (REQUIRED)
        /// </summary>
        [JsonProperty("friendly_name")]
        public string Friendly_Name { get; set; }
        /// <summary>
        /// URL to monitor (REQUIRED)
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// Type of monitor (REQUIRED)
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("sub_type")]
        public string Sub_Type { get; set; }
        [JsonProperty("port")]
        public string Port { get; set; }
        [JsonProperty("keyword_type")]
        public int? Keyword_Type { get; set; }
        [JsonProperty("keyword_case_type")]
        public int? Keyword_Case_Type { get; set; }
        [JsonProperty("keyword_value")]
        public string Keyword_Value { get; set; }
        [JsonProperty("interval")]
        public int? Interval { get; set; }
        [JsonProperty("timeout")]
        public int? Timeout { get; set; }
        [JsonProperty("status")]
        public int? Status { get; set; }
        [JsonProperty("http_username")]
        public string Http_Username { get; set; }
        [JsonProperty("http_password")]
        public string Http_Password { get; set; }
        [JsonProperty("http_auth_type")]
        public string Http_Auth_Type { get; set; }
        [JsonProperty("http_method")]
        public string Http_Method { get; set; }
        [JsonProperty("post_type")]
        public string Post_Type { get; set; }
        [JsonProperty("post_value")]
        public string Post_Value { get; set; }
        [JsonProperty("post_content_type")]
        public string Post_Content_Type { get; set; }
        [JsonProperty("alert_contacts")]
        public List<string> Alert_Contacts { get; set; }
        [JsonProperty("custom_http_headers")]
        public Dictionary<string, string> Custom_Http_Headers { get; set; }
        [JsonProperty("custom_http_statuses")]
        public string Custom_Http_Statuses { get; set; }
        [JsonProperty("ignore_ssl_errors")]
        public bool? Ignore_Ssl_Errors { get; set; }
        [JsonProperty("disable_domain_expire_notifications")]
        public bool? Disable_Domain_Expire_Notifications { get; set; }
    }

    public class MonitorDeleteParameters : BaseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public class MonitorSearchParameters : BaseModel
    {
        // Pagination properties

        [JsonProperty("limit")]
        public int? Limit { get; set; }
        
        [JsonProperty("offset")]
        public int? Offset { get; set; }

        // Filter properties

        [JsonProperty("monitors")]
        public string Monitors { get; set; }

        [JsonProperty("search")]
        public string Search { get; set; }

        // Optional data includes

        //[JsonProperty("alert_contacts")]
        //public int Include_AlertContacts { get; set; }

        [JsonProperty("custom_http_headers")]
        public int Include_Custom_Http_Headers { get; set; }

        [JsonProperty("http_request_details")]
        public bool Include_Http_Request_Details { get; set; }

        //[JsonProperty("custom_http_statuses")]
        //public int Include_Custom_Http_Statuses { get; set; }

        //[JsonProperty("mwindows")]
        //public int Include_Maintenance_Windows { get; set; }
    }

    public class MonitorUpdateParameters : Monitor
    {
    }
}
