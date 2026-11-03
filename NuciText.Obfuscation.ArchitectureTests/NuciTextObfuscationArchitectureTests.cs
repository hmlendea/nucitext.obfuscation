using System.Xml.Linq;
using ReflectionAssembly = System.Reflection.Assembly;

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

        [Test]
        public void GivenTheProductionAssembly_WhenCheckingItsClasses_ThenEveryPublicClassIsSealed()
        {
            IArchRule rule = Classes().That()
                .ArePublic()
                .And().ResideInAssembly(Architecture.Assemblies.Single())
                .Should().BeSealed();

            Architecture.CheckRule(rule);
        }

        [Test]
        public void GivenTheProductionProject_WhenCheckingItsAssemblyAttributes_ThenItDoesNotDeclareInternalsVisibleTo()
        {
            string repositoryPath = Path.GetDirectoryName(GetProductionProjectPath())!;
            string[] sourceFiles = Directory.GetFiles(repositoryPath, "*.cs", SearchOption.TopDirectoryOnly);
            bool declaresInternalsVisibleTo = sourceFiles
                .Select(File.ReadAllText)
                .Any(source => source.Contains("InternalsVisibleTo"));

            Assert.That(declaresInternalsVisibleTo, Is.False);
        }

        private static string GetProductionProjectPath()
        {
            DirectoryInfo testDirectory = new(TestContext.CurrentContext.TestDirectory);
            string repositoryPath = testDirectory.Parent!.Parent!.Parent!.Parent!.FullName;

            return Path.Combine(repositoryPath, "NuciText.Obfuscation", "NuciText.Obfuscation.csproj");
        }
    }
}
