using Core.Data;
using Core.Dto;
using Core.Models;

namespace Core.Services
{
    public class Plex {
        private readonly IRestApiService _restApiService;
        private readonly RestApiParam _apiToken;

        public Plex(IRestApiService apiSvc, string apiToken) {
            if (string.IsNullOrWhiteSpace(apiToken))
                throw new ArgumentNullException(nameof(apiToken));

            _restApiService = apiSvc;
            _apiToken = new RestApiParam { Key = Constants.XPlexToken, Value = apiToken };
        }

        #region Get Methods

        public IEnumerable<PlexPlaylist> GetPlaylists(long sectionId) {
            if (sectionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(sectionId));

            var request = new RestApiRequest {
                Uri = ApiEndpoints.Playlists,
                Params = new List<RestApiParam> {
                    _apiToken,
                    new RestApiParam { Key = ApiKeys.SectionId, Value = sectionId.ToString() },
                    new RestApiParam { Key = ApiKeys.Type, Value = MediaTypes.Playlist.ToString() }
                }
            };

            var response = _restApiService.Get<PlexPlaylistsResponse>(request).Data;
            if (response == null)
                throw new NullReferenceException(nameof(PlexPlaylistsResponse));

            return response.MediaContainer.Metadata;
        }

        public IEnumerable<PlexTrack> GetTracks(long sectionId) {
            if (sectionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(sectionId));

            var request = new RestApiRequest {
                Uri = $"{ApiEndpoints.LibrarySections}/{sectionId}{ApiEndpoints.All}",
                Params = new List<RestApiParam> { 
                    _apiToken,
                    new RestApiParam { Key = ApiKeys.Type, Value = MediaTypes.Track.ToString() }
                }
            };

            var response = _restApiService.Get<PlexTracksResponse>(request).Data;
            if (response == null)
                throw new NullReferenceException(nameof(PlexTracksResponse));

            var tracks = response.MediaContainer.Metadata;

            tracks.ForEach(t => t.ShortFileName = (t.FileName ?? "").GetPlexShortFileName());
            tracks.ForEach(t => t.ShortAlbumName = (t.FileName ?? "").GetPlexShortAlbumName());
            tracks.ForEach(t => t.ShortArtistName = (t.FileName ?? "").GetPlexShortArtistName());

#if DEBUG
            Console.WriteLine(@"Writing debug Plex tracks to C:\Temp.");
            tracks.SaveToTempFile();
#endif

            return tracks;
        }

        public IEnumerable<PlexSection> GetSectionsByType(string sectionType) {
            if (string.IsNullOrWhiteSpace(sectionType))
                throw new ArgumentNullException(nameof(sectionType));

            var request = new RestApiRequest { 
                Uri = ApiEndpoints.LibrarySections,
                Params = new List<RestApiParam> { _apiToken}
            };

            var response = _restApiService.Get<PlexLibrarySectionsResponse>(request).Data;
            if (response == null)
                throw new NullReferenceException(nameof(PlexLibrarySectionsResponse));

            return response.MediaContainer.Directory
                .Where(d => d.type == sectionType)
                .ToList();
        }

        public IEnumerable<PlexTrack> GetPlaylistTracks(long playlistId) {
            if (playlistId <= 0)
                throw new ArgumentOutOfRangeException(nameof(playlistId));

            var request = new RestApiRequest {
                Uri = $"{ApiEndpoints.Playlists}/{playlistId}/items",
                Params = new List<RestApiParam> { _apiToken }
            };

            var response = _restApiService.Get<PlexTracksResponse>(request).Data;
            if (response == null)
                throw new NullReferenceException(nameof(PlexTracksResponse));

            var tracks = response.MediaContainer.Metadata;
            if (tracks == null)
                return Enumerable.Empty<PlexTrack>();

            tracks.ForEach(t => t.ShortFileName = (t.FileName ?? "").GetPlexShortFileName());
            tracks.ForEach(t => t.ShortAlbumName = (t.FileName ?? "").GetPlexShortAlbumName());
            tracks.ForEach(t => t.ShortArtistName = (t.FileName ?? "").GetPlexShortArtistName());

            return tracks;
        }

        public string GetServerId() {
            var request = new RestApiRequest {
                Uri = string.Empty,
                Params = new List<RestApiParam> {
                    _apiToken
                }
            };

            var response = _restApiService.Get<PlexServerResponse>(request).Data;
            if (response == null)
                throw new NullReferenceException(nameof(PlexServerResponse));

            return response.MediaContainer.machineIdentifier;
        }

        #endregion Get Methods

        #region Put Methods

        public bool UpdatePlaylistSummary(long id, string summary) {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            if (string.IsNullOrWhiteSpace(summary))
                throw new ArgumentNullException(nameof(summary));

            var request = new RestApiRequest { 
                Uri = $"{ApiEndpoints.Playlists}/{id}",
                Params = new List<RestApiParam> {
                    _apiToken,
                    new RestApiParam { Key = ApiKeys.Summary, Value = summary }
                }
            };

            _restApiService.Put(request);
            return true;
        }

        public bool AddItemsToPlaylist(string serverUri, long playlistId, List<PlexTrack> tracks) {
            if (string.IsNullOrWhiteSpace(serverUri))
                throw new ArgumentNullException(nameof(serverUri));
            if (playlistId <= 0)
                throw new ArgumentOutOfRangeException(nameof(playlistId));

            if (tracks == null || tracks.Count == 0)
                return false;

            var plexKeys = string.Join(",", tracks.Select(t => t.Id));
            var itemsUri = $"{serverUri}/{ApiEndpoints.LibraryMetadata}/{plexKeys}";

            var request = new RestApiRequest {
                Uri = $"{ApiEndpoints.Playlists}/{playlistId}{ApiEndpoints.Items}",
                Params = new List<RestApiParam> {
                    _apiToken,
                    new RestApiParam { Key = ApiKeys.Uri, Value = itemsUri },
                }
            };

            var response = _restApiService.Put<PlexPlaylistsResponse>(request);
            var responseData = response.Data;
            if (responseData == null)
                throw new NullReferenceException(nameof(PlexPlaylistsResponse));

            return true;
        }

        public bool UpdateItemRating(long id, int rating) {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            if (!Enum.IsDefined(typeof(Ratings), rating))
                throw new ArgumentOutOfRangeException(nameof(rating));

            var request = new RestApiRequest {
                Uri = ApiEndpoints.Rate,
                Params = new List<RestApiParam> {
                    _apiToken,
                    new RestApiParam { Key = ApiKeys.Identifier, Value = ApiValues.DefaultIdentifier },
                    new RestApiParam { Key = ApiKeys.Key, Value = id.ToString() },
                    new RestApiParam { Key = ApiKeys.Rating, Value = rating.ToString() }
                }
            };

            _restApiService.Put(request);
            return true;
        }

        #endregion Put Methods

        #region Post Methods

        public bool CreatePlaylist(string serverUri, string title, IEnumerable<PlexTrack> tracks) {
            if (string.IsNullOrWhiteSpace(serverUri))
                throw new ArgumentNullException(nameof(serverUri));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));

            if (tracks == null || tracks.Count() == 0)
                return false;

            var plexKeys = string.Join(",", tracks.Select(t => t.Id));
            var itemsUri = $"{serverUri}/{ApiEndpoints.LibraryMetadata}/{plexKeys}";

            var request = new RestApiRequest {
                Uri = $"{ApiEndpoints.Playlists}",
                Params = new List<RestApiParam> { 
                    _apiToken,
                    new RestApiParam { Key = ApiKeys.Type, Value = ApiValues.Audio },
                    new RestApiParam { Key = ApiKeys.Smart, Value = "0" },
                    new RestApiParam { Key = ApiKeys.Title, Value = title },
                    new RestApiParam { Key = ApiKeys.Uri, Value = itemsUri },
                }
            };

            var response = _restApiService.Post<PlexPlaylistsResponse>(request);
            var responseData = response.Data;
            if (responseData == null)
                throw new NullReferenceException(nameof(PlexPlaylistsResponse));

            // update playlist summary
            var newPlaylist = responseData.MediaContainer.Metadata.FirstOrDefault();
            if (newPlaylist == null)
                throw new NullReferenceException(nameof(newPlaylist));

            // Plex won't take the summary on creation, so adding it in a second operation
            UpdatePlaylistSummary(newPlaylist.Id, Constants.PlaylistSummary);

            return true;
        }

        #endregion Post Methods

        #region Delete Methods

        public bool DeletePlaylistItem(long playlistId, long playlistItemId) {
            if (playlistId <= 0)
                throw new ArgumentOutOfRangeException(nameof(playlistId));
            if (playlistItemId <= 0)
                throw new ArgumentOutOfRangeException(nameof(playlistItemId));

            var request = new RestApiRequest {
                Uri = $"{ApiEndpoints.Playlists}/{playlistId}{ApiEndpoints.Items}/{playlistItemId}",
                Params = new List<RestApiParam> { _apiToken }
            };

            _restApiService.Delete(request);

            return true;
        }

        #endregion Delete Methods
    }
}
