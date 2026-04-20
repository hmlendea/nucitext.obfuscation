[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Latest GitHub release](https://img.shields.io/github/v/release/hmlendea/nucitext.obfuscation)](https://github.com/hmlendea/nucitext.obfuscation/releases/latest) [![Build Status](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucitext.obfuscation/actions/workflows/dotnet.yml)

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

## Contributing

Contributions are welcome.

When contributing:

- keep the project cross-platform
- preserve the existing public API unless a breaking change is intentional
- keep the changes focused and consistent with the the current coding style
- update the documentation when behavior changes
- include tests for any new behavior

## License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.
