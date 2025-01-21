using System.Collections.Generic;

namespace UptimeRobotDotnet.Models
{
    public interface IContentModel
    {
        Dictionary<string, object> GetContentForRequest();
    }
}