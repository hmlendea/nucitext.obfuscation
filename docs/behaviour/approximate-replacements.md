# Approximate Replacements

## Overview

Stronger obfuscation using visually similar but not identical Unicode characters.

## Usage

```csharp
using NuciText.Obfuscation;

var options = new NuciTextObfuscatorOptions
{
    UseApproximateReplacements = true
};

var obfuscator = new NuciTextObfuscator(12345);
string obfuscated = obfuscator.Obfuscate("Test string!", options);
```

## Behaviour

- Enables `ApproximateGroupReplacements` and `ApproximateReplacements`
- Additional ~60% chance per mappable character/group
- More visual variation, potentially less readable in some fonts
- Includes mappings from diverse scripts (Cherokee, Lisu, Vai, Tifinagh, etc.)

## Mapping Tables Activated

| Table | Entries | Example |
|-------|---------|---------|
| `ApproximateGroupReplacements` | 40+ | `II` → `Ⅱ`, `ae` → `æӕ` |
| `ApproximateReplacements` | 60+ | `A` → `ᗅᎪ`, `6` → `бᏮ` |

## Example Output

| Input | Approximate Output (seed 12345) |
|-------|----------------------------------|
| `Test` | `Ꭲеst` |
| `Hello` | `Ꮋеllo` |
| `12345` | `12ӠЗ45` |

## Deobfuscation

- Deobfuscation automatically reverses approximate mappings
- No option needed for deobfuscation
- Requires that obfuscation used approximate replacements

## Trade-offs

| Aspect | Identical Only | With Approximate |
|--------|----------------|------------------|
| Obfuscation strength | Lower | Higher |
| Visual similarity | Higher | Variable |
| Font compatibility | Better | Some mappings break in fonts |
| Reversibility | Complete | Complete (if used) |

## Font Rendering Notes

Some approximate mappings use characters that render differently or not at all in certain fonts:
- Cherokee characters may not display on all platforms
- Lisu, Vai, Tifinagh require specific font support
- Roman numeral characters may affect subsequent character casing on iOS
- Source code comments document known issues

## Recommendation

Use approximate replacements when:
- Stronger visual obfuscation is needed
- Target environment has good Unicode font support
- Exact visual fidelity is not required

Stick to identical replacements when:
- Maximum readability preservation is needed
- Font environment is unknown or constrained
- Cross-platform consistency is critical