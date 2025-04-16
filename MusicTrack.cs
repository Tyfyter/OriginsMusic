using PegasusLib;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OriginsMusic {
	public interface INeedToLoadLate {
		void LoadLate(Mod mod);
	}
	public abstract class AMusicTrack : IModType, IComparable<AMusicTrack> {
		public virtual string Name => GetType().Name;
		public virtual LocalizedText DisplayName => Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.{Name}.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " "));
		public virtual LocalizedText Subtitle => Language.GetOrRegister($"Mods.{Mod.Name}.Tracks.{Name}.Subtitle", () => Mod.DisplayName);
		public LocalizedText ComposerDisplayName => Language.GetOrRegister($"Mods.{Mod.Name}.Composers.{Composer}", Composer.ToString);
		public virtual int SortingIndex => (int)Composer;
		public virtual string TrackLocation => GetType().GetDefaultTMLName();
		public abstract Composer Composer { get; }
		public abstract TrackSlot TrackSlot { get; }
		public abstract int TrackID { get; protected set; }
		public Mod Mod { get; protected set; }
		public string FullName => $"{Mod.Name}/{Name}";
		protected void Register() {
			ModTypeLookup<AMusicTrack>.Register(this);
		}
		public virtual void UpdatePlaying() { }
		public void SetActive() {
			TrackSlot.TrackController = TrackID;
		}
		public bool IsActive => TrackSlot.TrackController == TrackID;
		public int CompareTo(AMusicTrack other) => SortingIndex.CompareTo(other.SortingIndex);
	}
	public abstract class MusicTrack<TTrackSlot> : AMusicTrack, ILoadable, INeedToLoadLate where TTrackSlot : TrackSlot {
		public override int TrackID { get; protected set; }
		public override TrackSlot TrackSlot => ModContent.GetInstance<TTrackSlot>();
		public void Load(Mod mod) {
			Mod = mod;
			OriginsMusic.tracksToLoad.Add((this, mod));
			_ = DisplayName;
			_ = Subtitle;
			_ = ComposerDisplayName;
		}
		public void LoadLate(Mod mod) {
			try {
				TrackSlot trackSlot = TrackSlot;
				if (trackSlot is null) return;
				LoadTrack();
				Register();
				OriginsMusic.tracksBySlot.Add(trackSlot, this);
				if (ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay)) {
					musicDisplay.Call("AddMusic",
						(short)TrackID,
						DisplayName,
						ComposerDisplayName,
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
	public abstract record class TrackSlot : ILoadable, IModType, IComparable<TrackSlot> {
		public virtual string Name => GetType().Name;
		public virtual LocalizedText DisplayName => Language.GetOrRegister($"Mods.OriginsMusic.TrackSlots.{Name}.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " "));
		public virtual LocalizedText Description => Language.GetOrRegister($"Mods.OriginsMusic.TrackSlots.{Name}.Description", () => "");
		public virtual int SortingIndex => 0;
		[NoJIT]
		public abstract ref int TrackController { get; }

		public Mod Mod { get; private set; }
		public string FullName => $"{Mod.Name}/{Name}";
		public void Load(Mod mod) {
			try {
				VerifyTrackControler();
				Mod = mod;
				ModTypeLookup<TrackSlot>.Register(this);
				_ = DisplayName;
				_ = Description;
			} catch (Exception e) {
				if (LogSlotLoadingError(this, e)) throw;
			}
		}
		public void Unload() { }
		public int CompareTo(TrackSlot other) => SortingIndex.CompareTo(other.SortingIndex);
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
	public abstract class TrackSet : ILoadable, IModType, IComparable<TrackSet> {
		public virtual string Name => GetType().Name;
		public virtual LocalizedText DisplayName => Language.GetOrRegister($"Mods.OriginsMusic.TrackSets.{Name}.DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim().Replace("_ ", " "));
		public virtual LocalizedText Description => Language.GetOrRegister($"Mods.OriginsMusic.TrackSets.{Name}.Description", () => "");
		public virtual int SortingIndex => 0;
		public abstract IEnumerable<AMusicTrack> GetMusicTracks();
		public Mod Mod { get; private set; }
		public string FullName => $"{Mod.Name}/{Name}";
		public virtual void Apply() {
			foreach (AMusicTrack track in GetMusicTracks()) track.SetActive();
		}
		public virtual void ApplyTo(Dictionary<TrackSlot, AMusicTrack> config) {
			foreach (AMusicTrack track in GetMusicTracks()) {
				config[track.TrackSlot] = track;
			}
		}
		public void Load(Mod mod) {
			Mod = mod;
			OriginsMusic.trackSets.Add(this);
			_ = DisplayName;
			_ = Description;
		}
		public void Unload() {}
		public int CompareTo(TrackSet other) => SortingIndex.CompareTo(other.SortingIndex);
	}
}
