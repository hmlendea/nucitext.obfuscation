using System;
using NUnit.Framework;

namespace NuciText.Obfuscation.UnitTests
{
    [TestFixture]
    public sealed class NuciTextObfuscatorTests
    {
        private const int TestSeed = 123456789;
        private const string TestPlainString = "Test string!";
        private const string TestObfuscatedString = "Ꭲеst strіng!";

        private static readonly NuciTextObfuscatorOptions IdenticalOptions = new();
        private static readonly NuciTextObfuscatorOptions ApproximateOptions = new()
        {
            UseApproximateReplacements = true
        };

        private INuciTextObfuscator obfuscator = null!;

        [SetUp]
        public void SetUp()
            => obfuscator = new NuciTextObfuscator(TestSeed);

        [Test]
        public void GivenNullOrEmptyText_WhenCallingEitherOperation_ThenTheInputIsPreserved()
        {
            Assert.That(obfuscator.Obfuscate(null), Is.Null);
            Assert.That(obfuscator.Obfuscate(string.Empty), Is.Empty);
            Assert.That(obfuscator.Deobfuscate(null), Is.Null);
            Assert.That(obfuscator.Deobfuscate(string.Empty), Is.Empty);
        }

        [Test]
        public void GivenNullOptions_WhenObfuscating_ThenDefaultOptionsAreUsed()
        {
            string withNullOptions = new NuciTextObfuscator(TestSeed).Obfuscate(TestPlainString, null!)!;
            string withDefaultOptions = new NuciTextObfuscator(TestSeed).Obfuscate(TestPlainString)!;

            Assert.That(withNullOptions, Is.EqualTo(withDefaultOptions));
        }

        [Test]
        public void GivenTheSameSeedInputAndOptions_WhenObfuscatingWithSeparateInstances_ThenTheResultsMatch()
        {
            string first = new NuciTextObfuscator(TestSeed).Obfuscate(TestPlainString, ApproximateOptions)!;
            string second = new NuciTextObfuscator(TestSeed).Obfuscate(TestPlainString, ApproximateOptions)!;

            Assert.That(first, Is.EqualTo(second));
        }

        [Test]
        public void GivenAStringSeed_WhenObfuscating_ThenItsHashSeedProducesTheSameResult()
        {
            string stringSeedResult = new NuciTextObfuscator("my-secret-seed")
                .Obfuscate(TestPlainString)!;
            string integerSeedResult = new NuciTextObfuscator("my-secret-seed".GetHashCode())
                .Obfuscate(TestPlainString)!;

            Assert.That(stringSeedResult, Is.EqualTo(integerSeedResult));
        }

        [Test]
        public void GivenAUsedInstance_WhenObfuscatingAgain_ThenItsRandomStateHasAdvanced()
        {
            string first = obfuscator.Obfuscate(TestPlainString)!;
            string second = obfuscator.Obfuscate(TestPlainString)!;

            Assert.That(second, Is.Not.EqualTo(first));
        }

        [Test]
        public void GivenDifferentSeeds_WhenObfuscating_ThenTheResultsCanDiffer()
        {
            string first = new NuciTextObfuscator(1).Obfuscate(TestPlainString)!;
            string second = new NuciTextObfuscator(2).Obfuscate(TestPlainString)!;

            Assert.That(second, Is.Not.EqualTo(first));
        }

        [Test]
        [TestCase("Bob Ross")]
        [TestCase("John Doe")]
        [TestCase("Ilarion Pintilie")]
        [TestCase("Solaire of Astora")]
        [TestCase("+40 (722) 512 537")]
        [TestCase("Assassin's Creed")]
        [TestCase("The quick brown fox jumps over the lazy dog 0123456789")]
        [TestCase("abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void GivenLosslessText_WhenObfuscatingAndDeobfuscating_ThenTheOriginalTextIsRestored(
            string text)
        {
            string obfuscated = obfuscator.Obfuscate(text, ApproximateOptions)!;

            Assert.That(obfuscator.Deobfuscate(obfuscated), Is.EqualTo(text));
        }

        [Test]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r\n")]
        [TestCase("\u0000")]
        [TestCase("\u001F")]
        [TestCase("\u007F")]
        [TestCase("\u00A0")]
        [TestCase("\u3000")]
        public void GivenUnmappableOrControlText_WhenObfuscating_ThenTheTextIsPreserved(string text)
            => Assert.That(obfuscator.Obfuscate(text, ApproximateOptions), Is.EqualTo(text));

        [Test]
        [TestCase("**", "ᕯ")]
        [TestCase("!!", "‼")]
        [TestCase("!?", "⁉")]
        [TestCase("??", "⁇")]
        [TestCase("?!", "⁈")]
        [TestCase("...", "…")]
        public void GivenAGroupWithASeedThatReplacesIt_WhenObfuscating_ThenTheGroupIsReplaced(
            string text,
            string expected)
            => Assert.That(
                new NuciTextObfuscator(GetGroupReplacementSeed(text)).Obfuscate(text),
                Is.EqualTo(expected));

        [Test]
        [TestCase("II", "Ⅱ")]
        [TestCase("III", "Ⅲ")]
        [TestCase("IV", "Ⅳ")]
        [TestCase("VII", "Ⅶ")]
        [TestCase("XII", "Ⅻ")]
        [TestCase("DZ", "Ǳ")]
        [TestCase("ae", "æ")]
        [TestCase("bl", "Ы")]
        [TestCase("log", "㏒")]
        [TestCase("mil", "㏕")]
        [TestCase("mol", "㏖")]
        [TestCase("ts", "ʦ")]
        public void GivenAnApproximateGroupWithASeedThatReplacesIt_WhenObfuscating_ThenTheGroupIsReplaced(
            string text,
            string expected)
            => Assert.That(
                new NuciTextObfuscator(GetApproximateGroupReplacementSeed(text)).Obfuscate(text, ApproximateOptions),
                Is.EqualTo(expected));

        [Test]
        [TestCase("ᕯ", "**")]
        [TestCase("‼", "!!")]
        [TestCase("⁉", "!?")]
        [TestCase("⁇", "??")]
        [TestCase("⁈", "?!")]
        [TestCase("…", "...")]
        [TestCase("Ⅱ", "II")]
        [TestCase("Ⅲ", "III")]
        [TestCase("Ǳ", "DZ")]
        [TestCase("æӕ", "aeae")]
        [TestCase("Ы", "bl")]
        [TestCase("㏒", "log")]
        [TestCase("ʦ", "ts")]
        public void GivenAGroupReplacement_WhenDeobfuscating_ThenTheOriginalGroupIsRestored(
            string text,
            string expected)
            => Assert.That(obfuscator.Deobfuscate(text), Is.EqualTo(expected));

        [Test]
        [TestCase("а", "a")]
        [TestCase("Α", "A")]
        [TestCase("е", "e")]
        [TestCase("і", "i")]
        [TestCase("ѕ", "s")]
        [TestCase("Ꮒ", "h")]
        [TestCase("υ", "u")]
        [TestCase("Ƽ", "5")]
        [TestCase("Ꮾ", "6")]
        [TestCase("ꮓ", "z")]
        [TestCase(" ", " ")]
        [TestCase("\u0000", "\u0000")]
        public void GivenAReplacementCandidate_WhenDeobfuscating_ThenItsOriginalIsRestored(
            string text,
            string expected)
            => Assert.That(obfuscator.Deobfuscate(text), Is.EqualTo(expected));

        [Test]
        public void GivenApproximateOptions_WhenObfuscating_ThenAdditionalCandidatesCanBeSelected()
        {
            string identical = new NuciTextObfuscator(1).Obfuscate("5", IdenticalOptions)!;
            string approximate = new NuciTextObfuscator(1).Obfuscate("5", ApproximateOptions)!;

            Assert.That(identical, Is.EqualTo("5"));
            Assert.That(approximate, Is.EqualTo("Ƽ"));
        }

        [Test]
        public void GivenAnUnmappableCharacter_WhenObfuscatingAcrossManySeeds_ThenItRemainsUnchanged()
        {
            foreach (int seed in new[] { 1, 2, 17, 123456789 })
            {
                Assert.That(
                    new NuciTextObfuscator(seed).Obfuscate("b", ApproximateOptions),
                    Is.EqualTo("b"));
            }
        }

        [Test]
        public void GivenAnAstralCharacter_WhenObfuscatingAndDeobfuscating_ThenTheSurrogatePairIsPreserved()
        {
            const string text = "😀";
            string obfuscated = obfuscator.Obfuscate(text, ApproximateOptions)!;

            Assert.That(obfuscator.Deobfuscate(obfuscated), Is.EqualTo(text));
        }

        [Test]
        public void GivenAWhitespaceGroup_WhenObfuscating_ThenRepeatedSpacesRemainRepeated()
        {
            string obfuscated = new NuciTextObfuscator(123456789).Obfuscate("  ")!;

            Assert.That(obfuscated, Has.Length.EqualTo(2));
            Assert.That(obfuscator.Deobfuscate(obfuscated), Is.EqualTo("  "));
        }

        [Test]
        [TestCase("ᕯ", "**")]
        [TestCase("‼", "!!")]
        [TestCase("⁉", "!?")]
        [TestCase("⁇", "??")]
        [TestCase("⁈", "?!")]
        [TestCase("…", "...")]
        [TestCase("Ⅱ", "II")]
        [TestCase("Ⅲ", "III")]
        [TestCase("Ⅳ", "IV")]
        [TestCase("Ⅵ", "VI")]
        [TestCase("Ⅶ", "VII")]
        [TestCase("Ⅷ", "VIII")]
        [TestCase("Ⅸ", "IX")]
        [TestCase("Ⅺ", "XI")]
        [TestCase("Ⅻ", "XII")]
        [TestCase("ⅱ", "ii")]
        [TestCase("ⅲ", "iii")]
        [TestCase("ⅳ", "iv")]
        [TestCase("ⅵ", "vi")]
        [TestCase("ⅶ", "vii")]
        [TestCase("ⅷ", "viii")]
        [TestCase("ⅸ", "ix")]
        [TestCase("ⅺ", "xi")]
        [TestCase("ⅻ", "xii")]
        [TestCase("Ǳ", "DZ")]
        [TestCase("ǲ", "Dz")]
        [TestCase("Ĳ", "IJ")]
        [TestCase("Ǉ", "LJ")]
        [TestCase("ǈ", "Lj")]
        [TestCase("Ǌ", "NJ")]
        [TestCase("ǋ", "Nj")]
        [TestCase("æӕ", "aeae")]
        [TestCase("Ы", "bl")]
        [TestCase("ǳʣ", "dzdz")]
        [TestCase("ĳ", "ij")]
        [TestCase("ǉ", "lj")]
        [TestCase("㏐", "lm")]
        [TestCase("㏑", "ln")]
        [TestCase("㏒", "log")]
        [TestCase("ʪ", "ls")]
        [TestCase("㏓", "lx")]
        [TestCase("ʫ", "lz")]
        [TestCase("㏔", "mb")]
        [TestCase("㏕", "mil")]
        [TestCase("㏖", "mol")]
        [TestCase("ǌ", "nj")]
        [TestCase("ѹ", "oy")]
        [TestCase("Ѹ", "Oy")]
        [TestCase("ʦ", "ts")]
        public void GivenAGroupValue_WhenDeobfuscating_ThenTheGroupsOriginalCharactersAreRestored(
            string text,
            string expected)
            => Assert.That(obfuscator.Deobfuscate(text), Is.EqualTo(expected));

        [Test]
        [TestCase("Ⅱ", "II")]
        [TestCase("Ⅲ", "III")]
        [TestCase("Ⅳ", "IV")]
        [TestCase("Ⅵ", "VI")]
        [TestCase("Ⅶ", "VII")]
        [TestCase("Ⅷ", "VIII")]
        [TestCase("Ⅸ", "IX")]
        [TestCase("Ⅺ", "XI")]
        [TestCase("Ⅻ", "XII")]
        [TestCase("ⅱ", "ii")]
        [TestCase("ⅲ", "iii")]
        [TestCase("ⅳ", "iv")]
        [TestCase("ⅵ", "vi")]
        [TestCase("ⅶ", "vii")]
        [TestCase("ⅷ", "viii")]
        [TestCase("ⅸ", "ix")]
        [TestCase("ⅺ", "xi")]
        [TestCase("ⅻ", "xii")]
        [TestCase("ǲ", "Dz")]
        [TestCase("Ĳ", "IJ")]
        [TestCase("Ǉ", "LJ")]
        [TestCase("ǈ", "Lj")]
        [TestCase("Ǌ", "NJ")]
        [TestCase("ǋ", "Nj")]
        [TestCase("ǳʣ", "dzdz")]
        [TestCase("ĳ", "ij")]
        [TestCase("ǉ", "lj")]
        [TestCase("㏐", "lm")]
        [TestCase("㏑", "ln")]
        [TestCase("ʪ", "ls")]
        [TestCase("㏓", "lx")]
        [TestCase("ʫ", "lz")]
        [TestCase("㏔", "mb")]
        [TestCase("㏕", "mil")]
        [TestCase("㏖", "mol")]
        [TestCase("ǌ", "nj")]
        [TestCase("ѹ", "oy")]
        [TestCase("Ѹ", "Oy")]
        public void GivenEveryApproximateGroupCandidate_WhenDeobfuscating_ThenItsGroupIsRestored(
            string text,
            string expected)
            => Assert.That(obfuscator.Deobfuscate(text), Is.EqualTo(expected));

        [Test]
        [TestCase("055D", "`")]
        [TestCase("0375", ",")]
        [TestCase("0589", ":")]
        [TestCase("037E", ";")]
        [TestCase("201D", "\"")]
        [TestCase("2033", "\"")]
        [TestCase("01C0", "|")]
        [TestCase("2044", "/")]
        [TestCase("29F5", "\\")]
        [TestCase("2010", "-")]
        [TestCase("1433", ">")]
        [TestCase("0410", "A")]
        [TestCase("0412", "B")]
        [TestCase("0415", "E")]
        [TestCase("041D", "H")]
        [TestCase("041E", "O")]
        [TestCase("0420", "P")]
        [TestCase("0430", "a")]
        [TestCase("0435", "e")]
        [TestCase("0441", "c")]
        [TestCase("0440", "p")]
        [TestCase("0445", "x")]
        public void GivenUnicodeCandidateCodePoint_WhenDeobfuscating_ThenTheOriginalCharacterIsRestored(
            string codePoint,
            string expected)
            => Assert.That(
                obfuscator.Deobfuscate(char.ConvertFromUtf32(Convert.ToInt32(codePoint, 16))),
                Is.EqualTo(expected));

        [Test]
        [TestCase("Test string!", "Test ѕtrіng!", false)]
        [TestCase("Test string!", "Ꭲеst strіng!", true)]
        [TestCase("Hello world!", "Hellο ꮃօrld!", false)]
        [TestCase("A+B=C", "A+ꓐ=C", false)]
        [TestCase("12345", "12З4Ƽ", true)]
        [TestCase("Bob Ross", "Bob Rοѕѕ", false)]
        [TestCase("a b c", "а b ꮯ", true)]
        [TestCase("mil", "㏕", true)]
        [TestCase("mol", "㏖", true)]
        [TestCase("log", "㏒", true)]
        [TestCase("DZ", "Ǳ", true)]
        [TestCase("IJ", "Ĳ", true)]
        public void GivenASeedAndOptions_WhenObfuscating_ThenTheOutputIsDeterministic(
            string text,
            string expected,
            bool useApproximateReplacements)
            => Assert.That(
                new NuciTextObfuscator(TestSeed).Obfuscate(
                    text,
                    new NuciTextObfuscatorOptions
                    {
                        UseApproximateReplacements = useApproximateReplacements
                    }),
                Is.EqualTo(expected));

        private static int GetGroupReplacementSeed(string group)
            => group switch
            {
                "!?" => 3,
                "??" => 1,
                "?!" => 5,
                "..." => 1,
                _ => 1
            };

        private static int GetApproximateGroupReplacementSeed(string group)
            => group switch
            {
                "III" => 3,
                "IV" => 2,
                "VII" => 12,
                "XII" => 33,
                "DZ" => 1,
                "ae" => 8,
                "mil" => 2,
                "ts" => 2,
                _ => 1
            };
    }
}