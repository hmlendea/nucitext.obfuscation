# Troubleshooting

## Common Issues

### 1. Different Output Each Call

**Problem:** Calling `Obfuscate` twice with same input gives different results.

**Cause:** Each call advances the internal `Random` state.

**Solution:** Create new instance per operation or use same instance for deterministic sequence.

```csharp
// Wrong - different results
var o = new NuciTextObfuscator(12345);
o.Obfuscate("test"); // Result A
o.Obfuscate("test"); // Result B (different!)

// Right - same result
var o1 = new NuciTextObfuscator(12345);
var o2 = new NuciTextObfuscator(12345);
o1.Obfuscate("test"); // Result A
o2.Obfuscate("test"); // Result A (same)
```

### 2. Thread Safety Issues

**Problem:** Corrupted output or exceptions under concurrent access.

**Cause:** `Random` is not thread-safe.

**Solution:** Use one instance per thread.

```csharp
// Wrong
var o = new NuciTextObfuscator(12345);
Parallel.For(0, 100, _ => o.Obfuscate("test"));

// Right - ThreadLocal
ThreadLocal<NuciTextObfuscator> local = new(() => new NuciTextObfuscator(12345));
Parallel.For(0, 100, _ => local.Value.Obfuscate("test"));

// Right - new instance per call
Parallel.For(0, 100, _ => new NuciTextObfuscator(12345).Obfuscate("test"));
```

### 3. Null Reference Exception on String Seed

**Problem:** `ArgumentNullException` when creating with string seed.

**Cause:** Passing `null` to string constructor.

**Solution:** Validate seed before passing.

```csharp
// Wrong
var o = new NuciTextObfuscator((string)null); // Throws

// Right
string seed = GetSeedFromConfig() ?? "default-seed";
var o = new NuciTextObfuscator(seed);
```

### 4. Deobfuscation Doesn't Restore Original

**Problem:** `Deobfuscate(Obfuscate(x)) != x`

**Causes:**
- Different seeds used
- Different instances (state advanced)
- Approximate options used for obfuscation but not needed for deobfuscation

**Solutions:**
```csharp
// Use same instance
var o = new NuciTextObfuscator(12345);
var obf = o.Obfuscate("test");
var deobf = o.Deobfuscate(obf); // Works

// Or same seed, new instances
var o1 = new NuciTextObfuscator(12345);
var obf = o1.Obfuscate("test");
var o2 = new NuciTextObfuscator(12345);
var deobf = o2.Deobfuscate(obf); // Works

// Deobfuscation doesn't need options
var o = new NuciTextObfuscator(12345);
var obf = o.Obfuscate("test", new NuciTextObfuscatorOptions { UseApproximateReplacements = true });
var deobf = o.Deobfuscate(obf); // Works - no options needed
```

### 5. Characters Not Obfuscated

**Problem:** Some characters pass through unchanged.

**Cause:** Character not in any mapping table.

**Solution:** This is expected behaviour. Only mapped characters are replaced.

```csharp
// Emoji, Chinese, Arabic, etc. are not mapped
var o = new NuciTextObfuscator(12345);
o.Obfuscate("Hello 世界 🎉"); // "Ꮋеllo 世界 🎉" - only Latin chars replaced
```

### 6. Approximate Replacements Not Working

**Problem:** `UseApproximateReplacements = true` but output looks identical.

**Cause:** Probabilistic - 60% chance per character. May not trigger on short strings.

**Solution:** Test with longer strings or verify mapping tables are being used.

```csharp
var o = new NuciTextObfuscator(12345);
var options = new NuciTextObfuscatorOptions { UseApproximateReplacements = true };

// Short string - may not trigger
o.Obfuscate("A", options); // Might return "A"

// Longer string - higher chance
o.Obfuscate("AAAAAAAAAA", options); // Likely has replacements
```

### 7. Font Rendering Issues

**Problem:** Obfuscated text shows as boxes (□) or wrong characters.

**Cause:** Font doesn't contain Cherokee/Cyrillic/Greek glyphs used in approximate mode.

**Solution:** Use identical mode or ensure font supports Unicode ranges.

```csharp
// Use identical mode for better font compatibility
var options = new NuciTextObfuscatorOptions { UseApproximateReplacements = false };
// Or ensure UI font supports: Cherokee (U+13A0–U+13FF), Cyrillic (U+0400–U+04FF), Greek (U+0370–U+03FF)
```

### 8. Seed Not Producing Expected Output

**Problem:** String seed doesn't match int seed output.

**Cause:** `string.GetHashCode()` is not stable across .NET versions/runtimes.

**Solution:** Use int seeds for cross-version determinism.

```csharp
// For stable cross-version seeds, use int
var o1 = new NuciTextObfuscator(12345); // Stable

// String seed - only stable within same .NET runtime version
var o2 = new NuciTextObfuscator("my-seed"); // May differ across .NET versions
```

## Debugging Tips

### Enable Deterministic Testing

```csharp
// Use fixed seed for reproducible tests
private const int TestSeed = 123456789;

[Test]
public void MyTest()
{
    var o = new NuciTextObfuscator(TestSeed);
    // Predictable output
}
```

### Inspect Mapping Tables

```csharp
// Check if character is mappable (reflection needed for private tables)
// Or test empirically
var o = new NuciTextObfuscator(12345);
var result = o.Obfuscate("A");
Console.WriteLine(result == "A" ? "Not mapped" : "Mapped");
```

### Verify Round-Trip

```csharp
// Always test round-trip in your integration
var o = new NuciTextObfuscator(seed);
var obf = o.Obfuscate(input, options);
var deobf = o.Deobfuscate(obf);
Debug.Assert(input == deobf, "Round-trip failed");
```

## Performance Issues

### High Allocation

**Problem:** Many allocations under load.

**Cause:** New `StringBuilder` per call.

**Solution:** Acceptable for typical use. For extreme throughput, pool instances.

```csharp
// Object pool pattern (see concurrency-and-scheduling.md)
```

### Slow Startup

**Problem:** First `Deobfuscate` call slower.

**Cause:** Lazy reverse map construction.

**Solution:** Warm up on startup.

```csharp
// Warm up
var o = new NuciTextObfuscator(12345);
o.Deobfuscate("test"); // Builds reverse map
```

## Getting Help

1. Check [FAQ](faq.md)
2. Review [invariants](invariants.md) for guaranteed behaviour
3. Check [GitHub Issues](https://github.com/hmlendea/nucitext.obfuscation/issues)
4. File new issue with:
   - .NET version
   - Code sample
   - Expected vs actual output
   - Seed used