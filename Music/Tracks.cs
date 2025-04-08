using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OriginsMusic.Music {
	public record class DefiledTrackSlot : TrackSlot {
		public override ref int TrackController => ref Origins.Origins.Music.Vol1.Defiled;
	}
	public class Abstract : MusicTrack<DefiledTrackSlot> {
		public override string ComposerName { get; } = "I forgor";
	}
	public class Stolen_Memories : MusicTrack<DefiledTrackSlot> {
		public override string ComposerName { get; } = "I forgor";
	}
}
