using Core.Models;

namespace Core.Dto
{
    public class PlexLibrarySectionsResponse
    {
        public required LibrarySectionsMediaContainer MediaContainer { get; set; }
    }

    public class LibrarySectionsMediaContainer {
        public int Size { get; set; }
        public bool AllowSync { get; set; }
        public required string Title1 { get; set; }

        public required List<PlexSection> Directory { get; set; }
    }
}

