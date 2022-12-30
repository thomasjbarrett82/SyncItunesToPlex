using Core;
using Core.Data;
using Core.Models;
using Core.Services;

namespace SyncItunesToPlexConsole {
    internal class Program {
        // TODO this would be nice as a GUI, maybe someday
        private static Config _config;

        private static Itunes _itunesDb;
        private static Plex _plexDb;
        private static Matcher _matcher;

        private static string _plexServerUri;
        private static List<(ItunesTrack, PlexTrack)> _matchedTracks;

        async static Task Main() {
            try {
                var plexSection = await VerifyAppConfig();
                Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Verified app config.");

                // if all config values present, get the libraries and match
                _matcher = new(_itunesDb, _plexDb);
                _matchedTracks = _matcher.MatchItunesToPlexTracks(plexSection.Id).ToList();
                Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Tracks matched.");

                var plexPlaylists = _plexDb.GetPlaylists(plexSection.Id); //Plex.GetPlaylists(_plexDb, plexSection.Id);
                Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Retrieved Plex playlists.");

                // if playlists must sync, do it
                if (_config.MustSyncPlaylists) {
                    var playlistsToSync = _config.ItunesPlaylists.Where(i => i.MustSync).ToList();
                    Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Retrieved iTunes playlists to sync.");

                    SyncPlaylists(playlistsToSync, plexPlaylists);
                    Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Finished synchronizing playlists.");
                }
                else
                    Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Playlists are not set to sync, skipping.");

                // if ratings must sync, do it
                if (_config.MustSyncRatings) {
                    Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Starting ratings sync.");

                    SyncTrackRatings();
                    Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Synchronized track ratings.");

                    SyncAlbumRatings();
                    Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Synchronized artist ratings.");

                    SyncArtistRatings();
                    Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Synchronized artist ratings.");
                }
                else
                    Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Ratings are not set to sync, skipping.");

                Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - iTunes to Plex sync finished, press any key to exit");
                Console.Read();
            }
            catch (Exception ex) {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}\n");
            }
        }

        /// <summary>
        /// Verify the app config
        /// </summary>
        /// <returns>Plex music library id</returns>
        private async static Task<PlexSection> VerifyAppConfig() {
            // create new config if it doesn't exist
            var appLocation = Path.GetDirectoryName(Environment.ProcessPath);
            if (string.IsNullOrWhiteSpace(appLocation))
                ExitApp("Unable to get current assembly location");
            var configPath = Path.Combine(appLocation, "config.json");
            if (!File.Exists(configPath)) {
                await AppConfig.SaveConfigASync(new Config(), configPath);
            }

            _config = AppConfig.GetConfig(configPath);

            SetupItunesConfig(configPath);

            return await SetupPlexConfig(configPath);
        }

        /// <summary>
        /// Set up Plex config parameters.
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        private async static Task<PlexSection> SetupPlexConfig(string configPath) {
            if (string.IsNullOrWhiteSpace(_config.PlexApiBaseUrl))
                ExitApp("Plex API base URL not set");
            if (string.IsNullOrWhiteSpace(_config.PlexApiToken))
                ExitApp("Plex API token not set");

            var restApiService = new RestApiService(_config.PlexApiBaseUrl);
            _plexDb = new(restApiService, _config.PlexApiToken);

            _plexServerUri = $"server://{_plexDb.GetServerId()}/com.plexapp.plugins.library";

            if (_config.PlexLibrarySections == null || _config.PlexLibrarySections.Count == 0) {
                _config.PlexLibrarySections = _plexDb.GetSectionsByType(SectionTypes.Music)
                    .Select(c => new PlexSectionConfig { 
                        Key = c.key,
                        Title = c.title,
                        IsSelected = false
                    })
                    .ToList();
                await AppConfig.SaveConfigASync(_config, configPath);
            }

            var psConfig = _config.PlexLibrarySections.FirstOrDefault(p => p.IsSelected == true);
            if (psConfig == null)
                ExitApp("Plex library section to sync is not set");

            return new PlexSection {
                key = psConfig.Key,
                title = psConfig.Title
            };
        }

        /// <summary>
        /// Set up iTunes config parameters.
        /// </summary>
        /// <param name="configPath"></param>
        private async static void SetupItunesConfig(string configPath) {
            if (string.IsNullOrWhiteSpace(_config.ItunesLibraryPath))
                ExitApp("iTunes library path not set");
            if (!File.Exists(_config.ItunesLibraryPath))
                ExitApp("iTunes library file does not exist");

            _itunesDb = new(_config.ItunesLibraryPath!);

            if (_config.ItunesPlaylists == null || _config.ItunesPlaylists.Count == 0) {
                _config.ItunesPlaylists = _itunesDb.GetPlaylists().ToList();
                await AppConfig.SaveConfigASync(_config, configPath);
            }

            // sync config playlists with current iTunes playlists and save back to file
            SyncCurrentItunesPlaylistsWithConfig(configPath);
        }

        private async static void SyncCurrentItunesPlaylistsWithConfig(string configPath) {
            // iTunes can change the ID's, so need to sync config if needed
            var currentItunesPlaylists = _itunesDb.GetPlaylists().ToList();

            var diff = currentItunesPlaylists.Except(_config.ItunesPlaylists, new ItunesPlaylistComparer());
            if (diff.Count() == 0)
                return;

            foreach (var newPC in currentItunesPlaylists) {
                var oldPC = _config.ItunesPlaylists.FirstOrDefault(p => p.Name == newPC.Name);
                if (oldPC == null)
                    continue;

                newPC.MustSync = oldPC.MustSync;
            }

            _config.ItunesPlaylists = currentItunesPlaylists;

            await AppConfig.SaveConfigASync(_config, configPath);
        }

        /// <summary>
        /// Synchronize iTunes playlists with Plex
        /// </summary>
        /// <param name="playlistsToSync"></param>
        /// <param name="plexPlaylists"></param>
        private static void SyncPlaylists(IEnumerable<ItunesPlaylist> playlistsToSync, IEnumerable<PlexPlaylist> plexPlaylists) {
            List<ItunesTrack>? iTracks;
            List<ItunesTrack>? iTracksToAdd;

            PlexPlaylist? plexPlaylist;
            List<PlexTrack>? plexTracks;
            List<PlexTrack>? tracksToRemove;
            List<PlexTrack>? tracksToAdd;

            foreach (var iPlaylist in playlistsToSync) {
                if (string.IsNullOrEmpty(iPlaylist.Name))
                    continue;

                iTracks = _itunesDb.GetPlaylistTracks(iPlaylist.Id).ToList();
                plexPlaylist = plexPlaylists.FirstOrDefault(p => p.title == iPlaylist.Name);
                if (plexPlaylist == null) { // create new playlist and add all tracks
                    plexTracks = _matchedTracks.Where(m =>
                        iTracks.Any(m2 => m2.Id == m.Item1.Id))
                        .Select(m => m.Item2)
                        .ToList();

                    _plexDb.CreatePlaylist(_plexServerUri, iPlaylist.Name, plexTracks);
                }
                else { // sync iTunes playlist with Plex
                    plexTracks = _plexDb.GetPlaylistTracks(plexPlaylist.Id).ToList();

                    // remove any Plex tracks that aren't in iTunes playlist
                    tracksToRemove = plexTracks.Where(t =>
                    iTracks.All(t2 => !t2.EqualsPlexTrack(t)))
                    .ToList();
                    foreach (var tRemove in tracksToRemove) {
                        if (tRemove.playlistItemID == null)
                            continue;
                        _plexDb.DeletePlaylistItem(plexPlaylist.Id, (long) tRemove.playlistItemID);
                    }

                    // sync any iTunes tracks that don't exist in Plex
                    iTracksToAdd = iTracks.Where(t =>
                        plexTracks.All(t2 => !t.EqualsPlexTrack(t2)))
                        .ToList();
                    tracksToAdd = _matchedTracks.Where(m =>
                    iTracksToAdd.Any(i => i.Id == m.Item1.Id))
                    .Select(m => m.Item2)
                    .ToList();
                    _plexDb.AddItemsToPlaylist(_plexServerUri, plexPlaylist.Id, tracksToAdd);
                }

                Console.WriteLine($"{DateTime.Now:yyMMdd HH:mm:ss} - Synchronized {iPlaylist.Name}.");
            }
        }

        private static void SyncTrackRatings() {
            foreach (var track in _matchedTracks) {
                if (track.Item1.Rating == null)
                    continue;

                _plexDb.UpdateItemRating(track.Item2.Id, (int)(track.Item1.Rating/10));
            }
        }

        private static void SyncAlbumRatings() {
            var albumRatings = _matchedTracks
                .GroupBy(g => g.Item2.parentRatingKey)
                .Select(s => new PlexItemRating {
                    Id = s.First().Item2.Id,
                    Rating = (Helpers.RoundTo20(s.Average(a => a.Item1.Rating)) / 10)
                })
                .ToList();

            foreach (var album in albumRatings) {
                _plexDb.UpdateItemRating(album.Id, album.Rating);
            }
        }

        private static void SyncArtistRatings() {
            var artistRatings = _matchedTracks
                .GroupBy(g => g.Item2.grandparentRatingKey)
                .Select(s => new PlexItemRating {
                    Id = s.First().Item2.Id,
                    Rating = (Helpers.RoundTo20(s.Average(a => a.Item1.Rating)) / 10)
                })
                .ToList();

            foreach (var artist in artistRatings) {
                _plexDb.UpdateItemRating(artist.Id, artist.Rating);
            }
        }

        /// <summary>
        /// Exit the application with a user message and prompt
        /// </summary>
        /// <param name="message"></param>
        private static void ExitApp(string message) {
            Console.WriteLine($"{message}, press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
