# Quick Start

## Installation

```bash
dotnet add package NuciText.Obfuscation
```

## Basic Usage

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator();
string original = "Hello world!";
string obfuscated = obfuscator.Obfuscate(original);
string restored = obfuscator.Deobfuscate(obfuscated);

Console.WriteLine($"Original:    {original}");
Console.WriteLine($"Obfuscated:  {obfuscated}");
Console.WriteLine($"Restored:    {restored}");
// Output:
// Original:    Hello world!
// Obfuscated:  Ꮋеllo world!
// Restored:    Hello world!
```

## Deterministic Output (Testing)

```csharp
using NuciText.Obfuscation;

// Same seed = same output every time
var obfuscator = new NuciTextObfuscator(12345);
string result = obfuscator.Obfuscate("Test string!");
// result == "Ꭲеst strіng!" (always)
```

## Stronger Obfuscation

```csharp
using NuciText.Obfuscation;

var options = new NuciTextObfuscatorOptions
{
    UseApproximateReplacements = true
};

var obfuscator = new NuciTextObfuscator(42);
string result = obfuscator.Obfuscate("Secret data", options);
// More visual variation, uses Cherokee/Cyrillic/Greek characters
```

## String Seed

```csharp
using NuciText.Obfuscation;

// Semantic seed for per-user/session obfuscation
var obfuscator = new NuciTextObfuscator("user-session-abc123");
string result = obfuscator.Obfuscate("User data");
```

## Complete Example

```csharp
using NuciText.Obfuscation;

class Program
{
    static void Main()
    {
        // Create obfuscator with fixed seed for demo
        var obfuscator = new NuciTextObfuscator(12345);

        // Optional: enable approximate replacements
        var options = new NuciTextObfuscatorOptions
        {
            UseApproximateReplacements = true
        };

        string[] samples = [
            "Hello world!",
            "Password: secret123",
            "Email: user@example.com",
            "Credit card: 4111-1111-1111-1111"
        ];

        foreach (var sample in samples)
        {
            var obfuscated = obfuscator.Obfuscate(sample, options);
            var restored = obfuscator.Deobfuscate(obfuscated);

            Console.WriteLine($"Original:    {sample}");
            Console.WriteLine($"Obfuscated:  {obfuscated}");
            Console.WriteLine($"Restored:    {restored}");
            Console.WriteLine($"Match: {sample == restored}");
            Console.WriteLine();
        }
    }
}
```

## Key Points

| Concept | Detail |
|---------|--------|
| **Reversible** | `Deobfuscate(Obfuscate(x)) == x` always |
| **Deterministic** | Same seed + same input = same output |
| **Probabilistic** | ~60% of mappable chars replaced per call |
| **Two modes** | Identical (default) or Approximate (stronger) |
| **No dependencies** | Zero runtime dependencies |
| **Thread safety** | Not thread-safe — use one instance per thread |

## Next Steps

- [API Usage Examples](api-usage-examples.md) — More patterns
- [Configuration](configuration.md) — Options reference
- [Behaviour](behaviour/basic-obfuscation.md) — How it works
- [Integrations](integrations.md) — DI, ASP.NET Core, etc.