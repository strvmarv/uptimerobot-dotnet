using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    public class UtrResponse
    {
        [JsonPropertyName("stat")]
        public string Stat { get; set; }
        [JsonPropertyName("pagination")]
        public UtrPagination Pagination { get; set; }
        [JsonPropertyName("error")]
        public object Error { get; set; }
        [JsonPropertyName("monitor")]
        public UtrResponseActionMonitor Monitor { get; set; }
        [JsonPropertyName("monitors")]
        public List<UtrResponseMonitor> Monitors { get; set; }
    }

    public class UtrPagination
    {
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
        [JsonPropertyName("limit")]
        public int Limit { get; set; }
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class UtrResponseActionMonitor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }

    public class UtrResponseMonitor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("friendly_name")]
        public string Friendly_Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("type")]
        public MonitorType? Type { get; set; }
        [JsonPropertyName("sub_type")]
        public string Sub_Type { get; set; }
        [JsonPropertyName("keyword_type")]
        public KeywordType? Keyword_Type { get; set; }
        [JsonPropertyName("keyword_case_type")]
        public KeywordCaseType? Keyword_Case_Type { get; set; }
        [JsonPropertyName("keyword_value")]
        public string Keyword_Value { get; set; }
        [JsonPropertyName("http_username")]
        public string Http_Username { get; set; }
        [JsonPropertyName("http_password")]
        public string Http_Password { get; set; }
        [JsonPropertyName("port")]
        public string Port { get; set; }
        [JsonPropertyName("interval")]
        public int? Interval { get; set; }
        [JsonPropertyName("status")]
        public Status? Status { get; set; }
        [JsonPropertyName("create_datetime")]
        public int? Create_DateTime { get; set; }

        // Optional data includes

        //[JsonPropertyName("alert_contacts")]
        //public List<object> AlertContacts { get; set; }

        [JsonPropertyName("custom_http_headers")]
        public Dictionary<string, string> Custom_Http_Headers { get; set; }

        //[JsonPropertyName("custom_http_statuses")]
        //public Dictionary<string, object> Custom_Http_Statuses { get; set; }

        [JsonPropertyName("http_method")]
        public HttpMethod Http_Method { get; set; }

        //[JsonPropertyName("mwindows")]
        //public Dictionary<string, object> Maintenance_Windows { get; set; }
	
        //[JsonPropertyName("ssl")]
        //public object Ssl { get; set; }
    }
}
