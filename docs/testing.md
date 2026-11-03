# Testing

## Test Project Structure

```
NuciText.Obfuscation.UnitTests/
├── NuciText.Obfuscation.UnitTests.csproj
└── NuciTextObfuscatorTests.cs
```

## Test Framework

- **NUnit 4.3.2** — Test framework
- **Moq 4.20.72** — Mocking (not used in current tests)
- **Microsoft.NET.Test.Sdk 17.13.0** — Test runner

## Running Tests

```bash
# From solution root
dotnet test

# From test project
dotnet test NuciText.Obfuscation.UnitTests/NuciText.Obfuscation.UnitTests.csproj

# With verbosity
dotnet test --verbosity normal

# Filter by name
dotnet test --filter "FullyQualifiedName~Deterministic"
```

## Test Categories

### 1. Null and Empty Handling

```csharp
[Test]
public void Obfuscate_Null_ReturnsNull()

[Test]
public void Obfuscate_Empty_ReturnsEmpty()

[Test]
public void Deobfuscate_Null_ReturnsNull()

[Test]
public void Deobfuscate_Empty_ReturnsEmpty()
```

### 2. Round-Trip

```csharp
[Test]
public void Obfuscate_Deobfuscate_RoundTrip()

[Test]
public void Obfuscate_Deobfuscate_RoundTrip_WithApproximateOptions()
```

### 3. Deterministic Output

```csharp
[Test]
public void Obfuscate_WithSeed_IsDeterministic()

[Test]
public void Obfuscate_StringSeed_EqualsIntSeed()
```

### 4. Approximate Replacements

```csharp
[Test]
public void Obfuscate_WithApproximateOptions_UsesApproximateMappings()

[Test]
public void Obfuscate_WithoutApproximateOptions_UsesIdenticalMappings()
```

### 5. Group Replacements

```csharp
[Test]
public void Obfuscate_GroupsReplacedBeforeSingles()

[Test]
public void Obfuscate_LongestGroupMatchWins()
```

### 6. Unmappable Characters

```csharp
[Test]
public void Obfuscate_UnmappableCharacters_Preserved()
```

## Test Constants

```csharp
private const int TestSeed = 123456789;
```

## Test Patterns

### Deterministic Testing

```csharp
[Test]
public void Obfuscate_WithSeed_IsDeterministic()
{
    var obfuscator1 = new NuciTextObfuscator(TestSeed);
    var obfuscator2 = new NuciTextObfuscator(TestSeed);

    var result1 = obfuscator1.Obfuscate("Test input");
    var result2 = obfuscator2.Obfuscate("Test input");

    Assert.That(result1, Is.EqualTo(result2));
}
```

### Round-Trip Testing

```csharp
[Test]
public void Obfuscate_Deobfuscate_RoundTrip()
{
    var obfuscator = new NuciTextObfuscator(TestSeed);
    var input = "Test string with numbers 123 and symbols !@#";

    var obfuscated = obfuscator.Obfuscate(input);
    var restored = obfuscator.Deobfuscate(obfuscated);

    Assert.That(restored, Is.EqualTo(input));
}
```

### Options Testing

```csharp
[Test]
public void Obfuscate_WithApproximateOptions_UsesApproximateMappings()
{
    var obfuscator = new NuciTextObfuscator(TestSeed);
    var options = new NuciTextObfuscatorOptions { UseApproximateReplacements = true };

    var result = obfuscator.Obfuscate("Test", options);

    // Verify approximate mappings were used (Cherokee chars present)
    Assert.That(result, Does.Contain("Ꭲ").Or.Contain("Ꭼ").Or.Contain("Ꮥ"));
}
```

## CI Integration

**File:** `.github/workflows/dotnet.yml`

```yaml
- name: Test
  run: dotnet test --no-build --verbosity normal
```

Runs on every push and PR to `master`.

## Coverage

Current tests cover:
- ✅ Null/empty handling
- ✅ Round-trip (identical + approximate)
- ✅ Deterministic output (int seed, string seed)
- ✅ Approximate vs identical modes
- ✅ Group priority and longest match
- ✅ Unmappable character preservation

Not covered (by design):
- ❌ Thread safety (documented as unsupported)
- ❌ Performance benchmarks
- ❌ Font rendering
- ❌ All mapping table entries (sample-based)

## Adding Tests

1. Add test method to `NuciTextObfuscatorTests.cs`
2. Follow naming convention: `Method_Scenario_ExpectedResult`
3. Use `TestSeed` constant for determinism
4. Run `dotnet test` to verify

## Test Data

No external test data files. All test inputs are inline strings.

## Mocking

Moq is available but not currently used. The library has no external dependencies to mock.

## Related Documents

- [invariants.md](invariants.md) — Invariants verified by tests
- [error-handling.md](error-handling.md) — Error scenarios tested
- [build-and-deployment.md](build-and-deployment.md) — CI test execution