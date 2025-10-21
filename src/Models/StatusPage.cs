using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Represents a public status page in UptimeRobot.
    /// </summary>
    public class StatusPage : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the status page ID.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the status page.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the monitors included in this status page (comma-separated IDs).
        /// </summary>
        [JsonPropertyName("monitors")]
        public string? Monitors { get; set; }

        /// <summary>
        /// Gets or sets the status of the status page.
        /// </summary>
        [JsonPropertyName("status")]
        public StatusPageStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the sort order for monitors on the status page.
        /// </summary>
        [JsonPropertyName("sort")]
        public StatusPageSort Sort { get; set; }

        /// <summary>
        /// Gets or sets a custom message to display on the status page.
        /// </summary>
        [JsonPropertyName("custom_message")]
        public string? CustomMessage { get; set; }

        /// <summary>
        /// Gets or sets the standard URL of the status page.
        /// </summary>
        [JsonPropertyName("standard_url")]
        public string? StandardUrl { get; set; }

        /// <summary>
        /// Gets or sets the custom URL of the status page.
        /// </summary>
        [JsonPropertyName("custom_url")]
        public string? CustomUrl { get; set; }
    }
}

