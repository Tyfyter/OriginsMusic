using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OriginsMusic {
	public class OriginsMusic : Mod {
		public static List<(INeedToLoadLate loader, Mod mod)> tracksToLoad = [];
		public static MultiDictionary<TrackSlot, AMusicTrack> tracksBySlot = [];
		public static Dictionary<int, AMusicTrack> tracksByID = [];
		public static List<TrackSet> trackSets = [];
		public static List<TrackSlot> slotsToShow = [];
		public OriginsMusic() {
			MusicAutoloadingEnabled = false;
		}
		public override void Load() {
			for (int i = 0; i < tracksToLoad.Count; i++) {
				tracksToLoad[i].LoadLate();
			}
			trackSets.Sort();
			slotsToShow = tracksBySlot.Keys.Where(tracksBySlot.ContainsKey).Order().ToList();
			foreach (List<AMusicTrack> item in tracksBySlot.Values) {
				item.Sort();
				for (int i = 0; i < item.Count; i++) {
					if (item[i].TrackID != 0) tracksByID.Add(item[i].TrackID, item[i]);
				}
			}
			AddConfig(nameof(OriginsMusicConfig), new OriginsMusicConfig());
		}
	}
	public class TrackUpdater : ModSystem {
		public override void PostUpdatePlayers() {
			if (OriginsMusic.tracksByID.TryGetValue(Main.LocalPlayer.CurrentSceneEffect.music.value, out AMusicTrack track)) track.UpdatePlaying();
		}
	}
	internal static class LateLoadExtension {
		public static void LoadLate(this (INeedToLoadLate loader, Mod mod) loader) => loader.loader.LoadLate(loader.mod);
	}
	public class MultiDictionary<TKey, TValue>(IEqualityComparer<TKey> comparer) : IDictionary<TKey, List<TValue>> {
		public MultiDictionary() : this(null) { }
		readonly Dictionary<TKey, List<TValue>> backingDictionary = new(comparer);
		public List<TValue> this[TKey key] {
			get => backingDictionary[key];
			set => backingDictionary[key] = value;
		}
		public ICollection<TKey> Keys => backingDictionary.Keys;
		public ICollection<List<TValue>> Values => backingDictionary.Values;
		public int Count => backingDictionary.Count;
		public bool IsReadOnly => false;
		public void Add(TKey key, TValue value) {
			if (!backingDictionary.TryGetValue(key, out List<TValue> list)) backingDictionary[key] = list = [];
			list.Add(value);
		}
		public void Add(TKey key, List<TValue> value) {
			backingDictionary.Add(key, value);
		}
		public void Add(KeyValuePair<TKey, List<TValue>> item) {
			((ICollection<KeyValuePair<TKey, List<TValue>>>)backingDictionary).Add(item);
		}
		public IEnumerable<TValue> GetValues(TKey key) => TryGetValue(key, out List<TValue> values) ? values : Enumerable.Empty<TValue>();
		public void Clear() {
			backingDictionary.Clear();
		}
		public bool Contains(KeyValuePair<TKey, List<TValue>> item) {
			return ((ICollection<KeyValuePair<TKey, List<TValue>>>)backingDictionary).Contains(item);
		}
		public bool ContainsKey(TKey key) {
			return backingDictionary.TryGetValue(key, out List<TValue> values) && values.Count > 0;
		}
		public void CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex) {
			((ICollection<KeyValuePair<TKey, List<TValue>>>)backingDictionary).CopyTo(array, arrayIndex);
		}
		public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator() {
			return backingDictionary.GetEnumerator();
		}
		public bool Remove(TKey key) {
			return backingDictionary.Remove(key);
		}
		public bool Remove(KeyValuePair<TKey, List<TValue>> item) {
			return ((ICollection<KeyValuePair<TKey, List<TValue>>>)backingDictionary).Remove(item);
		}
		public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out List<TValue> value) {
			return backingDictionary.TryGetValue(key, out value) && value.Count > 0;
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable)backingDictionary).GetEnumerator();
		}
	}
}