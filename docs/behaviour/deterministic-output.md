# Deterministic Output

## Overview

Achieving reproducible obfuscation results using fixed seeds.

## Usage

### Integer Seed

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator(123456789);
string input = "Test string!";
string obfuscated = obfuscator.Obfuscate(input);
// obfuscated == "Ꭲеst strіng!" (always with this seed)
```

### String Seed

```csharp
var obfuscator = new NuciTextObfuscator("my-secret-seed");
string obfuscated = obfuscator.Obfuscate("Test string!");
// Deterministic based on string.GetHashCode()
```

### With Options

```csharp
var options = new NuciTextObfuscatorOptions
{
    UseApproximateReplacements = true
};

var obfuscator = new NuciTextObfuscator(123456789);
string obfuscated = obfuscator.Obfuscate("Test string!", options);
// Deterministic with seed + options combination
```

## Guarantees

| Condition | Deterministic? |
|-----------|----------------|
| Same seed + same input + same options | ✅ Yes |
| Same seed + same input + different options | ❌ No (different mapping tables) |
| Different seed + same input + same options | ❌ No |
| Default constructor (no seed) | ❌ No (uses `Environment.TickCount`) |

## Implementation

- `Random` instance created with seed in constructor
- All probabilistic choices use `RandomGenerator.Next(1, 11) <= 6`
- Group replacements use `RandomGenerator.Next(candidates.Length)`
- Character replacements use `RandomGenerator.Next(candidates.Length)`

## Use Cases

- Unit testing (fixed expected outputs)
- Snapshots and golden files
- Reproducible builds
- Debugging specific obfuscation patterns

## Test Reference

See `NuciText.Obfuscation.UnitTests/NuciTextObfuscatorTests.cs`:
- `TestSeed = 123456789`
- `TestPlainString = "Test string!"`
- `TestObfuscatedString = "Ꭲеst strіng!"`