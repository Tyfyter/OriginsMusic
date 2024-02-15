using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Origins.Origins;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OriginsMusic {
    public class OriginsMusic : Mod {
		static FastStaticFieldInfo<MusicLoader, Dictionary<string, int>> musicByPath;
		static FastStaticFieldInfo<MusicLoader, Dictionary<string, string>> musicExtensions;
		static FastStaticFieldInfo<MusicLoader, int> MusicCount_BackingField;
		static Func<string, string, IAudioTrack> MusicLoader_LoadMusic;
		static int MusicCount {
			get => (MusicCount_BackingField ??= new("<MusicCount>k__BackingField", BindingFlags.NonPublic)).GetValue();
			set => (MusicCount_BackingField ??= new("<MusicCount>k__BackingField", BindingFlags.NonPublic)).SetValue(value);
		}
		public OriginsMusic() : base() {
			MusicAutoloadingEnabled = false;
		}
		public override void Load() {
			if (!Main.dedServ) {
				if (Main.audioSystem is not LegacyAudioSystem) {
					return;
				}
				musicByPath ??= new("musicByPath", BindingFlags.NonPublic);
				musicExtensions ??= new("musicExtensions", BindingFlags.NonPublic);
				MusicLoader_LoadMusic ??= typeof(MusicLoader).GetMethod("LoadMusic", BindingFlags.NonPublic | BindingFlags.Static).CreateDelegate<Func<string, string, IAudioTrack>>();
                LoadMusic("Sounds/Music/The_Room_Before", ".ogg", Music.Fiberglass);

                LoadMusic("Sounds/Music/Only_the_Brave", ".ogg", Music.BrinePool);

                LoadMusic("Sounds/Music/Dancing_With_Ghosts", ".ogg", Music.Dusk);

				LoadMusic("Sounds/Music/Shattered_Topography", ".ogg", Music.Defiled);

				LoadMusic("Sounds/Music/Heart_of_the_Beast", ".ogg", Music.UndergroundDefiled);

                LoadMusic("Sounds/Music/On_the_Hunt", ".ogg", Music.DefiledBoss);

                LoadMusic("Sounds/Music/Pereunt_Unum_Scindendum", ".ogg", Music.Riven);

                LoadMusic("Sounds/Music/Festering_Hives", ".ogg", Music.UndergroundRiven);

                LoadMusic("Sounds/Music/Dew_of_Venus", ".ogg", Music.RivenBoss);

                LoadMusic("Sounds/Music/Sizzling_Waves", ".ogg", Music.RivenOcean);
            }
		}
		public override void Unload() {
			musicByPath = null;
			musicExtensions = null;
			MusicCount_BackingField = null;
		}
		internal void LoadMusic(string path, string extension, int id) {
			int currentMusicCount = MusicCount;
			try {
				MusicCount = id;
				musicByPath.GetValue()[Name + "/" + path] = id;
				musicExtensions.GetValue()[Name + "/" + path] = extension;
				if (Main.audioSystem is LegacyAudioSystem audioSystem) {
					audioSystem.AudioTracks[id] = MusicLoader_LoadMusic(Name + "/" + path, extension);
				}
				MusicLoader.AddMusic(this, path);
			} finally {
				MusicCount = currentMusicCount;
			}
		}
	}
	public class FastStaticFieldInfo<TParent, T> : FastStaticFieldInfo<T> {
		public FastStaticFieldInfo(string name, BindingFlags bindingFlags, bool init = false) : base(typeof(TParent), name, bindingFlags, init) { }
	}
	public class FastStaticFieldInfo<T> {
		public readonly FieldInfo field;
		Func<T> getter;
		Action<T> setter;
		public FastStaticFieldInfo(Type type, string name, BindingFlags bindingFlags, bool init = false) {
			field = type.GetField(name, bindingFlags | BindingFlags.Static);
			if (field is null) throw new InvalidOperationException($"No such static field {name} exists");
			if (init) {
				getter = CreateGetter();
				setter = CreateSetter();
			}
		}
		public FastStaticFieldInfo(FieldInfo field, bool init = false) {
			if (!field.IsStatic) throw new InvalidOperationException($"field {field.Name} is not static");
			this.field = field;
			if (init) {
				getter = CreateGetter();
				setter = CreateSetter();
			}
		}
		public T GetValue() {
			return (getter ??= CreateGetter())();
		}
		public void SetValue(T value) {
			(setter ??= CreateSetter())(value);
		}
		private Func<T> CreateGetter() {
			if (field.FieldType != typeof(T)) throw new InvalidOperationException($"type of {field.Name} does not match provided type {typeof(T)}");
			string methodName = field.ReflectedType.FullName + ".get_" + field.Name;
			DynamicMethod getterMethod = new DynamicMethod(methodName, typeof(T), new Type[] { }, true);
			ILGenerator gen = getterMethod.GetILGenerator();

			gen.Emit(OpCodes.Ldsfld, field);
			gen.Emit(OpCodes.Ret);

			return (Func<T>)getterMethod.CreateDelegate(typeof(Func<T>));
		}
		private Action<T> CreateSetter() {
			if (field.FieldType != typeof(T)) throw new InvalidOperationException($"type of {field.Name} does not match provided type {typeof(T)}");
			string methodName = field.ReflectedType.FullName + ".set_" + field.Name;
			DynamicMethod setterMethod = new DynamicMethod(methodName, null, new Type[] { typeof(T) }, true);
			ILGenerator gen = setterMethod.GetILGenerator();

			gen.Emit(OpCodes.Ldarg_0);
			gen.Emit(OpCodes.Stsfld, field);
			gen.Emit(OpCodes.Ret);

			return (Action<T>)setterMethod.CreateDelegate(typeof(Action<T>));
		}
		public static explicit operator T(FastStaticFieldInfo<T> fastFieldInfo) {
			return fastFieldInfo.GetValue();
		}
	}
}