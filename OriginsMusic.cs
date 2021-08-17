using OriginsMusic.Tiles.MusicBoxes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Origins.Origins;
using static OriginsMusic.Tiles.MusicBoxes.Music_Box_DC;
using static OriginsMusic.Tiles.MusicBoxes.Music_Box_DW;

namespace OriginsMusic{
	public class OriginsMusic : Mod {
		public override void Load(){
			if (Main.netMode!=NetmodeID.Server) {
                //Origins.Origins is not needed because of the static using statement that imports Origins.Origins
				Music.Dusk = GetSoundSlot(SoundType.Music, "Sounds/Music/Dancing_With_Ghosts");
				AddMusicBox(Music.Defiled = GetSoundSlot(SoundType.Music, "Sounds/Music/Shattered_Topography"), ModContent.ItemType<Music_Box_DW_Item>(), ModContent.TileType<Music_Box_DW>());
				AddMusicBox(Music.UndergroundDefiled = GetSoundSlot(SoundType.Music, "Sounds/Music/Heart_Of_The_Beast"), ModContent.ItemType<Music_Box_DC_Item>(), ModContent.TileType<Music_Box_DC>());
			}
		}
	}
}