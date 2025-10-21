using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace UptimeRobotDotnet
{
    /// <summary>
    /// Custom form-urlencoded content handler for UptimeRobot API requests.
    /// </summary>
    public class UtrFormUrlEncodedContent : ByteArrayContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UtrFormUrlEncodedContent"/> class.
        /// </summary>
        /// <param name="nameValueCollection">The collection of key-value pairs to encode.</param>
        /// <exception cref="ArgumentNullException">Thrown when nameValueCollection is null.</exception>
        public UtrFormUrlEncodedContent(IEnumerable<KeyValuePair<string, object>> nameValueCollection)
            : base(GetContentByteArray(nameValueCollection))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Converts the collection to a byte array suitable for HTTP content.
        /// </summary>
        /// <param name="nameValueCollection">The collection to convert.</param>
        /// <returns>A byte array containing the encoded content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when nameValueCollection is null.</exception>
        private static byte[] GetContentByteArray(IEnumerable<KeyValuePair<string, object>> nameValueCollection)
        {
            if (nameValueCollection == null)
                throw new ArgumentNullException(nameof(nameValueCollection));

            // Encode and concatenate data
            var builder = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in nameValueCollection)
            {
                if (builder.Length > 0)
                {
                    builder.Append('&');
                }

                builder.Append(Encode(pair.Key.ToLowerInvariant()));
                builder.Append('=');

                if (pair.Value is string stringValue)
                {
                    builder.Append(Encode(stringValue));
                }
                else
                {
                    builder.Append(pair.Value);
                }
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }

        /// <summary>
        /// URL-encodes a string.
        /// </summary>
        /// <param name="data">The string to encode.</param>
        /// <returns>The encoded string, or empty string if input is null or empty.</returns>
        private static string Encode(string? data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            // Escape spaces as '+'.
            return Uri.EscapeDataString(data).Replace("%20", "+");
        }
    }
}
