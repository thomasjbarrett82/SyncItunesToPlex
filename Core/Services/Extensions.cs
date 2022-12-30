using Core.Models;

namespace Core.Services {
    public static class Extensions {
        public static string GetPlexShortFileName(this string fullFileName) {
            if (string.IsNullOrWhiteSpace(fullFileName))
                return "";

            var splits = fullFileName.Split('/');
            if (splits.Length < 2)
                return "";

            return splits
                .Last()
                .MakePlexMatchItunesNaming();
        }

        public static string GetPlexShortAlbumName(this string fullFileName) {
            return fullFileName
                .GetShortAlbumName()
                .MakePlexMatchItunesNaming();
        }

        public static string GetPlexShortArtistName(this string fullFileName) {
            return fullFileName
                .GetShortArtistName()
                .MakePlexMatchItunesNaming();
        }

        public static string MakePlexMatchItunesNaming(this string input) {
            // make some iTunes specific substitutions
            input = input
                .Replace(":", "_")
                .Replace(@"\", "_")
                .Replace("/", "_")
                .Replace("’", "_")
                .Replace("?", "_");

            // escape data string on the remaining characters
            input = Uri.EscapeDataString(input);

            // swap certain characters back to match iTunes
            return input
                .Replace("%21", "!")
                .Replace("%24", "$")
                .Replace("%26", "&")
                .Replace("%27", "'")
                .Replace("%28", "(")
                .Replace("%29", ")")
                .Replace("%2B", "+")
                .Replace("%2C", ",")
                .Replace("%3D", "=")
                .Replace("%40", "@");
        }

        public static string GetShortAlbumName(this string fullFileName) {
            if (string.IsNullOrWhiteSpace(fullFileName))
                return "";

            var splits = fullFileName.Split('/');
            if (splits.Length < 2)
                return "";

            return splits[^2];
        }

        public static string GetShortArtistName(this string fullFileName) {
            if (string.IsNullOrWhiteSpace(fullFileName))
                return "";

            var splits = fullFileName.Split('/');
            if (splits.Length < 3)
                return "";

            return splits[^3];
        }

        public static string GetShortFileName(this string fullFileName) {
            if (string.IsNullOrWhiteSpace(fullFileName))
                return "";

            var splits = fullFileName.Split('/');
            if (splits.Length < 2)
                return "";

            return splits
                .Last();
        }

        public static void SaveToTempFile(this List<PlexTrack> tracks) {
            var sortedTracks = tracks
                .OrderBy(t => t.ShortFileName)
                .ToList();

            using (var tw = new StreamWriter(@"C:\Temp\PlexTracks.txt")) {
                foreach (var t in sortedTracks) {
                    tw.WriteLine($"{t.ShortFileName}\t{t.ShortAlbumName}\t{t.ShortArtistName}");
                }
            }
        }
    }
}
