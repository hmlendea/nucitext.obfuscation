# NuciTextObfuscatorOptions Component

## Purpose

Configuration options for controlling obfuscation behaviour.

## Location

`NuciText.Obfuscation/NuciTextObfuscatorOptions.cs`

## Class Definition

```csharp
public sealed class NuciTextObfuscatorOptions
```

## Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `UseApproximateReplacements` | `bool` | `false` | Enables approximate character substitutions |

## Behaviour

### UseApproximateReplacements = false (Default)

- Only `IdenticalGroupReplacements` and `IdenticalReplacements` are used
- Substitutions are visually identical or near-identical
- Lower obfuscation strength, higher readability preservation

### UseApproximateReplacements = true

- Both identical and approximate tables are used
- `ApproximateGroupReplacements` and `ApproximateReplacements` included
- Higher obfuscation strength, more visual variation
- Some substitutions may be less visually similar

## Usage Example

```csharp
var options = new NuciTextObfuscatorOptions
{
    UseApproximateReplacements = true
};

var obfuscator = new NuciTextObfuscator(12345);
string result = obfuscator.Obfuscate("Test string!", options);
```

## Design Notes

- Plain data object (no behaviour)
- No validation — invalid values not possible for `bool`
- Immutable after construction (no setters needed beyond initialisation)
- Compatible with object initialiser syntax
- No external dependencies