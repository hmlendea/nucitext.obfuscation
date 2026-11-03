# NuciTextObfuscator Component

## Overview

Main implementation of `INuciTextObfuscator`. Provides reversible text obfuscation via homoglyph substitution.

## Class Definition

```csharp
public sealed class NuciTextObfuscator : INuciTextObfuscator
{
    // Constructors
    public NuciTextObfuscator();
    public NuciTextObfuscator(int seed);
    public NuciTextObfuscator(string seed);

    // Methods
    public string? Obfuscate(string? input);
    public string? Obfuscate(string? input, NuciTextObfuscatorOptions? options);
    public string? Deobfuscate(string? input);
}
```

## Constructors

### Default Constructor

```csharp
public NuciTextObfuscator()
```

- Uses `Environment.TickCount` as seed
- Non-deterministic across runs
- Suitable for one-time obfuscation

### Int Seed Constructor

```csharp
public NuciTextObfuscator(int seed)
```

- Deterministic output for given seed
- Recommended for testing and reproducible obfuscation
- Accepts any `int` value (including negative, zero)

### String Seed Constructor

```csharp
public NuciTextObfuscator(string seed)
```

- Uses `seed.GetHashCode()` internally
- Enables semantic seeds ("user-123", "session-abc")
- **Warning:** `GetHashCode()` not stable across .NET versions

## Methods

### Obfuscate (No Options)

```csharp
public string? Obfuscate(string? input)
```

- Uses identical replacements only
- Equivalent to `Obfuscate(input, null)`

### Obfuscate (With Options)

```csharp
public string? Obfuscate(string? input, NuciTextObfuscatorOptions? options)
```

- `options.UseApproximateReplacements = true` enables approximate mappings
- `null` options uses defaults (identical only)

### Deobfuscate

```csharp
public string? Deobfuscate(string? input)
```

- Reverses both identical and approximate mappings
- No options parameter needed
- Builds reverse map on first call (lazy, thread-safe)

## Internal Algorithm

### Obfuscation Flow

```
Input String
    │
    ▼
┌─────────────────────────────────────┐
│ 1. Group Replacements (Greedy)      │
│    - IdenticalGroupReplacements     │
│    - ApproximateGroupReplacements   │
│    - Longest match wins             │
└─────────────────────────────────────┘
    │
    ▼
┌─────────────────────────────────────┐
│ 2. Single Character Replacements    │
│    - IdenticalReplacements          │
│    - ApproximateReplacements        │
│    - 60% probability per character  │
└─────────────────────────────────────┘
    │
    ▼
Output String
```

### Deobfuscation Flow

```
Obfuscated String
    │
    ▼
┌─────────────────────────────────────┐
│ Reverse Map Lookup (All Tables)     │
│ - Built lazily on first call        │
│ - Thread-safe double-checked lock   │
└─────────────────────────────────────┘
    │
    ▼
Original String
```

## State

### Instance State

```csharp
private readonly Random _random;
```

- Initialized in constructor
- Advanced on each `Obfuscate` call
- **Not thread-safe**

### Static State

```csharp
private static readonly Dictionary<string, string[]> IdenticalGroupReplacements;
private static readonly Dictionary<string, string[]> ApproximateGroupReplacements;
private static readonly Dictionary<string, string[]> IdenticalReplacements;
private static readonly Dictionary<string, string[]> ApproximateReplacements;

private static Dictionary<string, string>? _reverseMap;
private static readonly object _reverseMapLock = new();
```

- Mapping tables: initialized at type load, immutable
- Reverse map: lazy-built on first `Deobfuscate`, cached forever

## Usage Examples

```csharp
// Basic
var o = new NuciTextObfuscator();
var obf = o.Obfuscate("Hello");
var orig = o.Deobfuscate(obf);

// Deterministic
var o = new NuciTextObfuscator(12345);
var obf = o.Obfuscate("Hello"); // Always same

// Approximate
var o = new NuciTextObfuscator(12345);
var opts = new NuciTextObfuscatorOptions { UseApproximateReplacements = true };
var obf = o.Obfuscate("Hello", opts);

// String seed
var o = new NuciTextObfuscator("user-session-123");
```

## Thread Safety

**Not thread-safe.** See [concurrency-and-scheduling.md](../concurrency-and-scheduling.md).

## Performance

- Obfuscate: ~0.5 µs per 10 chars
- Deobfuscate: ~0.3 µs per 10 chars (after warmup)
- First Deobfuscate: +~10 µs (reverse map build)
- Memory: ~64 bytes per instance + ~20 KB static

## Related Documents

- [INuciTextObfuscator API](../api-reference/inuci-textobfuscator.md)
- [Options](../components/options.md)
- [Mapping Tables](../components/mapping-tables.md)
- [Obfuscation Flow](../flows/obfuscation.md)
- [Deobfuscation Flow](../flows/deobfuscation.md)

| Original | Replacements |
|----------|--------------|
| `II` | `Ⅱ` |
| `III` | `Ⅲ` |
| `IV` | `Ⅳ` |
| `VI` | `Ⅵ` |
| ... | ... |
| `ae` | `æӕ` |
| `bl` | `Ы` |
| `dz` | `ǳʣ` |
| ... | ... |

### IdenticalReplacements

`Dictionary<char, string>` — Single-character exact visual substitutions.

| Original | Replacements |
|----------|--------------|
| `` ` `` | `՝` |
| `,` | `͵` |
| `:` | `։꞉` |
| `;` | `;` |
| `'` | `′´ʹ‘’` |
| `"` | `”″` |
| `|` | `ǀ` |
| `/` | `⁄⧸` |
| `\` | `⧵⧹` |
| `-` | `‐–` |
| `+` | `𖫵` |
| `<` | `ᐸ𖫬ⵦ` |
| `>` | `ᐳ` |
| `3` | `ƷЗӠ` |
| `A` | `АΑꓮ` |
| `B` | `ВΒꓐ` |
| ... | ... |

### ApproximateReplacements

`Dictionary<char, string>` — Single-character approximate visual substitutions (enabled via options).

| Original | Replacements |
|----------|--------------|
| `'` | `ʹ` |
| `3` | `ӠЗ` |
| `5` | `Ƽ` |
| `6` | `бᏮ` |
| `A` | `ᗅᎪ` |
| `B` | `Ᏼᗷꕗ` |
| ... | ... |

## Methods

### Obfuscate(string text)

```csharp
public string Obfuscate(string text)
    => Obfuscate(text, new NuciTextObfuscatorOptions());
```

Delegates to overload with default options.

### Obfuscate(string text, NuciTextObfuscatorOptions options)

Transforms input text by:
1. Null/empty check
2. Group replacements (identical + approximate if enabled)
3. Character-by-character processing with probabilistic substitution (60% chance)
4. Result construction via `StringBuilder`

**Probabilistic behaviour:** Each character/group has a 60% chance of being replaced when candidates exist.

### Deobfuscate(string text)

Reverses obfuscation by:
1. Null/empty check
2. Reverse group replacements (identical + approximate)
3. Character-by-character reverse lookup
4. Result construction via `StringBuilder`

**Deterministic:** No randomness; always reverses known mappings.

## Thread Safety

- Stateless except for `RandomGenerator` (seeded per instance)
- Safe for concurrent use when each thread has its own instance
- Static mapping tables are readonly and thread-safe

## Dependencies

- `System.Random` (BCL)
- `System.Collections.Generic` (BCL)
- `System.Text.StringBuilder` (BCL)
- `NuciTextObfuscatorOptions` (internal)