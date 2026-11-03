# Mapping Tables

## Overview

Four `static readonly Dictionary<string, string[]>` tables in `NuciTextObfuscator` containing Unicode homoglyph mappings.

## Table Structure

```csharp
private static readonly Dictionary<string, string[]> IdenticalGroupReplacements;
private static readonly Dictionary<string, string[]> ApproximateGroupReplacements;
private static readonly Dictionary<string, string[]> IdenticalReplacements;
private static readonly Dictionary<string, string[]> ApproximateReplacements;
```

- **Key:** Original string (1-3 characters for groups, 1 character for singles)
- **Value:** Array of replacement candidates (each used with equal probability)
- **Initialization:** At type load, immutable thereafter

## IdenticalGroupReplacements

Multi-character sequences with visually identical replacements. Used by default.

| Key | Replacements | Description |
|-----|--------------|-------------|
| `II` | `["Ⅱ", "Ⅱ"]` | Roman numeral 2 |
| `ae` | `["æ", "æ"]` | Latin small ligature ae |
| `AE` | `["Æ", "Æ"]` | Latin capital ligature AE |
| `oe` | `["œ", "œ"]` | Latin small ligature oe |
| `OE` | `["Œ", "Œ"]` | Latin capital ligature OE |
| `ij` | `["ĳ", "ĳ"]` | Latin small ligature ij |
| `IJ` | `["Ĳ", "Ĳ"]` | Latin capital ligature IJ |
| `ff` | `["ﬀ", "ﬀ"]` | Latin small ligature ff |
| `fi` | `["ﬁ", "ﬁ"]` | Latin small ligature fi |
| `fl` | `["ﬂ", "ﬂ"]` | Latin small ligature fl |
| `ffi` | `["ﬃ", "ﬃ"]` | Latin small ligature ffi |
| `ffl` | `["ﬄ", "ﬄ"]` | Latin small ligature ffl |
| `st` | `["ﬆ", "ﬆ"]` | Latin small ligature st |
| `...` | `["…", "…"]` | Horizontal ellipsis |
| `---` | `["—", "—"]` | Em dash |
| `--` | `["–", "–"]` | En dash |
| `<<` | `["«", "«"]` | Left guillemet |
| `>>` | `["»", "»"]` | Right guillemet |
| `''` | `["″", "″"]` | Double prime |
| ```` | `["″", "″"]` | Double prime (backticks) |
| `'''` | `["‴", "‴"]` | Triple prime |
| ````` | `["‴", "‴"]` | Triple prime (backticks) |

**Total:** ~40 entries

## ApproximateGroupReplacements

Multi-character sequences with visually similar replacements. Used when `UseApproximateReplacements = true`.

| Key | Replacements | Description |
|-----|--------------|-------------|
| `II` | `["Ⅱ", "Ⅱ"]` | Roman numeral 2 |
| `ae` | `["æ", "ӕ"]` | Latin + Cyrillic small ligature ae |
| `AE` | `["Æ", "Ӕ"]` | Latin + Cyrillic capital ligature AE |
| `oe` | `["œ", "œ"]` | Latin small ligature oe |
| `OE` | `["Œ", "Œ"]` | Latin capital ligature OE |
| `ij` | `["ĳ", "ĳ"]` | Latin small ligature ij |
| `IJ` | `["Ĳ", "Ĳ"]` | Latin capital ligature IJ |
| `ff` | `["ﬀ", "ﬀ"]` | Latin small ligature ff |
| `fi` | `["ﬁ", "ﬁ"]` | Latin small ligature fi |
| `fl` | `["ﬂ", "ﬂ"]` | Latin small ligature fl |
| `ffi` | `["ﬃ", "ﬃ"]` | Latin small ligature ffi |
| `ffl` | `["ﬄ", "ﬄ"]` | Latin small ligature ffl |
| `st` | `["ﬆ", "ﬆ"]` | Latin small ligature st |
| `...` | `["…", "…"]` | Horizontal ellipsis |
| `---` | `["—", "—"]` | Em dash |
| `--` | `["–", "–"]` | En dash |
| `<<` | `["«", "«"]` | Left guillemet |
| `>>` | `["»", "»"]` | Right guillemet |
| `''` | `["″", "″"]` | Double prime |
| ```` | `["″", "″"]` | Double prime (backticks) |
| `'''` | `["‴", "‴"]` | Triple prime |
| ````` | `["‴", "‴"]` | Triple prime (backticks) |

**Total:** ~40 entries (same keys, different values)

## IdenticalReplacements

Single characters with visually identical replacements. Used by default.

| Key | Replacements | Scripts |
|-----|--------------|---------|
| `A` | `["Α", "А", "Ꭺ"]` | Greek, Cyrillic, Cherokee |
| `B` | `["Β", "В", "Ᏼ"]` | Greek, Cyrillic, Cherokee |
| `E` | `["Ε", "Е", "Ꭼ"]` | Greek, Cyrillic, Cherokee |
| `H` | `["Η", "Н", "Ꮋ"]` | Greek, Cyrillic, Cherokee |
| `K` | `["Κ", "К", "Ꮶ"]` | Greek, Cyrillic, Cherokee |
| `M` | `["Μ", "М", "Ꮇ"]` | Greek, Cyrillic, Cherokee |
| `N` | `["Ν", "Н", "Ꮑ"]` | Greek, Cyrillic, Cherokee |
| `O` | `["Ο", "О", "Ꮎ"]` | Greek, Cyrillic, Cherokee |
| `P` | `["Ρ", "Р", "Ꮲ"]` | Greek, Cyrillic, Cherokee |
| `T` | `["Τ", "Т", "Ꮖ"]` | Greek, Cyrillic, Cherokee |
| `X` | `["Χ", "Х", "Ꮎ"]` | Greek, Cyrillic, Cherokee |
| `Y` | `["Υ", "У", "Ꭹ"]` | Greek, Cyrillic, Cherokee |
| `a` | `["α", "а", "Ꭺ"]` | Greek, Cyrillic, Cherokee |
| `c` | `["с", "ϲ"]` | Cyrillic, Greek |
| `e` | `["е", "е", "Ꭼ"]` | Cyrillic, Greek, Cherokee |
| `i` | `["і", "ι", "Ꭵ"]` | Cyrillic, Greek, Cherokee |
| `o` | `["о", "ο", "Ꮎ"]` | Cyrillic, Greek, Cherokee |
| `p` | `["р", "ρ", "Ꮲ"]` | Cyrillic, Greek, Cherokee |
| `x` | `["х", "χ", "Ꮎ"]` | Cyrillic, Greek, Cherokee |
| `y` | `["у", "υ", "Ꭹ"]` | Cyrillic, Greek, Cherokee |
| `0`-`9` | `["0", "0", "0"]` | Digits (no homoglyphs) |

**Total:** ~30 entries

## ApproximateReplacements

Single characters with visually similar replacements. Used when `UseApproximateReplacements = true`.

| Key | Replacements | Scripts |
|-----|--------------|---------|
| `A`-`Z` | Cherokee syllabary | Cherokee |
| `a`-`z` | Cherokee syllabary | Cherokee |
| `0`-`5`, `7`-`9` | Self | Digits |
| `6` | `["б", "Ꮾ", "б"]` | Cyrillic, Cherokee |

**Total:** ~60 entries (Cherokee-heavy)

## Deobfuscation

Reverse map built lazily on first `Deobfuscate` call:

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

- All four tables contribute to reverse map
- Thread-safe double-checked locking
- Built once, cached forever

## Statistics

| Table | Entries | Total Replacements |
|-------|---------|-------------------|
| IdenticalGroupReplacements | ~40 | ~80 |
| ApproximateGroupReplacements | ~40 | ~80 |
| IdenticalReplacements | ~30 | ~90 |
| ApproximateReplacements | ~60 | ~180 |
| **Total** | **~170** | **~430** |

## Related Documents

- [NuciTextObfuscator Component](nuci-textobfuscator.md)
- [Options](options.md)
- [Obfuscation Flow](../flows/obfuscation.md)
- [Approximate Replacements Behaviour](../behaviour/approximate-replacements.md)