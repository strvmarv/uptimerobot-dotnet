# Contributing to UptimeRobot .NET Client Library

Thank you for your interest in contributing to the UptimeRobot .NET Client Library! This document provides guidelines and instructions for contributing.

## Table of Contents

1. [Code of Conduct](#code-of-conduct)
2. [Getting Started](#getting-started)
3. [Development Setup](#development-setup)
4. [How to Contribute](#how-to-contribute)
5. [Coding Standards](#coding-standards)
6. [Testing Guidelines](#testing-guidelines)
7. [Pull Request Process](#pull-request-process)
8. [Reporting Issues](#reporting-issues)
9. [Feature Requests](#feature-requests)

## Code of Conduct

This project adheres to a code of conduct that all contributors are expected to follow:

- **Be Respectful**: Treat everyone with respect and kindness
- **Be Collaborative**: Work together constructively
- **Be Professional**: Keep discussions focused and professional
- **Be Inclusive**: Welcome contributors of all backgrounds and experience levels

## Security

**⚠️ IMPORTANT**: Never commit API keys or secrets to the repository.

See [SECURITY.md](../SECURITY.md) for:
- How to handle API keys safely in tests
- Protected file patterns in `.gitignore`
- Best practices for secret management
- What to do if you accidentally commit a key

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- **.NET SDK**: 9.0, 8.0, or 6.0 (preferably all three for testing)
  - Download from: https://dotnet.microsoft.com/download
- **Git**: For version control
  - Download from: https://git-scm.com/
- **IDE**: Visual Studio 2022, VS Code, or JetBrains Rider

### Fork and Clone

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/uptimerobot-dotnet.git
   cd uptimerobot-dotnet
   ```
3. Add the upstream repository:
   ```bash
   git remote add upstream https://github.com/strvmarv/uptimerobot-dotnet.git
   ```

## Development Setup

### 1. Restore Dependencies

```bash
dotnet restore
```

### 2. Build the Project

```bash
dotnet build
```

### 3. Run Tests

```bash
dotnet test
```

### 4. Verify All Target Frameworks

```bash
# Build for all frameworks
dotnet build --configuration Release

# Test on all frameworks
dotnet test --configuration Release
```

### Project Structure

```
uptimerobot-dotnet/
├── src/                     # Source code
│   ├── Apis/               # API implementations
│   ├── Exceptions/         # Custom exceptions
│   ├── Models/             # Data models
│   └── *.cs                # Core client files
├── test/                   # Test projects
│   └── UptimeRobotDotNetTests/
├── docs/                   # Documentation
├── .editorconfig           # Code style configuration
└── README.md              # Main documentation
```

## How to Contribute

### Types of Contributions

We welcome various types of contributions:

1. **Bug Fixes**: Fix issues reported in GitHub Issues
2. **New Features**: Implement new functionality
3. **Documentation**: Improve or add documentation
4. **Tests**: Add or improve test coverage
5. **Performance**: Optimize existing code
6. **Code Quality**: Refactoring and cleanup

### Contribution Workflow

1. **Check Existing Issues**: Look for existing issues or create a new one
2. **Discuss First**: For major changes, discuss in an issue first
3. **Create a Branch**: Create a feature branch from `main`
4. **Make Changes**: Implement your changes
5. **Write Tests**: Add tests for new functionality
6. **Update Documentation**: Update docs as needed
7. **Submit PR**: Create a pull request

### Branch Naming

Use descriptive branch names:

```
feature/add-account-details-api
bugfix/fix-pagination-offset
docs/improve-readme-examples
test/add-status-page-tests
refactor/optimize-reflection-caching
```

## Coding Standards

### .NET Conventions

Follow standard .NET coding conventions:

- **PascalCase** for class names, method names, and public properties
- **camelCase** for local variables and private fields
- **_camelCase** for private fields (with underscore prefix)
- **UPPERCASE** for constants

### EditorConfig

The project includes `.editorconfig` for consistent formatting. Ensure your IDE respects these settings.

### Key Guidelines

1. **Nullable Reference Types**
   - Enable nullable reference types in all new files
   - Explicitly mark nullable and non-nullable types
   ```csharp
   public string? NullableString { get; set; }
   public string NonNullableString { get; set; } = null!;
   ```

2. **Async/Await**
   - Use `async`/`await` for all I/O operations
   - Use `ConfigureAwait(false)` in library code
   - Accept `CancellationToken` in all async methods
   ```csharp
   public async Task<Result> GetDataAsync(CancellationToken cancellationToken = default)
   {
       var data = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
       return data;
   }
   ```

3. **Exception Handling**
   - Use custom exception types: `UptimeRobotException`, `UptimeRobotApiException`, `UptimeRobotValidationException`
   - Include meaningful error messages
   - Log errors appropriately
   ```csharp
   if (response.Stat == "fail")
   {
       throw new UptimeRobotApiException("API request failed", response);
   }
   ```

4. **XML Documentation**
   - Add XML comments to all public APIs
   - Include `<summary>`, `<param>`, `<returns>`, and `<exception>` tags
   ```csharp
   /// <summary>
   /// Gets a specific monitor by ID.
   /// </summary>
   /// <param name="parameters">Search parameters including monitor ID.</param>
   /// <param name="cancellationToken">Cancellation token for the request.</param>
   /// <returns>A <see cref="UtrResponse"/> containing the monitor details.</returns>
   /// <exception cref="UptimeRobotApiException">Thrown when the API returns an error.</exception>
   public async Task<UtrResponse> GetMonitorAsync(
       MonitorSearchParameters parameters,
       CancellationToken cancellationToken = default)
   ```

5. **Model Properties**
   - Use PascalCase for property names
   - Include `[JsonPropertyName]` for API field mapping
   - Add validation attributes where appropriate
   ```csharp
   [Required]
   [JsonPropertyName("friendly_name")]
   public string FriendlyName { get; set; } = null!;

   [Range(60, 86400)]
   [JsonPropertyName("interval")]
   public int? Interval { get; set; }
   ```

6. **Enums**
   - Use strongly-typed enums instead of magic numbers
   - Add XML documentation to enum members
   ```csharp
   /// <summary>
   /// Represents the type of monitor.
   /// </summary>
   public enum MonitorType : int
   {
       /// <summary>
       /// HTTP(s) monitor.
       /// </summary>
       HTTP = 1,
       
       /// <summary>
       /// Keyword monitor.
       /// </summary>
       Keyword = 2
   }
   ```

## Testing Guidelines

### Test Requirements

All contributions must include appropriate tests:

1. **Unit Tests**: Test individual components in isolation
2. **Integration Tests**: Test interaction between components
3. **Edge Cases**: Test boundary conditions and error scenarios

### Writing Tests

Use NUnit framework with the following pattern:

```csharp
[Test]
public async Task GetMonitor_WithValidId_ReturnsMonitor()
{
    // Arrange
    var mockParameters = new MonitorSearchParameters { Monitors = "123456" };
    Server.Given(Request.Create()
            .WithPath($"/{UptimeRobotClientBase.GetRelativePathWithDefaultVersion(UptimeRobotClient.MonitorsGetPath)}")
            .UsingPost())
        .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithBodyAsJson(new
            {
                stat = "ok",
                monitors = new[] { new { id = 123456, friendly_name = "Test" } }
            }));

    // Act
    var client = UptimeRobotClientFactory.Create(Server.CreateClient(), TestApiKey);
    var result = await client.GetMonitorAsync(mockParameters);

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Monitors?.Count, Is.GreaterThan(0));
}
```

### Test Organization

- Place tests in corresponding folders: `Monitors/`, `AlertContacts/`, etc.
- Name test classes with `Tests` suffix: `MonitorsTests`, `AlertContactsTests`
- Use descriptive test method names: `MethodName_Scenario_ExpectedResult`
- Mark manual integration tests with `[Explicit]` attribute

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests for specific framework
dotnet test --framework net9.0

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Run specific test
dotnet test --filter "FullyQualifiedName~MonitorsTests.GetMonitor"

# Run tests with verbose output
dotnet test --verbosity detailed
```

### Test Coverage Goals

- **Unit Tests**: Aim for >90% coverage
- **Integration Tests**: Cover all major API paths
- **Edge Cases**: Test error conditions and boundaries

## Pull Request Process

### Before Submitting

1. **Update Your Branch**
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. **Run All Tests**
   ```bash
   dotnet test
   ```

3. **Check for Linter Warnings**
   ```bash
   dotnet build
   ```

4. **Update Documentation**
   - Update README.md if adding features
   - Update CHANGELOG.md with your changes
   - Add XML documentation to new public APIs

### Creating the Pull Request

1. **Push Your Branch**
   ```bash
   git push origin feature/your-feature-name
   ```

2. **Create PR on GitHub**
   - Use a clear, descriptive title
   - Reference related issues (e.g., "Fixes #123")
   - Provide detailed description of changes
   - Include screenshots/examples if applicable

### PR Description Template

```markdown
## Description
Brief description of the changes

## Related Issues
Fixes #123

## Type of Change
- [ ] Bug fix (non-breaking change that fixes an issue)
- [ ] New feature (non-breaking change that adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## Testing
- [ ] Unit tests added/updated
- [ ] All tests pass
- [ ] Manual testing completed

## Checklist
- [ ] Code follows project style guidelines
- [ ] Self-review completed
- [ ] XML documentation added
- [ ] Tests added/updated
- [ ] Documentation updated
- [ ] No new warnings introduced
```

### Review Process

1. **Automated Checks**: GitHub Actions will run tests automatically
2. **Code Review**: Maintainers will review your code
3. **Address Feedback**: Make requested changes
4. **Approval**: Once approved, your PR will be merged

### After Merge

1. **Delete Your Branch** (optional)
   ```bash
   git branch -d feature/your-feature-name
   git push origin --delete feature/your-feature-name
   ```

2. **Update Your Fork**
   ```bash
   git checkout main
   git pull upstream main
   git push origin main
   ```

## Reporting Issues

### Bug Reports

When reporting a bug, include:

1. **Description**: Clear description of the bug
2. **Steps to Reproduce**: Step-by-step instructions
3. **Expected Behavior**: What should happen
4. **Actual Behavior**: What actually happens
5. **Environment**: .NET version, OS, library version
6. **Code Sample**: Minimal reproducible example
7. **Stack Trace**: If applicable

### Bug Report Template

```markdown
## Bug Description
[Clear description of the bug]

## Steps to Reproduce
1. Step one
2. Step two
3. Step three

## Expected Behavior
[What should happen]

## Actual Behavior
[What actually happens]

## Environment
- Library Version: 2.0.0
- .NET Version: 9.0
- OS: Windows 11 / Ubuntu 22.04 / macOS Sonoma

## Code Sample
```csharp
var client = UptimeRobotClientFactory.Create("api-key");
var result = await client.GetMonitorsAsync();
// Bug occurs here
```

## Stack Trace
[If applicable]
```

## Feature Requests

### Proposing New Features

1. **Check Existing Issues**: Ensure it's not already requested
2. **Create Issue**: Open a new issue with "Feature Request" label
3. **Describe Feature**: Explain what and why
4. **Provide Examples**: Show how it would be used
5. **Discuss**: Engage in discussion with maintainers

### Feature Request Template

```markdown
## Feature Description
[Clear description of the feature]

## Use Case
[Why is this feature needed? What problem does it solve?]

## Proposed Solution
[How would this feature work?]

## Example Usage
```csharp
// Example of how the feature would be used
```

## Alternatives Considered
[Other approaches you've thought about]

## Additional Context
[Any other relevant information]
```

## Development Best Practices

### 1. Keep PRs Focused

- One feature/fix per PR
- Smaller PRs are easier to review
- Split large changes into multiple PRs

### 2. Write Clear Commit Messages

```
feat: add support for account details API
fix: correct pagination offset calculation
docs: update README with new examples
test: add tests for status page API
refactor: optimize BaseModel reflection caching
```

### 3. Update Documentation

- Keep docs in sync with code
- Add examples for new features
- Update API reference if needed

### 4. Performance Considerations

- Profile before optimizing
- Add benchmarks for performance changes
- Consider memory allocations
- Use `ConfigureAwait(false)` in library code

### 5. Backward Compatibility

- Avoid breaking changes when possible
- Mark deprecated APIs with `[Obsolete]`
- Provide migration path for breaking changes
- Update CHANGELOG.md for all breaking changes

## Getting Help

### Resources

- **Documentation**: Read the [README](../README.md) and [API Reference](API_REFERENCE.md)
- **Architecture**: See [ARCHITECTURE](ARCHITECTURE.md) for design details
- **Issues**: Browse existing [GitHub Issues](https://github.com/strvmarv/uptimerobot-dotnet/issues)
- **Discussions**: Join [GitHub Discussions](https://github.com/strvmarv/uptimerobot-dotnet/discussions)

### Asking Questions

- Check existing documentation first
- Search closed issues for similar questions
- Create a new discussion for general questions
- Create an issue for bug reports or feature requests

## Recognition

Contributors will be recognized in:

- GitHub contributors list
- Release notes for significant contributions
- CHANGELOG.md for major features

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to the UptimeRobot .NET Client Library! Your contributions help make this library better for everyone. 🎉

