# Build and Deployment

## Build Pipeline

### Local Build

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test
```

### CI Pipeline (GitHub Actions)

**File:** `.github/workflows/dotnet.yml`

**Triggers:**
- Push to `master`
- Pull requests to `master`

**Jobs:**
1. **Build** — `dotnet build --no-restore`
2. **Test** — `dotnet test --no-build --verbosity normal`

**Environment:**
- Runner: `ubuntu-latest`
- .NET Version: `10.0.x`

### Release Pipeline (GitHub Actions)

**File:** `.github/workflows/nuget-release.yml`

**Trigger:** GitHub Release published

**Jobs:**
1. **Publish** — Creates and pushes NuGet package

**Steps:**
1. Checkout with full history (`fetch-depth: 0`)
2. Setup .NET 10.0.x
3. Extract version from Git tag (strips `v` prefix)
4. Restore dependencies
5. Pack: `dotnet pack -c Release --output ./nupkg -p:Version=<version>`
6. NuGet login via OIDC
7. Push: `dotnet nuget push ... --source https://api.nuget.org/v3/index.json --skip-duplicate`

**Permissions:**
- `contents: read`
- `id-token: write` (for NuGet OIDC)

## Target Framework

- **Primary:** net10.0 (.NET 10.0)
- **Compatibility:** .NET 10.0+, .NET Standard 2.1+ (via net10.0)

## Package Configuration

**Project:** `NuciText.Obfuscation/NuciText.Obfuscation.csproj`

```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <RootNamespace>NuciText.Obfuscation</RootNamespace>
  <Version>1.1.1</Version>
  <Description>NuciText Obfuscation</Description>
  <Authors>Horațiu Mlendea</Authors>
  <Copyright>Copyright 2026 © Horațiu Mlendea</Copyright>
  <RepositoryUrl>https://github.com/hmlendea/nucitext.obfuscation</RepositoryUrl>
  <PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
  <PackageTags>Text Obfuscation</PackageTags>
  <ImplicitUsings>disable</ImplicitUsings>
  <Nullable>enable</Nullable>
  <LangVersion>latest</LangVersion>
</PropertyGroup>
```

## NuGet Package

- **Package ID:** `NuciText.Obfuscation`
- **License:** GPL-3.0-or-later
- **Repository:** https://github.com/hmlendea/nucitext.obfuscation
- **Published to:** nuget.org

## Versioning

- Semantic versioning (MAJOR.MINOR.PATCH)
- Version set in `.csproj` and overridden at pack time from Git tag
- Breaking changes → MAJOR version bump
- New features → MINOR version bump
- Bug fixes → PATCH version bump

## Deployment Environments

| Environment | Purpose | Trigger |
|-------------|---------|---------|
| Development | Local development | Manual |
| CI | Validation | Push/PR to master |
| NuGet.org | Public distribution | GitHub Release |

## Artefacts

- `NuciText.Obfuscation.{version}.nupkg` — NuGet package
- `NuciText.Obfuscation.dll` — Library assembly
- `NuciText.Obfuscation.pdb` — Debug symbols (included in package)

## Requirements

- .NET SDK 10.0.x
- NuGet CLI (for manual push)
- GitHub CLI or web UI (for releases)