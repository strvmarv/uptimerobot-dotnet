using System.Collections.Generic;
using Newtonsoft.Json;

namespace UptimeRobotDotnet.Models
{
    public class UtrResponse
    {
        [JsonProperty("stat")]
        public string Stat { get; set; }
        [JsonProperty("pagination")]
        public UtrPagination Pagination { get; set; }
        [JsonProperty("error")]
        public object Error { get; set; }
        [JsonProperty("monitor")]
        public UtrResponseActionMonitor Monitor { get; set; }
        [JsonProperty("monitors")]
        public List<UtrResponseMonitor> Monitors { get; set; }
    }

    public class UtrPagination
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class UtrResponseActionMonitor
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("status")]
        public int? Status { get; set; }
    }

    public class UtrResponseMonitor
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("friendly_name")]
        public string Friendly_Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("type")]
        public int? Type { get; set; }
        [JsonProperty("sub_type")]
        public string Sub_Type { get; set; }
        [JsonProperty("keyword_type")]
        public int? Keyword_Type { get; set; }
        [JsonProperty("keyword_case_type")]
        public int? Keyword_Case_Type { get; set; }
        [JsonProperty("keyword_value")]
        public string Keyword_Value { get; set; }
        [JsonProperty("http_username")]
        public string Http_Username { get; set; }
        [JsonProperty("http_password")]
        public string Http_Password { get; set; }
        [JsonProperty("port")]
        public string Port { get; set; }
        [JsonProperty("interval")]
        public int? Interval { get; set; }
        [JsonProperty("status")]
        public int? Status { get; set; }
        [JsonProperty("create_datetime")]
        public int? Create_DateTime { get; set; }

        // Optional data includes

        //[JsonProperty("alert_contacts")]
        //public List<object> AlertContacts { get; set; }

        [JsonProperty("custom_http_headers")]
        public Dictionary<string, string> Custom_Http_Headers { get; set; }

        //[JsonProperty("custom_http_statuses")]
        //public Dictionary<string, object> Custom_Http_Statuses { get; set; }

        [JsonProperty("http_method")]
        public int Http_Method { get; set; }

        //[JsonProperty("mwindows")]
        //public Dictionary<string, object> Maintenance_Windows { get; set; }
	
        //[JsonProperty("ssl")]
        //public object Ssl { get; set; }
    }
}
