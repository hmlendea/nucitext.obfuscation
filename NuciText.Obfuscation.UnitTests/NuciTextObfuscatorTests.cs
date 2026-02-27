using NUnit.Framework;

namespace NuciText.Obfuscation.UnitTests
{
    public class NuciTextObfuscatorTests
    {
        static int StaticTestSeed => 123456789;
        static string StaticTestString => "Test string!";

        INuciTextObfuscator obfuscator;

        [SetUp]
        public void Setup()
        {
            obfuscator = new NuciTextObfuscator(StaticTestSeed);
        }

        [Test]
        public void GivenAnEmptyString_WhenObfuscating_ThenTheResultIsEmpty()
            => Assert.That(obfuscator.Obfuscate(string.Empty), Is.Empty);

        [Test]
        public void GivenANullString_WhenObfuscating_ThenTheResultIsNull()
            => Assert.That(obfuscator.Obfuscate(null), Is.Null);

        [Test]
        public void GivenAValidString_WhenObfuscating_ThenTheResultIsNotNull()
            => Assert.That(obfuscator.Obfuscate(StaticTestString), Is.Not.Null);

        [Test]
        public void GivenAValidString_WhenObfuscating_ThenTheResultIsNotEmpty()
            => Assert.That(obfuscator.Obfuscate(StaticTestString), Is.Not.Empty);

        [Test]
        public void GivenAValidString_WhenObfuscating_ThenTheResultHasBeenObfuscated()
            => Assert.That(obfuscator.Obfuscate(StaticTestString), Is.Not.EqualTo(StaticTestString));
    }
}