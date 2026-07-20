using Core.Models;

namespace Core.Dto {
    public class PlexTracksResponse
    {
        public required TracksMediaContainer MediaContainer { get; set; }
    }

    public class TracksMediaContainer {
        public int Size { get; set; }
        public bool AllowSync { get; set; }
        public required string Identifier { get; set; }
        public int LibrarySectionID { get; set; }
        public required string LibrarySectionTitle { get; set; }
        public required string LibrarySectionUUID { get; set; }
        public required string Title1 { get; set; }
        public required string Title2 { get; set; }
        public required string ViewGroup { get; set; }

        public required List<PlexTrack> Metadata { get; set; }
    }
}

