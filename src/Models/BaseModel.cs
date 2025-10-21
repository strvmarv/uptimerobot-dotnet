using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Base model for all UptimeRobot API request parameters.
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Gets or sets the API key for authentication.
        /// </summary>
        [JsonPropertyName("api_key")]
        public string? ApiKey { get; set; }

        // Cache for property reflection results to improve performance
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

        /// <summary>
        /// Converts the model properties to a dictionary for form-urlencoded requests.
        /// </summary>
        /// <returns>A dictionary containing the non-null property values.</returns>
        public Dictionary<string, object> GetContentForRequest()
        {
            var dict = new Dictionary<string, object>();
            var type = GetType();

            // Get cached properties or cache them if not present
            var properties = PropertyCache.GetOrAdd(type, t => t.GetProperties());

            foreach (var prop in properties)
            {
                var value = prop.GetValue(this);
                if (value != null)
                {
                    var propertyName = prop.Name;

                    // Get the JsonPropertyName attribute if it exists
                    var jsonAttr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                    if (jsonAttr != null)
                    {
                        propertyName = jsonAttr.Name;
                    }

                    // Special handling for complex types that need JSON serialization
                    if (propertyName.Equals("custom_http_headers", StringComparison.OrdinalIgnoreCase) ||
                        propertyName.Equals("custom_http_statuses", StringComparison.OrdinalIgnoreCase) ||
                        value is IDictionary<string, string> ||
                        (value is not string && value.GetType().IsClass && !value.GetType().IsPrimitive))
                    {
                        // Check if it's already a string to avoid double serialization
                        if (value is string stringValue)
                        {
                            dict.Add(propertyName, stringValue);
                        }
                        else
                        {
                            dict.Add(propertyName, JsonSerializer.Serialize(value));
                        }
                    }
                    else
                    {
                        dict.Add(propertyName, value);
                    }
                }
            }

            return dict;
        }
    }
}
