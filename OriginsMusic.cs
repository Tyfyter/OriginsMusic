using OriginsMusic.Tiles.MusicBoxes;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Origins.Origins;

namespace OriginsMusic {
    public class OriginsMusic : Mod {
		public override void Load(){
			if (!Main.dedServ) {
				Music.Dusk = new SoundStyle("Sounds/Music/Dancing_With_Ghosts", SoundType.Music);
				MusicLoader.AddMusicBox(Music.Defiled = new SoundStyle("Sounds/Music/Shattered_Topography", ModContent.ItemType<Music_Box_DW_Item>(), ModContent.TileType<Music_Box_DW>(), SoundType.Music));
				MusicLoader.AddMusicBox(Music.UndergroundDefiled = new SoundStyle("Sounds/Music/Heart_Of_The_Beast", ModContent.ItemType<Music_Box_DC_Item>(), ModContent.TileType<Music_Box_DC>(), SoundType.Music));
			}
		}
	}
}