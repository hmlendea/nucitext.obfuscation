# NuciText.Obfuscation Documentation

Master index and navigation for the NuciText.Obfuscation documentation.

## Repository Summary

NuciText.Obfuscation is a lightweight .NET library for text obfuscation using visually similar Unicode characters. It provides deterministic obfuscation and deobfuscation capabilities with zero external dependencies, suitable for inclusion in any .NET application.

## Root Document Map

- [ARCHITECTURE.md](../ARCHITECTURE.md) — High-level architecture
- [SECURITY.md](../SECURITY.md) — Security policy and vulnerability reporting
- [ROADMAP.md](../ROADMAP.md) — Project roadmap (if present)
- [PRIVACY.md](../PRIVACY.md) — Privacy and data handling
- [LICENSE](../LICENSE) — License (GPL-3.0-or-later)
- [CONTRIBUTING.md](../CONTRIBUTING.md) — Contribution guidelines
- [CODE_OF_CONDUCT.md](../CODE_OF_CONDUCT.md) — Code of conduct

## Documentation Catalogue

### Getting Started
- [Quick Start](./quick-start.md) — Getting started guide for newcomers
- [Repository Overview](./repository-overview.md) — Repository purpose, scope, entry points
- [Repository Structure](./repository-structure.md) — Source tree layout, module organisation

### Architecture & Design
- [Architecture](./architecture.md) — High-level architecture (complements root ARCHITECTURE.md)
- [Components](./components/) — Per-component deep dives
- [Flows](./flows/) — End-to-end execution flows
- [Design Decisions](./design-decisions.md) — Key architectural/design choices and rationale
- [Dependencies](./dependencies.md) — External and internal dependencies

### API Reference
- [API Reference Index](./api-reference/INDEX.md) — API reference index, endpoint summary table
- [INuciTextObfuscator](./api-reference/inuci-textobfuscator.md) — Interface documentation

### Behaviour & Usage
- [Behaviour](./behaviour/) — User-facing behaviours
- [API Usage Examples](./api-usage-examples.md) — Example requests/responses for API usage

### Operations
- [Build and Deployment](./build-and-deployment.md) — Build pipeline, deployment, environments
- [Configuration](./configuration.md) — Configuration schema, sources, precedence
- [Logging](./logging.md) — Logging framework, levels, structured fields, correlation, sinks
- [Testing](./testing.md) — Test strategy, organisation, coverage

### Reference
- [Data Model](./data-model.md) — Domain entities, relationships, persistence
- [Error Handling](./error-handling.md) — Error taxonomy, handling patterns, recovery
- [Security](./security.md) — Security model, threats, mitigations (complements root SECURITY.md)
- [State and Persistence](./state-and-persistence.md) — State management, stores, caches, migrations
- [Concurrency and Scheduling](./concurrency-and-scheduling.md) — Threading, async, schedulers, locks
- [Invariants](./invariants.md) — System-wide invariants and contracts
- [Integrations](./integrations.md) — External system integrations

### Maintenance
- [Change Guide](./change-guide.md) — How to modify common areas safely
- [Documentation Maintenance](./documentation-maintenance.md) — How to keep docs current, ownership
- [Troubleshooting](./troubleshooting.md) — Common issues and solutions
- [FAQ](./faq.md) — Frequently asked questions
- [Ambiguities and Open Questions](./ambiguities-and-open-questions.md) — Unresolved items, TODOs, known gaps

## Navigation Aids

- **Start here** (newcomers): [Quick Start](./quick-start.md) → [Repository Overview](./repository-overview.md) → [API Reference Index](./api-reference/INDEX.md)
- **Deep dive** (component owners): [Architecture](./architecture.md) → [Components](./components/) → [Flows](./flows/)
- **Flows** (debuggers): [Flows](./flows/) → [Error Handling](./error-handling.md) → [Troubleshooting](./troubleshooting.md)

## Maintenance Metadata

- Last reviewed: 2026-11-02
- Owner: Horațiu Mlendea
- Coverage status: Initial generation