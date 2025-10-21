# Architecture Documentation

This document describes the architecture and design decisions of the UptimeRobot .NET Client Library.

## Table of Contents

1. [Overview](#overview)
2. [Project Structure](#project-structure)
3. [Core Components](#core-components)
4. [Design Patterns](#design-patterns)
5. [Data Flow](#data-flow)
6. [Key Design Decisions](#key-design-decisions)
7. [Extension Points](#extension-points)

## Overview

The UptimeRobot .NET Client Library is a strongly-typed, modern .NET client for the UptimeRobot API. It provides:

- **Type Safety**: Strongly-typed models with nullable reference types
- **Ease of Use**: Fluent API with automatic pagination
- **Flexibility**: Support for multiple .NET versions (9.0, 8.0, 6.0, Standard 2.0)
- **Reliability**: Comprehensive error handling and logging support
- **Testability**: Dependency injection support and mockable interfaces

## Project Structure

```
uptimerobot-dotnet/
├── src/
│   ├── Apis/                    # API endpoint implementations
│   │   ├── ApiMonitors.cs
│   │   ├── ApiAlertContacts.cs
│   │   ├── ApiMaintenanceWindows.cs
│   │   └── ApiStatusPages.cs
│   ├── Exceptions/              # Custom exception types
│   │   ├── UptimeRobotException.cs
│   │   ├── UptimeRobotApiException.cs
│   │   └── UptimeRobotValidationException.cs
│   ├── Models/                  # Data models
│   │   ├── Monitor.cs
│   │   ├── AlertContact.cs
│   │   ├── MaintenanceWindow.cs
│   │   ├── StatusPage.cs
│   │   ├── Parameters.cs
│   │   ├── Responses.cs
│   │   ├── Enums.cs
│   │   ├── BaseModel.cs
│   │   └── Interfaces.cs
│   ├── UptimeRobotClient.cs     # Main client class
│   ├── UptimeRobotClientBase.cs # Base HTTP functionality
│   ├── UptimeRobotClientFactory.cs
│   └── UtrFormUrlEncodedContent.cs
├── test/
│   └── UptimeRobotDotNetTests/
│       ├── Monitors/
│       ├── AlertContacts/
│       ├── MaintenanceWindows/
│       ├── StatusPages/
│       └── Core/
└── docs/
    ├── API_REFERENCE.md
    ├── ARCHITECTURE.md
    └── CONTRIBUTING.md
```

## Core Components

### 1. UptimeRobotClient

**Location**: `src/UptimeRobotClient.cs`

**Purpose**: Main entry point for the library. Partial class that combines all API implementations.

**Responsibilities**:
- Stores API key
- Inherits base HTTP functionality
- Aggregates all API endpoint methods

```csharp
public partial class UptimeRobotClient : UptimeRobotClientBase
{
    private readonly string _apiKey;

    public UptimeRobotClient(HttpClient httpClient, string apiKey, string apiVersion, ILogger logger)
        : base(httpClient, apiVersion, logger)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
    }
}
```

**Design Note**: Uses `partial class` to separate API implementations into logical files (one per API section).

### 2. UptimeRobotClientBase

**Location**: `src/UptimeRobotClientBase.cs`

**Purpose**: Provides core HTTP communication functionality.

**Responsibilities**:
- HTTP client management
- Request serialization (form-urlencoded)
- Response deserialization (JSON)
- Error detection and parsing
- Logging HTTP operations

**Key Methods**:
```csharp
protected async Task<T> PostAsync<T>(Uri path, IContentModel content, CancellationToken cancellationToken)
```

**Design Note**: Separates HTTP concerns from API-specific logic.

### 3. UptimeRobotClientFactory

**Location**: `src/UptimeRobotClientFactory.cs`

**Purpose**: Factory for creating client instances with proper configuration.

**Responsibilities**:
- Create and configure `HttpClient`
- Set default headers (User-Agent)
- Provide singleton `HttpClient` instance
- Support custom `HttpClient` injection

**Usage**:
```csharp
// Simple creation
var client = UptimeRobotClientFactory.Create("api-key");

// With custom HttpClient
var httpClient = new HttpClient();
var client = UptimeRobotClientFactory.Create(httpClient, "api-key");

// With logging
var client = UptimeRobotClientFactory.Create("api-key", logger: logger);
```

**Design Note**: Follows the factory pattern to encapsulate client creation complexity.

### 4. API Implementations

**Location**: `src/Apis/Api*.cs`

**Purpose**: Implement specific API endpoints as partial classes of `UptimeRobotClient`.

**Structure**:
```csharp
namespace UptimeRobotDotnet
{
    public partial class UptimeRobotClient
    {
        // Endpoint path constants
        public const string MonitorsGetPath = "getMonitors";
        public const string MonitorsCreatePath = "newMonitor";
        
        // API methods
        public async Task<UtrResponse> GetMonitorsAsync(...)
        public async IAsyncEnumerable<Monitor> GetAllMonitorsAsync(...)
        public async Task<UtrResponse> CreateMonitorAsync(...)
    }
}
```

**Design Note**: Each API section (Monitors, Alert Contacts, etc.) is in its own file for maintainability.

### 5. Models

**Location**: `src/Models/`

**Purpose**: Data Transfer Objects (DTOs) for API requests and responses.

**Categories**:

#### Domain Models
- `Monitor.cs`: Monitor entity
- `AlertContact.cs`: Alert contact entity
- `MaintenanceWindow.cs`: Maintenance window entity
- `StatusPage.cs`: Status page entity

#### Parameter Models
- `MonitorSearchParameters`: Query parameters for searching monitors
- `MonitorCreateParameters`: Parameters for creating monitors
- `MonitorUpdateParameters`: Parameters for updating monitors
- Similar for other APIs

#### Response Models
- `UtrResponse`: Standard API response wrapper
- `UtrPagination`: Pagination information
- `UtrError`: Error details
- `UtrResponseActionMonitor`: Action response with monitor ID

**Key Features**:
- JSON serialization attributes (`[JsonPropertyName]`)
- Validation attributes (`[Required]`, `[Range]`)
- Nullable reference types
- Strongly-typed enums

### 6. Exception Hierarchy

**Location**: `src/Exceptions/`

**Purpose**: Provide detailed error information for different failure scenarios.

```
Exception
└── UptimeRobotException (base for all library exceptions)
    ├── UptimeRobotApiException (API returned error response)
    └── UptimeRobotValidationException (client-side validation failed)
```

**Usage**:
```csharp
try
{
    await client.CreateMonitorAsync(parameters);
}
catch (UptimeRobotValidationException ex)
{
    // Handle validation errors
    foreach (var error in ex.ValidationResults)
    {
        Console.WriteLine(error.ErrorMessage);
    }
}
catch (UptimeRobotApiException ex)
{
    // Handle API errors
    Console.WriteLine($"API Error: {ex.ErrorResponse?.Error?.Message}");
}
catch (UptimeRobotException ex)
{
    // Handle other errors
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Design Patterns

### 1. Factory Pattern

**Component**: `UptimeRobotClientFactory`

**Purpose**: Encapsulate client creation and configuration.

**Benefits**:
- Centralized configuration
- Reusable `HttpClient` instances
- Easy to mock for testing

### 2. Partial Classes

**Component**: `UptimeRobotClient` + API implementations

**Purpose**: Separate concerns while maintaining single class interface.

**Benefits**:
- Logical file organization
- Easy to navigate
- Maintains encapsulation

### 3. Template Method

**Component**: `UptimeRobotClientBase.PostAsync<T>()`

**Purpose**: Define skeleton of HTTP request/response handling.

**Benefits**:
- Consistent error handling
- Centralized logging
- Reusable serialization logic

### 4. Builder Pattern (Fluent API)

**Component**: Parameter classes

**Purpose**: Provide intuitive API for constructing requests.

**Example**:
```csharp
var parameters = new MonitorCreateParameters
{
    FriendlyName = "My Monitor",
    Url = "https://example.com",
    Type = MonitorType.HTTP,
    Interval = 300
};
```

### 5. Iterator Pattern

**Component**: `IAsyncEnumerable<T>` pagination methods

**Purpose**: Abstract pagination complexity.

**Example**:
```csharp
await foreach (var monitor in client.GetAllMonitorsAsync())
{
    // Handles pagination automatically
}
```

## Data Flow

### Request Flow

```
┌─────────────┐
│   Client    │
│    Code     │
└──────┬──────┘
       │ 1. Call API method
       ▼
┌─────────────────────┐
│ UptimeRobotClient   │
│ (API implementation)│
└──────┬──────────────┘
       │ 2. Prepare parameters
       ▼
┌─────────────────────┐
│   BaseModel         │
│ GetContentForRequest│
└──────┬──────────────┘
       │ 3. Serialize to Dictionary
       ▼
┌─────────────────────────┐
│ UptimeRobotClientBase   │
│      PostAsync<T>       │
└──────┬──────────────────┘
       │ 4. Create form-urlencoded content
       ▼
┌─────────────────────────────┐
│ UtrFormUrlEncodedContent    │
│ (HTTP request body)         │
└──────┬──────────────────────┘
       │ 5. HTTP POST
       ▼
┌─────────────────┐
│  HttpClient     │
└──────┬──────────┘
       │ 6. Send request
       ▼
┌─────────────────┐
│ UptimeRobot API │
└─────────────────┘
```

### Response Flow

```
┌─────────────────┐
│ UptimeRobot API │
└──────┬──────────┘
       │ 1. JSON response
       ▼
┌─────────────────┐
│  HttpClient     │
└──────┬──────────┘
       │ 2. HTTP response
       ▼
┌─────────────────────────┐
│ UptimeRobotClientBase   │
│      PostAsync<T>       │
└──────┬──────────────────┘
       │ 3. Check for "stat: fail"
       ├─ Yes → Throw UptimeRobotApiException
       │
       │ 4. Check HTTP status
       ├─ Error → Throw UptimeRobotException
       │
       │ 5. Deserialize JSON
       ▼
┌─────────────────────┐
│ System.Text.Json    │
└──────┬──────────────┘
       │ 6. Create typed object
       ▼
┌─────────────────────┐
│   Response Model    │
│  (e.g., Monitor)    │
└──────┬──────────────┘
       │ 7. Return to caller
       ▼
┌─────────────┐
│   Client    │
│    Code     │
└─────────────┘
```

### Pagination Flow

```csharp
// Automatic pagination with IAsyncEnumerable
public async IAsyncEnumerable<Monitor> GetAllMonitorsAsync(
    MonitorSearchParameters? parameters = null,
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    parameters ??= new MonitorSearchParameters { ApiKey = _apiKey };
    parameters.Limit ??= 50;

    var currentPage = 0;
    var total = 0;

    do
    {
        parameters.Offset = currentPage * parameters.Limit;
        var response = await GetMonitorsAsync(parameters, cancellationToken);

        if (response?.Monitors != null)
        {
            foreach (var monitor in response.Monitors)
            {
                yield return monitor; // Stream results
            }
        }

        total = response?.Pagination?.Total ?? 0;
        currentPage++;

    } while (currentPage * parameters.Limit < total && !cancellationToken.IsCancellationRequested);
}
```

## Key Design Decisions

### 1. Nullable Reference Types

**Decision**: Enable nullable reference types project-wide.

**Rationale**:
- Improved null-safety
- Better IDE support
- Prevents null reference exceptions

**Impact**:
- All properties explicitly marked as nullable or non-nullable
- Compiler warnings for potential null issues
- Breaking change from v1.x

### 2. PascalCase Property Names

**Decision**: Use PascalCase for all C# properties with `[JsonPropertyName]` for snake_case API fields.

**Rationale**:
- Follow .NET naming conventions
- Better developer experience
- Consistent with .NET ecosystem

**Example**:
```csharp
[JsonPropertyName("friendly_name")]
public string? FriendlyName { get; set; }
```

### 3. Strongly-Typed Enums

**Decision**: Replace integer types with enum types.

**Rationale**:
- Type safety
- IntelliSense support
- Self-documenting code

**Example**:
```csharp
// Before (v1.x)
Type = 1

// After (v2.0)
Type = MonitorType.HTTP
```

### 4. Form-URLEncoded Instead of JSON

**Decision**: Use form-urlencoded for requests (as per API requirement).

**Rationale**:
- UptimeRobot API requires form-urlencoded
- Simpler than multipart/form-data
- Custom implementation in `UtrFormUrlEncodedContent`

### 5. IAsyncEnumerable for Pagination

**Decision**: Use `IAsyncEnumerable<T>` for automatic pagination.

**Rationale**:
- Streams results efficiently
- Memory-efficient for large datasets
- Modern async/await pattern
- Supports cancellation

### 6. Partial Classes for API Organization

**Decision**: Split API implementations across multiple files using partial classes.

**Rationale**:
- Logical organization
- Easier navigation
- Maintains single public interface
- Reduces file size

### 7. Custom Exception Hierarchy

**Decision**: Create custom exception types instead of using generic exceptions.

**Rationale**:
- Detailed error information
- Easier exception handling
- API-specific error context
- Better debugging

### 8. Reflection Caching in BaseModel

**Decision**: Cache reflection results in `GetContentForRequest()`.

**Rationale**:
- Performance optimization
- Reduce reflection overhead
- One-time reflection per type
- Significant improvement for repeated calls

## Extension Points

### 1. Custom HttpClient Configuration

Users can provide their own `HttpClient` for custom configuration:

```csharp
var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(60)
};
httpClient.DefaultRequestHeaders.Add("X-Custom-Header", "value");

var client = UptimeRobotClientFactory.Create(httpClient, apiKey);
```

### 2. Logging Integration

Support for `ILogger` allows integration with any logging framework:

```csharp
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.AddApplicationInsights();
});

var logger = loggerFactory.CreateLogger<UptimeRobotClient>();
var client = UptimeRobotClientFactory.Create(apiKey, logger: logger);
```

### 3. Custom Parameter Validation

Users can extend parameter classes with additional validation:

```csharp
public class CustomMonitorParameters : MonitorCreateParameters
{
    [CustomValidation(typeof(MyValidator), "ValidateUrl")]
    public new string Url { get; set; }
}
```

### 4. Retry Logic with Polly

Users can add retry policies to the `HttpClient`:

```csharp
var retryPolicy = Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var httpClient = new HttpClient(new PolicyHttpMessageHandler(retryPolicy));
var client = UptimeRobotClientFactory.Create(httpClient, apiKey);
```

## Testing Architecture

### Unit Tests

- **WireMock.Net**: Mock HTTP responses
- **NUnit**: Test framework
- **Coverage**: >95% code coverage

**Structure**:
```
test/UptimeRobotDotNetTests/
├── BaseTest.cs              # Base test class with logging
├── BaseHttpClientTest.cs    # Base for HTTP mocking tests
├── Monitors/
│   ├── MonitorsTests.cs     # Unit tests
│   └── MonitorsManualTests.cs # Integration tests
├── AlertContacts/
├── MaintenanceWindows/
├── StatusPages/
└── Core/
    ├── BaseModelTests.cs
    └── UtrFormUrlEncodedContentTests.cs
```

### Test Patterns

1. **Arrange-Act-Assert**
2. **Mock HTTP responses with WireMock**
3. **Manual integration tests marked `[Explicit]`**
4. **Parameterized tests with `[TestCase]`**

## Performance Considerations

1. **HttpClient Reuse**: Singleton `HttpClient` in factory
2. **Reflection Caching**: Cache property info in `BaseModel`
3. **Streaming Results**: `IAsyncEnumerable` for pagination
4. **Async/Await**: Fully asynchronous API
5. **ConfigureAwait(false)**: Used throughout for library code

## Future Enhancements

1. **Source Generators**: Replace reflection with compile-time generation
2. **Polly Integration**: Built-in retry and circuit breaker patterns
3. **Response Caching**: Optional caching layer
4. **GraphQL Support**: If UptimeRobot adds GraphQL API
5. **Real-time Updates**: WebSocket support if available

## Summary

The UptimeRobot .NET Client Library is designed with:

- **Separation of Concerns**: Clear boundaries between HTTP, API, and domain logic
- **Extensibility**: Multiple extension points for customization
- **Type Safety**: Nullable types, enums, and validation
- **Performance**: Caching, streaming, and async patterns
- **Testability**: Dependency injection and mockable interfaces
- **Maintainability**: Organized structure and clear patterns

These architectural decisions ensure the library is robust, performant, and easy to use while maintaining flexibility for advanced scenarios.

