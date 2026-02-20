# Changelog

## [2.3.0](https://github.com/strvmarv/uptimerobot-dotnet/compare/UptimeRobotDotnet-v2.2.0...UptimeRobotDotnet-v2.3.0) (2026-02-20)


### Features

* automate releases with release-please ([f1f34e1](https://github.com/strvmarv/uptimerobot-dotnet/commit/f1f34e1c3ffa65911e34afda5474305503c564bb))
* automate releases with release-please ([36953c3](https://github.com/strvmarv/uptimerobot-dotnet/commit/36953c381e5803e05e4d0b76bfca7fa3b1b047b4))
* Release v2.0.0-rc.1 with breaking changes and new features ([2b86af9](https://github.com/strvmarv/uptimerobot-dotnet/commit/2b86af90361ec63fb853c4437a2d7d33e25fe66a))


### Bug Fixes

* use !cancelled() instead of always() in publish job condition ([9b12e32](https://github.com/strvmarv/uptimerobot-dotnet/commit/9b12e32445a670f4da2dce3dc01f33f342f91b4a))

## [2.2.0](https://github.com/strvmarv/uptimerobot-dotnet/compare/v2.1.0...v2.2.0) (2026-02-19)

### Features

- Added `net10.0` as an explicit target framework
- Library now supports .NET 10, .NET 9, .NET 8, and .NET Standard 2.0

## [2.1.0](https://github.com/strvmarv/uptimerobot-dotnet/compare/v2.0.0...v2.1.0) (2026-02-20)

### Miscellaneous Chores

- Dropped explicit `net6.0` target framework (end-of-life); .NET 6 consumers are still supported via `netstandard2.0`
- Updated `System.Text.Json` from `9.0.6` to `10.0.1` (netstandard2.0)
- Updated `Microsoft.Extensions.Logging.Abstractions` from `6.0.0` to `10.0.1` (netstandard2.0)

#### Test Dependency Updates

- Updated `Microsoft.NET.Test.Sdk` from `17.13.0` to `18.0.1`
- Updated `NUnit.Analyzers` from `4.10.0` to `4.11.2`
- Updated `NUnit3TestAdapter` from `5.1.0` to `6.0.1`

## [2.0.0](https://github.com/strvmarv/uptimerobot-dotnet/releases/tag/v2.0.0) (2025-11-05)

### Bug Fixes

- **Enum Serialization**: Fixed critical bug where enum values (like `MonitorType`, `MonitorStatus`, etc.) were being sent as string names instead of numeric values to the API, causing "must be a number" errors
  - Added explicit enum-to-integer conversion in `UtrFormUrlEncodedContent`
  - All enum parameters now correctly serialize as their underlying integer values (e.g., `MonitorType.HTTP` -> `1`)
- **Maintenance Windows Deserialization**: Fixed JSON deserialization error when requesting monitor details with `Mwindows = 1` parameter
  - Changed `Monitor.Mwindows` property from `List<string>?` to `object?` to handle polymorphic API responses
  - API returns string IDs when maintenance windows are not requested, and full objects when requested
  - Consistent with existing `AlertContacts` and `CustomHttpStatuses` properties
- **JSON Deserialization Error**: Fixed issue where nullable enum properties (like `MonitorSubType`) would throw exceptions when API returned empty strings or unknown values
  - Added `NullableEnumConverter<T>` that gracefully handles empty strings, unknown values, and both string and integer representations

### Features

- **Platform Support**: Added support for .NET 9.0 while maintaining compatibility with .NET 8.0, .NET 6.0, and .NET Standard 2.0
- **Alert Contacts API**: Full support for creating, reading, updating, and deleting alert contacts
- **Maintenance Windows API**: Complete implementation for managing maintenance windows
- **Status Pages API**: Full support for managing public status pages
- **Pagination Support**: New `GetAllMonitorsAsync()`, `GetAllAlertContactsAsync()`, `GetAllMaintenanceWindowsAsync()`, and `GetAllStatusPagesAsync()` methods with automatic pagination using `IAsyncEnumerable`
- **Cancellation Token Support**: All async methods now accept `CancellationToken` for proper async cancellation
- **Custom Exception Types**: New exception hierarchy with `UptimeRobotException`, `UptimeRobotApiException`, and `UptimeRobotValidationException`
- **Logging Support**: Optional `ILogger` support for diagnostics and debugging
- **Validation Attributes**: Data annotation validation for required and range-checked properties
- **Comprehensive XML Documentation**: All public APIs now have XML documentation comments
- **Nullable Reference Types**: Enabled nullable reference types for better null-safety
- **Performance Optimizations**: Reflection caching in `BaseModel.GetContentForRequest()`

### Miscellaneous Chores

- **BREAKING**: All model properties renamed from `snake_case` to `PascalCase` following .NET conventions
- **BREAKING**: `MonitorType`, `MonitorStatus`, and other enums are now strongly typed instead of integers
- **BREAKING**: Old method names marked as obsolete (`Monitors()` -> `GetMonitorsAsync()`, etc.)
- **BREAKING**: `HttpRequestException` is now wrapped in `UptimeRobotException` for HTTP errors
- **BREAKING**: API error responses now throw `UptimeRobotApiException` with detailed error information
- **BREAKING**: Updated minimum supported language version to C# 9.0

### Deprecated

- Old method names (now marked with `[Obsolete]` attribute):
  - `Monitors()` - Use `GetMonitorsAsync()` instead
  - `Monitor()` - Use `GetMonitorAsync()` instead
  - `MonitorCreate()` - Use `CreateMonitorAsync()` instead
  - `MonitorUpdate()` - Use `UpdateMonitorAsync()` instead
  - `MonitorDelete()` - Use `DeleteMonitorAsync()` instead

For migration instructions, see [docs/MIGRATION.md](docs/MIGRATION.md).

## [1.0.6](https://github.com/strvmarv/uptimerobot-dotnet/releases/tag/v1.0.6)

- Dependency updates

## [1.0.5](https://github.com/strvmarv/uptimerobot-dotnet/releases/tag/v1.0.5)

- Initial releases with basic monitor support
