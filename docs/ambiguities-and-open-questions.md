# Ambiguities and Open Questions

## Known Ambiguities

### 1. String Seed Stability Across .NET Versions

**Issue:** `string.GetHashCode()` implementation varies across .NET versions and runtimes.

**Impact:** Same string seed produces different output on .NET 8 vs .NET 10 vs .NET Framework.

**Current State:** Documented in FAQ and troubleshooting. No fix planned (by design).

**Options:**
- Accept and document (current)
- Implement custom stable hash (e.g., FNV-1a)
- Remove string seed constructor

### 2. Approximate Mode Font Compatibility

**Issue:** Cherokee, Lisu, Vai, Tifinagh characters may not render in all fonts.

**Impact:** Obfuscated text shows as □ or � in some environments.

**Current State:** Documented in behaviour/approximate-replacements.md and troubleshooting.md.

**Options:**
- Add font compatibility matrix
- Provide fallback font recommendations
- Add "safe approximate" subset (Latin/Greek/Cyrillic only)
- Let callers handle font selection

### 3. Replacement Probability Hardcoded at 60%

**Issue:** 60% replacement chance is not configurable.

**Impact:** Users cannot tune obfuscation strength vs readability.

**Current State:** Hardcoded in `Obfuscate` method.

**Options:**
- Add `ReplacementProbability` to `NuciTextObfuscatorOptions` (0.0-1.0)
- Keep hardcoded, document rationale
- Add `ObfuscationStrength` enum (Low/Medium/High)

### 4. Mapping Table Completeness

**Issue:** Only ~100 characters/groups mapped. Many Unicode characters unmapped.

**Impact:** Non-Latin scripts (CJK, Arabic, Devanagari, etc.) pass through unchanged.

**Current State:** By design — focused on Latin script obfuscation.

**Options:**
- Expand mappings for other scripts
- Document as Latin-focused tool
- Add script detection and per-script tables

### 5. Group Replacement Overlap Handling

**Issue:** Overlapping groups (e.g., "ffi" contains "ff" and "fi") resolved by longest-match.

**Impact:** Deterministic but may not match user expectations for partial overlaps.

**Current State:** Greedy longest-match implemented and tested.

**Options:**
- Document current behaviour clearly
- Add configuration for overlap strategy
- Keep as-is (simple, predictable)

## Open Questions

### 1. Multi-Target Framework Support

**Question:** Should the library target net8.0, net9.0, net10.0 simultaneously?

**Considerations:**
- Broader compatibility
- CI complexity
- Package size
- Language feature availability (collection expressions need net10.0+)

**Decision Pending:** See ROADMAP.md

### 2. Async API

**Question:** Add `ObfuscateAsync` / `DeobfuscateAsync` methods?

**Considerations:**
- Current sync API is fast (~µs)
- Async would enable cancellation, progress
- Would require `CancellationToken` parameters
- Breaking change for interface

**Decision Pending:** Likely v2.0.0

### 3. Streaming / Large Text Support

**Question:** Support `TextReader`/`TextWriter` or `IAsyncEnumerable<char>` for large inputs?

**Considerations:**
- Current API loads entire string in memory
- Streaming would reduce memory for large texts
- Adds API complexity
- Probabilistic replacement needs random access for determinism

**Decision Pending:** Low priority

### 4. Custom Mapping Tables

**Question:** Allow users to provide custom replacement dictionaries?

**Considerations:**
- Enables domain-specific obfuscation
- Breaks zero-dependency if using external config
- Serialization format needed
- Security: custom mappings could leak info

**Decision Pending:** Low priority

### 5. Seed Derivation for Hierarchical Use

**Question:** Support deriving child seeds from parent seed (e.g., per-user from app seed)?

**Considerations:**
- Useful for multi-tenant scenarios
- Could use HKDF or similar
- Adds crypto dependency or custom implementation
- Current: caller manages seeds

**Decision Pending:** Low priority

### 6. Benchmark / Performance Targets

**Question:** Define and enforce performance benchmarks?

**Considerations:**
- Current performance is adequate
- No regression detection in CI
- Could add BenchmarkDotNet project
- Would need baseline establishment

**Decision Pending:** Nice to have

### 7. Fuzz Testing

**Question:** Add fuzz testing for robustness?

**Considerations:**
- Input validation minimal
- Could uncover edge cases
- Requires fuzzing infrastructure
- Low risk given simple logic

**Decision Pending:** Low priority

### 8. WebAssembly / Blazor Specific Optimizations

**Question:** Optimize for WASM size/performance?

**Considerations:**
- Current size negligible
- No WASM-specific issues known
- Trimming friendly (no reflection)
- AOT compatible

**Decision Pending:** Monitor

## Resolved Questions

### Q: Should deobfuscation require the same options as obfuscation?

**Resolved:** No. Deobfuscation uses reverse map from all tables automatically. Documented in invariants.md and FAQ.

### Q: Should the library log diagnostic information?

**Resolved:** No. Zero logging policy. Documented in logging.md.

### Q: Should mapping tables be externalized to JSON/resources?

**Resolved:** No. Keep embedded for zero-dependency, determinism, deployment simplicity.

### Q: Should `Random` be replaced with `RandomNumberGenerator` for crypto seeds?

**Resolved:** No. `Random` with seed provides determinism needed for testing. Crypto RNG used only for seed generation by caller.

## Tracking

| Item | Status | Priority | Target Version |
|------|--------|----------|----------------|
| String seed stability | Documented | Low | — |
| Font compatibility | Documented | Medium | 1.2.0 |
| Configurable probability | Open | Medium | 1.2.0 |
| Mapping expansion | Open | Low | 2.0.0 |
| Multi-targeting | Planned | High | 1.2.0 |
| Async API | Planned | Medium | 2.0.0 |
| Streaming | Open | Low | 2.0.0 |
| Custom mappings | Open | Low | 2.0.0 |
| Seed derivation | Open | Low | 2.0.0 |
| Benchmarks | Open | Low | 1.2.0 |
| Fuzz testing | Open | Low | 1.2.0 |
| WASM optimization | Monitoring | Low | — |

## Contribution

Open questions are candidates for:
- GitHub Issues (discussion)
- Pull Requests (implementation)
- ROADMAP.md updates (planning)

See [CONTRIBUTING.md](../CONTRIBUTING.md) for process.