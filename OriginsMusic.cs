using OriginsMusic.Tiles.MusicBoxes;
using Terraria;
using Terraria.ModLoader;
using static Origins.Origins;
using static OriginsMusic.Tiles.MusicBoxes.Music_Box_DC;
using static OriginsMusic.Tiles.MusicBoxes.Music_Box_DW;

namespace OriginsMusic{
	public class OriginsMusic : Mod {
		public override void Load(){
			if (!Main.dedServ) {
				Origins.Origins.Music.Dusk = GetSoundSlot(SoundType.Music, "Sounds/Music/Dancing_With_Ghosts");
				Logger.Info(Origins.Origins.Music.Defiled + "");
				AddMusicBox(Origins.Origins.Music.Defiled = GetSoundSlot(SoundType.Music, "Sounds/Music/Heart_Of_The_Beast"), ModContent.ItemType<Music_Box_DC_Item>(), ModContent.TileType<Music_Box_DC>());
				AddMusicBox(Origins.Origins.Music.Defiled = GetSoundSlot(SoundType.Music, "Sounds/Music/Shattered_Topography"), ModContent.ItemType<Music_Box_DW_Item>(), ModContent.TileType<Music_Box_DW>());
				Logger.Info(Origins.Origins.Music.Defiled + "");
			}
		}
	}
}