using Core.Models;

namespace Core.Services {
    public class Matcher {
        private readonly Itunes _itunesDb;
        private readonly Plex _plexDb;

        public Matcher(Itunes itunesDb, Plex plexDb) {
            _itunesDb = itunesDb ?? throw new ArgumentNullException(nameof(itunesDb));
            _plexDb = plexDb ?? throw new ArgumentNullException(nameof(plexDb));
        }

        public IEnumerable<(ItunesTrack, PlexTrack)> MatchItunesToPlexTracks(long plexSectionId) {
            if (plexSectionId <= 0)
                throw new ArgumentOutOfRangeException(nameof(plexSectionId));

            var iTracks = _itunesDb.GetTracks();
            var pTracks = _plexDb.GetTracks(plexSectionId);

            var results = new List<(ItunesTrack, PlexTrack)>();

            foreach (var i in iTracks) {
                var match = pTracks.Where(p => i.EqualsPlexTrack(p)).ToList();

                if (match.Count == 0) {
                    Console.WriteLine($"No match found for {i.ShortFileName} \t {i.ShortAlbumName} \t {i.ShortArtistName} \t {i.FileName}.");
                    // TODO log it
                    continue;
                }

                if (match.Count > 1) {
                    Console.WriteLine($"Multiple matches found for {i.FileName}:"); // TODO log it
                    foreach (var m in match)
                        Console.WriteLine($"\t{m.FileName}"); // TODO log it
                    continue;
                }

                results.Add(new(i, match.First()));
            }

            return results;
        }
    }
}
