using Core.Data;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.Configuration;

namespace Test.IntegrationTests {
    [TestClass]
    public class MatcherTest {
        private readonly string _ituneslibrarypath;
        private readonly string _plexApiBaseUrl;
        private readonly string _plexApiToken;
        private readonly int _sectionId;

        private readonly RestApiService _restApiService;

        public MatcherTest() {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<MatcherTest>()
                .Build();
            _ituneslibrarypath = config["itunesLibraryPath"]; // TODO make local version for testing
            _plexApiBaseUrl = config["plexApiBaseUrl"];
            _plexApiToken = config["plexApiToken"];
            _sectionId = int.Parse(config["plexMusicSectionId"]);

            _restApiService = new RestApiService(_plexApiBaseUrl);
        }

        [TestInitialize]
        public void Init() { }

        [TestMethod]
        public void HappyPath_MatchPlexTracks() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(HappyPath_MatchPlexTracks));
            var itunesDb = new Itunes(_ituneslibrarypath);
            var plexDb = new Plex(_restApiService, _plexApiToken);
            var svc = new Matcher(itunesDb, plexDb);
            var results = svc.MatchItunesToPlexTracks(_sectionId);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void GetDistinctTrackRatingsFromItunes() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(GetDistinctTrackRatingsFromItunes));
            var itunesDb = new Itunes(_ituneslibrarypath);
            var plexDb = new Plex(_restApiService, _plexApiToken);
            var svc = new Matcher(itunesDb, plexDb);
            var results = svc.MatchItunesToPlexTracks(_sectionId);

            var ratings = results.Select(r => r.Item1.Rating)
                .Distinct()
                .ToList();
            Assert.IsTrue(ratings.Count > 0);
        }

        [TestMethod]
        public void GetDistinctAlbumRatingsFromItunes() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(GetDistinctAlbumRatingsFromItunes));
            var itunesDb = new Itunes(_ituneslibrarypath);
            var plexDb = new Plex(_restApiService, _plexApiToken);
            var svc = new Matcher(itunesDb, plexDb);
            var results = svc.MatchItunesToPlexTracks(_sectionId);

            var ratings = results
                .GroupBy(g => g.Item2.parentRatingKey)
                .Select(s => new PlexItemRating { 
                    Id = s.First().Item2.Id,
                    Rating = (Helpers.RoundTo20(s.Average(a => a.Item1.Rating)) / 10)
                })
                .ToList();
                
            Assert.IsTrue(ratings.Count > 0);
        }

        [TestMethod]
        public void GetDistinctArtistRatingsFromItunes() {
            Helpers.SetupLog(nameof(ItunesTest), nameof(GetDistinctArtistRatingsFromItunes));
            var itunesDb = new Itunes(_ituneslibrarypath);
            var plexDb = new Plex(_restApiService, _plexApiToken);
            var svc = new Matcher(itunesDb, plexDb);
            var results = svc.MatchItunesToPlexTracks(_sectionId);

            var ratings = results
                .GroupBy(g => g.Item2.grandparentRatingKey)
                .Select(s => new PlexItemRating {
                    Id = s.First().Item2.Id,
                    Rating = (Helpers.RoundTo20(s.Average(a => a.Item1.Rating)) / 10)
                })
                .ToList();

            Assert.IsTrue(ratings.Count > 0);
        }
    }
}