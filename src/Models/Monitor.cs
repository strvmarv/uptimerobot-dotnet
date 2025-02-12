﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    public class Monitor : BaseModel, IContentModel
    {
        public Monitor() { }

        public Monitor(MonitorCreateParameters createParameters)
        {
            Friendly_Name = createParameters.Friendly_Name;
            Url = createParameters.Url;
            Type = createParameters.Type;
            Sub_Type = createParameters.Sub_Type?.ToString();
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

        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("friendly_name")]
        public string Friendly_Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }
        [JsonPropertyName("sub_type")]
        public string Sub_Type { get; set; }
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
        public object Post_Value { get; set; }
        [JsonPropertyName("post_content_type")]
        public PostContentType? Post_Content_Type { get; set; }
        [JsonPropertyName("alert_contacts")]
        public object Alert_Contacts { get; set; }
        [JsonPropertyName("mwindows")]
        public List<string> Mwindows { get; set; }
        [JsonPropertyName("custom_http_headers")]
        public Dictionary<string, string> Custom_Http_Headers { get; set; }
        [JsonPropertyName("custom_http_statuses")]
        public object Custom_Http_Statuses { get; set; }
        [JsonPropertyName("ignore_ssl_errors")]
        public bool? Ignore_Ssl_Errors { get; set; }
        [JsonPropertyName("disable_domain_expire_notifications")]
        public bool? Disable_Domain_Expire_Notifications { get; set; }
    }
}
