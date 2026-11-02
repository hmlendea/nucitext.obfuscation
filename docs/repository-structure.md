# Repository Structure

## Source Tree Layout

```
nucitext.obfuscation/
├── .github/
│   └── workflows/
│       ├── dotnet.yml           # CI build and test
│       └── nuget-release.yml    # NuGet package publishing on release
├── NuciText.Obfuscation/        # Main library project
│   ├── INuciText.Obfuscation.cs # Public interface
│   ├── NuciText.Obfuscation.cs  # Core implementation
│   ├── NuciTextObfuscatorOptions.cs # Configuration options
│   ├── NuciText.Obfuscation.csproj
│   ├── bin/                     # Build output (gitignored)
│   └── obj/                     # Build intermediates (gitignored)
├── NuciText.Obfuscation.UnitTests/ # Unit test project
│   ├── NuciTextObfuscatorTests.cs
│   ├── NuciText.Obfuscation.UnitTests.csproj
│   ├── bin/                     # Build output (gitignored)
│   └── obj/                     # Build intermediates (gitignored)
├── docs/                        # Documentation (this directory)
├── ARCHITECTURE.md              # Root architecture document
├── CODE_OF_CONDUCT.md           # Code of conduct
├── CONTRIBUTING.md              # Contribution guidelines
├── LICENSE                      # GPL-3.0-or-later license
├── NuciText.Obfuscation.sln     # Solution file
├── PRIVACY.md                   # Privacy policy
├── README.md                    # Project README
├── SECURITY.md                  # Security policy
```

## Module Organisation

### NuciText.Obfuscation (Main Library)

**Namespace:** `NuciText.Obfuscation`

| File | Purpose |
|------|---------|
| `INuciText.Obfuscation.cs` | Public interface `INuciTextObfuscator` |
| `NuciText.Obfuscation.cs` | Implementation `NuciTextObfuscator` with mapping tables |
| `NuciTextObfuscatorOptions.cs` | Configuration class `NuciTextObfuscatorOptions` |

**Project Configuration:**
- Target Framework: net10.0
- ImplicitUsings: disabled
- Nullable: enabled
- LangVersion: latest
- IsPackable: true (NuGet package)

### NuciText.Obfuscation.UnitTests (Test Project)

**Namespace:** `NuciText.Obfuscation.UnitTests`

| File | Purpose |
|------|---------|
| `NuciTextObfuscatorTests.cs` | Unit tests for obfuscator |

**Project Configuration:**
- Target Framework: net10.0
- IsPackable: false
- IsTestProject: true (implicit via test SDK)
- Dependencies: NUnit 4.x, Moq, Microsoft.NET.Test.Sdk

## Build Artefacts

The following directories are gitignored and generated during build:
- `bin/` — Compiled assemblies
- `obj/` — MSBuild intermediates, generated files

## Documentation Structure

```
docs/
├── INDEX.md
├── architecture.md
├── repository-overview.md
├── repository-structure.md
├── quick-start.md
├── api-reference/
│   ├── INDEX.md
│   └── inuci-textobfuscator.md
├── components/
│   ├── nuci-textobfuscator.md
│   ├── mapping-tables.md
│   └── options.md
├── flows/
│   ├── obfuscation.md
│   └── deobfuscation.md
├── behaviour/
│   ├── basic-obfuscation.md
│   ├── deterministic-output.md
│   └── approximate-replacements.md
├── api-usage-examples.md
├── build-and-deployment.md
├── configuration.md
├── data-model.md
├── dependencies.md
├── design-decisions.md
├── error-handling.md
├── invariants.md
├── integrations.md
├── logging.md
├── security.md
├── state-and-persistence.md
├── concurrency-and-scheduling.md
├── testing.md
├── change-guide.md
├── documentation-maintenance.md
├── troubleshooting.md
├── faq.md
└── ambiguities-and-open-questions.md
```

## Naming Conventions

- Projects: PascalCase (`NuciText.Obfuscation`)
- Namespaces: PascalCase matching project (`NuciText.Obfuscation`)
- Files: PascalCase matching type (`NuciText.Obfuscation.cs`)
- Interfaces: `I` prefix (`INuciTextObfuscator`)
- Implementation: Same name without `I` (`NuciTextObfuscator`)
- Tests: Subject + `Tests` (`NuciTextObfuscatorTests`)