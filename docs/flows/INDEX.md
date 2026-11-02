# Flows Index

## Flows

| Flow | Description | Document |
|------|-------------|----------|
| Obfuscation | Input → Obfuscated output | [obfuscation.md](obfuscation.md) |
| Deobfuscation | Obfuscated input → Original output | [deobfuscation.md](deobfuscation.md) |

## Flow Overview

```
┌──────────────┐     Obfuscate()     ┌──────────────┐
│   Original   │ ──────────────────► │  Obfuscated  │
│    Text      │                     │     Text     │
└──────────────┘                     └──────────────┘
       ▲                                    │
       │                                    │
       │         Deobfuscate()              │
       └────────────────────────────────────┘
```

## Key Properties

- **Reversible:** `Deobfuscate(Obfuscate(x)) == x`
- **Deterministic:** Same seed + input = same output
- **Probabilistic:** ~60% of mappable characters replaced
- **Stateless:** No persistence between calls (except Random state)

## Related Documents

- [components/](../components/INDEX.md) — Component details
- [behaviour/](../behaviour/INDEX.md) — Behavioural details
- [invariants.md](../invariants.md) — Guaranteed properties