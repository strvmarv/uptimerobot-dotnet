namespace UptimeRobotDotnet.Models
{
    public enum MonitorType : int
    {
        HTTP = 1,
        Keyword = 2,
        Ping = 3,
        Port = 4
    };

    public enum MonitorSubType : int
    {
        HTTP = 1,
        HTTPS = 2,
        FTP = 3,
        SMTP = 4,
        POP3 = 5,
        IMAP = 6,
        Custom = 99
    };

    public enum KeywordType : int
    {
        Exists = 1,
        NotExists = 2
    };

    public enum KeywordCaseType : int
    {
        Sensitive = 0,
        Insensitive = 1
    };

    public enum HttpAuthType : int
    {
        Basic = 1,
        Digest = 2
    };

    public enum Status : int
    {
        Paused = 0,
        NotCheckedYet = 1,
        Up = 2,
        SeemsDown = 8,
        Down = 9
    }

    public enum HttpMethod : int
    {
        HEAD = 1,
        GET = 2,
        POST = 3,
        PUT = 4,
        PATCH = 5,
        DELETE = 6,
        OPTIONS = 7
    };

    public enum PostType : int
    {
        KeyValue = 0,
        Raw = 1
    };

    public enum PostContentType : int
    {
        TextHtml = 0,
        ApplicationJson = 1,
    }
}