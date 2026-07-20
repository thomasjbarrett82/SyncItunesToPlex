using Core.Models;
using Core.Services;
using Microsoft.Extensions.Configuration;

namespace Test.IntegrationTests {
    [TestClass]
    public class ConfigTest {
        private string TestConfigPath = "";

        [TestInitialize]
        public void Init() {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<ConfigTest>()
                .Build();
            TestConfigPath = config["testConfigPath"]!;
        }

        [TestMethod]
        public void ConfigNotDefined() {
            Assert.Throws<ArgumentNullException>(() => AppConfig.GetConfig(string.Empty));
        }

        [TestMethod]
        public void ConfigNotFound() {
            var result = AppConfig.GetConfig(@"C:\Temp\fileDoesNotExist.json");
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.ItunesLibraryPath);
        }

        [TestMethod]
        public void HappyPath_GetConfig() {
            var result = AppConfig.GetConfig(TestConfigPath);
            Assert.IsNotNull(result);
            Assert.AreEqual(@"C:\testItunesLibrary.xml", result.ItunesLibraryPath);
        }

        [TestMethod]
        public async Task HappyPath_SaveConfigASync() {
            var testPath = @"C:\Temp\HappyPath_SaveConfigASync.json";
            await AppConfig.SaveConfigASync(new Config(), testPath);
            Assert.IsTrue(File.Exists(testPath));
        }
    }
}