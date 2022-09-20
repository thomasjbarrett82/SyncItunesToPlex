using Core;
using Core.Data;
using Core.Services;
using Microsoft.Extensions.Configuration;

namespace Test.IntegrationTests {
    [TestClass]
    public class PlexTest {
        private string _plexApiBaseUrl;
        private string _plexApiToken;
        private int _sectionId;

        public PlexTest() {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<PlexTest>()
                .Build();
            _plexApiBaseUrl = config["plexApiBaseUrl"];
            _plexApiToken = config["plexApiToken"];
            _sectionId = int.Parse(config["plexMusicSectionId"]);
        }

        [TestInitialize]
        public void Init() { }

        [TestMethod]
        public void PlexServerCapabilities() {
            var svc = new RestApiService(_plexApiBaseUrl);
            var plexDb = new Plex(svc, _plexApiToken);

            var result = plexDb.GetServerId();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HappyPath_GetPlaylists() {
            var svc = new RestApiService(_plexApiBaseUrl);
            var plexDb = new Plex(svc, _plexApiToken);

            var result = plexDb.GetPlaylists(_sectionId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HappyPath_GetTracks() {
            var svc = new RestApiService(_plexApiBaseUrl);
            var plexDb = new Plex(svc, _plexApiToken);

            var result = plexDb.GetTracks(_sectionId);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HappyPath_GetMusicSections() {
            var svc = new RestApiService(_plexApiBaseUrl);
            var plexDb = new Plex(svc, _plexApiToken);

            var result = plexDb.GetSectionsByType(SectionTypes.Music);
            Assert.IsNotNull(result);
        }
    }
}
