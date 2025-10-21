# Manual Test Configuration

This directory contains manual integration tests that require a real UptimeRobot API key.

## Setup Instructions

Set environment variables before running tests:

**Windows (PowerShell)**:
```powershell
$env:UPTIMEROBOT_API_KEY="u1234567-abcdef1234567890"
$env:UPTIMEROBOT_TEST_URL="https://your-test-site.com"
```

**Windows (CMD)**:
```cmd
set UPTIMEROBOT_API_KEY=u1234567-abcdef1234567890
set UPTIMEROBOT_TEST_URL=https://your-test-site.com
```

**Linux/macOS**:
```bash
export UPTIMEROBOT_API_KEY="u1234567-abcdef1234567890"
export UPTIMEROBOT_TEST_URL="https://your-test-site.com"
```

## Running Manual Tests

Manual tests are marked with `[Explicit]` and won't run automatically. To run them:

### From Visual Studio
1. Open Test Explorer
2. Right-click on the specific test
3. Select "Run Selected Tests"

### From Rider
1. Open Unit Tests window
2. Right-click on the specific test
3. Select "Run"

### From Command Line
```bash
# Run all explicit tests
dotnet test --filter "Category=Explicit"

# Run specific test
dotnet test --filter "FullyQualifiedName~MonitorsManualTests.GetMonitors"
```

## Security Notes

⚠️ **IMPORTANT**: 
- Environment variables are safe and won't be committed to git
- Never hardcode your API key in test files
- The tests check for the environment variable and fail gracefully if not set

## Test Files

| File | Purpose |
|------|---------|
| `Monitors/MonitorsManualTests.cs` | Manual integration tests for Monitors API |

## Getting an API Key

1. Log in to your UptimeRobot account
2. Go to **Settings** → **API Settings**
3. Generate a Main API Key or Monitor-Specific API Key
4. For testing, a read-only or monitor-specific key is recommended

## Troubleshooting

### "API key not configured" error

This means the test couldn't find your API key. Make sure you've set the `UPTIMEROBOT_API_KEY` environment variable in your current shell session.

### Tests are skipped

This is normal - manual tests are marked `[Explicit]` and must be run manually to prevent accidental API calls during regular test runs.

### Rate limiting errors

If you're hitting rate limits:
- Use a test account if possible
- Reduce the frequency of test runs
- Consider using a paid UptimeRobot account (higher limits)

## See Also

- [SECURITY.md](../../SECURITY.md) - Security guidelines
- [CONTRIBUTING.md](../../docs/CONTRIBUTING.md) - Contribution guidelines
- [UptimeRobot API Documentation](https://uptimerobot.com/api/)

