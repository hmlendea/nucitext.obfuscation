# Design Decisions

## 1. Probabilistic Replacement (60% Chance)

**Decision:** Each mappable character has a 60% chance of being replaced.

**Rationale:**
- 100% replacement makes text obviously obfuscated
- 0% provides no obfuscation
- 60% balances readability with obfuscation strength
- Configurable via seed for deterministic testing

**Alternatives Considered:**
- Fixed percentage per character type — rejected (complexity)
- User-configurable probability — rejected (API surface bloat)
- Always replace first N characters — rejected (predictable pattern)

## 2. Seeded Random for Determinism

**Decision:** `Random` instantiated with seed (int or string.GetHashCode()).

**Rationale:**
- Enables reproducible obfuscation for testing
- Allows same input → same output with same seed
- String seed enables semantic seeds ("user-123", "session-abc")

**Alternatives Considered:**
- ThreadStatic Random — rejected (non-deterministic)
- RandomNumberGenerator — rejected (overkill, no seed)
- Static Random — rejected (thread safety, non-deterministic)

## 3. Two-Tier Replacement (Groups → Singles)

**Decision:** Process multi-character groups first (greedy longest-match), then single characters.

**Rationale:**
- Ligatures (ff, fi, fl) look more natural than individual replacements
- Prevents partial replacement of group sequences
- Greedy longest-match handles overlapping patterns (ffi → ﬃ not ﬀ + i)

**Alternatives Considered:**
- Single pass with all patterns — rejected (complexity, overlap issues)
- Single characters only — rejected (weaker obfuscation, misses ligatures)
- Regex-based — rejected (performance, dependency)

## 4. Four Mapping Tables (Identical + Approximate × Group + Single)

**Decision:** Separate tables for identical vs approximate, group vs single.

**Rationale:**
- Identical: preserves visual fidelity, safer for fonts
- Approximate: stronger obfuscation, more scripts
- Group: handles ligatures and punctuation sequences
- Single: per-character granularity

**Alternatives Considered:**
- Single unified table — rejected (no granularity control)
- Runtime-generated mappings — rejected (performance, determinism)
- External mapping files — rejected (deployment complexity)

## 5. Cherokee-Heavy Approximate Mappings

**Decision:** Approximate tables heavily use Cherokee syllabary characters.

**Rationale:**
- Cherokee characters are visually distinct but Latin-like
- Large repertoire (85 characters) covers A-Z, a-z
- Unicode block is contiguous and well-supported

**Known Issues:**
- iOS Safari renders Cherokee + Latin with casing side effects
- Some fonts lack Cherokee glyphs
- Documented in source comments

**Alternatives Considered:**
- Cyrillic/Greek only — rejected (insufficient coverage for all letters)
- Mathematical alphanumerics — rejected (font support varies)
- CJK characters — rejected (too visually distinct)

## 6. Deobfuscation via Reverse Lookup

**Decision:** Build reverse dictionary at runtime from all four tables.

**Rationale:**
- Single deobfuscation method handles both modes
- No option needed for deobfuscation
- Automatic handling of all mappings

**Trade-off:**
- Slight startup overhead (negligible, ~100 entries)
- Memory for reverse dictionary (~few KB)

## 7. Null/Empty Handling

**Decision:** Return input as-is for null or empty string.

**Rationale:**
- Null propagation follows .NET conventions
- Empty string is valid input, returns empty
- No exceptions for edge cases

## 8. Interface + Implementation Separation

**Decision:** `INuciTextObfuscator` interface with `NuciTextObfuscator` implementation.

**Rationale:**
- Enables dependency injection
- Facilitates testing with mocks
- Allows alternative implementations

## 9. Options Class (Not Flags/Enum)

**Decision:** `NuciTextObfuscatorOptions` class with properties.

**Rationale:**
- Extensible for future options
- Serializes cleanly to JSON
- Follows .NET Options pattern

## 10. Zero External Dependencies

**Decision:** No NuGet package references in main library.

**Rationale:**
- Minimal attack surface
- Trivial supply chain
- Easy to audit
- No version conflicts for consumers

## 11. GPL-3.0-or-later License

**Decision:** Strong copyleft license.

**Rationale:**
- Ensures derivative works remain open
- Aligns with author's values
- Compatible with GPL ecosystem

## 12. net10.0 Target Framework

**Decision:** Target .NET 10.0 exclusively.

**Rationale:**
- Latest language features (collection expressions, etc.)
- Performance improvements
- Long-term support timeline

**Trade-off:**
- Not compatible with .NET 8/9 without multi-targeting
- Acceptable for new library