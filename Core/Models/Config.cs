namespace Core.Models {
    public class Config {
        public string? ItunesLibraryPath { get; set; }
        public List<ItunesPlaylist>? ItunesPlaylists { get; set; }

        public string? PlexApiBaseUrl { get; set; }
        public string? PlexApiToken { get; set; }
        public List<PlexSectionConfig>? PlexLibrarySections { get; set; }

        public bool MustSyncRatings { get; set; } = true;
        public bool MustSyncPlaylists { get; set; } = true;
    }
}
