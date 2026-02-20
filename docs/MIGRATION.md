# Migration Guide from v1.x to v2.0

## Property Name Changes

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

## Strongly-Typed Enums

Replace integer types with enum types:

```csharp
// v1.x
Type = 1  // HTTP monitor

// v2.0
Type = MonitorType.HTTP
```

## Method Name Changes

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
await client.CreateMonitorAsync(parameters);
await client.UpdateMonitorAsync(parameters);
await client.DeleteMonitorAsync(parameters);
```

## Exception Handling

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

## Pagination

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

## Cancellation Tokens

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
