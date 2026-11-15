[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucitext.obfuscation)](https://github.com/hmlendea/nucitext.obfuscation/releases/latest)
[![Build Status](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/dotnet.yml)
[![Security Scan](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/security.yml/badge.svg)](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/security.yml)
[![NuGet](https://img.shields.io/nuget/v/NuciText.Obfuscation)](https://nuget.org/packages/NuciText.Obfuscation)
[![License](https://img.shields.io/github/license/hmlendea/nucitext.obfuscation)](https://github.com/hmlendea/nucitext.obfuscation/blob/main/LICENSE)

# NuciText.Obfuscation

NuciText.Obfuscation is a lightweight .NET library for text obfuscation using visually similar Unicode characters.

## ✨ Capabilities

- Obfuscate readable text while preserving visual similarity
- Reverse known substitutions through deobfuscation
- Produce deterministic results with a fixed seed
- Optionally include approximate replacements for stronger obfuscation

## 🚀 Usage

```csharp
using NuciText.Obfuscation;

INuciTextObfuscator obfuscator = new NuciTextObfuscator();

string input = "Test string!";
string obfuscated = obfuscator.Obfuscate(input);
string restored = obfuscator.Deobfuscate(obfuscated);
```

### Deterministic Output

If you need reproducible output (for tests or snapshots), use a fixed seed.

```csharp
using NuciText.Obfuscation;

var options = new NuciTextObfuscatorOptions
{
    UseApproximateReplacements = true
};

INuciTextObfuscator obfuscator = new NuciTextObfuscator(123456789);

string plain = "Test string!";
string obfuscated = obfuscator.Obfuscate(plain, options);
// obfuscated == "Ꭲеst strіng!"

string restored = obfuscator.Deobfuscate(obfuscated);
// restored == "Test string!"
```

## 📦 Installation

[![Obtain it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciText.Obfuscation)

### Package Manager Installation

```bash
dotnet add package NuciText.Obfuscation
```

Or, via the `Package Manager Console`:
```powershell
Install-Package NuciText.Obfuscation
```

## 🏗️ Development

### Prerequisites

- .NET SDK compatible with the target framework (net10.0)

### Build

```bash
dotnet build
```

### Test

```bash
dotnet test
```

## ⚙️ GitHub Actions

| Workflow | Purpose | What it does |
|----------|---------|--------------|
| [ .NET ](https://github.com/hmlendea/nucitext.obfuscation/blob/master/.github/workflows/dotnet.yml) | Build and test | Restores dependencies, builds the solution, and runs the unit and architecture tests. |
| [ Security ](https://github.com/hmlendea/nucitext.obfuscation/blob/master/.github/workflows/security.yml) | Security scanning | Checks for vulnerable NuGet packages and runs CodeQL analysis, publishing results to GitHub Code Scanning. |
| [ NuGet Release ](https://github.com/hmlendea/nucitext.obfuscation/blob/master/.github/workflows/nuget-release.yml) | Package publication | Packs and pushes the NuGet package on release publication. |
| [ GitHub Release ](https://github.com/hmlendea/nucitext.obfuscation/blob/master/.github/workflows/github-release.yml) | Release assets | Builds the package, uploads it to the GitHub Release, and appends its SHA256 checksum. |

## 🤝 Contributing

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 📜 Code of Conduct

This project follows the [Code of Conduct](./CODE_OF_CONDUCT.md).

## 🔒 Security

NuciText.Obfuscation provides visual obfuscation only and is not a security control.

For information on reporting security vulnerabilities, see the [Security Policy](./SECURITY.md).

This repository runs automated security scans on every push and pull request to `master`, and weekly. The scan includes:
- **Vulnerable packages** — `dotnet list package --vulnerable --include-transitive`
- **Source code analysis** — CodeQL analysis for C#

See the [Security Scan workflow](.github/workflows/security.yml) for details.

## 🛡️ Privacy

See [PRIVACY.md](./PRIVACY.md) for the complete data-handling description.

## 🏛️ Architecture

See [ARCHITECTURE.md](./ARCHITECTURE.md) for the system architecture.

## 📄 License

By contributing, you agree that your contributions will be licensed under the [GNU General Public License v3.0 or later](./LICENSE).
