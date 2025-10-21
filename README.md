[![build and test](https://github.com/strvmarv/uptimerobot-dotnet/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/strvmarv/uptimerobot-dotnet/actions/workflows/build-and-test.yml)

# UptimeRobot .NET Client

> ⚠️ **v2.0.0 Release Candidate** - This is a pre-release version with breaking changes from v1.x. Please test thoroughly before using in production. [See migration guide](#breaking-changes-from-v1x).

A modern, fully-featured [UptimeRobot](https://uptimerobot.com) .NET client library with complete API coverage, automatic pagination, and strongly-typed models.

**NuGet Package:** [UptimeRobotDotnet](https://www.nuget.org/packages/UptimeRobotDotnet)

**API Documentation:** [UptimeRobot API](https://uptimerobot.com/api/)

## Features

- ✅ **Complete API Coverage** - Monitors, Alert Contacts, Maintenance Windows, and Status Pages
- ✅ **Automatic Pagination** - `IAsyncEnumerable` support for seamless iteration over large result sets
- ✅ **Strongly-Typed Models** - Enums and validation attributes for type safety
- ✅ **Nullable Reference Types** - Enhanced null-safety throughout
- ✅ **Cancellation Token Support** - Proper async/await patterns with cancellation
- ✅ **Custom Exception Types** - Detailed error handling with `UptimeRobotApiException`
- ✅ **Logging Support** - Optional `ILogger` integration for diagnostics
- ✅ **Modern .NET** - Supports .NET 9.0, .NET 8.0, .NET 6.0, and .NET Standard 2.0
- ✅ **Comprehensive Documentation** - XML docs on all public APIs

## Installation

### Installing the Release Candidate

To test the v2.0.0 release candidate:

```bash
dotnet add package UptimeRobotDotnet --version 2.0.0-rc.1
```

Or via Package Manager Console:

```powershell
Install-Package UptimeRobotDotnet -Version 2.0.0-rc.1
```

### Installing Stable Version (v1.x)

For the stable v1.x version:

```bash
dotnet add package UptimeRobotDotnet
```

> 💡 **Tip**: Once v2.0.0 is released as stable, the above command will install v2.0.0 by default.

## Quick Start

```csharp
using UptimeRobotDotnet;
using UptimeRobotDotnet.Models;

// Create client
var client = UptimeRobotClientFactory.Create("your-api-key-here");

// Get all monitors with automatic pagination
await foreach (var monitor in client.GetAllMonitorsAsync())
{
    Console.WriteLine($"{monitor.FriendlyName}: {monitor.Status}");
}

// Create a new monitor
var parameters = new MonitorCreateParameters
{
    FriendlyName = "My Website",
    Type = MonitorType.HTTP,
    Url = "https://example.com",
    Interval = 300 // Check every 5 minutes
};

var result = await client.CreateMonitorAsync(parameters);
Console.WriteLine($"Monitor created with ID: {result.Monitor.Id}");
```

## API Coverage

| Entity | Status |
|--------|--------|
| **Monitors** | ✅ Full CRUD + Pagination |
| **Alert Contacts** | ✅ Full CRUD + Pagination |
| **Maintenance Windows** | ✅ Full CRUD + Pagination |
| **Status Pages** | ✅ Full CRUD + Pagination |

## Usage Examples

### Monitors

#### Get All Monitors (with automatic pagination)

```csharp
var client = UptimeRobotClientFactory.Create("your-api-key");

// Automatically handles pagination - fetches all monitors
await foreach (var monitor in client.GetAllMonitorsAsync())
{
    Console.WriteLine($"{monitor.FriendlyName}: {monitor.Url}");
}
```

#### Get Monitors (manual pagination)

```csharp
var parameters = new MonitorSearchParameters
{
    Limit = 50,
    Offset = 0,
    Search = "example.com"
};

var response = await client.GetMonitorsAsync(parameters);
Console.WriteLine($"Found {response.Monitors.Count} monitors");
Console.WriteLine($"Total available: {response.Pagination.Total}");
```

#### Get Specific Monitor by ID

```csharp
var parameters = new MonitorSearchParameters
{
    Monitors = "123456789" // Monitor ID
};

var response = await client.GetMonitorAsync(parameters);
var monitor = response.Monitors.FirstOrDefault();
```

#### Create a Monitor

```csharp
var parameters = new MonitorCreateParameters
{
    FriendlyName = "My API",
    Type = MonitorType.HTTP,
    Url = "https://api.example.com/health",
    Interval = 300, // Check every 5 minutes
    HttpMethod = HttpMethod.GET,
    CustomHttpHeaders = new Dictionary<string, string>
    {
        { "Authorization", "Bearer token" },
        { "User-Agent", "UptimeRobot-Monitor" }
    }
};

var result = await client.CreateMonitorAsync(parameters);
Console.WriteLine($"Created monitor with ID: {result.Monitor.Id}");
```

#### Update a Monitor

```csharp
var parameters = new MonitorUpdateParameters
{
    Id = 123456789,
    Interval = 600, // Change to 10 minutes
    Status = MonitorStatus.Paused
};

await client.UpdateMonitorAsync(parameters);
```

#### Delete a Monitor

```csharp
var parameters = new MonitorDeleteParameters
{
    Id = 123456789
};

await client.DeleteMonitorAsync(parameters);
```

### Alert Contacts

#### Get All Alert Contacts

```csharp
await foreach (var contact in client.GetAllAlertContactsAsync())
{
    Console.WriteLine($"{contact.FriendlyName}: {contact.Type}");
}
```

#### Create an Alert Contact

```csharp
var parameters = new AlertContactCreateParameters
{
    Type = AlertContactType.Email,
    FriendlyName = "Admin Email",
    Value = "admin@example.com"
};

var result = await client.CreateAlertContactAsync(parameters);
```

#### Update an Alert Contact

```csharp
var parameters = new AlertContactUpdateParameters
{
    Id = "12345",
    FriendlyName = "Updated Name",
    Status = AlertContactStatus.Active
};

await client.UpdateAlertContactAsync(parameters);
```

#### Delete an Alert Contact

```csharp
var parameters = new AlertContactDeleteParameters
{
    Id = "12345"
};

await client.DeleteAlertContactAsync(parameters);
```

### Maintenance Windows

#### Get All Maintenance Windows

```csharp
await foreach (var window in client.GetAllMaintenanceWindowsAsync())
{
    Console.WriteLine($"{window.FriendlyName}: {window.Type}");
}
```

#### Create a Maintenance Window

```csharp
var parameters = new MaintenanceWindowCreateParameters
{
    FriendlyName = "Weekly Maintenance",
    Type = MaintenanceWindowType.Weekly,
    StartTime = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds(),
    Duration = 3600, // 1 hour
    Value = "1" // Monday
};

var result = await client.CreateMaintenanceWindowAsync(parameters);
```

#### Update a Maintenance Window

```csharp
var parameters = new MaintenanceWindowUpdateParameters
{
    Id = 123456,
    Duration = 7200, // Change to 2 hours
    Status = MaintenanceWindowStatus.Active
};

await client.UpdateMaintenanceWindowAsync(parameters);
```

### Status Pages

#### Get All Status Pages

```csharp
await foreach (var page in client.GetAllStatusPagesAsync())
{
    Console.WriteLine($"{page.FriendlyName}: {page.StandardUrl}");
}
```

#### Create a Status Page

```csharp
var parameters = new StatusPageCreateParameters
{
    FriendlyName = "Public Status",
    Monitors = "123456789-987654321", // Comma-separated monitor IDs
    CustomMessage = "We monitor our services 24/7",
    Sort = StatusPageSort.FriendlyNameAZ
};

var result = await client.CreateStatusPageAsync(parameters);
```

#### Update a Status Page

```csharp
var parameters = new StatusPageUpdateParameters
{
    Id = 123456,
    FriendlyName = "Updated Status Page",
    Status = StatusPageStatus.Active
};

await client.UpdateStatusPageAsync(parameters);
```

## Advanced Usage

### Using with Dependency Injection

```csharp
// In Startup.cs or Program.cs
services.AddSingleton<IUptimeRobotClient>(sp =>
{
    var apiKey = configuration["UptimeRobot:ApiKey"];
    var logger = sp.GetRequiredService<ILogger<UptimeRobotClient>>();
    return UptimeRobotClientFactory.Create(apiKey, logger: logger);
});
```

### Custom HttpClient Configuration

```csharp
var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://api.uptimerobot.com"),
    Timeout = TimeSpan.FromSeconds(30)
};

httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");

var client = UptimeRobotClientFactory.Create(httpClient, "your-api-key");
```

### With Cancellation Tokens

```csharp
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

try
{
    var monitors = await client.GetMonitorsAsync(cancellationToken: cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Request timed out");
}
```

### Error Handling

```csharp
try
{
    await client.CreateMonitorAsync(parameters);
}
catch (UptimeRobotValidationException ex)
{
    Console.WriteLine($"Validation error: {ex.Message}");
    Console.WriteLine($"Parameter: {ex.ParameterName}");
}
catch (UptimeRobotApiException ex)
{
    Console.WriteLine($"API error: {ex.ErrorMessage}");
    Console.WriteLine($"Error type: {ex.ErrorType}");
}
catch (UptimeRobotException ex)
{
    Console.WriteLine($"Request failed: {ex.Message}");
}
```

### With Logging

```csharp
using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Debug);
});

var logger = loggerFactory.CreateLogger<UptimeRobotClient>();
var client = UptimeRobotClientFactory.Create("your-api-key", logger: logger);

// All HTTP requests and responses will be logged
await client.GetMonitorsAsync();
```

## Monitor Types

| Type | Enum Value | Description |
|------|------------|-------------|
| HTTP(s) | `MonitorType.HTTP` | Monitor HTTP/HTTPS endpoints |
| Keyword | `MonitorType.Keyword` | Check for keyword presence |
| Ping | `MonitorType.Ping` | ICMP ping monitoring |
| Port | `MonitorType.Port` | TCP port monitoring |
| Heartbeat | `MonitorType.Heartbeat` | Heartbeat monitoring |

## Testing the Release Candidate

We welcome feedback on this release candidate! Please help us make v2.0.0 stable by testing:

### What to Test

1. **Migration from v1.x**: Follow the [migration guide](#breaking-changes-from-v1x) and report any issues
2. **New Features**: Try the new Alert Contacts, Maintenance Windows, and Status Pages APIs
3. **Pagination**: Test with large datasets using `GetAllMonitorsAsync()` and similar methods
4. **Exception Handling**: Verify the new exception types work as expected
5. **Multi-Framework**: Test on .NET 9.0, 8.0, 6.0, or .NET Standard 2.0 projects

### How to Provide Feedback

- 🐛 **Bug Reports**: [Open an issue](https://github.com/strvmarv/uptimerobot-dotnet/issues/new) with the "v2.0.0-rc" label
- 💡 **Feature Requests**: [Start a discussion](https://github.com/strvmarv/uptimerobot-dotnet/discussions)
- ✅ **Success Stories**: Share your experience in the discussions

### Release Timeline

- **RC Phase**: Testing and feedback collection (2-4 weeks)
- **RC.2+**: Additional release candidates if critical issues are found
- **Final Release**: v2.0.0 stable release after successful RC testing

## Important Notes

### API Limitations

- **Pagination**: The UptimeRobot API limits results to 50 items per request. Use the `GetAll*Async()` methods for automatic pagination, or manage pagination manually using `Offset` and `Limit`.
- **Rate Limiting**: UptimeRobot enforces rate limits. Consider implementing retry logic with exponential backoff for production applications.

### Breaking Changes from v1.x

Version 2.0 introduces breaking changes for improved API design. See [CHANGELOG.md](CHANGELOG.md) for a complete migration guide.

**Key Changes:**
- Properties renamed from `snake_case` to `PascalCase`
- Integer enums replaced with strongly-typed enums
- New method names (e.g., `Monitors()` → `GetMonitorsAsync()`)
- Enhanced exception types

## Documentation

- 📖 **[API Reference](docs/API_REFERENCE.md)** - Complete UptimeRobot API reference and client mapping
- 🏗️ **[Architecture](docs/ARCHITECTURE.md)** - Project architecture and design decisions
- 🤝 **[Contributing Guide](docs/CONTRIBUTING.md)** - How to contribute to this project

## Contributing

Contributions are welcome! Please read our [Contributing Guide](docs/CONTRIBUTING.md) for details on:

- Development setup
- Coding standards
- Testing guidelines
- Pull request process

Quick start for contributors:

```bash
git clone https://github.com/strvmarv/uptimerobot-dotnet.git
cd uptimerobot-dotnet
dotnet restore
dotnet build
dotnet test
```

### Development Requirements

- .NET 9.0, 8.0, or 6.0 SDK
- Visual Studio 2022, VS Code, or Rider

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests for specific framework
dotnet test --framework net9.0

# Run with coverage
dotnet test /p:CollectCoverage=true
```

Manual integration tests are available but marked as `[Explicit]` and require a valid API key.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [UptimeRobot API](https://uptimerobot.com/api/) for the comprehensive API documentation
- All contributors who have helped improve this library

## Support

- 📖 [API Documentation](https://uptimerobot.com/api/)
- 🐛 [Issue Tracker](https://github.com/strvmarv/uptimerobot-dotnet/issues)
- 💬 [Discussions](https://github.com/strvmarv/uptimerobot-dotnet/discussions)

## Version History

- **v2.0.0** - Complete rewrite with breaking changes, full API coverage, and modern .NET features
- **v1.0.x** - Initial releases with basic monitor support

For detailed changes, see [CHANGELOG.md](CHANGELOG.md).
