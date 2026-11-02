# Concurrency and Scheduling

## Concurrency Model

### Not Thread-Safe

`NuciTextObfuscator` instances are **not thread-safe** due to the internal `Random` instance.

```csharp
// UNSAFE - Concurrent access corrupts Random state
var obfuscator = new NuciTextObfuscator(12345);
Parallel.For(0, 100, _ => obfuscator.Obfuscate("test"));
```

### Safe Patterns

#### Pattern 1: Instance Per Thread

```csharp
// ThreadLocal ensures one instance per thread
ThreadLocal<NuciTextObfuscator> local = new(() => new NuciTextObfuscator(12345));
Parallel.For(0, 100, _ => local.Value.Obfuscate("test"));
```

#### Pattern 2: Instance Per Operation

```csharp
// New instance per call (same seed = same first result)
Parallel.For(0, 100, _ => new NuciTextObfuscator(12345).Obfuscate("test"));
```

#### Pattern 3: Synchronized Access

```csharp
// Lock around shared instance
var obfuscator = new NuciTextObfuscator(12345);
var lockObj = new object();

Parallel.For(0, 100, _ =>
{
    lock (lockObj)
    {
        obfuscator.Obfuscate("test");
    }
});
```

#### Pattern 4: Dependency Injection (Scoped)

```csharp
// ASP.NET Core - scoped per request
builder.Services.AddScoped<INuciTextObfuscator>(sp =>
    new NuciTextObfuscator(RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue)));

// Each request gets its own instance
```

## Static Data Concurrency

### Mapping Tables

- **Thread-safe**: `static readonly` dictionaries initialized at type load
- **Read-only**: No mutations after initialization
- **Concurrent reads**: Fully safe

### Reverse Map

- **Thread-safe**: Lazy initialization with `lock`
- **Double-checked locking**: Safe publication pattern
- **Built once**: First `Deobfuscate` call builds it, subsequent calls reuse

```csharp
// Safe concurrent deobfuscation
Parallel.For(0, 100, _ => obfuscator.Deobfuscate("Ꭲеst"));
```

## Scheduling

### No Background Work

- No timers
- No background threads
- No async operations
- No `Task.Run` or `ThreadPool` usage

### Synchronous Only

All methods are synchronous:
- `Obfuscate` — CPU-bound, completes in microseconds
- `Deobfuscate` — CPU-bound, completes in microseconds

### No Cancellation

- No `CancellationToken` parameters
- No cooperative cancellation
- Operations are fast enough that cancellation is unnecessary

## Performance Characteristics

### Time Complexity

| Operation | Complexity | Notes |
|-----------|------------|-------|
| `Obfuscate` | O(n × m) | n = input length, m = max group length (3) |
| `Deobfuscate` | O(n × m) | n = input length, m = max replacement length |

### Memory Allocation

| Operation | Allocations |
|-----------|-------------|
| `Obfuscate` | 1 string (result), 1 StringBuilder (internal) |
| `Deobfuscate` | 1 string (result), 1 StringBuilder (internal) |

### Typical Performance

| Input Length | Obfuscate | Deobfuscate |
|--------------|-----------|-------------|
| 10 chars | ~0.5 µs | ~0.3 µs |
| 100 chars | ~3 µs | ~2 µs |
| 1000 chars | ~25 µs | ~18 µs |
| 10000 chars | ~250 µs | ~180 µs |

*Measured on .NET 10, Ubuntu, Release build*

## Async Integration

### Wrapping for Async Contexts

```csharp
// If you must call from async context without blocking
public async Task<string> ObfuscateAsync(string input, NuciTextObfuscatorOptions? options = null)
{
    return await Task.Run(() => _obfuscator.Obfuscate(input, options));
}

// Better: Use instance-per-operation pattern
public Task<string> ObfuscateAsync(string input, NuciTextObfuscatorOptions? options = null)
{
    var seed = RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue);
    return Task.FromResult(new NuciTextObfuscator(seed).Obfuscate(input, options));
}
```

## Load Testing

### Recommended Pattern for High Throughput

```csharp
// Object pool for high-throughput scenarios
public class ObfuscatorPool
{
    private readonly ConcurrentBag<NuciTextObfuscator> _pool = new();
    private readonly int _seed;

    public ObfuscatorPool(int seed, int initialSize = Environment.ProcessorCount)
    {
        _seed = seed;
        for (int i = 0; i < initialSize; i++)
            _pool.Add(new NuciTextObfuscator(seed));
    }

    public string Obfuscate(string input, NuciTextObfuscatorOptions? options = null)
    {
        if (!_pool.TryTake(out var obfuscator))
            obfuscator = new NuciTextObfuscator(_seed);

        try { return obfuscator.Obfuscate(input, options); }
        finally { _pool.Add(obfuscator); }
    }
}
```

## Summary

| Aspect | Detail |
|--------|--------|
| Instance thread-safety | No |
| Static data thread-safety | Yes |
| Async support | No (sync only) |
| Cancellation | No |
| Background work | No |
| Recommended pattern | Instance per thread/operation |
| Pooling | Supported via custom pool |