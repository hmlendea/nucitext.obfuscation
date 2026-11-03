# Behaviour Index

## Behaviours

| Behaviour | Description | Document |
|-----------|-------------|----------|
| Basic Obfuscation | Core obfuscation behaviour | [basic-obfuscation.md](basic-obfuscation.md) |
| Deterministic Output | Seed-based determinism | [deterministic-output.md](deterministic-output.md) |
| Approximate Replacements | Stronger obfuscation mode | [approximate-replacements.md](approximate-replacements.md) |

## Overview

The library exhibits three primary user-facing behaviours:

1. **Basic Obfuscation** — Default mode using identical visual replacements
2. **Deterministic Output** — Same seed produces same output
3. **Approximate Replacements** — Optional stronger mode with more variation

## Quick Reference

| Aspect | Basic | Deterministic | Approximate |
|--------|-------|---------------|-------------|
| Tables Used | Identical only | All (seed-controlled) | All four |
| Replacement Probability | 60% | 60% | 60% |
| Visual Fidelity | High | High | Variable |
| Reversibility | Complete | Complete | Complete |

## Related Documents

- [components/](../components/INDEX.md) — Component details
- [flows/](../flows/INDEX.md) — Execution flows
- [invariants.md](../invariants.md) — Guaranteed properties