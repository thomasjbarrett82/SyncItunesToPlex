using Core.Models;

namespace Core.Dto {
    public class PlexPlaylistsResponse
    {
        public PlaylistsMediaContainer MediaContainer { get; set; }
    }

    public class PlaylistsMediaContainer {
        public int size { get; set; }
        public List<PlexPlaylist> Metadata { get; set; }
    }
}

