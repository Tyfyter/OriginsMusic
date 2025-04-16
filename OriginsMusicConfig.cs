using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using OriginsMusic.Music;
using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Terraria.ModLoader.UI;
using Origins.Tiles.MusicBoxes;

namespace OriginsMusic {
	public class OriginsMusicConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;
		public static OriginsMusicConfig Instance;
		[CustomModConfigItem(typeof(TrackChoices.ConfigElement))]
		public TrackChoices trackChoices = new();
		public override bool Autoload(ref string name) => false;
		public override void OnChanged() {
			ModContent.GetInstance<DefaultTrackSet>().Apply();
			foreach (AMusicTrack item in trackChoices.Values) {
				item.SetActive();
			}
			Music_Box.ReloadMusicAssociations();
		}
	}
	[JsonConverter(typeof(JsonConverter))]
	public readonly struct TrackChoices() {
		Dictionary<TrackSlot, AMusicTrack> Choices { get; init; } = [];
		readonly Dictionary<string, string> unloadedChoices = [];
		public readonly IEnumerable<AMusicTrack> Values => Choices.Values;
		public bool ChangeTrack(AMusicTrack musicTrack, out TrackChoices newChoices) => ChangeTracks(out newChoices, musicTrack);
		public bool ChangeTracks(out TrackChoices newChoices, params AMusicTrack[] change) {
			newChoices = this with {
				Choices = new(Choices)
			};
			bool changed = false;
			for (int i = 0; i < change.Length; i++) {
				if (!newChoices.Choices.TryGetValue(change[i].TrackSlot, out AMusicTrack track) || track != change[i]) changed = true;
				newChoices.Choices[change[i].TrackSlot] = change[i];
			}
			return changed;
		}
		public bool ApplySet(TrackSet set, out TrackChoices newChoices) {
			newChoices = this with {
				Choices = new(Choices)
			};
			bool changed = false;
			foreach (AMusicTrack item in set.GetMusicTracks()) {
				if (item is null) continue;
				if (!newChoices.Choices.TryGetValue(item.TrackSlot, out AMusicTrack track) || track != item) changed = true;
				newChoices.Choices[item.TrackSlot] = item;
			}
			return changed;
		}
		public class ConfigElement : ConfigElement<TrackChoices> {
			public override void OnBind() {
				base.OnBind();
				TextDisplayFunction = () => "";
				Func<string> baseTooltipFunc = TooltipFunction;
				TooltipFunction = () => tooltip ?? baseTooltipFunc();
				SetupList();
			}
			string tooltip;
			public override void Update(GameTime gameTime) {
				tooltip = null;
				base.Update(gameTime);
			}
			protected void SetupList() {
				RemoveAllChildren();
				List<TrackSet> sets = OriginsMusic.trackSets;
				Height.Pixels -= 22;
				AutoFill(
					this,
					sets,
					set => set.DisplayName,
					set => set.Description,
					set => (_, _) => {
						if (Value.ApplySet(set, out TrackChoices newChoices)) Value = newChoices;
					},
					_ => false
				);
				Append(new UIHorizontalSeparator() {
					Top = new(Height.Pixels, 0),
					Width = new(-8, 1),
					HAlign = 0.5f,
					Color = new(180, 180, 180, 140)
				});
				Height.Pixels += 4;
				for (int i = 0; i < OriginsMusic.slotsToShow.Count; i++) {
					if (!OriginsMusic.tracksBySlot.TryGetValue(OriginsMusic.slotsToShow[i], out List<AMusicTrack> tracks)) continue;
					UIElement element = AutoFill(new UITextPanel<LocalizedText>(OriginsMusic.slotsToShow[i].DisplayName) {
							Top = new(Height.Pixels, 0),
							Width = new(0, 1),
							Height = new(20, 0),
							PaddingTop = 6,
							PaddingBottom = 6,
							TextHAlign = 0.05f
						},
						tracks,
						track => track.DisplayName,
						track => Language.GetOrRegister($"Mods.OriginsMusic.OptionDescription").WithFormatArgs(track.Subtitle, track.ComposerDisplayName),
						track => (_, _) => {
							if (Value.ChangeTrack(track, out TrackChoices newChoices)) Value = newChoices;
						},
						track => Value.Choices.TryGetValue(track.TrackSlot, out AMusicTrack currentTrack) && currentTrack == track
					);
					element.Height.Pixels += 8;
					Height.Pixels += element.Height.Pixels;
					Append(element);
				}
				Recalculate();
			}
			UIElement AutoFill<T>(UIElement element, List<T> items, Func<T, LocalizedText> textFunction, Func<T, LocalizedText> tooltipFunction, Func<T, MouseEvent> clickFunction, Predicate<T> selectedFunction, int maxPerRow = 3) {
				int lastRowCount = items.Count % maxPerRow;
				for (int i = 0; i < items.Count; i++) {
					T item = items[i];
					if (i % maxPerRow == 0) element.Height.Pixels += 32;
					UITextPanel<LocalizedText> newPanel = new(textFunction(item)) {
						Top = new(element.Height.Pixels - 32, 0),
						MaxHeight = new(32, 0),
						PaddingTop = 6,
						PaddingBottom = 6
					};
					newPanel.OnUpdate += element => {
						if (element is UIPanel panel) {
							if (selectedFunction(item)) {
								panel.BackgroundColor = new(222, 200, 34);
							} else {
								panel.BackgroundColor = element.IsMouseHovering ? new Color(91, 120, 227) : UICommon.DefaultUIBlueMouseOver;
							}
						}
						if (element.IsMouseHovering) {
							tooltip = tooltipFunction(item).Value;
						}
					};
					newPanel.OnLeftClick += clickFunction(item);
					if (i < items.Count - lastRowCount) {
						newPanel.Left.Set(4, (i % maxPerRow) / (float)maxPerRow);
						newPanel.Width.Set(-8, 1f / maxPerRow);
					} else {
						newPanel.Left.Set(4, (i % maxPerRow) / (float)lastRowCount);
						newPanel.Width.Set(-8, 1f / lastRowCount);
					}
					element.Append(newPanel);
				}
				return element;
			}
		}
		public class JsonConverter : Newtonsoft.Json.JsonConverter {
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
				TrackChoices result = new();
				reader.Read();
				while (reader.TokenType == JsonToken.PropertyName) {
					string slotName = reader.Value.ToString();
					string trackName = reader.ReadAsString();
					if (ModContent.TryFind(slotName, out TrackSlot slot) && ModContent.TryFind(trackName, out AMusicTrack track)) {
						result.Choices.Add(slot, track);
					} else {
						result.unloadedChoices.Add(slotName, trackName);
					}
					reader.Read();
				}
				return result;
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
				TrackChoices choices = (TrackChoices)value;
				foreach (TrackSlot item in choices.Choices.Keys) choices.unloadedChoices.Remove(item.FullName);
				writer.WriteStartObject();
				foreach (KeyValuePair<TrackSlot, AMusicTrack> item in choices.Choices) {
					writer.WritePropertyName(item.Key.FullName);
					writer.WriteValue(item.Value.FullName);
				}
				foreach (KeyValuePair<string, string> item in choices.unloadedChoices) {
					writer.WritePropertyName(item.Key);
					writer.WriteValue(item.Value);
				}
				writer.WriteEndObject();
			}
			public override bool CanConvert(Type objectType) => objectType == typeof(TrackChoices);
		}
	}
}
