# Release Checklist for v2.0.0-rc.1

This document outlines the steps for releasing the v2.0.0 Release Candidate.

## Pre-Release Verification ✅

### Build & Tests
- [x] All tests passing (156/156 succeeded)
- [x] Builds successfully on all target frameworks:
  - [x] .NET 9.0
  - [x] .NET 8.0
  - [x] .NET 6.0
  - [x] .NET Standard 2.0
- [x] Release build completed without errors
- [x] NuGet package created: `UptimeRobotDotnet.2.0.0-rc.1.nupkg`

### Code Quality
- [x] Nullable reference types enabled
- [x] No critical compiler warnings
- [x] XML documentation complete for public APIs
- [x] EditorConfig in place
- [x] Code follows .NET conventions

### Documentation
- [x] README.md updated with RC warning
- [x] CHANGELOG.md reflects RC status
- [x] Migration guide included
- [x] API_REFERENCE.md created
- [x] ARCHITECTURE.md created
- [x] CONTRIBUTING.md created
- [x] Installation instructions for RC version

### Package Metadata
- [x] Version set to `2.0.0-rc.1`
- [x] Package description includes RC warning
- [x] Release notes indicate this is a Release Candidate
- [x] All target frameworks listed
- [x] Package tags updated

## Release Steps

### 1. GitHub Preparation

```bash
# Ensure you're on main branch
git checkout main
git pull origin main

# Tag the release
git tag -a v2.0.0-rc.1 -m "Release Candidate 1 for v2.0.0"
git push origin v2.0.0-rc.1
```

### 2. Create GitHub Release

1. Go to: https://github.com/strvmarv/uptimerobot-dotnet/releases/new
2. Select tag: `v2.0.0-rc.1`
3. Release title: `v2.0.0-rc.1 - Release Candidate`
4. Mark as: **✅ Pre-release**
5. Release notes template:

```markdown
# v2.0.0-rc.1 - Release Candidate

⚠️ **This is a pre-release version for testing purposes. Do not use in production without thorough testing.**

## What's New in v2.0

This release candidate includes all planned features for v2.0.0:

### Major Features
- ✨ Complete API coverage: Monitors, Alert Contacts, Maintenance Windows, Status Pages
- 🎯 Strongly-typed enums (no more magic numbers)
- 🔄 Automatic pagination with `IAsyncEnumerable`
- 🛡️ Nullable reference types throughout
- 🚀 .NET 9.0 support (plus .NET 8.0, 6.0, and Standard 2.0)
- 📝 Comprehensive XML documentation
- ⚡ Performance optimizations

### Breaking Changes
- **Property Naming**: All properties now use PascalCase (was snake_case)
- **Enum Types**: Monitor types, statuses, etc. are now strongly-typed enums
- **Method Names**: Updated to async naming conventions (e.g., `GetMonitorsAsync()`)
- **Exception Types**: New custom exception hierarchy

## Installation

Install the release candidate:

```bash
dotnet add package UptimeRobotDotnet --version 2.0.0-rc.1
```

## Testing & Feedback

We need your help to make v2.0.0 stable! Please test:

1. Migration from v1.x
2. All API endpoints
3. Pagination with large datasets
4. Exception handling
5. Multi-framework compatibility

**Report Issues**: [Create an issue](https://github.com/strvmarv/uptimerobot-dotnet/issues/new) with the "v2.0.0-rc" label

## Migration Guide

See [CHANGELOG.md](https://github.com/strvmarv/uptimerobot-dotnet/blob/main/CHANGELOG.md) for detailed migration instructions.

### Quick Migration Example

```csharp
// v1.x
var parameters = new MonitorCreateParameters
{
    Friendly_Name = "My Monitor",
    Type = 1,  // HTTP
    Custom_Http_Headers = headers
};
var result = await client.MonitorCreate(parameters);

// v2.0.0-rc.1
var parameters = new MonitorCreateParameters
{
    FriendlyName = "My Monitor",
    Type = MonitorType.HTTP,  // Strongly-typed
    CustomHttpHeaders = headers
};
var result = await client.CreateMonitorAsync(parameters);
```

## Documentation

- 📖 [API Reference](https://github.com/strvmarv/uptimerobot-dotnet/blob/main/docs/API_REFERENCE.md)
- 🏗️ [Architecture](https://github.com/strvmarv/uptimerobot-dotnet/blob/main/docs/ARCHITECTURE.md)
- 🤝 [Contributing](https://github.com/strvmarv/uptimerobot-dotnet/blob/main/docs/CONTRIBUTING.md)

## Timeline

- **RC Phase**: 2-4 weeks for testing and feedback
- **RC.2+**: Additional candidates if critical issues found
- **Final Release**: v2.0.0 stable after successful RC testing

## Full Changelog

See [CHANGELOG.md](https://github.com/strvmarv/uptimerobot-dotnet/blob/main/CHANGELOG.md) for complete details.

---

**Thank you for helping test v2.0.0!** 🎉
```

6. Attach the NuGet package: `UptimeRobotDotnet.2.0.0-rc.1.nupkg`

### 3. Publish to NuGet.org

```bash
# Publish to NuGet (if automated push isn't set up)
dotnet nuget push UptimeRobotDotnet.2.0.0-rc.1.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

**Note**: Pre-release versions won't be installed by default - users must explicitly request the version.

### 4. Announce the Release Candidate

#### GitHub Discussions
Create a discussion post:
- **Category**: Announcements
- **Title**: "v2.0.0 Release Candidate Available for Testing"
- **Content**: Link to release notes, request feedback

#### Social Media (Optional)
- Twitter/X
- Reddit (r/dotnet, r/csharp)
- Dev.to blog post

## Post-Release Monitoring

### Monitor for Issues
- [ ] Watch GitHub Issues for bugs with "v2.0.0-rc" label
- [ ] Monitor NuGet download stats
- [ ] Check for discussion activity
- [ ] Review test results from community

### Feedback Collection Period
- **Duration**: 2-4 weeks
- **Critical Issues**: Address immediately with RC.2
- **Minor Issues**: Collect for final release
- **Success Metrics**: 
  - No critical bugs reported
  - Positive community feedback
  - Successful migration stories

## Path to Stable Release

### If Critical Issues Found
1. Fix issues in main branch
2. Increment RC version (e.g., 2.0.0-rc.2)
3. Repeat release process
4. Extend testing period

### If No Critical Issues
1. Wait for feedback period to complete (2-4 weeks)
2. Address any minor issues
3. Update version to `2.0.0` (remove `-rc.1`)
4. Update all documentation to remove RC warnings
5. Create final release
6. Announce stable release

## Success Criteria for Stable Release

- [ ] No critical bugs reported during RC period
- [ ] At least 5 successful migration reports
- [ ] Positive community feedback
- [ ] All documentation reviewed and updated
- [ ] Final testing on all target frameworks
- [ ] Performance benchmarks meet expectations

## Rollback Plan

If critical issues are discovered that cannot be quickly fixed:

1. Mark GitHub release as "yanked"
2. Add warning to README
3. Unpublish pre-release from NuGet (if possible)
4. Communicate issue to users
5. Plan for RC.2 with fixes

## Communication Templates

### Issue Response Template
```markdown
Thank you for testing v2.0.0-rc.1 and reporting this issue!

This will be addressed before the stable v2.0.0 release. 

- [ ] Confirmed bug
- [ ] Fix planned for: RC.2 / Final Release
- [ ] Workaround available: [describe if applicable]

We appreciate your help making v2.0.0 stable!
```

### Success Story Response
```markdown
Thank you for sharing your successful migration to v2.0.0-rc.1!

Your feedback is valuable and helps us ensure a stable final release. 

If you encounter any issues, please don't hesitate to open an issue.
```

## Checklist Before Final v2.0.0

- [ ] All RC feedback addressed
- [ ] Documentation updated (remove RC warnings)
- [ ] Version bumped to 2.0.0
- [ ] GitHub release created (not pre-release)
- [ ] NuGet package published
- [ ] Release announcement published
- [ ] Archive this checklist for reference

## Notes

- Package size: ~284 KB
- Target frameworks: 4 (net9.0, net8.0, net6.0, netstandard2.0)
- Total tests: 165 (156 passing, 9 explicit manual tests)
- Documentation: 5 markdown files (~5,000+ lines)

## Contact

For questions about the release process:
- GitHub Issues: https://github.com/strvmarv/uptimerobot-dotnet/issues
- GitHub Discussions: https://github.com/strvmarv/uptimerobot-dotnet/discussions

