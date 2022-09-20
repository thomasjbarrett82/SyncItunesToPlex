using Core.Models;

namespace Core.Dto
{
    public class PlexLibrarySectionsResponse
    {
        public LibrarySectionsMediaContainer MediaContainer { get; set; }
    }

    public class LibrarySectionsMediaContainer {
        public int size { get; set; }
        public bool allowSync { get; set; }
        public string title1 { get; set; }

        public List<PlexSection> Directory { get; set; }
    }
}

