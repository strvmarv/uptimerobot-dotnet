using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    public class BaseModel
    {
        [JsonPropertyName("api_key")]
        public string Api_Key { get; set; }
    }
}
