# Configuration

## NuciTextObfuscatorOptions

The `NuciTextObfuscatorOptions` class controls obfuscation behaviour.

```csharp
public class NuciTextObfuscatorOptions
{
    public bool UseApproximateReplacements { get; set; } = false;
}
```

## Properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `UseApproximateReplacements` | `bool` | `false` | Enable visually similar but non-identical Unicode replacements |

## Usage

```csharp
using NuciText.Obfuscation;

var options = new NuciTextObfuscatorOptions
{
    UseApproximateReplacements = true
};

var obfuscator = new NuciTextObfuscator(12345);
string result = obfuscator.Obfuscate("Input text", options);
```

## Effect on Mapping Tables

| Option Value | Tables Used |
|--------------|-------------|
| `false` (default) | `IdenticalGroupReplacements`, `IdenticalReplacements` |
| `true` | All four tables (adds `ApproximateGroupReplacements`, `ApproximateReplacements`) |

## Default Behaviour

When no options are provided (or `null` is passed):

```csharp
// These are equivalent:
obfuscator.Obfuscate("text");
obfuscator.Obfuscate("text", null);
obfuscator.Obfuscate("text", new NuciTextObfuscatorOptions());
```

All use identical replacements only.

## Serialization

The options class is a simple POCO and can be serialized with any JSON serializer:

```csharp
var options = new NuciTextObfuscatorOptions { UseApproximateReplacements = true };
string json = JsonSerializer.Serialize(options);
// {"UseApproximateReplacements":true}

var restored = JsonSerializer.Deserialize<NuciTextObfuscatorOptions>(json);
```

## Configuration Sources

| Source | Example |
|--------|---------|
| Code (hardcoded) | `new NuciTextObfuscatorOptions { UseApproximateReplacements = true }` |
| JSON config file | `appsettings.json` → bind to options |
| Environment variables | `NUCI_USE_APPROXIMATE=true` → bind |
| Command line | `--use-approximate` → parse and set |

## appsettings.json Example

```json
{
  "NuciTextObfuscatorOptions": {
    "UseApproximateReplacements": true
  }
}
```

```csharp
var options = configuration.GetSection("NuciTextObfuscatorOptions")
    .Get<NuciTextObfuscatorOptions>() ?? new NuciTextObfuscatorOptions();
```

## Validation

No validation is performed on the options object. Invalid values (e.g., `null` passed to methods) are handled by the obfuscator methods themselves.