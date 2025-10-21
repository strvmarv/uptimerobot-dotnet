using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Represents an alert contact in UptimeRobot.
    /// </summary>
    public class AlertContact : BaseModel, IContentModel
    {
        /// <summary>
        /// Gets or sets the alert contact ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the friendly name of the alert contact.
        /// </summary>
        [JsonPropertyName("friendly_name")]
        public string? FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the type of alert contact.
        /// </summary>
        [JsonPropertyName("type")]
        public AlertContactType Type { get; set; }

        /// <summary>
        /// Gets or sets the status of the alert contact.
        /// </summary>
        [JsonPropertyName("status")]
        public AlertContactStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the value of the alert contact (email, phone, webhook URL, etc.).
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}

