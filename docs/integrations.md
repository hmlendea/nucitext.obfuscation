# Integrations

## ASP.NET Core Dependency Injection

```csharp
// Program.cs
builder.Services.AddSingleton<INuciTextObfuscator>(sp =>
    new NuciTextObfuscator(Environment.TickCount));

// Or with fixed seed for testing
builder.Services.AddSingleton<INuciTextObfuscator>(sp =>
    new NuciTextObfuscator(12345));

// Controller usage
public class TextController : ControllerBase
{
    private readonly INuciTextObfuscator _obfuscator;

    public TextController(INuciTextObfuscator obfuscator)
    {
        _obfuscator = obfuscator;
    }

    [HttpPost("obfuscate")]
    public IActionResult Obfuscate([FromBody] ObfuscateRequest request)
    {
        var options = request.UseApproximate
            ? new NuciTextObfuscatorOptions { UseApproximateReplacements = true }
            : null;

        var result = _obfuscator.Obfuscate(request.Text, options);
        return Ok(new { obfuscated = result });
    }
}
```

## Configuration Binding

```csharp
// appsettings.json
{
  "NuciTextObfuscator": {
    "UseApproximateReplacements": true
  }
}

// Program.cs
builder.Services.Configure<NuciTextObfuscatorOptions>(
    builder.Configuration.GetSection("NuciTextObfuscator"));

builder.Services.AddSingleton<INuciTextObfuscator>(sp =>
{
    var options = sp.GetRequiredService<IOptions<NuciTextObfuscatorOptions>>().Value;
    var seed = builder.Configuration.GetValue<int?>("ObfuscationSeed") ?? Environment.TickCount;
    return new NuciTextObfuscator(seed);
});

// Usage with options injection
public class MyService
{
    private readonly INuciTextObfuscator _obfuscator;
    private readonly NuciTextObfuscatorOptions _options;

    public MyService(INuciTextObfuscator obfuscator, IOptions<NuciTextObfuscatorOptions> options)
    {
        _obfuscator = obfuscator;
        _options = options.Value;
    }

    public string Process(string input) => _obfuscator.Obfuscate(input, _options);
}
```

## Minimal API

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<INuciTextObfuscator>(_ => new NuciTextObfuscator(12345));

var app = builder.Build();

app.MapPost("/obfuscate", (INuciTextObfuscator obfuscator, ObfuscateRequest request) =>
{
    var options = request.UseApproximate
        ? new NuciTextObfuscatorOptions { UseApproximateReplacements = true }
        : null;
    return Results.Ok(new { obfuscated = obfuscator.Obfuscate(request.Text, options) });
});

app.MapPost("/deobfuscate", (INuciTextObfuscator obfuscator, DeobfuscateRequest request) =>
    Results.Ok(new { deobfuscated = obfuscator.Deobfuscate(request.Text) }));

app.Run();

record ObfuscateRequest(string Text, bool UseApproximate);
record DeobfuscateRequest(string Text);
```

## Console Application

```csharp
// Program.cs
using NuciText.Obfuscation;

var obfuscator = new NuciTextObfuscator(args.Length > 0 ? args[0].GetHashCode() : 12345);
var options = args.Contains("--approximate")
    ? new NuciTextObfuscatorOptions { UseApproximateReplacements = true }
    : null;

string input = args.Length > 1 ? args[1] : Console.ReadLine() ?? "";
string output = obfuscator.Obfuscate(input, options);
Console.WriteLine(output);
```

## WPF / WinForms / MAUI

```csharp
// ViewModel or code-behind
private readonly INuciTextObfuscator _obfuscator = new NuciTextObfuscator(12345);

private void OnObfuscateClick(object sender, EventArgs e)
{
    var options = chkApproximate.IsChecked == true
        ? new NuciTextObfuscatorOptions { UseApproximateReplacements = true }
        : null;

    txtOutput.Text = _obfuscator.Obfuscate(txtInput.Text, options);
}

private void OnDeobfuscateClick(object sender, EventArgs e)
{
    txtOutput.Text = _obfuscator.Deobfuscate(txtInput.Text);
}
```

## Blazor (WebAssembly / Server)

```razor
@page "/obfuscator"
@inject INuciTextObfuscator Obfuscator

<textarea @bind="inputText" rows="5" cols="50"></textarea>
<br />
<label><input type="checkbox" @bind="useApproximate" /> Approximate</label>
<br />
<button @onclick="Obfuscate">Obfuscate</button>
<button @onclick="Deobfuscate">Deobfuscate</button>
<br />
<textarea @bind="outputText" rows="5" cols="50" readonly></textarea>

@code {
    string inputText = "";
    string outputText = "";
    bool useApproximate = false;

    void Obfuscate()
    {
        var options = useApproximate
            ? new NuciTextObfuscatorOptions { UseApproximateReplacements = true }
            : null;
        outputText = Obfuscator.Obfuscate(inputText, options);
    }

    void Deobfuscate()
    {
        outputText = Obfuscator.Deobfuscate(inputText);
    }
}
```

## Azure Functions

```csharp
// Function.cs
public class ObfuscateFunction
{
    private readonly INuciTextObfuscator _obfuscator;

    public ObfuscateFunction(INuciTextObfuscator obfuscator)
    {
        _obfuscator = obfuscator;
    }

    [Function("Obfuscate")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        var request = await req.ReadFromJsonAsync<ObfuscateRequest>();
        var options = request.UseApproximate
            ? new NuciTextObfuscatorOptions { UseApproximateReplacements = true }
            : null;

        var result = _obfuscator.Obfuscate(request.Text, options);
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { obfuscated = result });
        return response;
    }
}

record ObfuscateRequest(string Text, bool UseApproximate);
```

## PowerShell Module

```powershell
# NuciTextObfuscation.psm1
Add-Type -Path "NuciText.Obfuscation.dll"

function Invoke-Obfuscate {
    param(
        [string]$Text,
        [int]$Seed = 12345,
        [switch]$Approximate
    )

    $obfuscator = [NuciText.Obfuscation.NuciTextObfuscator]::new($Seed)
    $options = if ($Approximate) {
        [NuciText.Obfuscation.NuciTextObfuscatorOptions]::new() | % { $_.UseApproximateReplacements = $true; $_ }
    } else { $null }

    return $obfuscator.Obfuscate($Text, $options)
}

function Invoke-Deobfuscate {
    param([string]$Text, [int]$Seed = 12345)
    $obfuscator = [NuciText.Obfuscation.NuciTextObfuscator]::new($Seed)
    return $obfuscator.Deobfuscate($Text)
}

Export-ModuleMember -Function Invoke-Obfuscate, Invoke-Deobfuscate
```

## Testing Integration

```csharp
// Test fixture
public class ObfuscationFixture : IDisposable
{
    public INuciTextObfuscator Obfuscator { get; } = new NuciTextObfuscator(123456789);
    public NuciTextObfuscatorOptions ApproximateOptions { get; } = new() { UseApproximateReplacements = true };

    public void Dispose() { }
}

// xUnit test
public class MyTests : IClassFixture<ObfuscationFixture>
{
    private readonly ObfuscationFixture _fixture;

    public MyTests(ObfuscationFixture fixture) => _fixture = fixture;

    [Fact]
    public void RoundTrip_Works()
    {
        var input = "Test data";
        var obfuscated = _fixture.Obfuscator.Obfuscate(input, _fixture.ApproximateOptions);
        var restored = _fixture.Obfuscator.Deobfuscate(obfuscated);
        Assert.Equal(input, restored);
    }
}
```

## Unsupported / Not Applicable

| Integration | Status | Reason |
|-------------|--------|--------|
| gRPC | Not applicable | Library is synchronous, no streaming |
| GraphQL | Not applicable | No schema, simple method calls |
| Message queues | Not applicable | No async/background processing |
| Database | Not applicable | No persistence layer |
| Caching | Not applicable | Deterministic, no cache needed |
| Logging | Not implemented | No logging in library (see logging.md) |