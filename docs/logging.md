# Logging

## Overview

The library currently has **no logging infrastructure**. This document defines the logging requirements for future implementation.

## Requirements

### 1. No Logging in Normal Operation

- Obfuscation and deobfuscation must not log during normal operation
- Logging must not affect performance of the hot path
- No log statements in `Obfuscate` or `Deobfuscate` methods

### 2. No External Logging Dependencies

- Must not add NuGet packages for logging (e.g., Microsoft.Extensions.Logging)
- Must not introduce `ILogger` abstractions
- Zero-dependency policy takes precedence

### 3. Optional Diagnostic Output

- If diagnostics are needed, use `System.Diagnostics.Debug.WriteLine`
- Debug output must be compiled out in Release builds
- No conditional compilation symbols required (Debug.WriteLine is no-op in Release)

### 4. No Sensitive Data in Logs

- Never log input text, obfuscated text, or deobfuscated text
- Never log seeds or options values
- If diagnostics are added, log only operation counts or timing

### 5. No Exception Logging

- The library does not catch exceptions (see error-handling.md)
- No try/catch blocks for logging purposes
- Exceptions propagate to caller

### 6. No Telemetry

- No usage tracking
- No analytics
- No phone-home
- No metrics collection

### 7. No EventSource

- No `EventSource` implementation
- No ETW/EventPipe integration
- Keep dependency footprint minimal

### 8. No Trace Listeners

- No custom `TraceListener` implementations
- No configuration of `System.Diagnostics.Trace`
- No `Trace.CorrelationManager` usage

### 9. No Console Output

- No `Console.WriteLine` in library code
- No `Console.Error` writes
- Library must be usable in headless environments

### 10. No File I/O

- No log files
- No temp files
- No file system access

### 11. No Network I/O

- No remote logging
- No HTTP endpoints
- No socket connections

### 12. No Static State for Logging

- No static loggers
- No static counters
- No static diagnostic state
- All state must be instance-based (see state-and-persistence.md)

## Future Implementation Guidance

If logging is ever added:

1. **Prefer callback pattern** over dependency injection:
   ```csharp
   public NuciTextObfuscator(int seed, Action<string>? diagnostic = null)
   ```

2. **Keep it optional** — null callback means no diagnostics

3. **Never log PII** — text content, seeds, options

4. **Measure first** — add diagnostics only when a real need is identified

5. **Document the change** — update this file, ARCHITECTURE.md, and README.md

## Verification

These requirements are verified by:

- Code review (no logging statements in source)
- Dependency check (`dotnet list package` shows zero runtime dependencies)
- Static analysis (no `Console`, `Trace`, `Debug` in Release builds)
- Unit tests (no log output captured during test runs)

## Related Documents

- [error-handling.md](error-handling.md) — Exception policy
- [state-and-persistence.md](state-and-persistence.md) — State management
- [dependencies.md](dependencies.md) — Zero-dependency policy
- [security.md](security.md) — Data handling policy