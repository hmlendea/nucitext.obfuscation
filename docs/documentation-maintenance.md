# Documentation Maintenance

## Documentation Structure

```
docs/
├── INDEX.md                    # Navigation hub
├── architecture.md             # System architecture (ref ARCHITECTURE.md)
├── repository-overview.md      # High-level repo summary
├── repository-structure.md     # Directory/file structure
├── quick-start.md              # Getting started guide
├── api-usage-examples.md       # Code examples
├── build-and-deployment.md     # Build/release process
├── configuration.md            # Options reference
├── data-model.md               # Types and mapping tables
├── dependencies.md             # Dependency inventory
├── design-decisions.md         # Architectural rationale
├── error-handling.md           # Exception/error policy
├── invariants.md               # Guaranteed properties
├── integrations.md             # Framework integration patterns
├── logging.md                  # Logging requirements (no logging)
├── security.md                 # Security model
├── state-and-persistence.md    # State management
├── concurrency-and-scheduling.md # Threading model
├── testing.md                  # Test strategy
├── change-guide.md             # Version history
├── troubleshooting.md          # Common issues
├── faq.md                      # Frequently asked questions
├── ambiguities-and-open-questions.md # Known unknowns
├── components/
│   ├── INDEX.md
│   ├── nuci-textobfuscator.md
│   ├── mapping-tables.md
│   └── options.md
├── flows/
│   ├── INDEX.md
│   ├── obfuscation.md
│   └── deobfuscation.md
├── api-reference/
│   ├── INDEX.md
│   └── inuci-textobfuscator.md
└── behaviour/
    ├── INDEX.md
    ├── basic-obfuscation.md
    ├── deterministic-output.md
    └── approximate-replacements.md
```

## Maintenance Rules

### 1. Single Source of Truth

- Root docs (ARCHITECTURE.md, SECURITY.md, etc.) are authoritative
- docs/ files reference, never duplicate root docs
- Code comments are implementation details, not user documentation

### 2. Update Triggers

Update documentation when:
- Adding/removing public APIs
- Changing behaviour (invariants, error handling)
- Adding/removing dependencies
- Changing build/deployment process
- Fixing bugs that affect user-visible behaviour
- Adding configuration options

### 3. Review Checklist

Before merging changes:
- [ ] All public API changes reflected in api-reference/
- [ ] Behaviour changes reflected in behaviour/ and flows/
- [ ] New configuration documented in configuration.md
- [ ] Breaking changes in change-guide.md
- [ ] Security implications in security.md
- [ ] Quick-start still works
- [ ] Examples in api-usage-examples.md compile

### 4. Naming Conventions

- Files: kebab-case.md
- Directories: kebab-case/
- Headings: Sentence case
- Code blocks: Language specified (csharp, bash, yaml, json)

### 5. Cross-References

- Use relative links: `[text](path/to/file.md)`
- Reference root docs: `[ARCHITECTURE.md](../ARCHITECTURE.md)`
- No absolute URLs for internal docs

### 6. Code Examples

- All examples must compile
- Use `TestSeed = 123456789` for deterministic output
- Show both identical and approximate modes
- Include null/empty handling

## Automation

### Validation Scripts (Future)

```bash
# Check for broken links
markdown-link-check docs/**/*.md

# Validate code examples compile
dotnet build --project docs/examples/

# Check spelling
cspell docs/**/*.md
```

### CI Integration (Future)

Add to `.github/workflows/dotnet.yml`:
```yaml
- name: Validate documentation
  run: |
    markdown-link-check docs/**/*.md
    cspell docs/**/*.md
```

## Style Guide

### Headings

```markdown
# H1 - Document title (one per file)
## H2 - Major section
### H3 - Subsection
#### H4 - Detail (avoid deeper)
```

### Tables

```markdown
| Column 1 | Column 2 |
|----------|----------|
| Value 1  | Value 2  |
```

### Code Blocks

```markdown
```csharp
var x = 1;
```

```bash
dotnet build
```
```

### Admonitions (Future)

```markdown
> **Note:** Important information
> **Warning:** Potential issues
> **Tip:** Helpful suggestion
```

## Versioning

- Documentation version tracks library version
- `docs/` updated in same commit as code changes
- No separate documentation versioning

## Archival

- Old versions not maintained in repo
- Git history provides version history
- Release tags correspond to doc state

## Responsibilities

| Role | Responsibility |
|------|----------------|
| Author | Update docs with code changes |
| Reviewer | Verify docs updated in PR |
| Maintainer | Periodic structure review |
| Contributor | Follow this guide |

## Related Documents

- [CONTRIBUTING.md](../CONTRIBUTING.md) — Contribution guidelines
- [change-guide.md](change-guide.md) — Version history
- [README.md](../README.md) — Project overview