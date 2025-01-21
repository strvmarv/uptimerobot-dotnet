using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Models
{
    public class BaseModel
    {
        [JsonPropertyName("api_key")]
        public string api_key { get; set; }


        public Dictionary<string, object> GetContentForRequest()
        {
            var dict = new Dictionary<string, object>();

            foreach (var prop in this.GetType().GetProperties())
            {
                var value = prop.GetValue(this);
                if (value != null)
                {
                    if (prop.Name.Equals("custom_http_headers", StringComparison.OrdinalIgnoreCase))
                    {
                        // Overrides
                        dict.Add("custom_http_headers", JsonSerializer.Serialize(value));
                    }
                    else
                    {
                        dict.Add(prop.Name, value);
                    }
                }
            }

            return dict;
        }
    }
}
