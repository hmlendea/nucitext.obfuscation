# Change Guide

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.1.1 | 2026 | Current version |
| 1.1.0 | 2026 | Added approximate replacements |
| 1.0.0 | 2026 | Initial release |

## Breaking Changes

### 1.0.0 → 1.1.0

**Added:** `NuciTextObfuscatorOptions` parameter to `Obfuscate` method.

```csharp
// 1.0.0
string Obfuscate(string? input);

// 1.1.0
string Obfuscate(string? input);
string Obfuscate(string? input, NuciTextObfuscatorOptions? options);
```

**Impact:** Source compatible (overload added). Binary compatible.

### 1.1.0 → 1.1.1

No breaking changes.

## Migration Guide

### To 1.1.0+

No code changes required. New overload is optional.

```csharp
// Old code still works
var obfuscator = new NuciTextObfuscator();
string result = obfuscator.Obfuscate("text");

// New capability
var options = new NuciTextObfuscatorOptions { UseApproximateReplacements = true };
string result = obfuscator.Obfuscate("text", options);
```

## Deprecation Policy

- No APIs deprecated in current version
- Future deprecations will:
  1. Mark with `[Obsolete]` attribute
  2. Document in release notes
  3. Provide migration path
  4. Remove in next MAJOR version

## Upgrade Checklist

- [ ] Update NuGet package: `dotnet add package NuciText.Obfuscation --version 1.1.1`
- [ ] Rebuild solution: `dotnet build`
- [ ] Run tests: `dotnet test`
- [ ] Verify round-trip still works with your seeds

## Future Changes (Planned)

| Version | Planned Changes | Breaking |
|---------|-----------------|----------|
| 1.2.0 | Multi-targeting (net8.0, net9.0, net10.0) | No |
| 2.0.0 | Async API, streaming support | Yes |
| 2.0.0 | Configurable replacement probability | Yes |

## Contributing Changes

See [CONTRIBUTING.md](../CONTRIBUTING.md) for contribution guidelines.

## Release Process

1. Update version in `NuciText.Obfuscation.csproj`
2. Update `CHANGELOG.md` (if exists)
3. Commit and push to `master`
4. Create GitHub Release with tag `v{version}`
5. GitHub Actions publishes to NuGet.org automatically

## Rollback Procedure

If a release has issues:

1. Do not delete the NuGet package (immutable)
2. Release patch version with fix
3. Document known issues in release notes
4. Consider deprecating problematic version in README