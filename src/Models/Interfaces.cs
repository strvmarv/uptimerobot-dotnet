using System.Collections.Generic;

namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Interface for models that can be converted to request content.
    /// </summary>
    public interface IContentModel
    {
        /// <summary>
        /// Converts the model to a dictionary suitable for form-urlencoded content.
        /// </summary>
        /// <returns>A dictionary of property names and values.</returns>
        Dictionary<string, object> GetContentForRequest();
    }
}
