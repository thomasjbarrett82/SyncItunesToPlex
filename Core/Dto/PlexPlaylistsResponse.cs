using Core.Models;

namespace Core.Dto {
    public class PlexPlaylistsResponse
    {
        public required PlaylistsMediaContainer MediaContainer { get; set; }
    }

    public class PlaylistsMediaContainer {
        public int Size { get; set; }
        public required List<PlexPlaylist> Metadata { get; set; }
    }
}

