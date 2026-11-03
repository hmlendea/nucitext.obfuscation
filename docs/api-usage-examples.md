# API Usage Examples

## Basic Obfuscation

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator();
string input = "Hello world!";
string obfuscated = obfuscator.Obfuscate(input);
string restored = obfuscator.Deobfuscate(obfuscated);

Console.WriteLine($"Original: {input}");
Console.WriteLine($"Obfuscated: {obfuscated}");
Console.WriteLine($"Restored: {restored}");
```

## Deterministic Obfuscation (Testing)

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator(123456789);
string input = "Test string!";
string obfuscated = obfuscator.Obfuscate(input);
// obfuscated == "Ꭲеst strіng!"

string restored = obfuscator.Deobfuscate(obfuscated);
// restored == "Test string!"
```

## Strong Obfuscation with Approximate Replacements

```csharp
using NuciText.Obfuscation;

var options = new NuciTextObfuscatorOptions
{
    UseApproximateReplacements = true
};

var obfuscator = new NuciTextObfuscator(42);
string input = "Secret message 123";
string obfuscated = obfuscator.Obfuscate(input, options);
string restored = obfuscator.Deobfuscate(obfuscated);
```

## String Seed

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator("my-application-seed");
string obfuscated = obfuscator.Obfuscate("User data");
// Deterministic based on "my-application-seed".GetHashCode()
```

## Dependency Injection (ASP.NET Core)

```csharp
// Program.cs or Startup.cs
builder.Services.AddSingleton<INuciTextObfuscator>(sp =>
    new NuciTextObfuscator(Environment.TickCount)); // Or fixed seed for testing

// Controller
public class MyController : ControllerBase
{
    private readonly INuciTextObfuscator _obfuscator;

    public MyController(INuciTextObfuscator obfuscator)
    {
        _obfuscator = obfuscator;
    }

    [HttpPost("obfuscate")]
    public IActionResult Obfuscate([FromBody] string text)
    {
        var result = _obfuscator.Obfuscate(text);
        return Ok(result);
    }
}
```

## Round-Trip Verification

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator(12345);
var options = new NuciTextObfuscatorOptions { UseApproximateReplacements = true };

string[] testCases = [
    "Simple text",
    "Text with numbers 123",
    "Special chars: !@#$%^&*()",
    "Unicode: café naïve résumé",
    "Multiple   spaces",
    "Line\nbreaks\ttabs"
];

foreach (var input in testCases)
{
    var obfuscated = obfuscator.Obfuscate(input, options);
    var restored = obfuscator.Deobfuscate(obfuscated);

    Console.WriteLine($"Original:  {input}");
    Console.WriteLine($"Obfuscated: {obfuscated}");
    Console.WriteLine($"Restored:  {restored}");
    Console.WriteLine($"Match: {input == restored}");
    Console.WriteLine();
}
```

## Null and Empty Handling

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator();

// Null input
string nullResult = obfuscator.Obfuscate(null);        // Returns null
string nullDeobf = obfuscator.Deobfuscate(null);       // Returns null

// Empty string
string emptyResult = obfuscator.Obfuscate("");         // Returns ""
string emptyDeobf = obfuscator.Deobfuscate("");        // Returns ""
```

## Multiple Operations on Same Instance

```csharp
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator(12345);

// Each call advances the Random state
string first = obfuscator.Obfuscate("Test");   // Uses first random values
string second = obfuscator.Obfuscate("Test");  // Uses next random values (different output)

// For identical results, create new instance or use same seed
var obfuscator2 = new NuciTextObfuscator(12345);
string third = obfuscator2.Obfuscate("Test");  // Same as first
```