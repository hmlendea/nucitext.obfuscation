# Data Model

## Core Types

### INuciTextObfuscator

```csharp
public interface INuciTextObfuscator
{
    string? Obfuscate(string? input);
    string? Obfuscate(string? input, NuciTextObfuscatorOptions? options);
    string? Deobfuscate(string? input);
}
```

### NuciTextObfuscator

```csharp
public sealed class NuciTextObfuscator : INuciTextObfuscator
{
    public NuciTextObfuscator();
    public NuciTextObfuscator(int seed);
    public NuciTextObfuscator(string seed);

    public string? Obfuscate(string? input);
    public string? Obfuscate(string? input, NuciTextObfuscatorOptions? options);
    public string? Deobfuscate(string? input);
}
```

### NuciTextObfuscatorOptions

```csharp
public class NuciTextObfuscatorOptions
{
    public bool UseApproximateReplacements { get; set; } = false;
}
```

## Mapping Tables (Internal)

All tables are `static readonly Dictionary<string, string[]>` in `NuciTextObfuscator`.

### IdenticalGroupReplacements

Multi-character sequences replaced with identical-looking alternatives.

| Key | Replacements | Example |
|-----|--------------|---------|
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

### ApproximateGroupReplacements

Multi-character sequences replaced with visually similar alternatives.

| Key | Replacements | Example |
|-----|--------------|---------|
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

### IdenticalReplacements

Single characters replaced with identical-looking alternatives.

| Key | Replacements | Example |
|-----|--------------|---------|
| `A` | `["Α", "А", "Ꭺ"]` | Greek Alpha, Cyrillic A, Cherokee A |
| `B` | `["Β", "В", "Ᏼ"]` | Greek Beta, Cyrillic Ve, Cherokee B |
| `E` | `["Ε", "Е", "Ꭼ"]` | Greek Epsilon, Cyrillic Ie, Cherokee E |
| `H` | `["Η", "Н", "Ꮋ"]` | Greek Eta, Cyrillic En, Cherokee H |
| `K` | `["Κ", "К", "Ꮶ"]` | Greek Kappa, Cyrillic Ka, Cherokee K |
| `M` | `["Μ", "М", "Ꮇ"]` | Greek Mu, Cyrillic Em, Cherokee M |
| `N` | `["Ν", "Н", "Ꮑ"]` | Greek Nu, Cyrillic En, Cherokee N |
| `O` | `["Ο", "О", "Ꮎ"]` | Greek Omicron, Cyrillic O, Cherokee O |
| `P` | `["Ρ", "Р", "Ꮲ"]` | Greek Rho, Cyrillic Er, Cherokee P |
| `T` | `["Τ", "Т", "Ꮖ"]` | Greek Tau, Cyrillic Te, Cherokee T |
| `X` | `["Χ", "Х", "Ꮎ"]` | Greek Chi, Cyrillic Ha, Cherokee X |
| `Y` | `["Υ", "У", "Ꭹ"]` | Greek Upsilon, Cyrillic U, Cherokee Y |
| `a` | `["α", "а", "Ꭺ"]` | Greek alpha, Cyrillic a, Cherokee a |
| `c` | `["с", "ϲ"]` | Cyrillic es, Greek lunate sigma |
| `e` | `["е", "е", "Ꭼ"]` | Cyrillic ie, Greek epsilon, Cherokee e |
| `i` | `["і", "ι", "Ꭵ"]` | Cyrillic i, Greek iota, Cherokee i |
| `o` | `["о", "ο", "Ꮎ"]` | Cyrillic o, Greek omicron, Cherokee o |
| `p` | `["р", "ρ", "Ꮲ"]` | Cyrillic er, Greek rho, Cherokee p |
| `x` | `["х", "χ", "Ꮎ"]` | Cyrillic ha, Greek chi, Cherokee x |
| `y` | `["у", "υ", "Ꭹ"]` | Cyrillic u, Greek upsilon, Cherokee y |
| `0` | `["0", "0", "0"]` | Digit zero (no homoglyphs) |
| `1` | `["1", "1", "1"]` | Digit one (no homoglyphs) |
| `2` | `["2", "2", "2"]` | Digit two (no homoglyphs) |
| `3` | `["3", "3", "3"]` | Digit three (no homoglyphs) |
| `4` | `["4", "4", "4"]` | Digit four (no homoglyphs) |
| `5` | `["5", "5", "5"]` | Digit five (no homoglyphs) |
| `6` | `["6", "6", "6"]` | Digit six (no homoglyphs) |
| `7` | `["7", "7", "7"]` | Digit seven (no homoglyphs) |
| `8` | `["8", "8", "8"]` | Digit eight (no homoglyphs) |
| `9` | `["9", "9", "9"]` | Digit nine (no homoglyphs) |

**Total:** ~30 entries (letters + digits)

### ApproximateReplacements

Single characters replaced with visually similar alternatives.

| Key | Replacements | Example |
|-----|--------------|---------|
| `A` | `["ᗅ", "Ꭺ", "ᗅ"]` | Canadian syllabics, Cherokee |
| `B` | `["Ᏼ", "Ᏼ", "Ᏼ"]` | Cherokee |
| `C` | `["Ꮯ", "Ꮯ", "Ꮯ"]` | Cherokee |
| `D` | `["Ꭰ", "Ꭰ", "Ꭰ"]` | Cherokee |
| `E` | `["Ꭼ", "Ꭼ", "Ꭼ"]` | Cherokee |
| `F` | `["Ꭽ", "Ꭽ", "Ꭽ"]` | Cherokee |
| `G` | `["Ꮆ", "Ꮆ", "Ꮆ"]` | Cherokee |
| `H` | `["Ꮋ", "Ꮋ", "Ꮋ"]` | Cherokee |
| `I` | `["Ꭵ", "Ꭵ", "Ꭵ"]` | Cherokee |
| `J` | `["Ꭻ", "Ꭻ", "Ꭻ"]` | Cherokee |
| `K` | `["Ꮶ", "Ꮶ", "Ꮶ"]` | Cherokee |
| `L` | `["Ꮮ", "Ꮮ", "Ꮮ"]` | Cherokee |
| `M` | `["Ꮇ", "Ꮇ", "Ꮇ"]` | Cherokee |
| `N` | `["Ꮑ", "Ꮑ", "Ꮑ"]` | Cherokee |
| `O` | `["Ꮎ", "Ꮎ", "Ꮎ"]` | Cherokee |
| `P` | `["Ꮲ", "Ꮲ", "Ꮲ"]` | Cherokee |
| `Q` | `["Ꮴ", "Ꮴ", "Ꮴ"]` | Cherokee |
| `R` | `["Ꮢ", "Ꮢ", "Ꮢ"]` | Cherokee |
| `S` | `["Ꮥ", "Ꮥ", "Ꮥ"]` | Cherokee |
| `T` | `["Ꮖ", "Ꮖ", "Ꮖ"]` | Cherokee |
| `U` | `["Ꮜ", "Ꮜ", "Ꮜ"]` | Cherokee |
| `V` | `["Ꮴ", "Ꮴ", "Ꮴ"]` | Cherokee |
| `W` | `["Ꮹ", "Ꮹ", "Ꮹ"]` | Cherokee |
| `X` | `["Ꮎ", "Ꮎ", "Ꮎ"]` | Cherokee |
| `Y` | `["Ꭹ", "Ꭹ", "Ꭹ"]` | Cherokee |
| `Z` | `["Ꮓ", "Ꮓ", "Ꮓ"]` | Cherokee |
| `a` | `["Ꭺ", "Ꭺ", "Ꭺ"]` | Cherokee |
| `b` | `["Ᏼ", "Ᏼ", "Ᏼ"]` | Cherokee |
| `c` | `["Ꮯ", "Ꮯ", "Ꮯ"]` | Cherokee |
| `d` | `["Ꭰ", "Ꭰ", "Ꭰ"]` | Cherokee |
| `e` | `["Ꭼ", "Ꭼ", "Ꭼ"]` | Cherokee |
| `f` | `["Ꭽ", "Ꭽ", "Ꭽ"]` | Cherokee |
| `g` | `["Ꮆ", "Ꮆ", "Ꮆ"]` | Cherokee |
| `h` | `["Ꮋ", "Ꮋ", "Ꮋ"]` | Cherokee |
| `i` | `["Ꭵ", "Ꭵ", "Ꭵ"]` | Cherokee |
| `j` | `["Ꭻ", "Ꭻ", "Ꭻ"]` | Cherokee |
| `k` | `["Ꮶ", "Ꮶ", "Ꮶ"]` | Cherokee |
| `l` | `["Ꮮ", "Ꮮ", "Ꮮ"]` | Cherokee |
| `m` | `["Ꮇ", "Ꮇ", "Ꮇ"]` | Cherokee |
| `n` | `["Ꮑ", "Ꮑ", "Ꮑ"]` | Cherokee |
| `o` | `["Ꮎ", "Ꮎ", "Ꮎ"]` | Cherokee |
| `p` | `["Ꮲ", "Ꮲ", "Ꮲ"]` | Cherokee |
| `q` | `["Ꮴ", "Ꮴ", "Ꮴ"]` | Cherokee |
| `r` | `["Ꮢ", "Ꮢ", "Ꮢ"]` | Cherokee |
| `s` | `["Ꮥ", "Ꮥ", "Ꮥ"]` | Cherokee |
| `t` | `["Ꮖ", "Ꮖ", "Ꮖ"]` | Cherokee |
| `u` | `["Ꮜ", "Ꮜ", "Ꮜ"]` | Cherokee |
| `v` | `["Ꮴ", "Ꮴ", "Ꮴ"]` | Cherokee |
| `w` | `["Ꮹ", "Ꮹ", "Ꮹ"]` | Cherokee |
| `x` | `["Ꮎ", "Ꮎ", "Ꮎ"]` | Cherokee |
| `y` | `["Ꭹ", "Ꭹ", "Ꭹ"]` | Cherokee |
| `z` | `["Ꮓ", "Ꮓ", "Ꮓ"]` | Cherokee |
| `0` | `["0", "0", "0"]` | Digit zero |
| `1` | `["1", "1", "1"]` | Digit one |
| `2` | `["2", "2", "2"]` | Digit two |
| `3` | `["3", "3", "3"]` | Digit three |
| `4` | `["4", "4", "4"]` | Digit four |
| `5` | `["5", "5", "5"]` | Digit five |
| `6` | `["б", "Ꮾ", "б"]` | Cyrillic be, Cherokee |
| `7` | `["7", "7", "7"]` | Digit seven |
| `8` | `["8", "8", "8"]` | Digit eight |
| `9` | `["9", "9", "9"]` | Digit nine |

**Total:** ~60 entries (Cherokee-heavy)

## Data Flow

```
Input String
    │
    ▼
┌─────────────────────┐
│ Group Replacements  │ ──► Longest-match first (greedy)
│ (Identical or       │
│  Approximate)       │
└─────────────────────┘
    │
    ▼
┌─────────────────────┐
│ Single Char         │ ──► 60% chance per mappable char
│ Replacements        │
│ (Identical or       │
│  Approximate)       │
└─────────────────────┘
    │
    ▼
Output String
```

## Deobfuscation Mapping

Reverse lookup dictionaries built at runtime:

```csharp
// For each replacement in all tables:
reverseMap[replacement] = original;
```

All four tables contribute to the reverse map. Deobfuscation is deterministic and option-independent.