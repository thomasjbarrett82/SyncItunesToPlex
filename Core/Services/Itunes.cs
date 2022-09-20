using Core.Models;
using ITunesLibraryParser;

namespace Core.Services {
    public class Itunes {
        private ITunesLibrary Library { get; set; }

        public Itunes(string path) {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(path);
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            Library = new ITunesLibrary(path);
        }

        public IEnumerable<ItunesPlaylist> GetPlaylists() {
            var playlists = Library.Playlists;
            var results = playlists.Select(p => new ItunesPlaylist {
                Id = p.PlaylistId,
                Name = p.Name
            });

            return results;
        }

        public IEnumerable<ItunesTrack> GetTracks() {
            var results = Library.Tracks
                .Where(t => t.Kind.Contains("audio", StringComparison.OrdinalIgnoreCase))
                .Select(t => new ItunesTrack {
                Id = t.TrackId,
                Name = t.Name,
                Artist = t.Artist,
                Album = t.Album,
                Rating = t.Rating,
                AlbumRating = t.AlbumRating,
                PersistentId = t.PersistentId,
                FileName = t.Location,
                ShortFileName = (t.Location ?? "").GetShortFileName(),
                ShortAlbumName = (t.Location ?? "").GetShortAlbumName(),
                ShortArtistName = (t.Location ?? "").GetShortArtistName()
            });

            return results;
        }

        public IEnumerable<ItunesTrack> GetPlaylistTracks(int id) {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var playlist = Library.Playlists.FirstOrDefault(p => p.PlaylistId == id);
            if (playlist == null)
                throw new KeyNotFoundException($"Playlist {id} not found.");

            var results = playlist.Tracks.Select(t => new ItunesTrack {
                Id = t.TrackId,
                Name = t.Name,
                Artist = t.Artist,
                Album = t.Album,
                Rating = t.Rating,
                AlbumRating = t.AlbumRating,
                PersistentId = t.PersistentId,
                FileName = t.Location,
                ShortFileName = (t.Location ?? "").GetShortFileName(),
                ShortAlbumName = (t.Location ?? "").GetShortAlbumName(),
                ShortArtistName = (t.Location ?? "").GetShortArtistName()
            });

            return results;
        }
    }
}
