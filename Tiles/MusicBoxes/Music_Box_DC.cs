using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace OriginsMusic.Tiles.MusicBoxes
{
	internal class Music_Box_DC : ModTile
	{
		public override void SetDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Music Box");
			AddMapEntry(new Color(255, 255, 255), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
		}


		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
		}
	}
	public class Music_Box_DC_Item : ModItem {
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Music Box (Defiled Caverns)");
		}
		public override void SetDefaults() {
			item.accessory = true;
		}
	}
}
