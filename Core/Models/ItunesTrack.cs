namespace Core.Models {
    public class ItunesTrack {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public int? Rating { get; set; }
        public int? AlbumRating { get; set; }
        public string? PersistentId { get; set; }
        public string? FileName { get; set; }

        public string? ShortFileName { get; set; }
        public string? ShortAlbumName { get; set; }
        public string? ShortArtistName { get; set; }

        public override string ToString() {
            return $"{Id}:{Name} - {Artist} - {Album} ({PersistentId})";
        }

        public bool EqualsPlexTrack(PlexTrack pTrack) {
            return (ShortFileName ?? "").Equals(pTrack.ShortFileName, StringComparison.OrdinalIgnoreCase) &&
            (ShortAlbumName ?? "").Equals(pTrack.ShortAlbumName, StringComparison.OrdinalIgnoreCase) &&
            (ShortArtistName ?? "").Equals(pTrack.ShortArtistName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
