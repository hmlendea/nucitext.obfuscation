# Security

## Threat Model

### Assets
- Input text (may contain PII, secrets, credentials)
- Obfuscated output (may be stored/transmitted)
- Seeds (deterministic obfuscation keys)

### Threats
| Threat | Likelihood | Impact | Mitigation |
|--------|------------|--------|------------|
| Input logging | Low | High | No logging in library (see logging.md) |
| Seed exposure | Medium | Medium | Seeds not logged, caller responsibility |
| Output interception | Medium | Low | Obfuscation is not encryption |
| Reverse engineering | High | Low | Deobfuscation is public API |
| Supply chain | Low | High | Zero dependencies (see dependencies.md) |

## Data Handling

### Input Text
- Processed in memory only
- Not persisted, not logged, not transmitted
- Caller controls lifetime and scope

### Seeds
- `int` seed: stored in `Random` instance
- `string` seed: converted via `GetHashCode()`, not stored
- Not exposed via properties or methods

### Output Text
- Returned to caller
- Caller responsible for handling

## Cryptographic Properties

**This is NOT encryption.** It provides:
- Visual obfuscation (homoglyph substitution)
- Deterministic reversible transformation
- No confidentiality guarantees

**Does NOT provide:**
- Confidentiality (reversible without key)
- Integrity (no tamper detection)
- Authentication (no identity verification)
- Non-repudiation

## Secure Usage Guidelines

### Do
- Use for visual obfuscation only (UI display, logs, screenshots)
- Treat obfuscated text as plaintext for security purposes
- Generate seeds via cryptographic RNG if unpredictability needed
- Validate round-trip in your test suite

### Don't
- Use for password storage
- Use for token obfuscation in transit
- Use for PII protection in databases
- Assume obfuscated text is secure

## Seed Security

```csharp
// Good: Cryptographic seed for unpredictable obfuscation
using System.Security.Cryptography;
var seed = RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue);
var obfuscator = new NuciTextObfuscator(seed);

// Acceptable: Fixed seed for testing/determinism
var obfuscator = new NuciTextObfuscator(12345);

// Avoid: Predictable seeds in production
var obfuscator = new NuciTextObfuscator(Environment.TickCount); // Predictable
var obfuscator = new NuciTextObfuscator("user-123".GetHashCode()); // Predictable
```

## Input Validation

The library performs no input validation beyond null checks. Callers should:

- Sanitize inputs if from untrusted sources
- Limit input length if DoS is a concern
- Validate encoding (UTF-16 strings assumed)

## Output Safety

Obfuscated output:
- Contains only Unicode characters from mapping tables
- May include characters from Cherokee, Cyrillic, Greek, etc.
- Safe for UTF-8/UTF-16 storage and transmission
- May not render correctly in all fonts (see behaviour/approximate-replacements.md)

## Compliance

| Standard | Status | Notes |
|----------|--------|-------|
| GDPR | Compatible | No personal data processing |
| HIPAA | Not applicable | Not a security control |
| PCI DSS | Not applicable | Not encryption |
| SOC 2 | Compatible | No data retention |

## Vulnerability Reporting

See [SECURITY.md](../SECURITY.md) for vulnerability disclosure process.

## Related Documents

- [logging.md](logging.md) — No logging policy
- [error-handling.md](error-handling.md) — Exception policy
- [dependencies.md](dependencies.md) — Zero dependencies
- [state-and-persistence.md](state-and-persistence.md) — No persistence