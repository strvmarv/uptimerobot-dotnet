[![build and test](https://github.com/strvmarv/uptimerobot-dotnet/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/strvmarv/uptimerobot-dotnet/actions/workflows/build-and-test.yml)

# uptimerobot-dotnet
A simple [UptimeRobot](https://uptimerobot.com) .NET Client

Nuget.org Link: https://www.nuget.org/packages/UptimeRobotDotnet

API Documentation: https://uptimerobot.com/api/

# Notes

- This client is a simple wrapper around the UptimeRobot API. It does not implement all the API endpoints.
- The client uses the Newtonsoft.Json library to serialize and deserialize JSON data.
- The client is asynchronous and uses the HttpClient class to make HTTP requests.
- The HttpClient is implemented per Micrsoft recommendations.  In this case, a Singleton that is reused.
- You may provide your own HttpClient instance if you want to manage the lifecycle of the HttpClient.
- Manual tests are provided if you'd like to observe the client in action.  You will need to provide your own API key.

# State

| Entity | Implemented |
---------|------------
| Alert Contacts | :x: |
| Maintenance Windows | :x: |
| Monitors | :white_check_mark: |
| Status Pages | :x: |

# Caveats

- The UTR API limits max results to 50.  This client does not handle pagination.  You will need to manage this yourself using Offset.
- The UTR API implementation only supports POST requests (99.9%), so the client API surface may not be ideal.
- The UTR API requires the API Key to be passed in each POST body, so the client API surface may not be ideal.
- The client will automatically inject the API Key into the payload from the active client, but you can set it manually as well.

# Usage

Example usage using Monitors.  Implementation across entities is similar.  Though some entities may not support all methods.  

***Use manual tests for reference.***

## Get all monitors
https://uptimerobot.com/api/#getMonitorsWrap
```csharp
var client = UptimeRobotClientFactory.Create("YOUR-API-KEY-HERE");
var monitors = await client.Monitors(); // A MonitorSearchParameters object can be passed to filter results
```

## Get monitor by Id
```csharp
var client = UptimeRobotClientFactory.Create("YOUR-API-KEY-HERE");
var parameters = new MonitorSearchParameters() { Monitors = "EXISTING-MONITOR-ID" };
var monitor = await client.Monitor(parameters);
```

## Create a monitor
https://uptimerobot.com/api/#newMonitorWrap

Example: Create a monitor for https://your-url-here.com
```csharp
var client = UptimeRobotClientFactory.Create("YOUR-API-KEY-HERE");
var parameters = new MonitorParameters
{
    FriendlyName = "Your Monitor Name",  // Required
    Type = 1, // Required
    Url = "https://your-url-here.com", // Required
};
var create = await client.MonitorCreate(parameters);

```

## Update a monitor
https://uptimerobot.com/api/#editMonitorWrap

Example: Update the monitor interval to 300 seconds
```csharp
var client = UptimeRobotClientFactory.Create("YOUR-API-KEY-HERE");
var updateParameters = new MonitorParameters
{
    Id = "EXISTING-MONITOR-ID", // Required
    Interval = 300
};
var update = await client.MonitorUpdate(updateParameters);
```

## Delete a monitor
https://uptimerobot.com/api/#deleteMonitorWrap

```csharp
var deleteParameters = new MonitorDeleteParameters
{
    Id = "EXISTING-MONITOR-ID" // Required
};
var client = UptimeRobotClientFactory.Create("YOUR-API-KEY-HERE");
var delete = await client.MonitorDelete(deleteParameters);

```

# Contributing
Use your favorite IDE to open the project.  The project was developed using Visual Studio.

```bash
git clone https://github.com/strvmarv/uptimerobot-dotnet.git
cd uptimerobot-dotnet
dotnet restore
dotnet build
```

## Run Tests
```bash
dotnet test
```
