namespace NuciText.Obfuscation
{
    /// <summary>
    /// Defines the options for the NuciText obfuscator, allowing customisation of the obfuscation process.
    /// </summary>
    public sealed class NuciTextObfuscatorOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use approximate replacements for characters.
        /// </summary>
        public bool UseApproximateReplacements { get; set; } = false;
    }
}
