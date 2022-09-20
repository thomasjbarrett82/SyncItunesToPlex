using Core;
using Core.Data;
using Core.Dto;
using Microsoft.Extensions.Configuration;

namespace Test.IntegrationTests
{
    [TestClass]
    public class RestApiServiceTest {
        private readonly string _plexApiBaseUrl;
        private readonly RestApiParam _plexApiToken;
        private readonly string _sectionId;
        private readonly string _requestBin;

        public RestApiServiceTest() {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<RestApiServiceTest>()
                .Build();
            _plexApiBaseUrl = config["plexApiBaseUrl"];
            _plexApiToken = new RestApiParam { Key = Constants.XPlexToken, Value = config["plexApiToken"] };
            _sectionId = config["plexMusicSectionId"];
            _requestBin = config["requestBin"];
        }

        [TestInitialize]
        public void Init() { }

        [TestMethod]
        public void HappyPath_GetPlexServerCapabilities() {
            var svc = new RestApiService(_plexApiBaseUrl);
            var req = new RestApiRequest {
                Uri = string.Empty,
                Params = new List<RestApiParam> {
                    _plexApiToken
                }
            };

            var response = svc.Get<PlexServerResponse>(req);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void HappyPath_GetPlexLibrarySections() {
            var svc = new RestApiService(_plexApiBaseUrl);
            var req = new RestApiRequest {
                Uri = ApiEndpoints.LibrarySections,
                Params = new List<RestApiParam> {
                    _plexApiToken
                }
            };

            var response = svc.Get<PlexLibrarySectionsResponse>(req);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void HappyPath_GetPlexMusicPlaylists() {
            var svc = new RestApiService(_plexApiBaseUrl);
            var req = new RestApiRequest {
                Uri = ApiEndpoints.Playlists,
                Params = new List<RestApiParam> {
                    _plexApiToken,
                    new RestApiParam { Key = ApiKeys.SectionId, Value = _sectionId },
                    new RestApiParam { Key = ApiKeys.Type, Value = MediaTypes.Playlist.ToString() }
                }
            };

            var response = svc.Get<PlexPlaylistsResponse>(req);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void HappyPath_GetPlexTracks() {
            var svc = new RestApiService(_plexApiBaseUrl);
            var req = new RestApiRequest {
                Uri = $"{ApiEndpoints.LibrarySections}/{_sectionId}{ApiEndpoints.All}",
                Params = new List<RestApiParam> {
                    _plexApiToken,
                    new RestApiParam { Key = ApiKeys.Type, Value = MediaTypes.Track.ToString() }
                }
            };

            var response = svc.Get<PlexTracksResponse>(req);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostEncodesSpaces() {
            var svc = new RestApiService(_requestBin);
            var req = new RestApiRequest {
                Uri = ApiEndpoints.Playlists,
                Params = new List<RestApiParam> {
                    new RestApiParam { Key = "title", Value = "Playlist test" }
                }
            };

            var response = svc.Post<PlexPlaylistsResponse>(req);
            Assert.IsNotNull(response);
        }
    }
}
