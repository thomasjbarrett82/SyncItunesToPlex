using Core.Models;
using Core.Services;
using Microsoft.Extensions.Configuration;

namespace Test.IntegrationTests {
    [TestClass]
    public class ItunesTest {
        const string DebugLogFile = @"C:\Temp\ItunesTest.log";

        private string _itunesLibraryPath;
        private int _itunesPlaylistId;

        [TestInitialize]
        public void Init() {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<ItunesTest>()
                .Build();
            _itunesLibraryPath = config["itunesLibraryPath"]; // TODO make local version for testing
            _itunesPlaylistId = int.Parse(config["itunesPlaylistId"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ItunesLibraryNotDefined() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(ItunesLibraryNotDefined));
            var svc = new Itunes(string.Empty);
            svc.GetTracks();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ItunesLibraryNotFound() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(ItunesLibraryNotFound));
            var svc = new Itunes(@"C:\fileDoesNotExist.xml");
            svc.GetTracks();
        }

        [TestMethod]
        public void HappyPath_GetPlaylists() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(HappyPath_GetPlaylists));
            var svc = new Itunes(_itunesLibraryPath);
            var result = svc.GetPlaylists();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HappyPath_GetTracks() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(HappyPath_GetTracks));
            var svc = new Itunes(_itunesLibraryPath);
            var result = svc.GetTracks();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HappyPath_GetPlaylistTracks() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(HappyPath_GetTracks));
            var svc = new Itunes(_itunesLibraryPath);
            var result = svc.GetPlaylistTracks(_itunesPlaylistId);
            Assert.IsNotNull(result);
        }
    }
}