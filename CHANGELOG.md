# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0-rc.3] - 2025-01-29 (Release Candidate 3)

### Fixed
- **Enum Serialization**: Fixed critical bug where enum values (like `MonitorType`, `MonitorStatus`, etc.) were being sent as string names instead of numeric values to the API, causing "must be a number" errors
  - Added explicit enum-to-integer conversion in `UtrFormUrlEncodedContent`
  - All enum parameters now correctly serialize as their underlying integer values (e.g., `MonitorType.HTTP` → `1`)
- **Maintenance Windows Deserialization**: Fixed JSON deserialization error when requesting monitor details with `Mwindows = 1` parameter
  - Changed `Monitor.Mwindows` property from `List<string>?` to `object?` to handle polymorphic API responses
  - API returns string IDs when maintenance windows are not requested, and full objects when requested
  - Consistent with existing `AlertContacts` and `CustomHttpStatuses` properties

### Changed
- Applied consistent polymorphic type handling across all dynamic API response properties

## [2.0.0-rc.2] - 2025-01-28 (Release Candidate 2)

### Fixed
- **JSON Deserialization Error**: Fixed issue where nullable enum properties (like `MonitorSubType`) would throw exceptions when API returned empty strings or unknown values
  - Added `NullableEnumConverter<T>` that gracefully handles:
    - Empty strings from API (converts to null)
    - Unknown enum values (converts to null instead of throwing)
    - Both string and integer enum representations
  - Improved API resilience - library no longer crashes on unexpected enum values

### Changed
- Applied NullableEnumConverter to all nullable enum properties for consistent behavior

## [2.0.0-rc.1] - 2025-01-XX (Release Candidate)

⚠️ **RELEASE CANDIDATE** - This is a pre-release version for testing. Please report any issues on GitHub.

**What is a Release Candidate?**
This version contains all planned features for v2.0.0 and is feature-complete. We're releasing it as RC to gather feedback and ensure stability before the final release. If no critical issues are found, this will become v2.0.0.

**Testing Needed:**
- Migration from v1.x to v2.0.0-rc.1
- All API endpoints (Monitors, Alert Contacts, Maintenance Windows, Status Pages)
- Pagination with large datasets
- Exception handling scenarios
- Multi-target framework compatibility

### Added
- **Platform Support**: Added support for .NET 9.0 while maintaining compatibility with .NET 8.0, .NET 6.0, and .NET Standard 2.0
- **Alert Contacts API**: Full support for creating, reading, updating, and deleting alert contacts
- **Maintenance Windows API**: Complete implementation for managing maintenance windows
- **Status Pages API**: Full support for managing public status pages
- **Pagination Support**: New `GetAllMonitorsAsync()`, `GetAllAlertContactsAsync()`, `GetAllMaintenanceWindowsAsync()`, and `GetAllStatusPagesAsync()` methods with automatic pagination using `IAsyncEnumerable`
- **Cancellation Token Support**: All async methods now accept `CancellationToken` for proper async cancellation
- **Custom Exception Types**: New exception hierarchy with `UptimeRobotException`, `UptimeRobotApiException`, and `UptimeRobotValidationException` for better error handling
- **Logging Support**: Optional `ILogger` support for diagnostics and debugging
- **Validation Attributes**: Data annotation validation for required and range-checked properties
- **Comprehensive XML Documentation**: All public APIs now have XML documentation comments
- **Nullable Reference Types**: Enabled nullable reference types for better null-safety
- **Performance Optimizations**: Reflection caching in `BaseModel.GetContentForRequest()` for improved performance

### Changed
- **BREAKING**: All model properties renamed from `snake_case` to `PascalCase` following .NET conventions:
  - `Friendly_Name` → `FriendlyName`
  - `Custom_Http_Headers` → `CustomHttpHeaders`
  - `api_key` → `ApiKey`
  - And many more...
- **BREAKING**: `MonitorType`, `MonitorStatus`, and other enums are now strongly typed instead of integers
- **BREAKING**: `Monitor.Type` is now `MonitorType` enum instead of `int`
- **BREAKING**: `MonitorCreateParameters.Type` is now `MonitorType` enum instead of `int`
- **BREAKING**: Old method names marked as obsolete. New naming convention:
  - `Monitors()` → `GetMonitorsAsync()`
  - `Monitor()` → `GetMonitorAsync()`
  - `MonitorCreate()` → `CreateMonitorAsync()`
  - `MonitorUpdate()` → `UpdateMonitorAsync()`
  - `MonitorDelete()` → `DeleteMonitorAsync()`
- **BREAKING**: `HttpRequestException` is now wrapped in `UptimeRobotException` for HTTP errors
- **BREAKING**: API error responses now throw `UptimeRobotApiException` with detailed error information
- **BREAKING**: Updated minimum supported language version to C# 9.0 to support all features
- **Improved**: Error handling now parses and includes detailed API error messages
- **Improved**: User-Agent updated to `uptimerobot-dotnet/2.0`
- **Enhanced**: Better null-safety throughout the codebase with nullable reference types

### Deprecated
- Old method names (now marked with `[Obsolete]` attribute):
  - `Monitors()` - Use `GetMonitorsAsync()` instead
  - `Monitor()` - Use `GetMonitorAsync()` instead
  - `MonitorCreate()` - Use `CreateMonitorAsync()` instead
  - `MonitorUpdate()` - Use `UpdateMonitorAsync()` instead
  - `MonitorDelete()` - Use `DeleteMonitorAsync()` instead

## Migration Guide from v1.x to v2.0

### Property Name Changes

Update all property accessors to use PascalCase:

```csharp
// v1.x
var monitor = new MonitorCreateParameters
{
    Friendly_Name = "My Monitor",
    Custom_Http_Headers = headers
};

// v2.0
var monitor = new MonitorCreateParameters
{
    FriendlyName = "My Monitor",
    CustomHttpHeaders = headers
};
```

### Strongly-Typed Enums

Replace integer types with enum types:

```csharp
// v1.x
Type = 1  // HTTP monitor

// v2.0
Type = MonitorType.HTTP
```

### Method Name Changes

Update method calls to use the new naming convention:

```csharp
// v1.x
var result = await client.Monitors();
var monitor = await client.Monitor(parameters);
await client.MonitorCreate(createParams);
await client.MonitorUpdate(updateParams);
await client.MonitorDelete(deleteParams);

// v2.0
var result = await client.GetMonitorsAsync();
var monitor = await client.GetMonitorAsync(parameters);
await client.CreateMonitorAsync(createParams);
await client.UpdateMonitorAsync(updateParams);
await client.DeleteMonitorAsync(deleteParams);
```

### Exception Handling

Update exception catching to use new exception types:

```csharp
// v1.x
try
{
    await client.MonitorCreate(parameters);
}
catch (HttpRequestException ex)
{
    // Handle error
}

// v2.0
try
{
    await client.CreateMonitorAsync(parameters);
}
catch (UptimeRobotApiException ex)
{
    // API returned an error response
    Console.WriteLine($"API Error: {ex.ErrorMessage}");
}
catch (UptimeRobotException ex)
{
    // HTTP or other error
    Console.WriteLine($"Error: {ex.Message}");
}
```

### Pagination

Take advantage of automatic pagination:

```csharp
// v1.x
var offset = 0;
var allMonitors = new List<Monitor>();
while (true)
{
    var response = await client.Monitors(new MonitorSearchParameters
    {
        Limit = 50,
        Offset = offset
    });
    
    if (response.Monitors == null || response.Monitors.Count == 0)
        break;
        
    allMonitors.AddRange(response.Monitors);
    offset += 50;
}

// v2.0
var allMonitors = new List<Monitor>();
await foreach (var monitor in client.GetAllMonitorsAsync())
{
    allMonitors.Add(monitor);
}
```

### Cancellation Tokens

Add cancellation token support:

```csharp
// v2.0
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
try
{
    var monitors = await client.GetMonitorsAsync(cancellationToken: cts.Token);
}
catch (OperationCanceledException)
{
    // Request was cancelled
}
```

## [1.0.6] - 2024-XX-XX
- Dependency updates

## [1.0.5] - Previous versions
- Initial releases with basic monitor support

