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
            { '"', "”″" },
            { '|', "ǀ" },
            { '/', "⁄⧸" },
            { '\\', "⧵⧹" },
            { '-', "‐–" },
            { '+', "𖫵" },
            { '<', "ᐸ𖫬ⵦ" },
            { '>', "ᐳ" },
            { '3', "З" },
            { 'Æ', "Ӕ" },
            { 'æ', "ӕ" },
            { 'A', "АΑꓮ" },
            { 'B', "ВΒꓐ" },
            { 'C', "" }, // intentionally empty (JS had no identical C)
            { 'D', "ᗞꓓ" },
            { 'E', "ЕΕꓰ" },
            { 'F', "ꓝ" },
            { 'G', "ꓖ" },
            { 'H', "НΗꓧ" },
            { 'I', "ІΙӀӏ" },
            { 'J', "Јꓙ" },
            { 'K', "КKΚꓗ" },
            { 'L', "Ꮮꓡ" },
            { 'M', "МΜϺ" },
            { 'N', "Νꓠ" },
            { 'O', "ОΟՕꓳ" },
            { 'P', "РΡ" },
            { 'S', "Ѕჽ" },
            { 'T', "ТΤꓔ" },
            { 'U', "ꓴՍ" },
            { 'V', "ⴸꛟꓦ" },
            { 'W', "Ԝꓪ" },
            { 'X', "ХΧⵝꓫ" },
            { 'Y', "ΥҮꓬ" },
            { 'Z', "Ζꓜ" },

            { 'a', "а" },
            { 'c', "сϲᴄ" },
            { 'e', "е" },
            { 'i', "і" },
            { 'j', "ј" },
            { 'k', "ĸк" },
            { 'o', "оօο" },
            { 'p', "р" },
            { 'q', "ԛ" },
            { 's', "ѕ" },
            { 'w', "ԝꮃ" },
            { 'x', "х" },
            { 'y', "уү" },
            { 'z', "ꮓ" }
        };

        private static readonly Dictionary<char, string> ApproximateReplacements = new()
        {
            { '\'', "ʹ" },
            { '3', "Ӡ" },
            { '5', "Ƽ" },
            { '6', "бᏮ" },

            { 'A', "ᗅᎪ" },
            { 'B', "Ᏼᗷꕗ" },
            { 'C', "ᏟᑕⅭ" },
            { 'D', "ᎠⅮ" },
            { 'E', "Ꭼⴹ⋿ꗋ" },
            { 'G', "ᏀႺ" },
            { 'I', "ꓲⅠⵏߊꕯ" },
            { 'J', "Ꭻ" },
            { 'K', "Ꮶ" },
            { 'L', "Ⅼ" },
            { 'M', "ꓟᎷⅯ" },
            { 'P', "ꓑᏢ" },
            { 'R', "ᎡꓣᏒ" },
            { 'S', "ꓢᏚՏႽ" },
            { 'T', "ㄒᎢꔋ" },
            { 'V', "ᐯᏙⅤ" },
            { 'W', "ᎳᏔ" },
            { 'X', "Ⅹ" },
            { 'Z', "Ꮓ" },

            { 'c', "ꮯⅽ" },
            { 'd', "ძⅾ" },
            { 'g', "ɡց" },
            { 'h', "ᏂႹ" },
            { 'i', "Ꭵⅰ" },
            { 'k', "κꮶ" },
            { 'm', "ⅿ" },
            { 'o', "ჿ" },
            { 'p', "ρƿ" },
            { 's', "ടꮪꜱ" },
            { 'u', "υ" },
            { 'v', "ꮩνⅴ" },
            { 'x', "ⅹ" },
            { 'y', "γʏ" }
        };

        public NuciTextObfuscator(string seed) : this(seed.GetHashCode()) { }

        public NuciTextObfuscator() : this(Environment.TickCount) { }

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

            StringBuilder builder = new StringBuilder(input.Length);

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

        public string Obfuscate(string text)
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

                if (ApproximateReplacements.ContainsKey(character))
                {
                    candidatesBuilder.Append(ApproximateReplacements[character]);
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