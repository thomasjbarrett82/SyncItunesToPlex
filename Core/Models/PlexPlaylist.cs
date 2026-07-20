namespace Core.Models {
    public class PlexPlaylist {
        public long Id => long.Parse(RatingKey);

        public required string RatingKey { get; set; }
        public required string Key { get; set; }
        public required string Guid { get; set; }
        public required string Type { get; set; }
        public required string Title { get; set; }
        public required string TitleSort { get; set; }
        public required string Summary { get; set; }
        public bool Smart { get; set; }
        public required string PlaylistType { get; set; }
        public required string Composite { get; set; }
        public required string Icon { get; set; }
    }
}
