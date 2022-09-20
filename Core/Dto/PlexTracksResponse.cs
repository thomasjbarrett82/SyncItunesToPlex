using Core.Models;

namespace Core.Dto {
    public class PlexTracksResponse
    {
        public TracksMediaContainer MediaContainer { get; set; }
    }

    public class TracksMediaContainer {
        public int size { get; set; }
        public bool allowSync { get; set; }
        public string identifier { get; set; }
        public int librarySectionID { get; set; }
        public string librarySectionTitle { get; set; }
        public string librarySectionUUID { get; set; }
        public string title1 { get; set; }
        public string title2 { get; set; }
        public string viewGroup { get; set; }

        public List<PlexTrack> Metadata { get; set; }
    }
}

