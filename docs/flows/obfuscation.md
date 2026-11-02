# Obfuscation Flow

## Overview

End-to-end flow for the `Obfuscate` operation.

## Entry Point

```csharp
public string Obfuscate(string text, NuciTextObfuscatorOptions options)
```

## Flow Diagram

```mermaid
flowchart TD
    Start([Obfuscate called]) --> NullCheck{text == null?}
    NullCheck -->|Yes| ReturnNull[Return null]
    NullCheck -->|No| EmptyCheck{text == ""?}
    EmptyCheck -->|Yes| ReturnEmpty[Return ""]
    EmptyCheck -->|No| InitOptions[options ??= new()]
    InitOptions --> GroupLoop[For each IdenticalGroupReplacements]
    GroupLoop --> GroupProb{Random 1-10 <= 6?}
    GroupProb -->|Yes| GroupReplace[Replace with random candidate]
    GroupProb -->|No| GroupKeep[Keep original]
    GroupReplace --> GroupNext[Next group]
    GroupKeep --> GroupNext
    GroupNext --> MoreGroups{More groups?}
    MoreGroups -->|Yes| GroupLoop
    MoreGroups -->|No| ApproxCheck{options.UseApproximateReplacements?}
    ApproxCheck -->|Yes| ApproxGroupLoop[For each ApproximateGroupReplacements]
    ApproxCheck -->|No| CharLoop[For each character in text]
    ApproxGroupLoop --> ApproxGroupProb{Random 1-10 <= 6?}
    ApproxGroupProb -->|Yes| ApproxGroupReplace[Replace with random candidate]
    ApproxGroupProb -->|No| ApproxGroupKeep[Keep original]
    ApproxGroupReplace --> ApproxGroupNext[Next group]
    ApproxGroupKeep --> ApproxGroupNext
    ApproxGroupNext --> MoreApproxGroups{More groups?}
    MoreApproxGroups -->|Yes| ApproxGroupLoop
    MoreApproxGroups -->|No| CharLoop
    CharLoop --> IdenticalCheck{Char in IdenticalReplacements?}
    IdenticalCheck -->|Yes| IdenticalCandidates[Get candidates]
    IdenticalCheck -->|No| ApproxCheck2{Approximate enabled?}
    ApproxCheck2 -->|Yes| ApproxCharCheck{Char in ApproximateReplacements?}
    ApproxCheck2 -->|No| CharAppend[Append original char]
    ApproxCharCheck -->|Yes| ApproxCandidates[Get candidates]
    ApproxCharCheck -->|No| CharAppend
    IdenticalCandidates --> CombinedCandidates[Combine identical + approximate]
    ApproxCandidates --> CombinedCandidates
    CombinedCandidates --> CharProb{Random 1-10 <= 6?}
    CharProb -->|Yes| CharReplace[Replace with random candidate]
    CharProb -->|No| CharAppend
    CharReplace --> CharNext[Next character]
    CharAppend --> CharNext
    CharNext --> MoreChars{More characters?}
    MoreChars -->|Yes| CharLoop
    MoreChars -->|No| BuildResult[Return StringBuilder.ToString()]
    BuildResult --> End([Return obfuscated string])
```

## Step-by-Step Description

### 1. Input Validation
- Null input → return null
- Empty string → return empty string
- Options null → instantiate default `NuciTextObfuscatorOptions`

### 2. Group Replacements (Identical)
- Iterate `IdenticalGroupReplacements` dictionary
- For each group (e.g., `**`, `!!`, `...`):
  - 60% chance: replace with random candidate from value string
  - 40% chance: keep original group
- Uses `string.Replace` on entire input string

### 3. Group Replacements (Approximate) — Conditional
- Only if `options.UseApproximateReplacements == true`
- Iterate `ApproximateGroupReplacements` dictionary
- Same 60% probabilistic replacement logic
- Uses `string.Replace` on current input string

### 4. Character-by-Character Processing
- Iterate each character in the (potentially group-modified) string
- Build candidates from:
  - `IdenticalReplacements[char]` (always)
  - `ApproximateReplacements[char]` (if approximate enabled)
- If candidates exist:
  - 60% chance: append random candidate to result
  - 40% chance: append original character
- If no candidates: append original character
- Uses `StringBuilder` for efficient construction

### 5. Result Construction
- Return `StringBuilder.ToString()`

## Probabilistic Behaviour

| Operation | Replacement Probability | Seed Dependency |
|-----------|------------------------|-----------------|
| Group replacement | 60% | Yes (per-instance `Random`) |
| Character replacement | 60% | Yes (per-instance `Random`) |

**Deterministic with fixed seed:** Same seed + same input + same options = same output.

## Side Effects

- None (pure function)
- No state mutation beyond local variables
- `RandomGenerator` state advances (affects subsequent calls on same instance)

## Error Handling

- Null input handled explicitly
- No exceptions thrown for valid inputs
- `options` null-coalesced to default