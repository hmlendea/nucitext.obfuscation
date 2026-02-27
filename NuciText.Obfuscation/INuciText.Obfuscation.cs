namespace NuciText.Obfuscation
{
    public interface INuciTextObfuscator
    {
        string Deobfuscate(string text);

        string Obfuscate(string text);
    }
}
