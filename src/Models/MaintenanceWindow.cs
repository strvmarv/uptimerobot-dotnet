using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Represents a maintenance window in UptimeRobot.
    /// </summary>
    public class MaintenanceWindow : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the maintenance window ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the maintenance window.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the type of maintenance window.
        /// </summary>
        [JsonPropertyName("type")]
        public MaintenanceWindowType Type { get; set; }

        /// <summary>
        /// Gets or sets the status of the maintenance window.
        /// </summary>
        [JsonPropertyName("status")]
        public MaintenanceWindowStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the start time (Unix timestamp).
        /// </summary>
        [JsonPropertyName("start_time")]
        public int StartTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in seconds.
        /// </summary>
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the value for weekly/monthly windows (e.g., "1" for Monday in weekly).
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}

