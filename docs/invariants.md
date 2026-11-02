# Invariants

## Core Invariants

### 1. Round-Trip Identity

For any input string `s`, seed `seed`, and options `opts`:

```
Deobfuscate(Obfuscate(s, opts)) == s
```

**Conditions:**
- Same `NuciTextObfuscator` instance (or same seed)
- Same `NuciTextObfuscatorOptions` for both operations
- `s` is not `null`

### 2. Deterministic Output

For same seed, same input, same options:

```
Obfuscate(s, opts) == Obfuscate(s, opts)  // Always identical
```

### 3. Null Preservation

```
Obfuscate(null) == null
Deobfuscate(null) == null
```

### 4. Empty String Preservation

```
Obfuscate("") == ""
Deobfuscate("") == ""
```

### 5. Unmappable Character Preservation

For any character `c` not in any mapping table:

```
Obfuscate(c) contains c
Deobfuscate(c) contains c
```

### 6. Options Independence for Deobfuscation

```
Deobfuscate(Obfuscate(s, opts1)) == Deobfuscate(Obfuscate(s, opts2)) == s
```

Deobfuscation does not require or use options.

## Probabilistic Invariants

### 7. Replacement Probability

For any mappable character, over many invocations with different seeds:

```
P(replaced) ≈ 0.6
P(unchanged) ≈ 0.4
```

### 8. Seed Determines Sequence

For a given seed, the sequence of random choices is fixed:

```
new NuciTextObfuscator(seed).Obfuscate("aaaaaaaaaa")
// Produces same pattern every time
```

## Mapping Invariants

### 9. Group Priority

Multi-character groups are always matched before single characters:

```
"ffi" → "ﬃ" (group)  NOT  "ﬀ" + "i" (singles)
```

### 10. Longest Match Wins

For overlapping groups, longest match is chosen:

```
"ffl" → "ﬄ" (3 chars)  NOT  "ﬀ" + "l" (2+1 chars)
```

### 11. Reverse Map Completeness

Every replacement in all four tables has a reverse mapping:

```
∀ table ∈ {IdenticalGroup, ApproximateGroup, Identical, Approximate}:
  ∀ (key, values) ∈ table:
    ∀ v ∈ values: reverseMap[v] == key
```

### 12. No Duplicate Reverse Keys

No two different originals map to the same replacement:

```
∀ v: |{k | reverseMap[v] == k}| ≤ 1
```

## State Invariants

### 13. Random State Advances

Each `Obfuscate` call advances the internal `Random` state:

```
var o = new NuciTextObfuscator(1);
o.Obfuscate("a");  // Uses first random values
o.Obfuscate("a");  // Uses next random values (different result)
```

### 14. Instance Isolation

Different instances with same seed produce same first result:

```
new NuciTextObfuscator(1).Obfuscate("a") == new NuciTextObfuscator(1).Obfuscate("a")
```

### 15. String Seed Consistency

```
new NuciTextObfuscator("seed").Obfuscate("a")
== new NuciTextObfuscator("seed".GetHashCode()).Obfuscate("a")
```

## Violation Detection

These invariants are verified by unit tests in `NuciText.Obfuscation.UnitTests`:

| Invariant | Test Method |
|-----------|-------------|
| Round-trip | `Obfuscate_Deobfuscate_RoundTrip` |
| Deterministic | `Obfuscate_WithSeed_IsDeterministic` |
| Null preservation | `Obfuscate_Null_ReturnsNull` |
| Empty preservation | `Obfuscate_Empty_ReturnsEmpty` |
| Unmappable chars | `Obfuscate_UnmappableCharacters_Preserved` |
| Options independence | `Deobfuscate_WorksWithBothOptions` |
| Group priority | `Obfuscate_GroupsReplacedBeforeSingles` |
| Longest match | `Obfuscate_LongestGroupMatchWins` |
| Seed consistency | `Obfuscate_StringSeed_EqualsIntSeed` |