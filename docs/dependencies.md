# Dependencies

## Runtime Dependencies

**None.** The library has zero external runtime dependencies.

## Build Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.NET.Test.Sdk` | 17.13.0 | Test runner |
| `NUnit` | 4.3.2 | Test framework |
| `Moq` | 4.20.72 | Mocking framework |

## Framework Dependencies

| Framework | Version | Status |
|-----------|---------|--------|
| .NET | 10.0 | Target framework |
| .NET Standard | 2.1+ | Compatible via net10.0 |

## Dependency Graph

```
NuciText.Obfuscation (net10.0)
├── No runtime dependencies
└── NuciText.Obfuscation.UnitTests
    ├── Microsoft.NET.Test.Sdk (17.13.0)
    ├── NUnit (4.3.2)
    └── Moq (4.20.72)
```

## NuGet Package Dependencies

The published NuGet package `NuciText.Obfuscation` declares **no dependencies** in its `.nuspec`:

```xml
<dependencies>
  <group targetFramework="net10.0" />
</dependencies>
```

## Transitive Dependencies

None. Consumers only get the `NuciText.Obfuscation.dll` assembly.

## Version Compatibility

| Consumer Framework | Compatible |
|--------------------|------------|
| .NET 10.0+ | ✅ Yes |
| .NET 9.0 | ❌ No (net10.0 only) |
| .NET 8.0 | ❌ No |
| .NET Standard 2.1 | ✅ Yes (via net10.0) |
| .NET Framework 4.8.1 | ❌ No |

## Adding Dependencies

If dependencies are needed in the future:

1. Add to `NuciText.Obfuscation.csproj`:
   ```xml
   <ItemGroup>
     <PackageReference Include="PackageName" Version="x.y.z" />
   </ItemGroup>
   ```

2. Update this document

3. Consider impact on:
   - Package size
   - Attack surface
   - License compatibility (GPL-3.0-or-later)
   - Native dependencies (avoid if possible)

## Security

- No dependencies = minimal supply chain risk
- No native code = no platform-specific vulnerabilities
- Regular `dotnet list package --vulnerable` checks in CI (recommended)