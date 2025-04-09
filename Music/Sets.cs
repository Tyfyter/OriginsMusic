using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace OriginsMusic.Music {
	public class DefaultTrackSet : TrackSet {
		public override int SortingIndex => -1;
		public override IEnumerable<AMusicTrack> GetMusicTracks() => [

		];
	}
	public class TestTrackSet1 : TrackSet {
		public override IEnumerable<AMusicTrack> GetMusicTracks() => [
			ModContent.GetInstance<Abstract>()
		];
	}
	public class TestTrackSet2 : TrackSet {
		public override IEnumerable<AMusicTrack> GetMusicTracks() => [
			ModContent.GetInstance<Stolen_Memories>()
		];
	}
	public class TestTrackSet3 : TrackSet {
		public override IEnumerable<AMusicTrack> GetMusicTracks() => [

		];
	}
	public class TestTrackSet4 : TrackSet {
		public override IEnumerable<AMusicTrack> GetMusicTracks() => [

		];
	}
}
