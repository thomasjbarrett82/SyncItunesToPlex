namespace Core.Models {
    public class PlexSection {
        public long Id => long.Parse(key);

        public string key { get; set; }
        public string title { get; set; }
        public string? type { get; set; }
    }
}
