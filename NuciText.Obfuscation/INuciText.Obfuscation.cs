namespace NuciText.Obfuscation
{
    /// <summary>
    /// Defines the interface for the NuciText obfuscator.
    /// </summary>
    public interface INuciTextObfuscator
    {
        /// <summary>
        /// Deobfuscates the specified text.
        /// </summary>
        /// <param name="text">The text to deobfuscate.</param>
        /// <returns>The deobfuscated text.</returns>
        string Deobfuscate(string text);

        /// <summary>
        /// Obfuscates the specified text.
        /// </summary>
        /// <param name="text">The text to obfuscate.</param>
        /// <returns>The obfuscated text.</returns>
        string Obfuscate(string text);

        /// <summary>
        /// Obfuscates the specified text using the provided options.
        /// </summary>
        /// <param name="text">The text to obfuscate.</param>
        /// <param name="options">The options to use for obfuscation.</param>
        /// <returns>The obfuscated text.</returns>
        string Obfuscate(string text, NuciTextObfuscatorOptions options);
    }
}
