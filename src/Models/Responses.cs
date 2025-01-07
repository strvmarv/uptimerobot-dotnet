using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    public class UtrResponse
    {
        [JsonPropertyName("stat")]
        public string Stat { get; set; }
        [JsonPropertyName("monitor")]
        public UrtResponseMonitor Monitor { get; set; }
    }

    public class UrtResponseMonitor
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }
}
