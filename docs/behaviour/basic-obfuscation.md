# Basic Obfuscation

## Overview

Basic usage of the obfuscator with default settings.

## Usage

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator();
string input = "Hello world!";
string obfuscated = obfuscator.Obfuscate(input);
string restored = obfuscator.Deobfuscate(obfuscated);
```

## Behaviour

- Uses `IdenticalReplacements` and `IdenticalGroupReplacements` only
- Probabilistic: ~60% of mappable characters/groups are replaced
- Non-deterministic by default (uses `Environment.TickCount` seed)
- Visually similar output, readable in most fonts

## Example Output

| Input | Possible Output |
|-------|-----------------|
| `Hello world!` | `Нello world!` |
| `Test string` | `Test strіng` |
| `A+B=C` | `А𖫵В=C` |

## Notes

- Output varies between calls (different random selections)
- Deobfuscation reverses only what was actually replaced
- Characters not in mapping tables pass through unchanged