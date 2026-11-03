# State and Persistence

## State Model

### Instance State

Each `NuciTextObfuscator` instance maintains:

```csharp
private readonly Random _random;  // Only mutable state
```

### Random State

- Initialized once in constructor with seed
- Advanced on each `Obfuscate` call
- Not reset between calls
- Not accessible externally

### No Persistent State

- No file I/O
- No database
- No cache
- No configuration storage
- No static fields (except mapping tables)

## Mapping Tables (Static Readonly)

```csharp
private static readonly Dictionary<string, string[]> IdenticalGroupReplacements = ...;
private static readonly Dictionary<string, string[]> ApproximateGroupReplacements = ...;
private static readonly Dictionary<string, string[]> IdenticalReplacements = ...;
private static readonly Dictionary<string, string[]> ApproximateReplacements = ...;
```

- Initialized at type load time
- Immutable after initialization
- Shared across all instances
- Thread-safe for reads

## Reverse Mapping (Built on First Deobfuscate)

```csharp
private static Dictionary<string, string>? _reverseMap;
private static readonly object _reverseMapLock = new();

private static Dictionary<string, string> GetReverseMap()
{
    if (_reverseMap != null) return _reverseMap;
    lock (_reverseMapLock)
    {
        return _reverseMap ??= BuildReverseMap();
    }
}
```

- Lazy initialization
- Thread-safe double-checked locking
- Built once, cached forever
- Contains entries from all four tables

## Persistence

### None

The library does not persist any data:
- No serialization of instance state
- No checkpointing
- No recovery
- No migration

### Seed Persistence (Caller Responsibility)

If deterministic output is needed across restarts:
- Caller must store the seed
- Caller must recreate instance with same seed
- Library provides no seed storage

## Thread Safety

### Mapping Tables

- **Thread-safe**: Static readonly dictionaries, initialized before any access
- **Read-only**: No mutations after initialization

### Reverse Map

- **Thread-safe**: Lazy initialization with lock
- **Read-only after init**: No mutations after first build

### Random Instance

- **NOT thread-safe**: `System.Random` is not thread-safe
- **Per-instance**: Each `NuciTextObfuscator` has its own `Random`
- **Concurrent access corrupts state**: See error-handling.md

## Memory Profile

### Per Instance

| Component | Size |
|-----------|------|
| `Random` | ~48 bytes |
| Object overhead | ~16 bytes |
| **Total** | **~64 bytes** |

### Static (Shared)

| Component | Size |
|-----------|------|
| 4 mapping tables | ~15 KB |
| Reverse map (after first deobfuscate) | ~5 KB |
| **Total** | **~20 KB** |

## Lifetime

### Instance

- Created by caller
- Eligible for GC when no references
- No finalizer
- No `IDisposable`

### Static Data

- Loaded with type (`NuciTextObfuscator`)
- Lives for AppDomain/process lifetime
- No unload hook

## Serialization

### Not Supported

- `NuciTextObfuscator` is not serializable
- No `[Serializable]` attribute
- No `ISerializable` implementation
- No JSON serialization support

### Seed Serialization (Caller Pattern)

```csharp
// Serialize seed only
var seed = 12345; // or obfuscatorSeed if stored
string json = JsonSerializer.Serialize(seed);

// Deserialize and recreate
int restoredSeed = JsonSerializer.Deserialize<int>(json);
var obfuscator = new NuciTextObfuscator(restoredSeed);
```

## State Diagram

```
┌─────────────────┐
│   Constructor   │
│  (seed → Random)│
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Obfuscate()    │
│  (advances      │
│   Random)       │
└────────┬────────┘
         │
         ▼ (repeatable)
┌─────────────────┐
│  Deobfuscate()  │
│  (lazy builds   │
│   reverse map)  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   GC / Dispose  │
│   (no cleanup)  │
└─────────────────┘
```

## Related Documents

- [error-handling.md](error-handling.md) — Thread safety warnings
- [invariants.md](invariants.md) — State-dependent invariants
- [logging.md](logging.md) — No logging state
- [security.md](security.md) — Seed handling