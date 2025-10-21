# UptimeRobot API Reference

This document provides a comprehensive reference to the UptimeRobot API and how this .NET client library maps to it.

## Official UptimeRobot API Documentation

**Official API Docs**: [https://uptimerobot.com/api/](https://uptimerobot.com/api/)

The UptimeRobot API is a RESTful API that uses HTTP POST requests with form-urlencoded data. All responses are in JSON format.

## Base URL

```
https://api.uptimerobot.com/v2/
```

## Authentication

UptimeRobot uses API keys for authentication. You can obtain your API key from your account settings.

### API Key Types

- **Main API Key**: Full access to all account features
- **Monitor-Specific API Keys**: Limited access to specific monitors

### Using API Keys in This Library

```csharp
var client = UptimeRobotClientFactory.Create("your-api-key-here");
```

## Rate Limiting

UptimeRobot enforces rate limits on API requests:
- Free accounts: Lower limits
- Paid accounts: Higher limits

When rate limited, the API returns HTTP 429. Consider implementing retry logic with exponential backoff.

## API Endpoints

### 1. Monitors API

Monitor endpoints allow you to create, read, update, and delete monitors.

#### 1.1 Get Monitors

**Endpoint**: `POST /v2/getMonitors`

**Library Method**: `GetMonitorsAsync()` or `GetAllMonitorsAsync()`

**Parameters**:
- `monitors` (optional): Comma-separated monitor IDs
- `types` (optional): Filter by monitor types
- `statuses` (optional): Filter by monitor statuses
- `custom_uptime_ratios` (optional): Custom uptime ratio periods
- `custom_uptime_ranges` (optional): Custom date ranges for uptime
- `all_time_uptime_ratio` (optional): Include all-time uptime ratio
- `logs` (optional): Include logs (1 = yes)
- `logs_start_date` (optional): Start date for logs
- `logs_end_date` (optional): End date for logs
- `logs_limit` (optional): Number of logs to return
- `response_times` (optional): Include response times
- `response_times_limit` (optional): Number of response times
- `response_times_average` (optional): Average interval for response times
- `response_times_start_date` (optional): Start date for response times
- `response_times_end_date` (optional): End date for response times
- `alert_contacts` (optional): Include alert contacts (1 = yes)
- `mwindows` (optional): Include maintenance windows
- `custom_http_headers` (optional): Include custom HTTP headers
- `custom_http_statuses` (optional): Include custom HTTP statuses
- `ssl` (optional): Include SSL certificate info
- `timezone` (optional): Timezone for dates
- `offset` (optional): Pagination offset (default: 0)
- `limit` (optional): Results per page (max: 50, default: 50)
- `search` (optional): Search term

**Example**:
```csharp
// Get all monitors with automatic pagination
await foreach (var monitor in client.GetAllMonitorsAsync())
{
    Console.WriteLine($"{monitor.FriendlyName}: {monitor.Status}");
}

// Manual pagination
var response = await client.GetMonitorsAsync(new MonitorSearchParameters
{
    Limit = 50,
    Offset = 0,
    Search = "example.com"
});
```

#### 1.2 Create Monitor

**Endpoint**: `POST /v2/newMonitor`

**Library Method**: `CreateMonitorAsync()`

**Required Parameters**:
- `friendly_name`: Display name for the monitor
- `url`: URL or IP to monitor
- `type`: Monitor type (1-7)

**Optional Parameters**:
- `sub_type`: Sub-type for port monitoring
- `port`: Port number for port monitoring
- `keyword_type`: Keyword search type (1 = exists, 2 = not exists)
- `keyword_case_type`: Case sensitivity (1 = case sensitive, 2 = case insensitive)
- `keyword_value`: Keyword to search for
- `interval`: Check interval in seconds (60, 300, 600, 900, 1800, 3600)
- `timeout`: Request timeout in seconds (30-120)
- `http_username`: HTTP authentication username
- `http_password`: HTTP authentication password
- `http_auth_type`: HTTP auth type (1 = Basic, 2 = Digest)
- `http_method`: HTTP method (1 = HEAD, 2 = GET, 3 = POST, 4 = PUT, 5 = PATCH, 6 = DELETE, 7 = OPTIONS)
- `post_type`: POST data type
- `post_value`: POST data
- `post_content_type`: Content-Type header for POST
- `alert_contacts`: Alert contact IDs
- `mwindows`: Maintenance window IDs
- `custom_http_headers`: JSON string of custom headers
- `custom_http_statuses`: Comma-separated status codes
- `ignore_ssl_errors`: Ignore SSL errors (1 = yes)

**Example**:
```csharp
var parameters = new MonitorCreateParameters
{
    FriendlyName = "My Website",
    Url = "https://example.com",
    Type = MonitorType.HTTP,
    Interval = 300,
    CustomHttpHeaders = new Dictionary<string, string>
    {
        { "User-Agent", "MyMonitor/1.0" }
    }
};

var result = await client.CreateMonitorAsync(parameters);
Console.WriteLine($"Created monitor with ID: {result.Monitor.Id}");
```

#### 1.3 Update Monitor

**Endpoint**: `POST /v2/editMonitor`

**Library Method**: `UpdateMonitorAsync()`

**Required Parameters**:
- `id`: Monitor ID

**Optional Parameters**: Same as Create Monitor (all optional)

**Example**:
```csharp
var parameters = new MonitorUpdateParameters
{
    Id = 12345678,
    Interval = 600,
    Status = MonitorStatus.Paused
};

await client.UpdateMonitorAsync(parameters);
```

#### 1.4 Delete Monitor

**Endpoint**: `POST /v2/deleteMonitor`

**Library Method**: `DeleteMonitorAsync()`

**Required Parameters**:
- `id`: Monitor ID

**Example**:
```csharp
var parameters = new MonitorDeleteParameters { Id = 12345678 };
await client.DeleteMonitorAsync(parameters);
```

#### 1.5 Reset Monitor

**Endpoint**: `POST /v2/resetMonitor`

**Library Method**: `ResetMonitorAsync()`

**Required Parameters**:
- `id`: Monitor ID

**Example**:
```csharp
var parameters = new MonitorResetParameters { Id = 12345678 };
await client.ResetMonitorAsync(parameters);
```

### 2. Alert Contacts API

Alert contacts are the notification endpoints (email, SMS, webhook, etc.) that receive alerts.

#### 2.1 Get Alert Contacts

**Endpoint**: `POST /v2/getAlertContacts`

**Library Method**: `GetAlertContactsAsync()` or `GetAllAlertContactsAsync()`

**Parameters**:
- `alert_contacts` (optional): Comma-separated alert contact IDs
- `offset` (optional): Pagination offset
- `limit` (optional): Results per page (max: 50)

**Example**:
```csharp
await foreach (var contact in client.GetAllAlertContactsAsync())
{
    Console.WriteLine($"{contact.FriendlyName} ({contact.Type}): {contact.Value}");
}
```

#### 2.2 Create Alert Contact

**Endpoint**: `POST /v2/newAlertContact`

**Library Method**: `CreateAlertContactAsync()`

**Required Parameters**:
- `type`: Alert contact type (1-13)
- `value`: Contact value (email, phone, URL, etc.)
- `friendly_name`: Display name

**Example**:
```csharp
var parameters = new AlertContactCreateParameters
{
    Type = AlertContactType.Email,
    FriendlyName = "Admin Email",
    Value = "admin@example.com"
};

await client.CreateAlertContactAsync(parameters);
```

#### 2.3 Update Alert Contact

**Endpoint**: `POST /v2/editAlertContact`

**Library Method**: `UpdateAlertContactAsync()`

#### 2.4 Delete Alert Contact

**Endpoint**: `POST /v2/deleteAlertContact`

**Library Method**: `DeleteAlertContactAsync()`

### 3. Maintenance Windows API

Maintenance windows allow you to pause monitoring during scheduled maintenance.

#### 3.1 Get Maintenance Windows

**Endpoint**: `POST /v2/getMWindows`

**Library Method**: `GetMaintenanceWindowsAsync()` or `GetAllMaintenanceWindowsAsync()`

**Example**:
```csharp
await foreach (var window in client.GetAllMaintenanceWindowsAsync())
{
    Console.WriteLine($"{window.FriendlyName}: {window.Type}");
}
```

#### 3.2 Create Maintenance Window

**Endpoint**: `POST /v2/newMWindow`

**Library Method**: `CreateMaintenanceWindowAsync()`

**Required Parameters**:
- `friendly_name`: Display name
- `type`: Window type (1 = once, 2 = daily, 3 = weekly, 4 = monthly)
- `start_time`: Unix timestamp of start time
- `duration`: Duration in seconds

**Example**:
```csharp
var parameters = new MaintenanceWindowCreateParameters
{
    FriendlyName = "Weekly Maintenance",
    Type = MaintenanceWindowType.Weekly,
    StartTime = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds(),
    Duration = 3600 // 1 hour
};

await client.CreateMaintenanceWindowAsync(parameters);
```

#### 3.3 Update Maintenance Window

**Endpoint**: `POST /v2/editMWindow`

**Library Method**: `UpdateMaintenanceWindowAsync()`

#### 3.4 Delete Maintenance Window

**Endpoint**: `POST /v2/deleteMWindow`

**Library Method**: `DeleteMaintenanceWindowAsync()`

### 4. Status Pages API (Public Status Pages)

Public status pages display the status of your monitors publicly.

#### 4.1 Get Status Pages

**Endpoint**: `POST /v2/getPSPs`

**Library Method**: `GetStatusPagesAsync()` or `GetAllStatusPagesAsync()`

**Example**:
```csharp
await foreach (var page in client.GetAllStatusPagesAsync())
{
    Console.WriteLine($"{page.FriendlyName}: {page.StandardUrl}");
}
```

#### 4.2 Create Status Page

**Endpoint**: `POST /v2/newPSP`

**Library Method**: `CreateStatusPageAsync()`

**Required Parameters**:
- `friendly_name`: Display name
- `monitors`: Comma-separated monitor IDs

**Example**:
```csharp
var parameters = new StatusPageCreateParameters
{
    FriendlyName = "Public Status",
    Monitors = "123456-789012",
    CustomMessage = "Service status",
    Sort = StatusPageSort.FriendlyNameAZ
};

await client.CreateStatusPageAsync(parameters);
```

#### 4.3 Update Status Page

**Endpoint**: `POST /v2/editPSP`

**Library Method**: `UpdateStatusPageAsync()`

#### 4.4 Delete Status Page

**Endpoint**: `POST /v2/deletePSP`

**Library Method**: `DeleteStatusPageAsync()`

## Monitor Types

| Type | Value | Description |
|------|-------|-------------|
| HTTP(s) | 1 | Monitor HTTP/HTTPS endpoints |
| Keyword | 2 | Check for presence/absence of keyword |
| Ping | 3 | ICMP ping monitoring |
| Port | 4 | TCP port monitoring |
| Heartbeat | 5 | Heartbeat/push monitoring |

## Monitor Statuses

| Status | Value | Description |
|--------|-------|-------------|
| Paused | 0 | Monitoring is paused |
| Not Checked Yet | 1 | New monitor, not yet checked |
| Up | 2 | Monitor is up and responding |
| Seems Down | 8 | Monitor appears down (temporary) |
| Down | 9 | Monitor is confirmed down |

## Alert Contact Types

| Type | Value | Description |
|------|-------|-------------|
| SMS | 1 | SMS notification |
| Email | 2 | Email notification |
| Twitter DM | 3 | Twitter direct message |
| Boxcar | 4 | Boxcar notification |
| Webhook | 5 | Custom webhook |
| Pushbullet | 6 | Pushbullet notification |
| Zapier | 7 | Zapier webhook |
| Pushover | 9 | Pushover notification |
| HipChat | 10 | HipChat notification |
| Slack | 11 | Slack webhook |
| Telegram | 13 | Telegram notification |

## Response Formats

All API responses follow this structure:

```json
{
  "stat": "ok" | "fail",
  "pagination": {
    "offset": 0,
    "limit": 50,
    "total": 100
  },
  "monitors": [...],
  "error": {
    "type": "error_type",
    "message": "error message"
  }
}
```

### Success Response

```json
{
  "stat": "ok",
  "pagination": {
    "offset": 0,
    "limit": 50,
    "total": 3
  },
  "monitors": [
    {
      "id": 777749809,
      "friendly_name": "Google",
      "url": "https://www.google.com",
      "type": 1,
      "sub_type": "",
      "keyword_type": "",
      "keyword_value": "",
      "http_username": "",
      "http_password": "",
      "port": "",
      "interval": 300,
      "status": 2,
      "create_datetime": 1468410324
    }
  ]
}
```

### Error Response

```json
{
  "stat": "fail",
  "error": {
    "type": "invalid_parameter",
    "parameter_name": "id",
    "passed_value": "abc",
    "message": "The id parameter is required and must be an integer."
  }
}
```

## Error Handling in This Library

The library provides custom exception types for different error scenarios:

```csharp
try
{
    await client.CreateMonitorAsync(parameters);
}
catch (UptimeRobotValidationException ex)
{
    // Client-side validation failed
    Console.WriteLine($"Validation error: {ex.Message}");
    foreach (var result in ex.ValidationResults)
    {
        Console.WriteLine($"- {result.ErrorMessage}");
    }
}
catch (UptimeRobotApiException ex)
{
    // API returned an error
    Console.WriteLine($"API error: {ex.Message}");
    Console.WriteLine($"Error type: {ex.ErrorResponse?.Error?.Type}");
}
catch (UptimeRobotException ex)
{
    // Other client errors (HTTP, network, etc.)
    Console.WriteLine($"Request failed: {ex.Message}");
}
```

## Pagination

The UptimeRobot API limits results to 50 items per request. This library provides two ways to handle pagination:

### Automatic Pagination

```csharp
// Automatically fetches all results
await foreach (var monitor in client.GetAllMonitorsAsync())
{
    // Process each monitor
}
```

### Manual Pagination

```csharp
var offset = 0;
var limit = 50;
var total = 0;

do
{
    var response = await client.GetMonitorsAsync(new MonitorSearchParameters
    {
        Offset = offset,
        Limit = limit
    });

    foreach (var monitor in response.Monitors)
    {
        // Process monitor
    }

    total = response.Pagination.Total;
    offset += limit;
}
while (offset < total);
```

## Best Practices

1. **Use Automatic Pagination**: Leverage `GetAllXxxAsync()` methods for simplicity
2. **Handle Cancellation**: Pass `CancellationToken` to all async methods
3. **Implement Retry Logic**: Handle rate limiting with exponential backoff
4. **Cache API Keys**: Don't hardcode API keys; use configuration or environment variables
5. **Log API Calls**: Enable logging for debugging and monitoring
6. **Validate Parameters**: Use strongly-typed enums and validation attributes
7. **Monitor Rate Limits**: Track API usage to avoid hitting limits

## Additional Resources

- [Official UptimeRobot API Documentation](https://uptimerobot.com/api/)
- [UptimeRobot Help Center](https://uptimerobot.com/help/)
- [UptimeRobot Status Page](https://status.uptimerobot.com/)
- [Library GitHub Repository](https://github.com/strvmarv/uptimerobot-dotnet)

