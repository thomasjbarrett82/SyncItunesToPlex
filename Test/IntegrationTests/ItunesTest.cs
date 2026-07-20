using Core.Services;
using Microsoft.Extensions.Configuration;

namespace Test.IntegrationTests {
    [TestClass]
    public class ItunesTest {
        const string DebugLogFile = @"C:\Temp\ItunesTest.log";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private string _itunesLibraryPath;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private int _itunesPlaylistId;

        [TestInitialize]
        public void Init() {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<ItunesTest>()
                .Build();
            _itunesLibraryPath = config["itunesLibraryPath"]!; // TODO make local version for testing
            _itunesPlaylistId = int.Parse(config["itunesPlaylistId"]!);
        }

        [TestMethod]
        public void ItunesLibraryNotDefined() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(ItunesLibraryNotDefined));
            var svc = new Itunes(string.Empty);
            Assert.Throws<ArgumentNullException>(() => svc.GetTracks());
        }

        [TestMethod]
        public void ItunesLibraryNotFound() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(ItunesLibraryNotFound));
            var svc = new Itunes(@"C:\fileDoesNotExist.xml");
            Assert.Throws<FileNotFoundException>(() => svc.GetTracks());
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