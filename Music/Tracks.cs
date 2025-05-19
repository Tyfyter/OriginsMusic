using PegasusLib;
using System;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static OriginsMusic.Composer;
using Slot = Origins.Origins.Music;

namespace OriginsMusic.Music {
	#region Defiled Wastelands
	public record class DefiledTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.Defiled;
		public override int SortingIndex => 0;
	}
	public class Stolen_Memories : MusicTrack<DefiledTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class Wasteland : MusicTrack<DefiledTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	public record class UndergroundDefiledTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.UndergroundDefiled;
		public override int SortingIndex => 1;
	}
	public class Heart_Of_The_Beast : MusicTrack<UndergroundDefiledTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class Dread_Heart : MusicTrack<UndergroundDefiledTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	public record class DefiledBossTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.DefiledBoss;
		public override int SortingIndex => 2;
	}
	public class ADJUDICATE : MusicTrack<DefiledBossTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class ARBITRATE : MusicTrack<DefiledBossTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	public record class AncientDefiledTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.AncientDefiled;
		public override int SortingIndex => 3;
	}
	public class Shattered_Topography_Old : MusicTrack<AncientDefiledTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	#endregion
	#region Riven Hive
	public record class RivenTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.Riven;
		public override int SortingIndex => 4;
	}
	public class Pereunt_Unum_Scindendum : MusicTrack<RivenTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class Skin : MusicTrack<RivenTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	public record class UndergroundRivenTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.UndergroundRiven;
		public override int SortingIndex => 5;
	}
	public class Festering_Hives : MusicTrack<UndergroundRivenTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class Internal_Organism : MusicTrack<UndergroundRivenTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	public record class RivenBossTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.RivenBoss;
		public override int SortingIndex => 6;
	}
	public class Ad_Laceratur : MusicTrack<RivenBossTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class Frayed_And_Stretched : MusicTrack<RivenBossTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	public record class RivenOceanTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.RivenOcean;
		public override int SortingIndex => 7;
	}
	public class This_Ocean_Of_Alkahest : MusicTrack<RivenOceanTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class Alkahest : MusicTrack<RivenOceanTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	public record class AncientRivenTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.AncientRiven;
		public override int SortingIndex => 8;
	}
	#endregion
	#region Fiberglass
	public record class FiberglassTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.Fiberglass;
		public override int SortingIndex => 9;
	}
	public class The_Room_Before : MusicTrack<FiberglassTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class Skulking : MusicTrack<FiberglassTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	#endregion
	#region Brine Pool
	public record class BrinePoolTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.BrinePool;
		public override int SortingIndex => 10;
	}
	public class Below_The_Brine : MusicTrack<BrinePoolTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	public class Deep_Luminance : MusicTrack<BrinePoolTrackSlot> {
		public override Composer Composer { get; } = Goji;
	}
	public record class BrineBossTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.LostDiver;
		public override int SortingIndex => 10;
	}
	public class Chee_Brine_Boss : MusicTrack<BrineBossTrackSlot> {
		public override Composer Composer { get; } = Chee;
		public override bool AutoRegisterMusicDisplay => false;
		int mildewCarrionTrack;
		int crownJewelTrack;
		public override void LoadTrack() {
			Mod pml = ModLoader.GetMod("ProceduralMusicLib");
			TrackID = (int)pml.Call("AddMusic", Mod, "Music/Mildewy_Situation");

			MusicLoader.AddMusic(Mod, "Music/Unknown/Carrion_Awakened");
			mildewCarrionTrack = MusicLoader.GetMusicSlot($"{Mod.Name}/Music/Unknown/Carrion_Awakened");

			crownJewelTrack = (int)pml.Call("AddMusic", Mod, "Music/Carrion_Awakened_Finish");

			if (ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay)) {
				musicDisplay.Call("AddMusic",
					(short)TrackID,
					Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.Mildewy_Situation.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " ")).Value,
					Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.Mildewy_Situation.Subtitle", () => Mod.DisplayName),
					Subtitle
				);
				musicDisplay.Call("AddMusic",
					(short)mildewCarrionTrack,
					Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.Carrion_Awakened.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " ")).Value,
					Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.Carrion_Awakened.Subtitle", () => Mod.DisplayName),
					Subtitle
				);
				musicDisplay.Call("AddMusic",
					(short)crownJewelTrack,
					Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.Carrion_Awakened_Finish.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " ")).Value,
					Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.Carrion_Awakened_Finish.Subtitle", () => Mod.DisplayName),
					Subtitle
				);
			}
		}
		public override void UpdatePlaying() {
			Main.musicNoCrossFade[TrackID] = true;
			Main.musicNoCrossFade[crownJewelTrack] = true;
		}
		public override void SetActive() {
			base.SetActive();
			Slot.MildewCarrion = mildewCarrionTrack;
			Slot.CrownJewel = crownJewelTrack;
		}
	}
	public record class AncientBrinePoolTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.AncientBrinePool;
		public override int SortingIndex => 11;
	}
	public class Only_the_Brave : MusicTrack<AncientBrinePoolTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	#endregion
	public record class ShimmerBossTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.ShimmerConstruct;
		public override int SortingIndex => 12;
	}
	public class Shimmer_Construct : MusicTrack<ShimmerBossTrackSlot> {
		public override Composer Composer { get; } = Chee;
		public override bool AutoRegisterMusicDisplay => false;
		int lowHealthTrack;
		public override void LoadTrack() {
			//MusicLoader.AddMusic(Mod, "Music/Unknown/Carrion_Awakened");
			TrackID = MusicID.OtherworldlyBoss1;

			//MusicLoader.AddMusic(Mod, "Music/Unknown/Carrion_Awakened");
			lowHealthTrack = MusicID.OtherworldlyBoss2;

			if (ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay)) {

			}
		}
		public override void UpdatePlaying() {
			float life = 0;
			int npcIndex = NPC.FindFirstNPC(ModContent.NPCType<Origins.NPCs.MiscB.Shimmer_Construct.Shimmer_Construct>());
			if (npcIndex != -1) life = Main.npc[npcIndex].GetLifePercent();
			Main.musicFade[TrackID] = MathF.Pow(life, 0.5f);
			Main.musicFade[lowHealthTrack] = MathF.Pow(1 - life, 0.5f);
			if (NPC.MoonLordCountdown > 0) return;
			Main.audioSystem.UpdateCommonTrack(Main.instance.IsActive, lowHealthTrack, Main.musicFade[lowHealthTrack] * Main.musicVolume, ref Main.musicFade[lowHealthTrack]);
		}
	}
	#region Dusk
	public record class DuskTrackSlot : TrackSlot {
		protected override ref int TrackController => ref Slot.Dusk;
		public override int SortingIndex => 13;
	}
	public class Dancing_With_Ghosts : MusicTrack<DuskTrackSlot> {
		public override Composer Composer { get; } = Chee;
	}
	#endregion
}
