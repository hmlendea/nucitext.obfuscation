# INuciTextObfuscator Interface

## Overview

Public contract for text obfuscation and deobfuscation operations.

## Namespace

`NuciText.Obfuscation`

## Assembly

`NuciText.Obfuscation.dll`

## Interface Definition

```csharp
public interface INuciTextObfuscator
{
    string Deobfuscate(string text);
    string Obfuscate(string text);
    string Obfuscate(string text, NuciTextObfuscatorOptions options);
}
```

## Methods

### Deobfuscate(string text)

Reverses obfuscation for known Unicode mappings.

#### Signature

```csharp
string Deobfuscate(string text);
```

#### Parameters

| Name | Type | Description |
|------|------|-------------|
| `text` | `string` | The obfuscated text to deobfuscate |

#### Returns

| Type | Description |
|------|-------------|
| `string` | The deobfuscated text, or null if input was null |

#### Behaviour

- **Null input:** Returns null
- **Empty string:** Returns empty string
- **Unknown characters:** Passed through unchanged
- **Deterministic:** Same input always produces same output
- **No side effects:** Pure function

#### Mapping Tables Used

- `IdenticalGroupReplacements` (reverse)
- `ApproximateGroupReplacements` (reverse)
- `IdenticalReplacements` (reverse)
- `ApproximateReplacements` (reverse)

#### Example

```csharp
var obfuscator = new NuciTextObfuscator(12345);
string obfuscated = "Ꭲеst strіng!";
string restored = obfuscator.Deobfuscate(obfuscated);
// restored == "Test string!"
```

---

### Obfuscate(string text)

Obfuscates text using default options (identical replacements only).

#### Signature

```csharp
string Obfuscate(string text);
```

#### Parameters

| Name | Type | Description |
|------|------|-------------|
| `text` | `string` | The text to obfuscate |

#### Returns

| Type | Description |
|------|-------------|
| `string` | The obfuscated text, or null if input was null |

#### Behaviour

- **Null input:** Returns null
- **Empty string:** Returns empty string
- **Default options:** `UseApproximateReplacements = false`
- **Probabilistic:** 60% chance per mappable character/group
- **Seed-dependent:** Deterministic with fixed seed

#### Mapping Tables Used

- `IdenticalGroupReplacements`
- `IdenticalReplacements`

#### Example

```csharp
var obfuscator = new NuciTextObfuscator();
string result = obfuscator.Obfuscate("Hello world!");
// Result varies (probabilistic)
```

---

### Obfuscate(string text, NuciTextObfuscatorOptions options)

Obfuscates text using custom options.

#### Signature

```csharp
string Obfuscate(string text, NuciTextObfuscatorOptions options);
```

#### Parameters

| Name | Type | Description |
|------|------|-------------|
| `text` | `string` | The text to obfuscate |
| `options` | `NuciTextObfuscatorOptions` | Configuration options |

#### Returns

| Type | Description |
|------|-------------|
| `string` | The obfuscated text, or null if input was null |

#### Behaviour

- **Null input:** Returns null
- **Empty string:** Returns empty string
- **Null options:** Treated as default `new NuciTextObfuscatorOptions()`
- **Probabilistic:** 60% chance per mappable character/group
- **Seed-dependent:** Deterministic with fixed seed

#### Mapping Tables Used

| Option | Tables Used |
|--------|-------------|
| `UseApproximateReplacements = false` | IdenticalGroupReplacements, IdenticalReplacements |
| `UseApproximateReplacements = true` | All four tables |

#### Example

```csharp
var obfuscator = new NuciTextObfuscator(12345);
var options = new NuciTextObfuscatorOptions { UseApproximateReplacements = true };
string result = obfuscator.Obfuscate("Test string!", options);
// result == "Ꭲеst strіng!" (deterministic with seed 12345)
```

## Implementation

**Concrete type:** `NuciText.Obfuscation.NuciTextObfuscator`

## Thread Safety

- Interface defines stateless operations
- Implementation uses per-instance `Random` (not thread-safe for shared instance)
- Use separate instances per thread or synchronize access

## Version History

| Version | Changes |
|---------|---------|
| 1.0.0 | Initial release |
| 1.1.0 | Added string seed constructor |
| 1.1.1 | Current |