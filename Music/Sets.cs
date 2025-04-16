using System.Collections.Generic;
using System.Linq;

namespace OriginsMusic.Music {
	public class DefaultTrackSet : TrackSet {
		public override int SortingIndex => -1;
		public override IEnumerable<AMusicTrack> GetMusicTracks() => OriginsMusic.tracksBySlot.Values.Select(Enumerable.First);
	}
	public class CheeTrackSet : TrackSet {
		public override IEnumerable<AMusicTrack> GetMusicTracks() => OriginsMusic.tracksByID.Values.Where(track => track.Composer == Composer.Chee);
	}
	public class GojiTrackSet : TrackSet {
		public override IEnumerable<AMusicTrack> GetMusicTracks() => OriginsMusic.tracksByID.Values.Where(track => track.Composer == Composer.Goji);
	}
}
