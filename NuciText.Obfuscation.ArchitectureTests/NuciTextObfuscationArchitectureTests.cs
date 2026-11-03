using System.IO;
using System.Linq;
using System.Xml.Linq;
using ReflectionAssembly = System.Reflection.Assembly;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.NUnit;
using NUnit.Framework;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NuciText.Obfuscation.ArchitectureTests
{
    [TestFixture]
    public sealed class NuciTextObfuscationArchitectureTests
    {
        private static Architecture Architecture => new ArchLoader()
            .LoadAssemblies(ReflectionAssembly.Load("NuciText.Obfuscation"))
            .Build();

        [Test]
        public void GivenTheProductionAssembly_WhenCheckingItsDependencies_ThenItsPublicTypesUseTheExpectedNamespace()
        {
            IArchRule rule = Types().That()
                .ArePublic()
                .And().ResideInAssembly(Architecture.Assemblies.Single())
                .Should().ResideInNamespace("NuciText.Obfuscation");

            Architecture.CheckRule(rule);
        }

        [Test]
        public void GivenTheProductionAssembly_WhenCheckingItsImplementation_ThenTheObfuscatorImplementsThePublicContract()
        {
            Interface contract = Interfaces().That()
                .HaveFullName("NuciText.Obfuscation.INuciTextObfuscator")
                .GetObjects(Architecture)
                .Single();
            IArchRule rule = Classes().That()
                .HaveName("NuciTextObfuscator")
                .Should().ImplementInterface(contract);

            Architecture.CheckRule(rule);
        }

        [Test]
        public void GivenTheProductionAssembly_WhenCheckingItsConfiguration_ThenTheOptionsTypeUsesTheProductionNamespace()
        {
            IArchRule rule = Classes().That()
                .HaveName("NuciTextObfuscatorOptions")
                .Should().ResideInNamespace("NuciText.Obfuscation");

            Architecture.CheckRule(rule);
        }

        [Test]
        public void GivenTheProductionProject_WhenCheckingItsProjectReferences_ThenItHasNoProjectDependencies()
        {
            string projectPath = GetProductionProjectPath();
            XDocument project = XDocument.Load(projectPath);
            int projectReferenceCount = project
                .Descendants("ProjectReference")
                .Count();

            Assert.That(projectReferenceCount, Is.Zero);
        }

        [Test]
        public void GivenTheProductionAssembly_WhenCheckingItsLoadedArchitecture_ThenNoTestAssemblyIsLoaded()
        {
            string[] testAssemblyNames = Architecture.Assemblies
                .Select(assembly => assembly.Name)
                .Where(name => name.Contains("Tests"))
                .ToArray();

            Assert.That(testAssemblyNames, Is.Empty);
        }

        private static string GetProductionProjectPath()
        {
            DirectoryInfo testDirectory = new(TestContext.CurrentContext.TestDirectory);
            string repositoryPath = testDirectory.Parent!.Parent!.Parent!.Parent!.FullName;

            return Path.Combine(repositoryPath, "NuciText.Obfuscation", "NuciText.Obfuscation.csproj");
        }
    }
}
