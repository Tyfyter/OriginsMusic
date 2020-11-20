using Terraria.ModLoader;

namespace OriginsMusic{
	public class OriginsMusic : Mod {
		public override void PostAddRecipes(){
            //Origins.____ =  GetLegacySoundSlot(SoundType.____, "Sounds/Custom/____");
            //Origins.____ =  GetSoundSlot(SoundType.Music, "Sounds/Music/____");
            Origins.Origins.Music.Dusk =  GetSoundSlot(SoundType.Music, "Sounds/Music/Dusk");
		}
	}
}