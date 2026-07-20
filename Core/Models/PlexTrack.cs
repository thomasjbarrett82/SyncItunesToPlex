namespace Core.Models {
    public class PlexTrack {
        public long Id => long.Parse(RatingKey);
        public string FileName => Media?.FirstOrDefault()!
                                    .Part?.FirstOrDefault()!
                                    .File ?? string.Empty;

        public required string RatingKey { get; set; }
        public required string Key { get; set; }
        public required string Guid { get; set; }
        public required string Type { get; set; }
        public required string Title { get; set; }
        public decimal UserRating { get; set; }

        public required string ParentRatingKey { get; set; }
        public required string ParentKey { get; set; }
        public required string ParentGuid { get; set; }
        public required string ParentStudio { get; set; }
        public required string ParentTitle { get; set; }

        public required string GrandparentRatingKey { get; set; }
        public required string GrandparentKey { get; set; }
        public required string GrandparentGuid { get; set; }
        public required string GrandparentTitle { get; set; }

        public long? PlaylistItemID { get; set; }

        public required List<PlexTrackMedia> Media { get; set; }

        public required string ShortFileName { get; set; }
        public required string ShortAlbumName { get; set; } = string.Empty;
        public required string ShortArtistName { get; set; } = string.Empty;
    }

    public class PlexTrackMedia {
        public long Id { get; set; }
        public required string Container { get; set; }

        public required List<PlexTrackMediaPart> Part { get; set; }
    }

    public class PlexTrackMediaPart {
        public long Id { get; set; }
        public required string Key { get; set; }
        public required string File { get; set; }
        public required string Container { get; set; }
    }

    public class PlexItemRating { 
        public long Id { get; set; }
        public int Rating { get; set; }
    }
}
