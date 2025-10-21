# Security Policy

## Reporting Security Issues

If you discover a security vulnerability in this project, please report it by emailing the maintainers or opening a private security advisory on GitHub.

**Please do not report security vulnerabilities through public GitHub issues.**

## API Key Security

### For Users

1. **Never commit API keys to version control**
   - Use environment variables or secret management tools
   - Never hardcode API keys in your application code

2. **Use read-only keys when possible**
   - UptimeRobot supports monitor-specific API keys with limited permissions
   - Use the minimum required permissions for your use case

3. **Rotate keys regularly**
   - Change your API keys periodically
   - Immediately rotate keys if you suspect they've been compromised

4. **Secure your keys in production**
   - Use Azure Key Vault, AWS Secrets Manager, or similar services
   - Never store keys in plain text configuration files
   - Use secure configuration providers in ASP.NET Core

### For Contributors

When writing tests that require API keys:

1. **Never commit real API keys**
   - Use placeholder values like `"YOUR-API-KEY-HERE"` in constants
   - Mark integration tests as `[Explicit]` so they don't run automatically
   - Always check for the placeholder before running tests

2. **Use environment variables for manual testing**
   ```csharp
   private const string ApiKeyPlaceholder = "YOUR-API-KEY-HERE";
   
   private static string GetApiKey() => 
       Environment.GetEnvironmentVariable("UPTIMEROBOT_API_KEY") ?? ApiKeyPlaceholder;
   
   [Test, Explicit]
   public async Task MyTest()
   {
       var apiKey = GetApiKey();
       if (apiKey == ApiKeyPlaceholder)
       {
           Assert.Inconclusive("API key not configured. Set UPTIMEROBOT_API_KEY environment variable.");
           return;
       }
       
       // Your test code...
   }
   ```

3. **Review your commits**
   - Always review your changes before committing
   - Use `git diff` to check for accidentally included keys
   - Consider using pre-commit hooks to detect secrets

## Protected Files

The following file patterns are automatically ignored by `.gitignore` to prevent accidental commits:

- `*.env` and `*.env.local`
- `*secrets.json` and `*secrets*.json`
- `appsettings.Development.json`
- `appsettings.Local.json`
- `TestConfiguration.json` and `TestSettings.json`

## Best Practices

### In Application Code

```csharp
// ❌ BAD - Hardcoded API key
var client = UptimeRobotClientFactory.Create("u1234567-abcdef1234567890");

// ✅ GOOD - Environment variable
var apiKey = Environment.GetEnvironmentVariable("UPTIMEROBOT_API_KEY");
var client = UptimeRobotClientFactory.Create(apiKey);

// ✅ GOOD - Configuration (ASP.NET Core)
var apiKey = configuration["UptimeRobot:ApiKey"];
var client = UptimeRobotClientFactory.Create(apiKey);

// ✅ GOOD - Azure Key Vault
var apiKey = await keyVaultClient.GetSecretAsync("uptimerobot-api-key");
var client = UptimeRobotClientFactory.Create(apiKey.Value);
```

### In Tests

```csharp
// ❌ BAD - Real API key in code
private const string ApiKey = "u1234567-abcdef1234567890";

// ✅ GOOD - Placeholder with environment variable fallback
private const string ApiKeyPlaceholder = "YOUR-API-KEY-HERE";

private static string GetApiKey() => 
    Environment.GetEnvironmentVariable("UPTIMEROBOT_API_KEY") ?? ApiKeyPlaceholder;

[Test, Explicit]
public async Task MyTest()
{
    var apiKey = GetApiKey();
    if (apiKey == ApiKeyPlaceholder)
    {
        Assert.Inconclusive("API key not configured");
        return;
    }
    // Test code...
}
```

## Checking for Exposed Secrets

Before committing, you can check for potential secrets:

```bash
# Search for UptimeRobot API key pattern
git grep -E "u[0-9]{7}-[a-f0-9]{20}"

# Check staged changes
git diff --cached | grep -E "u[0-9]{7}-[a-f0-9]{20}"
```

## If You Accidentally Commit an API Key

1. **Immediately rotate the key** in your UptimeRobot account
2. **Remove the key from git history** using `git filter-branch` or BFG Repo-Cleaner
3. **Force push** the cleaned history (coordinate with other contributors)
4. **Document the incident** internally

### Remove from Git History

```bash
# Using BFG Repo-Cleaner (recommended)
bfg --replace-text passwords.txt

# Using git filter-branch
git filter-branch --force --index-filter \
  "git rm --cached --ignore-unmatch path/to/file" \
  --prune-empty --tag-name-filter cat -- --all

# Force push (DANGEROUS - coordinate with team)
git push origin --force --all
```

## Dependency Security

This library has minimal dependencies:

- `System.Text.Json` - Official Microsoft package
- `Microsoft.Extensions.Logging.Abstractions` - Official Microsoft package

We regularly update dependencies to address known vulnerabilities.

## Reporting Other Security Issues

For non-API-key security concerns:

1. **GitHub Security Advisories**: https://github.com/strvmarv/uptimerobot-dotnet/security/advisories
2. **Email**: security@[domain] (if configured)
3. **Private Issue**: Contact maintainers directly

## Security Updates

Security updates will be released as:

- **Patch versions** for fixes in current major version
- **Immediate releases** for critical vulnerabilities
- **Security advisories** published on GitHub

## Supported Versions

| Version | Supported          |
| ------- | ------------------ |
| 2.0.x   | ✅ Yes             |
| 1.x     | ⚠️ Limited (critical only) |
| < 1.0   | ❌ No              |

## Acknowledgments

We appreciate responsible disclosure of security vulnerabilities and will acknowledge contributors (with permission) in our security advisories.
