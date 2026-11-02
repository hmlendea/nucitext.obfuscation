[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Latest GitHub release](https://img.shields.io/github/v/release/hmlendea/nucitext.obfuscation)](https://github.com/hmlendea/nucitext.obfuscation/releases/latest) [![Build Status](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/dotnet.yml) [![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# NuciText.Obfuscation

NuciText.Obfuscation is a lightweight .NET library for text obfuscation using visually similar Unicode characters.

It can:
- Obfuscate readable text while preserving visual similarity
- Reverse known substitutions through deobfuscation
- Produce deterministic results with a fixed seed
- Optionally include approximate replacements for stronger obfuscation

## Features

- Randomized obfuscation based on configurable seed
- Support for both single-character and grouped replacements
- Optional approximate substitutions via options
- Null-safe and empty-string-safe API behavior
- Simple API surface (`Obfuscate` / `Deobfuscate`)

## Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciText.Obfuscation)

### .NET CLI

```bash
dotnet add package NuciText.Obfuscation
```

### NuGet Package Manager

```powershell
Install-Package NuciText.Obfuscation
```

## Quick Start

```csharp
using NuciText.Obfuscation;

INuciTextObfuscator obfuscator = new NuciTextObfuscator();

string input = "Test string!";
string obfuscated = obfuscator.Obfuscate(input);
string restored = obfuscator.Deobfuscate(obfuscated);
```

## Deterministic Output

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

## API

`INuciTextObfuscator`
- `string Obfuscate(string text)`
- `string Obfuscate(string text, NuciTextObfuscatorOptions options)`
- `string Deobfuscate(string text)`

`NuciTextObfuscator` constructors
- `NuciTextObfuscator()`
- `NuciTextObfuscator(int seed)`
- `NuciTextObfuscator(string seed)`

`NuciTextObfuscatorOptions`
- `bool UseApproximateReplacements` (default: `false`)

## Behavior Notes

- Passing `null` returns `null`.
- Passing an empty string returns an empty string.
- Obfuscation is probabilistic by default.
- Deobfuscation restores known substitutions from the internal mapping tables.

## Development

### Prerequisites

- .NET SDK compatible with the target framework

## Project Structure

- `NuciText.Obfuscation/` - main library
- `NuciText.Obfuscation.UnitTests/` - NUnit test project

### Build

```bash
dotnet build NuciText.Obfuscation.csproj
```

### Run

```bash
dotnet run --project NuciText.Obfuscation.csproj
```

### Test

```bash
dotnet test
```

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Preserve the existing public contract unless a breaking change is intentional
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Include tests for additional or modified functionality
- Raise a new [issue](https://github.com/hmlendea/nucitext.obfuscation/issues) for problems or suggestions

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 📜 Code of Conduct

See the [Code of Conduct](./CODE_OF_CONDUCT.md) for the standards of conduct expected in all project spaces.

## 🛡️ Privacy and Data

NuciText.Obfuscation is a pure .NET text-transformation library with no network access, telemetry, or data collection. The library performs in-memory obfuscation and deobfuscation only. See [PRIVACY.md](./PRIVACY.md) for the complete data-handling description.

## License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.
