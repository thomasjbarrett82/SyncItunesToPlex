namespace Core.Models {
    public class PlexTrack {
        public long Id => long.Parse(ratingKey);
        public string FileName => Media?.FirstOrDefault()
                                    .Part?.FirstOrDefault()
                                    .file ?? string.Empty;

        public string ratingKey { get; set; }
        public string key { get; set; }
        public string guid { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public decimal userRating { get; set; }

        public string parentRatingKey { get; set; }
        public string parentKey { get; set; }
        public string parentGuid { get; set; }
        public string parentStudio { get; set; }
        public string parentTitle { get; set; }

        public string grandparentRatingKey { get; set; }
        public string grandparentKey { get; set; }
        public string grandparentGuid { get; set; }
        public string grandparentTitle { get; set; }

        public long? playlistItemID { get; set; }

        public List<PlexTrackMedia> Media { get; set; }

        public string ShortFileName { get; set; }
        public string ShortAlbumName { get; set; } = string.Empty;
        public string ShortArtistName { get; set; } = string.Empty;
    }

    public class PlexTrackMedia {
        public long id { get; set; }
        public string container { get; set; }

        public List<PlexTrackMediaPart> Part { get; set; }
    }

    public class PlexTrackMediaPart {
        public long id { get; set; }
        public string key { get; set; }
        public string file { get; set; }
        public string container { get; set; }
    }

    public class PlexItemRating { 
        public long Id { get; set; }
        public int Rating { get; set; }
    }
}
