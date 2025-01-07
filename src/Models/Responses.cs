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
        public UrtResponseActionMonitor Monitor { get; set; }
        [JsonPropertyName("monitors")]
        public List<UrtResponseMonitor> Monitors { get; set; }
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

    public class UrtResponseActionMonitor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }

    public class UrtResponseMonitor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("friendly_name")]
        public string Friendly_Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("type")]
        public int? Type { get; set; }
        [JsonPropertyName("sub_type")]
        public string Sub_Type { get; set; }
        [JsonPropertyName("keyword_type")]
        public int? Keyword_Type { get; set; }
        [JsonPropertyName("keyword_case_type")]
        public int? Keyword_Case_Type { get; set; }
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
        public int? Status { get; set; }
        [JsonPropertyName("create_datetime")]
        public int? Create_DateTime { get; set; }

        [JsonPropertyName("custom_http_headers")]
        public Dictionary<string, string> Custom_Http_Headers { get; set; }
    }
}
