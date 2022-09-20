namespace Core.Models {
    public class PlexPlaylist {
        public long Id => long.Parse(ratingKey);

        public string ratingKey { get; set; }
        public string key { get; set; }
        public string guid { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string titleSort { get; set; }
        public string summary { get; set; }
        public bool smart { get; set; }
        public string playlistType { get; set; }
        public string composite { get; set; }
        public string icon { get; set; }
    }
}
