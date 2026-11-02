# Repository Overview

## Purpose

NuciText.Obfuscation is a lightweight .NET library for text obfuscation using visually similar Unicode characters. It enables applications to transform readable text into visually similar but obfuscated forms, and reverse the transformation.

## Scope

**In scope:**
- Text obfuscation via Unicode character substitution
- Text deobfuscation (reverse transformation)
- Deterministic output with fixed seeds
- Optional approximate replacements for stronger obfuscation
- Zero external dependencies

**Out of scope:**
- Network operations
- Persistent storage
- Cryptographic security
- Natural language processing
- File I/O

## Entry Points

### Public API

- `INuciTextObfuscator` — Interface defining the contract
- `NuciTextObfuscator` — Concrete implementation
- `NuciTextObfuscatorOptions` — Configuration options

### Methods

| Method | Description |
|--------|-------------|
| `Obfuscate(string)` | Obfuscates text with default options |
| `Obfuscate(string, NuciTextObfuscatorOptions)` | Obfuscates text with custom options |
| `Deobfuscate(string)` | Reverses obfuscation for known mappings |

### Constructors

| Constructor | Description |
|-------------|-------------|
| `NuciTextObfuscator()` | Uses `Environment.TickCount` as seed |
| `NuciTextObfuscator(int seed)` | Uses specified integer seed |
| `NuciTextObfuscator(string seed)` | Uses string hash code as seed |

## Target Framework

- .NET 10.0 (net10.0)

## Distribution

- NuGet package: `NuciText.Obfuscation`
- License: GPL-3.0-or-later
- Repository: https://github.com/hmlendea/nucitext.obfuscation

## Key Characteristics

- **Zero dependencies** — No external packages, fully self-contained
- **Thread-safe** — Stateless operations, safe for singleton or transient use
- **Deterministic** — Fixed seed produces identical output
- **Cross-platform** — Pure managed code, runs on Windows, Linux, macOS
- **Minimal API** — Three public types, three methods