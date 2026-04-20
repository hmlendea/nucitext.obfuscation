using System;
using System.Collections.Generic;
using System.Text;

namespace NuciText.Obfuscation
{
    public sealed class NuciTextObfuscator(int seed) : INuciTextObfuscator
    {
        private readonly Random RandomGenerator = new(seed);

        private static readonly Dictionary<string, string> IdenticalGroupReplacements = new()
        {
            { "**", "ᕯ" },
            { "!!", "‼" },
            { "!?", "⁉" },
            { "??", "⁇" },
            { "?!", "⁈" },
            { "...", "…" },
            { " ", "     " }
        };

        private static readonly Dictionary<string, string> ApproximateGroupReplacements = new()
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

        private static readonly Dictionary<char, string> IdenticalReplacements = new()
        {
            { '`', "՝" },
            { ',', "͵" },
            { ':', "։꞉" },
            { ';', ";" },
            { '\'', "′´ʹ‘’" },
            { '\"', "”″" },
            { '|', "ǀ" },
            { 'ǀ', "|" },
            { '/', "⁄⧸" },
            { '\\', "⧵⧹" },
            { '-', "‐–" },
            { '+', "𖫵" },
            { '<', "ᐸ𖫬ⵦ" },
            { '>', "ᐳ" },
            { '3', "З" },
            { 'Ʒ', "Ӡ" },
            { 'Ӡ', "Ʒ" },

            { 'Æ', "Ӕ" },
            { 'æ', "ӕ" },

            { 'A', "АΑꓮ" }, // The following don't work in some fonts: 𝖠
            { 'Ă', "ӐǍ" },
            { 'Â', "Ȃ" },
            { 'B', "ВΒꓐ" }, // The following don't work in some fonts: 𝖡
            { 'D', "ᗞꓓ" }, // The following don't work in some fonts: 𝖣
            { 'Đ', "ÐƉ" },
            { 'Ð', "ĐƉ" },
            { 'Ɖ', "ÐĐ" },
            { 'E', "ЕΕꓰ" }, // The following don't work in some fonts: 𝖤
            { 'Ĕ', "Ӗ" },
            { 'Ë', "Ё" },
            { 'F', "ꓝ" }, // The following don't work in some fonts: 𝖥
            { 'G', "ꓖ" }, // The following don't work in some fonts: 𝖦
            { 'H', "НΗꓧ" }, // The following don't work in some fonts: 𝖧
            { 'I', "ІΙӀӏ" },
            { 'Î', "Ȋ" },
            { 'Ï', "ЇΪ" },
            { 'J', "Јꓙ" }, // The following don't work in some fonts: 𝖩 // The following look different in some fonts: Ϳ
            { 'K', "КKΚꓗ" }, // The following don't work in some fonts: 𝖪
            { 'Ḱ', "Ќ" },
            { 'L', "Ꮮꓡ" }, // The following don't work in some fonts: 𝖫𐐛
            { 'M', "МΜϺ" }, // The following don't work in some fonts: 𝖬
            { 'N', "Νꓠ" }, // The following don't work in some fonts: 𝖭
            { 'O', "ОΟՕꓳ" }, // The following don't work in some fonts: 𝖮𐓂𖫩
            { 'Ö', "Ӧ" },
            { 'Ө', "Ѳθ" },
            { 'ϴ', "Ɵ" },
            { 'P', "РΡ" }, // The following don't work in some fonts: 𝖯
            { 'Q', "Ԛ" }, // The following don't work in some fonts: 𝖰
            { 'S', "Ѕჽ" }, // The following don't work in some fonts: 𖫖𝖲
            { 'Ṣ', "ȘŞ" },
            { 'Ş', "ȘṢ" },
            { 'Ș', "ȘṢ" },
            { 'Ţ', "ȚṬ" },
            { 'Ț', "ȚŢ" },
            { 'Ṭ', "ȚŢ" },
            { 'U', "ꓴՍ" }, // The following don't work in some fonts: 𝖴𐓎
            { 'V', "ⴸꛟꓦ" }, // The following don't work in some fonts: 𝖵
            { 'W', "Ԝꓪ" }, // The following don't work in some fonts: 𝖶
            { 'X', "ХΧⵝꓫ" }, // The following don't work in some fonts: 𝖷
            { 'Y', "ΥҮꓬ" }, // The following don't work in some fonts: 𝖸
            { 'Z', "Ζꓜ" }, // The following don't work in some fonts: 𝖹Ⴭꛉ

            { 'a', "а" }, // The following don't work in some fonts: 𝖺
            { 'ă', "ӑǎ" },
            { 'â', "ȃ" },
            { 'c', "сϲᴄ" },
            { 'e', "е" }, // The following don't work in some fonts: 𝖾
            { 'è', "ѐ" },
            { 'ë', "ё" },
            { 'ĕ', "ӗ" },
            { 'i', "і" },
            { 'î', "ȋ" },
            { 'ï', "ї" },
            { 'j', "ј" },
            { 'k', "ĸк" },
            { 'ɫ', "ɬᏐ" },
            { 'o', "оօο" }, // The following don't work in some fonts: 𐓪𐐬 // The following is looks too differnt in some fonts: ௦
            { 'ö', "ӧ" },
            { 'ó', "όό" },
            { 'ò', "ὸ" },
            { 'ө', "ɵꮎ" },
            { 'θ', "ӨѲ𖺀" },
            { 'p', "р" },
            { 'q', "ԛ" },
            { 's', "ѕ" }, // The following don't work in some fonts: 𝗌
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

        private static readonly Dictionary<char, string> ApproximateReplacements = new()
        {
            { '\'', "ʹ" },

            //"0": "߀", // This can turn the text left-to-right
            { '3', "Ӡ" }, // The following look too different: Ʒ
            { 'Ӡ', "3З" },
            { '5', "Ƽ" },
            { '6', "бᏮ" },

            { 'A', "ᗅᎪ" },
            { 'Ă', "Ā" },
            { 'B', "Ᏼᗷꕗ" },
            { 'C', "ᏟᑕⅭ" }, // Ⅽ makes the next character uppercase on iOS
            { 'D', "ᎠⅮ" }, // "Ⅾ" makes the next character uppercase on iOS
            { 'E', "Ꭼⴹ⋿ꗋ" },
            { 'F', "Ϝ" }, // Ϝ does not look identical in some fonts
            { 'G', "ᏀႺ" },
            { 'H', "Ꮋ" },
            { 'I', "ꓲⅠⵏߊꕯ" }, // Stretch // The following look different on some fonts: ꕯ // The following don't work on some fonts: 𝖨𞥑 𞠢
            // The following looks too different: Լ
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
            // { 'U', // The following don't work in some fonts: 𞤋 // The following look too different in some fonts: ᑌ⋃
            { 'V', "ᐯᏙⅤ" },
            { 'W', "ᎳᏔ" },
            // { 'X', "᙭Ꭓχ"
            { 'X', "Ⅹ" },
            { 'Z', "Ꮓ" },

            { 'ă', "ā" },
            // { 'b', // The following are broken in some fonts: ხ // The following look different in some fonts: ᏏЬƅ
            { 'c', "ꮯⅽ" }, // "ꮯ" has a weird top right at least on iOS and "ⅽ" makes the next character after it uppercase
            { 'd', "ძⅾ" }, // The following don't work in some fonts: 𝖽. The following look too different in some fonts: ԁ. "ⅾ" makes the next character uppercase on iOS

            { 'g', "ɡց" },
            //{ 'h', "հ" }, // "հ" looks like some arabic letter on iOS
            { 'h', "ᏂႹ" }, // "һ", // Confirmed to look quite different in many fonts
            { 'i', "Ꭵⅰ" }, // This might be a bit of a stretch
            { 'ı', "ɪ" },
            { 'ɪ', "ı" },
            { 'ĭ', "ǐ" },
            { 'j', "ϳ" },
            { 'k', "κкκꮶ" },
            { 'ł', "ɫɬᏐ" },
            { 'ɫ', "ł" },
            { 'ɬ', "ł" },
            { 'm', "ⅿ" }, // The following don't work in some fonts: ՠ. ՠ also makes the next character uppercase on iOS
            { 'o', "ഠჿ" }, // The following look different in some fonts: ೦. ჿ is broken in Instagram on Android
            { 'p', "ρƿ" },
            { 's', "ടꮪꜱ" }, // ꜱ looks ok but doesn't work on e.g. Apple Watch
            // u // The following don't work in some fonts: 𐓶
            { 'u', "υ" },
            { 'ú', "ύ" },
            { 'v', "ꮩνⅴ" }, // The following have large paddings: ∨
            { 'w', "ꮤ" }, // The following look different in some fonts: ꮃ
            { 'x', "ⅹ" },
            { 'y', "γʏ" }
        };

        public NuciTextObfuscator(string seed) : this(seed.GetHashCode()) { }

        public NuciTextObfuscator() : this(Environment.TickCount) { }

        /// <summary>
        /// Deobfuscates the specified text.
        /// </summary>
        /// <param name="text">The text to deobfuscate.</param>
        /// <returns>The deobfuscated text.</returns>
        public string Deobfuscate(string text)
        {
            if (text is null)
            {
                return null;
            }

            if (text.Equals(string.Empty))
            {
                return string.Empty;
            }

            string input = text;

            foreach (KeyValuePair<string, string> entry in IdenticalGroupReplacements)
            {
                string originalGroup = entry.Key;
                string candidates = entry.Value;

                if (!string.IsNullOrEmpty(candidates))
                {
                    foreach (char candidate in candidates)
                    {
                        input = input.Replace(candidate.ToString(), originalGroup);
                    }
                }
            }

            foreach (KeyValuePair<string, string> entry in ApproximateGroupReplacements)
            {
                string originalGroup = entry.Key;
                string candidates = entry.Value;

                if (!string.IsNullOrEmpty(candidates))
                {
                    foreach (char candidate in candidates)
                    {
                        input = input.Replace(candidate.ToString(), originalGroup);
                    }
                }
            }

            StringBuilder builder = new(input.Length);

            foreach (char character in input)
            {
                bool wasReplaced = false;

                foreach (KeyValuePair<char, string> entry in IdenticalReplacements)
                {
                    if (!string.IsNullOrEmpty(entry.Value) && entry.Value.Contains(character))
                    {
                        builder.Append(entry.Key);
                        wasReplaced = true;
                        break;
                    }
                }

                if (wasReplaced)
                {
                    continue;
                }

                foreach (KeyValuePair<char, string> entry in ApproximateReplacements)
                {
                    if (!string.IsNullOrEmpty(entry.Value) && entry.Value.Contains(character))
                    {
                        builder.Append(entry.Key);
                        wasReplaced = true;
                        break;
                    }
                }

                if (!wasReplaced)
                {
                    builder.Append(character);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Obfuscates the specified text.
        /// </summary>
        /// <param name="text">The text to obfuscate.</param>
        /// <returns>The obfuscated text.</returns>
        public string Obfuscate(string text)
            => Obfuscate(text, new NuciTextObfuscatorOptions());

        /// <summary>
        /// Obfuscates the specified text using the provided options.
        /// </summary>
        /// <param name="text">The text to obfuscate.</param>
        /// <param name="options">The options to use for obfuscation.</param>
        /// <returns>The obfuscated text.</returns>
        public string Obfuscate(string text, NuciTextObfuscatorOptions options)
        {
            if (text is null)
            {
                return null;
            }

            if (text.Equals(string.Empty))
            {
                return string.Empty;
            }

            options ??= new NuciTextObfuscatorOptions();

            string input = text;

            foreach (KeyValuePair<string, string> entry in IdenticalGroupReplacements)
            {
                string group = entry.Key;
                string candidates = entry.Value;

                string replacement = group;

                if (RandomGenerator.Next(1, 11) <= 6 && candidates.Length > 0)
                {
                    int index = RandomGenerator.Next(candidates.Length);
                    replacement = candidates[index].ToString();
                }

                input = input.Replace(group, replacement);
            }

            if (options.UseApproximateReplacements)
            {
                foreach (KeyValuePair<string, string> entry in ApproximateGroupReplacements)
                {
                    string group = entry.Key;
                    string candidates = entry.Value;

                    string replacement = group;

                    if (RandomGenerator.Next(1, 11) <= 6 && candidates.Length > 0)
                    {
                        int index = RandomGenerator.Next(candidates.Length);
                        replacement = candidates[index].ToString();
                    }

                    input = input.Replace(group, replacement);
                }
            }

            StringBuilder builder = new(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                char character = input[i];
                bool wasObfuscated = false;
                StringBuilder candidatesBuilder = new();

                if (IdenticalReplacements.ContainsKey(character))
                {
                    candidatesBuilder.Append(IdenticalReplacements[character]);
                }

                if (options.UseApproximateReplacements)
                {
                    if (ApproximateReplacements.ContainsKey(character))
                    {
                        candidatesBuilder.Append(ApproximateReplacements[character]);
                    }
                }

                string candidates = candidatesBuilder.ToString();

                if (!string.IsNullOrEmpty(candidates))
                {
                    if (RandomGenerator.Next(1, 11) <= 6)
                    {
                        int index = RandomGenerator.Next(candidates.Length);
                        builder.Append(candidates[index]);
                        wasObfuscated = true;
                    }
                }

                if (!wasObfuscated)
                {
                    builder.Append(character);
                }
            }

            return builder.ToString();
        }
    }
}