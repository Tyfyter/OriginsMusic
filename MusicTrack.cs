using Origins.Questing;
using PegasusLib;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria.GameContent.Tile_Entities;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OriginsMusic {
	public interface INeedToLoadLate {
		void LoadLate(Mod mod);
	}
	public abstract class AMusicTrack : IModType {
		public virtual string Name => GetType().Name;
		public abstract TrackSlot TrackSlot { get; }
		public abstract int TrackID { get; protected set; }
		public Mod Mod { get; protected set; }
		public string FullName => $"{Mod.Name}/{Name}";
		protected void Register(Mod mod) {
			Mod = mod;
			ModTypeLookup<AMusicTrack>.Register(this);
		}
		public void SetActive() {
			TrackSlot.TrackController = TrackID;
		}
		public bool IsActive => TrackSlot.TrackController == TrackID;
	}
	public abstract class MusicTrack<TTrackSlot> : AMusicTrack, ILoadable, INeedToLoadLate where TTrackSlot : TrackSlot {
		public virtual LocalizedText DisplayName => Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.{Name}.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " "));
		public virtual LocalizedText Subtitle => Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.{Name}.Subtitle", () => Mod.DisplayName);
		public virtual string TrackLocation => GetType().GetDefaultTMLName();
		public abstract string ComposerName { get; }
		public override int TrackID { get; protected set; }
		public override TrackSlot TrackSlot => ModContent.GetInstance<TTrackSlot>();
		public void Load(Mod mod) {
			OriginsMusic.tracksToLoad.Add((this, mod));
		}
		public void LoadLate(Mod mod) {
			try {
				TrackSlot trackSlot = TrackSlot;
				if (trackSlot is null) return;
				LoadTrack();
				Register(mod);
				OriginsMusic.tracksBySlot.Add(trackSlot, this);
				if (ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay)) {
					musicDisplay.Call("AddMusic",
						(short)TrackID,
						DisplayName,
						Language.GetOrRegister($"Mods.{Mod.Name}.Composers.{ComposerName}", () => ComposerName),
						Subtitle
					);
				}
			} catch (Exception e) {
				if (LogTrackLoadingError(this, e)) throw;
			}
		}
		public virtual void LoadTrack() {
			int splitPos = TrackLocation.IndexOf('/');
			MusicLoader.AddMusic(ModLoader.GetMod(TrackLocation[..splitPos]), TrackLocation[(splitPos + 1)..]);
			TrackID = MusicLoader.GetMusicSlot(TrackLocation);
		}
		public void Unload() { }
		public static bool LogTrackLoadingError(MusicTrack<TTrackSlot> track, Exception exception) {
#if DEBUG
			return true;
#else
			LocalizedText message = Language.GetOrRegister("Mods.OriginsMusic.MusicLoadingException").WithFormatArgs(track.DisplayName, exception);
			ModContent.GetInstance<OriginsMusic>().Logger.Warn(message.Value);
			Origins.Origins.loadingWarnings.Add(message);
			return false;
#endif
		}
	}
	public abstract record class TrackSlot : ILoadable, IModType {
		public virtual string Name => GetType().Name;
		public virtual LocalizedText DisplayName => Language.GetOrRegister($"Mods.OriginsMusic.Tracks.{Name}.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " "));
		[NoJIT]
		public abstract ref int TrackController { get; }

		public Mod Mod { get; private set; }
		public string FullName => $"{Mod.Name}/{Name}";
		public void Load(Mod mod) {
			try {
				VerifyTrackControler();
				Mod = mod;
				ModTypeLookup<TrackSlot>.Register(this);
			} catch (Exception e) {
				if (LogSlotLoadingError(this, e)) throw;
			}
		}
		public void Unload() { }
		[NoJIT]
		internal void VerifyTrackControler() {
			_ = TrackController;
		}
		public static bool LogSlotLoadingError(TrackSlot slot, Exception exception) {
#if DEBUG
			return true;
#else
			LocalizedText message = Language.GetOrRegister("Mods.OriginsMusic.SlotLoadingException").WithFormatArgs(slot.DisplayName, exception);
			ModContent.GetInstance<OriginsMusic>().Logger.Warn(message.Value);
			Origins.Origins.loadingWarnings.Add(message);
			return false;
#endif
		}
	}
	public abstract class TrackSet : ILoadable {
		public virtual string Name => GetType().Name;
		public virtual LocalizedText DisplayName => Language.GetOrRegister($"Mods.OriginsMusic.Tracks.{Name}.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " "));
		public abstract IEnumerable<AMusicTrack> GetMusicTracks();
		public virtual void Apply() {
			foreach (AMusicTrack track in GetMusicTracks()) track.SetActive();
		}
		public void Load(Mod mod) {
			OriginsMusic.trackSets.Add(this);
		}
		public void Unload() {}
	}
}
