# Frequently Asked Questions

## General

### What is NuciText.Obfuscation?

A .NET library for reversible text obfuscation using homoglyph substitution. It replaces characters with visually similar Unicode alternatives.

### Is this encryption?

**No.** It provides visual obfuscation only. The transformation is reversible without a key. Do not use for security-sensitive data.

### What .NET versions are supported?

- **Target:** .NET 10.0 (net10.0)
- **Compatible:** .NET 10.0+, .NET Standard 2.1+
- **Not supported:** .NET Framework, .NET 8/9 (without multi-targeting)

### Are there any dependencies?

**Zero runtime dependencies.** The NuGet package has no dependencies.

## Usage

### How do I get deterministic output?

Use a fixed seed:

```csharp
var obfuscator = new NuciTextObfuscator(12345); // Same output every time
```

### Can I use a string as a seed?

Yes:

```csharp
var obfuscator = new NuciTextObfuscator("my-seed");
// Uses "my-seed".GetHashCode() internally
```

**Note:** `string.GetHashCode()` is not stable across .NET versions. Use `int` seeds for cross-version determinism.

### Why does calling Obfuscate twice give different results?

Each call advances the internal `Random` state. Create a new instance per operation for consistent results:

```csharp
// Different results
var o = new NuciTextObfuscator(12345);
o.Obfuscate("test");
o.Obfuscate("test");

// Same results
new NuciTextObfuscator(12345).Obfuscate("test");
new NuciTextObfuscator(12345).Obfuscate("test");
```

### Is it thread-safe?

**No.** The `Random` instance is not thread-safe. Use one instance per thread:

```csharp
ThreadLocal<NuciTextObfuscator> local = new(() => new NuciTextObfuscator(12345));
Parallel.For(0, 100, _ => local.Value.Obfuscate("test"));
```

### What's the difference between Identical and Approximate modes?

| Mode | Tables Used | Visual Fidelity | Strength |
|------|-------------|-----------------|----------|
| Identical (default) | IdenticalGroup + IdenticalReplacements | High | Lower |
| Approximate | All four tables | Variable | Higher |

Approximate mode uses Cherokee, Cyrillic, Greek, and other scripts for more variation.

### Why aren't all characters replaced?

- Only characters in mapping tables are replaced (~100 mappings)
- Each mappable character has ~60% chance of replacement
- Unmappable characters (emoji, CJK, Arabic, etc.) pass through unchanged

### Does Deobfuscate need the same options?

**No.** Deobfuscation works without options and handles both modes automatically.

```csharp
var o = new NuciTextObfuscator(12345);
var obf = o.Obfuscate("test", new NuciTextObfuscatorOptions { UseApproximateReplacements = true });
var deobf = o.Deobfuscate(obf); // Works, no options needed
```

## Technical

### What characters are supported?

Primarily Latin letters (A-Z, a-z), digits (0-9), and common punctuation sequences (ligatures, dashes, quotes, ellipsis).

### Can I add custom mappings?

Not currently. Mapping tables are `static readonly` and internal. Fork the repository to customize.

### How does the probabilistic replacement work?

For each mappable character/group, `Random.NextDouble() < 0.6` determines replacement. The seed controls the sequence.

### What's the maximum input length?

No hard limit. Memory scales with input (StringBuilder). Tested with 100KB+ strings.

### Can I serialize the obfuscator?

No. The class is not serializable. Serialize the seed instead:

```csharp
int seed = 12345;
string json = JsonSerializer.Serialize(seed);
// Later:
int restored = JsonSerializer.Deserialize<int>(json);
var o = new NuciTextObfuscator(restored);
```

## Integration

### How do I use it in ASP.NET Core?

```csharp
builder.Services.AddSingleton<INuciTextObfuscator>(_ => new NuciTextObfuscator(12345));

// In controller:
public IActionResult Obfuscate(string text) => Ok(_obfuscator.Obfuscate(text));
```

### Does it work with dependency injection?

Yes. Implement `INuciTextObfuscator` for mocking/testing.

### Can I use it in Blazor WebAssembly?

Yes. Pure .NET library, no native dependencies.

## Licensing

### What license?

GPL-3.0-or-later. Strong copyleft — derivative works must be GPL-compatible.

### Can I use it in a commercial product?

Only if your product is also GPL-3.0-or-later licensed. For proprietary use, contact the author for alternative licensing.

### Can I modify and redistribute?

Yes, under GPL-3.0-or-later terms. Source must be made available.

## Troubleshooting

### Output shows boxes (□) or wrong characters

Your font doesn't support the Unicode characters used (Cherokee, Cyrillic, Greek). Use identical mode or install a font with broader Unicode coverage.

### Deobfuscation fails on some inputs

Ensure you're using the same seed and that the obfuscated text wasn't modified. Round-trip is guaranteed for unmodified output.

### Performance is slow

The library is fast (~µs per call). If you see slowness:
- First `Deobfuscate` builds reverse map (one-time cost)
- Consider instance pooling for high throughput
- Profile your specific usage

## Support

### Where do I report bugs?

[GitHub Issues](https://github.com/hmlendea/nucitext.obfuscation/issues)

### Where is the source code?

https://github.com/hmlendea/nucitext.obfuscation

### Is there a changelog?

See [change-guide.md](change-guide.md) and GitHub Releases.