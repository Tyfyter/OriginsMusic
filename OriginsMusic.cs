using OriginsMusic.Tiles.MusicBoxes;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Origins.Origins;

namespace OriginsMusic {
    public class OriginsMusic : Mod {
		public override void Load(){
			if (!Main.dedServ) {
				Music.Dusk = MusicLoader.MusicCount;
				MusicLoader.AddMusic(this, "Sounds/Music/Dancing_With_Ghosts");

				MusicLoader.AddMusic(this, "Sounds/Music/Shattered_Topography");
				MusicLoader.AddMusicBox(this, Music.Defiled = MusicLoader.MusicCount - 1, ModContent.ItemType<Music_Box_DW_Item>(), ModContent.TileType<Music_Box_DW>());

				MusicLoader.AddMusic(this, "Sounds/Music/Heart_Of_The_Beast");
				MusicLoader.AddMusicBox(this, Music.UndergroundDefiled = MusicLoader.MusicCount - 1, ModContent.ItemType<Music_Box_DC_Item>(), ModContent.TileType<Music_Box_DC>());
			}
		}
	}
}