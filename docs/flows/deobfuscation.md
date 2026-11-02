# Deobfuscation Flow

## Overview

End-to-end flow for the `Deobfuscate` operation.

## Entry Point

```csharp
public string Deobfuscate(string text)
```

## Flow Diagram

```mermaid
flowchart TD
    Start([Deobfuscate called]) --> NullCheck{text == null?}
    NullCheck -->|Yes| ReturnNull[Return null]
    NullCheck -->|No| EmptyCheck{text == ""?}
    EmptyCheck -->|Yes| ReturnEmpty[Return ""]
    EmptyCheck -->|No| IdenticalGroupLoop[For each IdenticalGroupReplacements]
    IdenticalGroupLoop --> GroupCandidates[Get candidate chars from value]
    GroupCandidates --> CandidateLoop[For each candidate char]
    CandidateLoop --> ReplaceAll[Replace all occurrences with key]
    ReplaceAll --> NextCandidate[Next candidate]
    NextCandidate --> MoreCandidates{More candidates?}
    MoreCandidates -->|Yes| CandidateLoop
    MoreCandidates -->|No| NextGroup[Next group]
    NextGroup --> MoreGroups{More groups?}
    MoreGroups -->|Yes| IdenticalGroupLoop
    MoreGroups -->|No| ApproxGroupLoop[For each ApproximateGroupReplacements]
    ApproxGroupLoop --> ApproxGroupCandidates[Get candidate chars from value]
    ApproxGroupCandidates --> ApproxCandidateLoop[For each candidate char]
    ApproxCandidateLoop --> ApproxReplaceAll[Replace all occurrences with key]
    ApproxReplaceAll --> ApproxNextCandidate[Next candidate]
    ApproxNextCandidate --> MoreApproxCandidates{More candidates?}
    MoreApproxCandidates -->|Yes| ApproxCandidateLoop
    MoreApproxCandidates -->|No| ApproxNextGroup[Next group]
    ApproxNextGroup --> MoreApproxGroups{More groups?}
    MoreApproxGroups -->|Yes| ApproxGroupLoop
    MoreApproxGroups -->|No| CharLoop[For each character in text]
    CharLoop --> IdenticalCharCheck{Char in any IdenticalReplacements value?}
    IdenticalCharCheck -->|Yes| IdenticalReverse[Find key, append to builder]
    IdenticalCharCheck -->|No| ApproxCharCheck{Char in any ApproximateReplacements value?}
    ApproxCharCheck -->|Yes| ApproxReverse[Find key, append to builder]
    ApproxCharCheck -->|No| CharAppend[Append original char]
    IdenticalReverse --> CharNext[Next character]
    ApproxReverse --> CharNext
    CharAppend --> CharNext
    CharNext --> MoreChars{More characters?}
    MoreChars -->|Yes| CharLoop
    MoreChars -->|No| BuildResult[Return StringBuilder.ToString()]
    BuildResult --> End([Return deobfuscated string])
```

## Step-by-Step Description

### 1. Input Validation
- Null input → return null
- Empty string → return empty string

### 2. Group Replacements Reverse (Identical)
- Iterate `IdenticalGroupReplacements` dictionary
- For each entry (key = original group, value = candidate string):
  - For each character in the value string:
    - Replace all occurrences of that character in input with the key
- Example: `ᕯ` → `**`, ` ` → ` `, ` ` → ` `, etc.

### 3. Group Replacements Reverse (Approximate)
- Iterate `ApproximateGroupReplacements` dictionary
- Same logic: each candidate character → replace with original group key
- Example: `Ⅱ` → `II`, `Ⅲ` → `III`, `Ǳ` → `DZ`, etc.

### 4. Character-by-Character Reverse Lookup
- Iterate each character in the (group-reversed) string
- For each character:
  - Search `IdenticalReplacements` values for a match
  - If found: append the corresponding key to result
  - Else: search `ApproximateReplacements` values for a match
  - If found: append the corresponding key to result
  - Else: append original character
- Uses `StringBuilder` for efficient construction

### 5. Result Construction
- Return `StringBuilder.ToString()`

## Deterministic Behaviour

- No randomness involved
- No seed dependency
- Same input always produces same output
- Pure function with no side effects

## Reversibility Guarantees

| Condition | Guarantee |
|-----------|-----------|
| Character was obfuscated with identical mapping | Always reversed |
| Character was obfuscated with approximate mapping | Always reversed (if approximate enabled during obfuscation) |
| Character not in any mapping table | Passed through unchanged |
| Group sequence was obfuscated | Always reversed |

## Limitations

- **Approximate mappings require approximate enabled during obfuscation:** If obfuscation used `UseApproximateReplacements = false`, approximate reverse mappings won't match (they weren't used)
- **Probabilistic obfuscation:** Not all mappable characters are replaced (60% chance), so deobfuscation only reverses what was actually replaced
- **Order matters:** Group replacements happen before character replacements in both directions

## Error Handling

- Null input handled explicitly
- No exceptions thrown for valid inputs
- Unknown characters passed through unchanged