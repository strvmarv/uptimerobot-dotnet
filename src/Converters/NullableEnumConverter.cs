using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UptimeRobotDotnet.Converters
{
    /// <summary>
    /// JSON converter for nullable enums that handles empty strings and unknown values gracefully.
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    public class NullableEnumConverter<T> : JsonConverter<T?> where T : struct, Enum
    {
        /// <summary>
        /// Reads and converts the JSON to a nullable enum value.
        /// </summary>
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                
                // Handle empty strings as null
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return null;
                }

                // Try to parse as enum
                if (Enum.TryParse<T>(stringValue, ignoreCase: true, out var enumValue))
                {
                    return enumValue;
                }

                // Unknown value - return null instead of throwing
                return null;
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                var intValue = reader.GetInt32();
                
                // Check if the integer value is defined in the enum
                if (Enum.IsDefined(typeof(T), intValue))
                {
                    return (T)(object)intValue;
                }

                // Unknown value - return null instead of throwing
                return null;
            }

            // Unexpected token type - return null
            return null;
        }

        /// <summary>
        /// Writes a nullable enum value as JSON.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                // Write as integer
                writer.WriteNumberValue(Convert.ToInt32(value.Value));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}

