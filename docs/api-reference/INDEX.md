# API Reference Index

## Overview

NuciText.Obfuscation exposes a minimal public API with three types.

## Types

| Type | Kind | Description |
|------|------|-------------|
| `INuciTextObfuscator` | Interface | Public contract for obfuscation/deobfuscation |
| `NuciTextObfuscator` | Class | Concrete implementation |
| `NuciTextObfuscatorOptions` | Class | Configuration options |

## Interface: INuciTextObfuscator

**Namespace:** `NuciText.Obfuscation`
**File:** `INuciText.Obfuscation.cs`

### Methods

| Method | Returns | Description |
|--------|---------|-------------|
| `Obfuscate(string text)` | `string` | Obfuscates text with default options |
| `Obfuscate(string text, NuciTextObfuscatorOptions options)` | `string` | Obfuscates text with custom options |
| `Deobfuscate(string text)` | `string` | Reverses obfuscation for known mappings |

### Behaviour Summary

- **Null input:** Returns null
- **Empty string:** Returns empty string
- **Deterministic:** Same seed + same input + same options = same output
- **Thread-safe:** Stateless operations (per-instance Random)

[Detailed Documentation](./inuci-textobfuscator.md)

## Class: NuciTextObfuscator

**Namespace:** `NuciText.Obfuscation`
**File:** `NuciText.Obfuscation.cs`
**Implements:** `INuciTextObfuscator`

### Constructors

| Constructor | Description |
|-------------|-------------|
| `NuciTextObfuscator()` | Seed from `Environment.TickCount` |
| `NuciTextObfuscator(int seed)` | Explicit integer seed |
| `NuciTextObfuscator(string seed)` | String hash code as seed |

### Methods

Inherits all methods from `INuciTextObfuscator`.

[Detailed Documentation](../components/nuci-textobfuscator.md)

## Class: NuciTextObfuscatorOptions

**Namespace:** `NuciText.Obfuscation`
**File:** `NuciTextObfuscatorOptions.cs`

### Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `UseApproximateReplacements` | `bool` | `false` | Enable approximate substitutions |

[Detailed Documentation](../components/options.md)

## Usage Example

```csharp
using NuciText.Obfuscation;

// Basic usage
var obfuscator = new NuciTextObfuscator();
string obfuscated = obfuscator.Obfuscate("Hello world!");
string restored = obfuscator.Deobfuscate(obfuscated);

// Deterministic with seed
var seeded = new NuciTextObfuscator(12345);
string deterministic = seeded.Obfuscate("Test");

// With approximate replacements
var options = new NuciTextObfuscatorOptions { UseApproximateReplacements = true };
string strong = obfuscator.Obfuscate("Secret", options);
```

## Versioning

- Current version: 1.1.1
- Target framework: net10.0
- Breaking changes require major version bump