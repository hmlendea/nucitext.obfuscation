# Error Handling

## Exception Policy

The library **does not throw exceptions** for normal operation. All error conditions are handled by returning values.

## Null Handling

| Method | Null Input | Return Value |
|--------|------------|--------------|
| `Obfuscate(null)` | `null` | `null` |
| `Obfuscate(null, options)` | `null` | `null` |
| `Deobfuscate(null)` | `null` | `null` |

```csharp
var obfuscator = new NuciTextObfuscator();
string? result = obfuscator.Obfuscate(null); // Returns null, no exception
```

## Empty String Handling

| Method | Empty Input | Return Value |
|--------|-------------|--------------|
| `Obfuscate("")` | `""` | `""` |
| `Obfuscate("", options)` | `""` | `""` |
| `Deobfuscate("")` | `""` | `""` |

## Invalid Options

Passing `null` for options is valid and uses defaults:

```csharp
// All equivalent - no exceptions
obfuscator.Obfuscate("text", null);
obfuscator.Obfuscate("text", new NuciTextObfuscatorOptions());
obfuscator.Obfuscate("text");
```

## Invalid Seed

| Constructor | Invalid Input | Behaviour |
|-------------|---------------|-----------|
| `NuciTextObfuscator(int)` | Any `int` | Valid (including negative, zero, `int.MinValue`) |
| `NuciTextObfuscator(string)` | `null` | `ArgumentNullException` |
| `NuciTextObfuscator(string)` | `""` | Valid (uses `0` as seed via `GetHashCode()`) |

```csharp
// Throws ArgumentNullException
var obfuscator = new NuciTextObfuscator((string)null);

// Valid - empty string hashes to 0
var obfuscator = new NuciTextObfuscator("");
```

## Unmappable Characters

Characters not in any mapping table are passed through unchanged:

```csharp
var obfuscator = new NuciTextObfuscator(12345);
string result = obfuscator.Obfuscate("🎉🎊🎈"); // Returns "🎉🎊🎈" unchanged
```

## Deobfuscation of Unknown Sequences

Sequences not in the reverse map are passed through unchanged:

```csharp
var obfuscator = new NuciTextObfuscator(12345);
string result = obfuscator.Deobfuscate("Ꭲеst"); // "Test" (known mappings)
string result2 = obfuscator.Deobfuscate("🎉");  // "🎉" (unknown, passed through)
```

## Thread Safety

**Not thread-safe.** The `Random` instance is shared across method calls.

```csharp
// Unsafe - concurrent access corrupts Random state
var obfuscator = new NuciTextObfuscator(12345);
Parallel.For(0, 100, _ => obfuscator.Obfuscate("test"));

// Safe - one instance per thread
Parallel.For(0, 100, _ => new NuciTextObfuscator(12345).Obfuscate("test"));

// Safe - thread-local with same seed
ThreadLocal<NuciTextObfuscator> local = new(() => new NuciTextObfuscator(12345));
Parallel.For(0, 100, _ => local.Value.Obfuscate("test"));
```

## Error Scenarios Summary

| Scenario | Exception | Handling |
|----------|-----------|----------|
| Null input to Obfuscate/Deobfuscate | No | Returns null |
| Empty string input | No | Returns empty string |
| Null options | No | Uses defaults |
| Null string seed | Yes (ArgumentNullException) | Validate before calling |
| Concurrent access | No (corrupts state) | Use per-thread instances |
| Unmappable characters | No | Passed through unchanged |
| Invalid reverse mapping | No | Passed through unchanged |

## Best Practices

1. **Validate string seeds** before passing to constructor
2. **Use one instance per thread** or synchronize access
3. **Don't rely on exceptions** for control flow — none are thrown in normal use
4. **Test round-trip** with your specific inputs to verify behaviour