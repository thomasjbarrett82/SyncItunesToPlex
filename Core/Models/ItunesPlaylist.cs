using System.Diagnostics.CodeAnalysis;

namespace Core.Models {
    public class ItunesPlaylist {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<ItunesTrack>? Tracks { get; set; }
        public bool MustSync { get; set; } = true;

        public override string ToString() {
            return $"{Id}:{Name} - {Tracks?.Count() ?? 0} tracks";
        }
    }

    public class ItunesPlaylistComparer : IEqualityComparer<ItunesPlaylist> {
        public bool Equals(ItunesPlaylist? x, ItunesPlaylist? y) {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode([DisallowNull] ItunesPlaylist obj) {
            if (obj == null)
                return 0;
            return obj.Id.GetHashCode() ^ 
                (obj.Name != null ? obj.Name.GetHashCode() : 0);
        }
    }
}
