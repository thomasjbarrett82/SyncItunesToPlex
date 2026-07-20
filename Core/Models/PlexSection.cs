namespace Core.Models {
    public class PlexSection {
        public long Id => long.Parse(Key);

        public required string Key { get; set; }
        public required string Title { get; set; }
        public string? Type { get; set; }
    }
}
