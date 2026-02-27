using NUnit.Framework;

namespace NuciText.Obfuscation.UnitTests
{
    public class NuciTextObfuscatorTests
    {
        static int TestSeed => 123456789;
        static string TestPlainString => "Test string!";
        static string TestObfuscatedString => "ꓔеst strіng!";
        static NuciTextObfuscatorOptions TestObfuscatorOptions => new()
        {
            UseApproximateReplacements = true
        };

        INuciTextObfuscator obfuscator;

        [SetUp]
        public void Setup()
        {
            obfuscator = new NuciTextObfuscator(TestSeed);
        }

        [Test]
        public void GivenAnEmptyString_WhenDebfuscating_ThenTheResultIsEmpty()
            => Assert.That(obfuscator.Deobfuscate(string.Empty), Is.Empty);

        [Test]
        public void GivenAnEmptyString_WhenObfuscating_ThenTheResultIsEmpty()
            => Assert.That(obfuscator.Obfuscate(string.Empty, TestObfuscatorOptions), Is.Empty);

        [Test]
        public void GivenANullString_WhenDeobfuscating_ThenTheResultIsNull()
            => Assert.That(obfuscator.Deobfuscate(null), Is.Null);

        [Test]
        public void GivenANullString_WhenObfuscating_ThenTheResultIsNull()
            => Assert.That(obfuscator.Obfuscate(null, TestObfuscatorOptions), Is.Null);

        [Test]
        public void GivenAValidString_WhenDeobfuscating_ThenTheResultIsNotNull()
            => Assert.That(obfuscator.Deobfuscate(TestObfuscatedString), Is.Not.Null);

        [Test]
        public void GivenAValidString_WhenObfuscating_ThenTheResultIsNotNull()
            => Assert.That(obfuscator.Obfuscate(TestPlainString, TestObfuscatorOptions), Is.Not.Null);

        [Test]
        public void GivenAValidString_WhenDeobfuscating_ThenTheResultIsNotEmpty()
            => Assert.That(obfuscator.Deobfuscate(TestObfuscatedString), Is.Not.Empty);

        [Test]
        public void GivenAValidString_WhenObfuscating_ThenTheResultIsNotEmpty()
            => Assert.That(obfuscator.Obfuscate(TestPlainString, TestObfuscatorOptions), Is.Not.Empty);

        [Test]
        public void GivenAValidString_WhenDeobfuscating_ThenTheResultHasBeenDeobfuscated()
            => Assert.That(obfuscator.Deobfuscate(TestObfuscatedString), Is.EqualTo(TestPlainString));

        [Test]
        public void GivenAValidString_WhenObfuscating_ThenTheResultHasBeenObfuscated()
            => Assert.That(obfuscator.Obfuscate(TestPlainString, TestObfuscatorOptions), Is.EqualTo(TestObfuscatedString));
    }
}