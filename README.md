[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucitext.obfuscation)](https://github.com/hmlendea/nucitext.obfuscation/releases/latest)
[![Build Status](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/dotnet.yml)
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

## 🤝 Contributing

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 📜 Code of Conduct

This project follows the [Code of Conduct](./CODE_OF_CONDUCT.md).

## 🔒 Security

Report security vulnerabilities per the [Security Policy](./SECURITY.md).

## 🛡️ Privacy

See [PRIVACY.md](./PRIVACY.md) for the complete data-handling description.

## 🏛️ Architecture

See [ARCHITECTURE.md](./ARCHITECTURE.md) for the system architecture.

## 📄 License

By contributing, you agree that your contributions will be licensed under the [GNU General Public License v3.0 or later](./LICENSE).
