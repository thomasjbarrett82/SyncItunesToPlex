namespace Core.Services {
    public static class StringExtensions {
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
            return input
                // symbols
                .Replace(" ", "%20")
                .Replace(":", "_")
                .Replace("\"", "_")
                .Replace("/", "_")
                .Replace("՚", "%D5%9A")
                .Replace("’", "_")
                .Replace("ʻ", "%CA%BB")
                .Replace("?", "_")
                .Replace("#", "%23")
                .Replace("[", "%5B")
                .Replace("]", "%5D")
                .Replace("【", "%E3%80%90")
                .Replace("】", "%E3%80%91")
                .Replace("‐", "%E2%80%90")
                .Replace("–", "%E2%80%93")
                .Replace("·", "%C2%B7")
                .Replace("¢", "%C2%A2")
                .Replace("…", "%E2%80%A6")
                .Replace("÷", "%C3%B7")
                .Replace("×", "%C3%97")
                // accent characters
                .Replace("Æ", "%C3%86")
                .Replace("À", "%C3%80")
                .Replace("Å", "%C3%85")
                .Replace("ä", "%C3%A4")
                .Replace("á", "%C3%A1")
                .Replace("ç", "%C3%A7")
                .Replace("É", "%C3%89")
                .Replace("ê", "%C3%AA")
                .Replace("é", "%C3%A9")
                .Replace("ë", "%C3%AB")
                .Replace("Ī", "%C4%AA")
                .Replace("ï", "%C3%AF")
                .Replace("í", "%C3%AD")
                .Replace("ł", "%C5%82")
                .Replace("ñ", "%C3%B1")
                .Replace("Ø", "%C3%98")
                .Replace("ø", "%C3%B8")
                .Replace("ö", "%C3%B6")
                .Replace("ó", "%C3%B3")
                .Replace("ō", "%C5%8D")
                .Replace("ş", "%C5%9F")
                .Replace("Ü", "%C3%9C")
                .Replace("û", "%C3%BB")
                .Replace("ú", "%C3%BA")
                .Replace("ü", "%C3%BC")
                .Replace("ÿ", "%C3%BF")
                .Replace("ý", "%C3%BD")
                // Korean language characters 
                .Replace("강", "%EA%B0%95")
                .Replace("남", "%EB%82%A8")
                .Replace("스", "%EC%8A%A4")
                .Replace("타", "%ED%83%80")
                .Replace("일", "%EC%9D%BC")
                // Japanese language characters
                .Replace("エ", "%E3%82%A8")
                .Replace("ア", "%E3%82%A2")
                .Replace("リ", "%E3%83%AA")
                .Replace("ス", "%E3%82%B9")
                .Replace("の", "%E3%81%AE")
                .Replace("テ", "%E3%83%86")
                .Replace("ー", "%E3%83%BC")
                .Replace("マ", "%E3%83%9E");
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
    }
}
