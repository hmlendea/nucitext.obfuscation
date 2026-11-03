# Components Index

## Core Components

| Component | Description | Document |
|-----------|-------------|----------|
| `NuciTextObfuscator` | Main obfuscation implementation | [nuci-textobfuscator.md](nuci-textobfuscator.md) |
| `NuciTextObfuscatorOptions` | Configuration options | [options.md](options.md) |
| Mapping Tables | Static replacement dictionaries | [mapping-tables.md](mapping-tables.md) |

## Interface

| Interface | Description | Document |
|-----------|-------------|----------|
| `INuciTextObfuscator` | Public contract | [../api-reference/inuci-textobfuscator.md](../api-reference/inuci-textobfuscator.md) |

## Relationships

```
INuciTextObfuscator (interface)
    ▲
    │ implements
    │
NuciTextObfuscator (class)
    │
    ├── uses ──► NuciTextObfuscatorOptions
    │
    └── uses ──► Mapping Tables (static)
        ├── IdenticalGroupReplacements
        ├── ApproximateGroupReplacements
        ├── IdenticalReplacements
        └── ApproximateReplacements
```

## Quick Reference

### NuciTextObfuscator

```csharp
// Constructors
new NuciTextObfuscator()
new NuciTextObfuscator(int seed)
new NuciTextObfuscator(string seed)

// Methods
string? Obfuscate(string? input)
string? Obfuscate(string? input, NuciTextObfuscatorOptions? options)
string? Deobfuscate(string? input)
```

### NuciTextObfuscatorOptions

```csharp
public class NuciTextObfuscatorOptions
{
    public bool UseApproximateReplacements { get; set; } = false;
}
```

### Mapping Tables

Four `static readonly Dictionary<string, string[]>` tables with ~180 total entries covering:
- Latin letters (A-Z, a-z)
- Digits (0-9)
- Ligatures (ff, fi, fl, ffi, ffl, st, æ, œ, ij)
- Punctuation sequences (..., ---, --, <<, >>, '', ``, ''', ```)