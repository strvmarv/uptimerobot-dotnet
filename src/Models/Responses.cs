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
        public List<Monitor> Monitors { get; set; }
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
}
