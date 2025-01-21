using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace UptimeRobotDotnet
{
    public class UtrFormUrlEncodedContent : ByteArrayContent
    {
        public UtrFormUrlEncodedContent(IEnumerable<KeyValuePair<string, object>> nameValueCollection)
            : base(GetContentByteArray(nameValueCollection))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        }

        private static byte[] GetContentByteArray(IEnumerable<KeyValuePair<string, object>> nameValueCollection)
        {
            if (nameValueCollection == null) throw new ArgumentNullException(nameof(nameValueCollection));

            // Encode and concatenate data
            var builder = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in nameValueCollection)
            {
                if (builder.Length > 0)
                {
                    builder.Append('&');
                }

                builder.Append(Encode(pair.Key.ToLower()));
                builder.Append('=');

                if (pair.Value is string)
                {
                    builder.Append(Encode(pair.Value.ToString()));
                }
                else
                {
                    builder.Append(pair.Value);
                }
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }

        private static string Encode(string data)
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
