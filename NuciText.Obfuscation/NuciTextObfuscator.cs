using System;
using System.Collections.Generic;
using System.Text;

namespace NuciText.Obfuscation
{
    /// <summary>
    /// Provides deterministic obfuscation and deobfuscation of text using visually similar replacements.
    /// </summary>
    /// <param name="seed">The seed used to make the obfuscation deterministic.</param>
    public sealed class NuciTextObfuscator(int seed) : INuciTextObfuscator
    {
        private static readonly Dictionary<string, string> identicalGroupReplacements = new()
        {
            { "**", "ᕯ" },
            { "!!", "‼" },
            { "!?", "⁉" },
            { "??", "⁇" },
            { "?!", "⁈" },
            { "...", "…" },
            { " ", "     " }
        };

        private static readonly Dictionary<string, string> approximateGroupReplacements = new()
        {
            { "II", "Ⅱ" },
            { "III", "Ⅲ" },
            { "IV", "Ⅳ" },
            { "VI", "Ⅵ" },
            { "VII", "Ⅶ" },
            { "VIII", "Ⅷ" },
            { "IX", "Ⅸ" },
            { "XI", "Ⅺ" },
            { "XII", "Ⅻ" },

            { "ii", "ⅱ" },
            { "iii", "ⅲ" },
            { "iv", "ⅳ" },
            { "vi", "ⅵ" },
            { "vii", "ⅶ" },
            { "viii", "ⅷ" },
            { "ix", "ⅸ" },
            { "xi", "ⅺ" },
            { "xii", "ⅻ" },

            { "DZ", "Ǳ" },
            { "Dz", "ǲ" },
            { "IJ", "Ĳ" },
            { "LJ", "Ǉ" },
            { "Lj", "ǈ" },
            { "NJ", "Ǌ" },
            { "Nj", "ǋ" },

            { "ae", "æӕ" },
            { "bl", "Ы" },
            { "dz", "ǳʣ" },
            { "ij", "ĳ" },
            { "lj", "ǉ" },
            { "lm", "㏐" },
            { "ln", "㏑" },
            { "log", "㏒" },
            { "ls", "ʪ" },
            { "lx", "㏓" },
            { "lz", "ʫ" },
            { "mb", "㏔" },
            { "mil", "㏕" },
            { "mol", "㏖" },
            { "nj", "ǌ" },
            { "oy", "ѹ" },
            { "Oy", "Ѹ" },
            { "ts", "ʦ" }
        };

        private static readonly Dictionary<char, string> identicalReplacements = new()
        {
            { '`', "՝" },
            { ',', "͵" },
            { ':', "։꞉" },
            { ';', ";" },
            { '\'', "′´ʹ‘’" },
            { '"', "”″" },
            { '|', "ǀ" },
            { '/', "⁄⧸" },
            { '\\', "⧵⧹" },
            { '-', "‐–" },
            { '+', "𖫵" },
            { '<', "ᐸ𖫬ⵦ" },
            { '>', "ᐳ" },
            { '3', "ƷЗӠ" },

            { 'Æ', "Ӕ" },
            { 'æ', "ӕ" },

            { 'A', "АΑꓮ" }, // The following don't work in some fonts: A.
            { 'Ă', "ӐǍ" },
            { 'Â', "Ȃ" },
            { 'B', "ВΒꓐ" }, // The following don't work in some fonts: B.
            { 'D', "ᗞꓓ" }, // The following don't work in some fonts: D.
            { 'Đ', "ÐƉ" },
            { 'Ð', "ĐƉ" },
            { 'Ɖ', "ÐĐ" },
            { 'E', "ЕΕꓰ" }, // The following don't work in some fonts: E.
            { 'Ĕ', "Ӗ" },
            { 'Ë', "Ё" },
            { 'F', "ꓝ" }, // The following don't work in some fonts: F.
            { 'G', "ꓖ" }, // The following don't work in some fonts: G.
            { 'H', "НΗꓧ" }, // The following don't work in some fonts: H.
            { 'I', "ІΙӀӏ" },
            { 'Î', "Ȋ" },
            { 'Ï', "ЇΪ" },
            { 'J', "Јꓙ" }, // The following don't work in some fonts: J // The following look different in some fonts: Ϳ.
            { 'K', "КKΚꓗ" }, // The following don't work in some fonts: K.
            { 'Ḱ', "Ќ" },
            { 'L', "Ꮮꓡ" }, // The following don't work in some fonts: L𐐛.
            { 'M', "МΜϺ" }, // The following don't work in some fonts: M.
            { 'N', "Νꓠ" }, // The following don't work in some fonts: N.
            { 'O', "ОΟՕꓳ" }, // The following don't work in some fonts: O𐓂𖫩.
            { 'Ö', "Ӧ" },
            { 'Ө', "Ѳθ" },
            { 'ϴ', "Ɵ" },
            { 'P', "РΡ" }, // The following don't work in some fonts: P.
            { 'Q', "Ԛ" }, // The following don't work in some fonts: Q.
            { 'S', "Ѕჽ" }, // The following don't work in some fonts: 𖫖S.
            { 'Ṣ', "ȘŞ" },
            { 'Ş', "ȘṢ" },
            { 'Ș', "ȘṢ" },
            { 'Ţ', "ȚṬ" },
            { 'Ț', "ȚŢ" },
            { 'Ṭ', "ȚŢ" },
            { 'U', "ꓴՍ" }, // The following don't work in some fonts: U𐓎.
            { 'V', "ⴸꛟꓦ" }, // The following don't work in some fonts: V.
            { 'W', "Ԝꓪ" }, // The following don't work in some fonts: W.
            { 'X', "ХΧⵝꓫ" }, // The following don't work in some fonts: X.
            { 'Y', "ΥҮꓬ" }, // The following don't work in some fonts: Y.
            { 'Z', "Ζꓜ" }, // The following don't work in some fonts: ZჍꛉ.

            { 'a', "а" }, // The following don't work in some fonts: a.
            { 'ă', "ӑǎ" },
            { 'â', "ȃ" },
            { 'c', "сϲᴄ" },
            { 'e', "е" }, // The following don't work in some fonts: e.
            { 'è', "ѐ" },
            { 'ë', "ё" },
            { 'ĕ', "ӗ" },
            { 'i', "і" },
            { 'î', "ȋ" },
            { 'ï', "ї" },
            { 'j', "ј" },
            { 'k', "ĸк" },
            { 'ɫ', "ɬᏐ" },
            { 'o', "оօο" }, // The following don't work in some fonts: 𐓪𐐬 // The following is looks too differnt in some fonts: ௦.
            { 'ö', "ӧ" },
            { 'ó', "όό" },
            { 'ò', "ὸ" },
            { 'ө', "ɵꮎ" },
            { 'θ', "ӨѲ𖺀" },
            { 'p', "р" },
            { 'q', "ԛ" },
            { 's', "ѕ" }, // The following don't work in some fonts: s.
            { 'ș', "şṣ" },
            { 'ş', "șṣ" },
            { 'ṣ', "șş" },
            { 'ț', "ţṭ" },
            { 'ţ', "țṭ" },
            { 'ṭ', "țţ" },
            { 'w', "ԝꮃ" },
            { 'x', "х" },
            { 'y', "уү" },
            { 'ÿ', "ӱ" },
            { 'z', "ꮓ" }
        };

        private static readonly Dictionary<char, string> approximateReplacements = new()
        {
            { '\'', "ʹ" },

            // "0": "߀", // This can turn the text left-to-right.
            { '3', "ӠЗ" }, // The following look too different: Ʒ.
            { '5', "Ƽ" },
            { '6', "бᏮ" },

            { 'A', "ᗅᎪ" },
            { 'Ă', "Ā" },
            { 'B', "Ᏼᗷꕗ" },
            { 'C', "ᏟᑕC" }, // C makes the next character uppercase on iOS.
            { 'D', "ᎠD" }, // "D" makes the next character uppercase on iOS.
            { 'E', "Ꭼⴹ⋿ꗋ" },
            { 'F', "Ϝ" }, // Ϝ does not look identical in some fonts.
            { 'G', "ᏀႺ" },
            { 'H', "Ꮋ" },
            { 'I', "ꓲIⵏߊꕯ" }, // Stretch // The following look different on some fonts: ꕯ // The following don't work on some fonts: I𞥑 𞠢.
            // The following looks too different: Լ.
            { 'J', "Ꭻ" },
            { 'K', "Ꮶ" },
            { 'L', "Ⅼ" },
            { 'M', "ꓟᎷⅯ" },
            { 'O', "ⵔ" },
            { 'Ө', "ƟϴᎾ" },
            { 'P', "ꓑᏢ" },
            { 'Q', "ǪႭ" },
            { 'R', "ᎡꓣᏒ" },
            { 'S', "ꓢᏚՏႽ" },
            { 'T', "ㄒᎢꔋ" },
            { 'Ț', "Ҭ" },
            // { 'U', // The following don't work in some fonts: 𞤋 // The following look too different in some fonts: ᑌ⋃.
            { 'V', "ᐯᏙⅤ" },
            { 'W', "ᎳᏔ" },
            // { 'X', "᙭Ꭓχ" } // The candidates for X look too different in some fonts.
            { 'X', "Ⅹ" },
            { 'Z', "Ꮓ" },

            { 'ă', "ā" },
            // { 'b', // The following are broken in some fonts: ხ // The following look different in some fonts: ᏏЬƅ.
            { 'c', "ꮯc" }, // "ꮯ" has a weird top right at least on iOS and "c" makes the next character after it uppercase.
            { 'd', "ძd" }, // The following don't work in some fonts: d. The following look too different in some fonts: ԁ. "d" makes the next character uppercase on iOS.

            { 'g', "ɡց" },
            // { 'h', "հ" }, // "հ" looks like some arabic letter on iOS.
            { 'h', "ᏂႹ" }, // "һ", // Confirmed to look quite different in many fonts.
            { 'i', "Ꭵiıɪ" }, // This might be a bit of a stretch.
            { 'ĭ', "ǐ" },
            { 'j', "ϳ" },
            { 'k', "κкκꮶ" },
            { 'ł', "ɫɬᏐ" },
            { 'ɫ', "ł" },
            { 'ɬ', "ł" },
            { 'm', "m" }, // The following don't work in some fonts: ՠ. ՠ also makes the next character uppercase on iOS.
            { 'o', "ഠჿ" }, // The following look different in some fonts: ೦. ჿ is broken in Instagram on Android.
            { 'p', "ρƿ" },
            { 's', "ടꮪꜱ" }, // ꜱ looks ok but doesn't work on e.g. Apple Watch.
            // For the letter u, the following don't work in some fonts: 𐓶.
            { 'u', "υ" },
            { 'ú', "ύ" },
            { 'v', "ꮩνv" }, // The following have large paddings: ∨.
            { 'w', "ꮤ" }, // The following look different in some fonts: ꮃ.
            { 'x', "ⅹ" },
            { 'y', "γʏ" }
        };

        private readonly Random randomGenerator = new(seed);

        private static int ReplacementRollLowerBound => 1;

        private static int ReplacementRollUpperBound => 11;

        private static int ReplacementRollSuccessThreshold => 6;

        /// <summary>
        /// Initialises a new instance of the <see cref="NuciTextObfuscator"/> class with a time-based seed.
        /// </summary>
        public NuciTextObfuscator() : this(Environment.TickCount)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="NuciTextObfuscator"/> class with the specified seed.
        /// </summary>
        /// <param name="seed">The seed used to make the obfuscation deterministic.</param>
        public NuciTextObfuscator(string seed) : this(seed.GetHashCode())
        {
        }

        /// <summary>
        /// Deobfuscates the specified text.
        /// </summary>
        /// <param name="text">The text to deobfuscate.</param>
        /// <returns>The deobfuscated text.</returns>
        public string? Deobfuscate(string? text)
        {
            if (text is null)
            {
                return null;
            }

            if (string.Equals(text, string.Empty))
            {
                return string.Empty;
            }

            string input = ApplyDeterministicGroupReplacements(text, identicalGroupReplacements);
            input = ApplyDeterministicGroupReplacements(input, approximateGroupReplacements);

            return RestoreCharacters(input);
        }

        /// <summary>
        /// Obfuscates the specified text.
        /// </summary>
        /// <param name="text">The text to obfuscate.</param>
        /// <returns>The obfuscated text.</returns>
        public string? Obfuscate(string? text)
            => Obfuscate(text, new NuciTextObfuscatorOptions());

        /// <summary>
        /// Obfuscates the specified text using the provided options.
        /// </summary>
        /// <param name="text">The text to obfuscate.</param>
        /// <param name="options">The options to use for obfuscation.</param>
        /// <returns>The obfuscated text.</returns>
        public string? Obfuscate(string? text, NuciTextObfuscatorOptions options)
        {
            if (text is null)
            {
                return null;
            }

            if (string.Equals(text, string.Empty))
            {
                return string.Empty;
            }

            options ??= new NuciTextObfuscatorOptions();

            string input = ApplyRandomGroupReplacements(text, identicalGroupReplacements);

            if (options.UseApproximateReplacements)
            {
                input = ApplyRandomGroupReplacements(input, approximateGroupReplacements);
            }

            return ObfuscateCharacters(input, options);
        }

        private static string ApplyDeterministicGroupReplacements(
            string input,
            IEnumerable<KeyValuePair<string, string>> replacements)
        {
            string result = input;

            foreach (KeyValuePair<string, string> entry in replacements)
            {
                result = ApplyDeterministicGroupReplacement(result, entry);
            }

            return result;
        }

        private static string ApplyDeterministicGroupReplacement(
            string input,
            KeyValuePair<string, string> entry)
        {
            string result = input;
            string originalGroup = entry.Key;
            string candidates = entry.Value;

            if (string.IsNullOrEmpty(candidates))
            {
                return result;
            }

            foreach (char candidate in candidates)
            {
                result = result.Replace(candidate.ToString(), originalGroup);
            }

            return result;
        }

        private string ApplyRandomGroupReplacements(
            string input,
            IEnumerable<KeyValuePair<string, string>> replacements)
        {
            string result = input;

            foreach (KeyValuePair<string, string> entry in replacements)
            {
                string group = entry.Key;
                string candidates = entry.Value;
                string replacement = SelectGroupReplacement(group, candidates);

                result = result.Replace(group, replacement);
            }

            return result;
        }

        private string SelectGroupReplacement(string group, string candidates)
        {
            string replacement = group;

            if (!string.IsNullOrEmpty(candidates) && ShouldReplace())
            {
                replacement = PickRandomCandidate(candidates);
            }

            return replacement;
        }

        private string ObfuscateCharacters(string input, NuciTextObfuscatorOptions options)
        {
            StringBuilder builder = new(input.Length);

            foreach (char character in input)
            {
                builder.Append(ObfuscateCharacter(character, options));
            }

            return builder.ToString();
        }

        private string ObfuscateCharacter(char character, NuciTextObfuscatorOptions options)
        {
            string candidates = CollectCandidates(character, options);
            string obfuscatedCharacter = character.ToString();

            if (!string.IsNullOrEmpty(candidates) && ShouldReplace())
            {
                obfuscatedCharacter = PickRandomCandidate(candidates);
            }

            return obfuscatedCharacter;
        }

        private static string CollectCandidates(
            char character,
            NuciTextObfuscatorOptions options)
        {
            StringBuilder candidatesBuilder = new();

            if (identicalReplacements.ContainsKey(character))
            {
                candidatesBuilder.Append(identicalReplacements[character]);
            }

            if (options.UseApproximateReplacements &&
                approximateReplacements.ContainsKey(character))
            {
                candidatesBuilder.Append(approximateReplacements[character]);
            }

            return candidatesBuilder.ToString();
        }

        private bool ShouldReplace()
        {
            int roll = randomGenerator.Next(ReplacementRollLowerBound, ReplacementRollUpperBound);

            return roll <= ReplacementRollSuccessThreshold;
        }

        private string PickRandomCandidate(string candidates)
        {
            int index = randomGenerator.Next(candidates.Length);

            return candidates[index].ToString();
        }

        private static string RestoreCharacters(string input)
        {
            StringBuilder builder = new(input.Length);

            foreach (char character in input)
            {
                builder.Append(RestoreCharacter(character));
            }

            return builder.ToString();
        }

        private static char RestoreCharacter(char character)
        {
            char restoredCharacter = character;
            char? originalCharacter = FindOriginalCharacter(character, identicalReplacements);

            if (originalCharacter is null)
            {
                originalCharacter = FindOriginalCharacter(character, approximateReplacements);
            }

            if (originalCharacter is not null)
            {
                restoredCharacter = originalCharacter.Value;
            }

            return restoredCharacter;
        }

        private static char? FindOriginalCharacter(
            char character,
            IEnumerable<KeyValuePair<char, string>> replacements)
        {
            foreach (KeyValuePair<char, string> entry in replacements)
            {
                if (!string.IsNullOrEmpty(entry.Value) &&
                    entry.Value.Contains(character))
                {
                    return entry.Key;
                }
            }

            return null;
        }
    }
}