namespace UptimeRobotDotnet.Models
{
    /// <summary>
    /// Types of monitors supported by UptimeRobot.
    /// </summary>
    public enum MonitorType : int
    {
        /// <summary>
        /// HTTP(s) monitor.
        /// </summary>
        HTTP = 1,

        /// <summary>
        /// Keyword monitor (checks for keyword presence in response).
        /// </summary>
        Keyword = 2,

        /// <summary>
        /// Ping monitor.
        /// </summary>
        Ping = 3,

        /// <summary>
        /// Port monitor.
        /// </summary>
        Port = 4,

        /// <summary>
        /// Heartbeat monitor.
        /// </summary>
        Heartbeat = 5
    }

    /// <summary>
    /// Sub-types for port monitors.
    /// </summary>
    public enum MonitorSubType : int
    {
        /// <summary>
        /// HTTP port (80).
        /// </summary>
        HTTP = 1,

        /// <summary>
        /// HTTPS port (443).
        /// </summary>
        HTTPS = 2,

        /// <summary>
        /// FTP port (21).
        /// </summary>
        FTP = 3,

        /// <summary>
        /// SMTP port (25).
        /// </summary>
        SMTP = 4,

        /// <summary>
        /// POP3 port (110).
        /// </summary>
        POP3 = 5,

        /// <summary>
        /// IMAP port (143).
        /// </summary>
        IMAP = 6,

        /// <summary>
        /// Custom port.
        /// </summary>
        Custom = 99
    }

    /// <summary>
    /// Keyword search type for keyword monitors.
    /// </summary>
    public enum KeywordType : int
    {
        /// <summary>
        /// Keyword should exist in the response.
        /// </summary>
        Exists = 1,

        /// <summary>
        /// Keyword should not exist in the response.
        /// </summary>
        NotExists = 2
    }

    /// <summary>
    /// Case sensitivity for keyword searches.
    /// </summary>
    public enum KeywordCaseType : int
    {
        /// <summary>
        /// Case-sensitive keyword search.
        /// </summary>
        Sensitive = 0,

        /// <summary>
        /// Case-insensitive keyword search.
        /// </summary>
        Insensitive = 1
    }

    /// <summary>
    /// HTTP authentication types.
    /// </summary>
    public enum HttpAuthType : int
    {
        /// <summary>
        /// HTTP Basic authentication.
        /// </summary>
        Basic = 1,

        /// <summary>
        /// HTTP Digest authentication.
        /// </summary>
        Digest = 2
    }

    /// <summary>
    /// Monitor status values.
    /// </summary>
    public enum MonitorStatus : int
    {
        /// <summary>
        /// Monitor is paused.
        /// </summary>
        Paused = 0,

        /// <summary>
        /// Monitor has not been checked yet.
        /// </summary>
        NotCheckedYet = 1,

        /// <summary>
        /// Monitor is up and running.
        /// </summary>
        Up = 2,

        /// <summary>
        /// Monitor seems to be down (temporary issue).
        /// </summary>
        SeemsDown = 8,

        /// <summary>
        /// Monitor is down.
        /// </summary>
        Down = 9
    }

    /// <summary>
    /// HTTP methods for monitor requests.
    /// </summary>
    public enum HttpMethod : int
    {
        /// <summary>
        /// HTTP HEAD method.
        /// </summary>
        HEAD = 1,

        /// <summary>
        /// HTTP GET method.
        /// </summary>
        GET = 2,

        /// <summary>
        /// HTTP POST method.
        /// </summary>
        POST = 3,

        /// <summary>
        /// HTTP PUT method.
        /// </summary>
        PUT = 4,

        /// <summary>
        /// HTTP PATCH method.
        /// </summary>
        PATCH = 5,

        /// <summary>
        /// HTTP DELETE method.
        /// </summary>
        DELETE = 6,

        /// <summary>
        /// HTTP OPTIONS method.
        /// </summary>
        OPTIONS = 7
    }

    /// <summary>
    /// POST data format types.
    /// </summary>
    public enum PostType : int
    {
        /// <summary>
        /// Key-value pairs format.
        /// </summary>
        KeyValue = 0,

        /// <summary>
        /// Raw format.
        /// </summary>
        Raw = 1
    }

    /// <summary>
    /// POST content type for monitor requests.
    /// </summary>
    public enum PostContentType : int
    {
        /// <summary>
        /// text/html content type.
        /// </summary>
        TextHtml = 0,

        /// <summary>
        /// application/json content type.
        /// </summary>
        ApplicationJson = 1
    }

    /// <summary>
    /// Alert contact types.
    /// </summary>
    public enum AlertContactType : int
    {
        /// <summary>
        /// SMS alert.
        /// </summary>
        SMS = 1,

        /// <summary>
        /// Email alert.
        /// </summary>
        Email = 2,

        /// <summary>
        /// Twitter direct message alert.
        /// </summary>
        TwitterDM = 3,

        /// <summary>
        /// Boxcar alert.
        /// </summary>
        Boxcar = 4,

        /// <summary>
        /// Webhook alert.
        /// </summary>
        Webhook = 5,

        /// <summary>
        /// Pushbullet alert.
        /// </summary>
        Pushbullet = 6,

        /// <summary>
        /// Zapier webhook alert.
        /// </summary>
        Zapier = 7,

        /// <summary>
        /// Pushover alert.
        /// </summary>
        Pushover = 9,

        /// <summary>
        /// HipChat alert.
        /// </summary>
        HipChat = 10,

        /// <summary>
        /// Slack alert.
        /// </summary>
        Slack = 11
    }

    /// <summary>
    /// Alert contact status values.
    /// </summary>
    public enum AlertContactStatus : int
    {
        /// <summary>
        /// Alert contact is not activated.
        /// </summary>
        NotActivated = 0,

        /// <summary>
        /// Alert contact is paused.
        /// </summary>
        Paused = 1,

        /// <summary>
        /// Alert contact is active.
        /// </summary>
        Active = 2
    }

    /// <summary>
    /// Maintenance window types.
    /// </summary>
    public enum MaintenanceWindowType : int
    {
        /// <summary>
        /// One-time maintenance window.
        /// </summary>
        Once = 1,

        /// <summary>
        /// Daily maintenance window.
        /// </summary>
        Daily = 2,

        /// <summary>
        /// Weekly maintenance window.
        /// </summary>
        Weekly = 3,

        /// <summary>
        /// Monthly maintenance window.
        /// </summary>
        Monthly = 4
    }

    /// <summary>
    /// Maintenance window status values.
    /// </summary>
    public enum MaintenanceWindowStatus : int
    {
        /// <summary>
        /// Maintenance window is paused.
        /// </summary>
        Paused = 0,

        /// <summary>
        /// Maintenance window is active.
        /// </summary>
        Active = 1
    }

    /// <summary>
    /// Status page status values.
    /// </summary>
    public enum StatusPageStatus : int
    {
        /// <summary>
        /// Status page is paused.
        /// </summary>
        Paused = 0,

        /// <summary>
        /// Status page is active.
        /// </summary>
        Active = 1
    }

    /// <summary>
    /// Status page sort order.
    /// </summary>
    public enum StatusPageSort : int
    {
        /// <summary>
        /// Sort by friendly name (A-Z).
        /// </summary>
        FriendlyNameAZ = 1,

        /// <summary>
        /// Sort by friendly name (Z-A).
        /// </summary>
        FriendlyNameZA = 2,

        /// <summary>
        /// Sort by status (up-down-paused).
        /// </summary>
        StatusUpDownPaused = 3,

        /// <summary>
        /// Sort by status (down-up-paused).
        /// </summary>
        StatusDownUpPaused = 4
    }
}
