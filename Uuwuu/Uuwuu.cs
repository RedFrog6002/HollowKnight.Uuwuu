using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;
using Language;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using Uuwuu;

namespace Uuwuu
{
	// Token: 0x02000010 RID: 16
	[UsedImplicitly]
	public class Uuwuu : Mod<Settings, SaveSettings>, IMod, Modding.ILogger
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000022A1 File Offset: 0x000004A1
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000022A8 File Offset: 0x000004A8
		[PublicAPI]
		public static Uuwuu Instance { get; private set; }

		// Token: 0x06000045 RID: 69 RVA: 0x000022B0 File Offset: 0x000004B0
		public override string GetVersion()
		{
			return Assembly.GetAssembly(typeof(Uuwuu)).GetName().Version.ToString();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000022D0 File Offset: 0x000004D0
		public Uuwuu() : base("Uuwuu")
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000231F File Offset: 0x0000051F
		private void SceneChanged(Scene arg0, Scene arg1)
		{
			this._lastScene = arg0.name;
		}
		private int OnGetPlayerIntHook(string target)
		{
			if (target == "charmCost_" + charmnum1.ToString())
			{
				return 6;
			}
			return PlayerData.instance.GetIntInternal(target);
		}

		private bool OnGetPlayerBoolHook(string target)
		{
			if (target == "newCharm_" + charmnum1.ToString())
			{
				return false;
			}
			if (target == "gotCharm_" + charmnum1.ToString())
			{
				return true;
			}
			if (target == "equippedCharm_" + charmnum1.ToString())
			{
				return Settings.EquippedCharm;
			}
			return PlayerData.instance.GetBoolInternal(target);
		}
		private void OnSetPlayerBoolHook(string target, bool boo)
		{
			if (target == "equippedCharm_" + charmnum1.ToString())
			{
				Settings.EquippedCharm = boo;
			}
			PlayerData.instance.SetBoolInternal(target, boo);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000032A4 File Offset: 0x000014A4
		private string OnLangGet(string key, string sheettitle)
		{
			if (key != null)
			{
				if (key == "CHARM_NAME_" + charmnum1.ToString())
				{
					return "Ooma Core";
				}
				if (key == "CHARM_DESC_" + charmnum1.ToString())
				{
					return "Uuwuu will grant you some mercy.";
				}
				if (key == "MEGA_JELLY_MAIN" && PlayerData.instance.statueStateUumuu.usingAltVersion)
				{
					return "Uuwuu";
				}
				if (key == "MEGA_JELLY_SUPER" && PlayerData.instance.statueStateUumuu.usingAltVersion)
				{
					return "";
				}
				if (key == "MEGA_JELLY_SUB" && PlayerData.instance.statueStateUumuu.usingAltVersion)
				{
					return "Upside down Uumuu";
				}
				if (key == "Uuwuu_Desc")
				{
					return "Upside down Uumuu";
				}
				if (key == "Uuwuu_Name")
				{
					return "Uuwuu";
				}
			}
			return Language.Language.GetInternal(key, sheettitle);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000235A File Offset: 0x0000055A
		private void SaveGame(SaveGameData data)
		{
			Uuwuu.AddComponent();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002384 File Offset: 0x00000584
		private static void AddComponent()
		{
			GameManager.instance.gameObject.AddComponent<UumuuFinder>();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003350 File Offset: 0x00001550
		public void Unload()
		{
			ModHooks.Instance.AfterSavegameLoadHook -= this.SaveGame;
			ModHooks.Instance.NewGameHook -= Uuwuu.AddComponent;
			ModHooks.Instance.LanguageGetHook -= this.OnLangGet;
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= this.SceneChanged;
			ModHooks.Instance.GetPlayerVariableHook -= this.GetVariableHook;
			ModHooks.Instance.GetPlayerBoolHook -= this.OnGetPlayerBoolHook;
			ModHooks.Instance.SetPlayerBoolHook -= this.OnSetPlayerBoolHook;
			ModHooks.Instance.GetPlayerIntHook -= this.OnGetPlayerIntHook;
			UumuuFinder component = GameManager.instance.gameObject.GetComponent<UumuuFinder>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003510 File Offset: 0x00001710
		public override List<ValueTuple<string, string>> GetPreloadNames()
		{
			return new List<ValueTuple<string, string>>
			{
				new ValueTuple<string, string>("White_Palace_07", "wp_saw (30)")
			};
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000358C File Offset: 0x0000178C
		public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
		{
			Uuwuu.Instance = this;
			Uuwuu.PreloadedGameObjects.Add("saw", preloadedObjects["White_Palace_07"]["wp_saw (30)"]);
			this.Unload();
			this.SetupSettings();
			ModHooks.Instance.BeforeSavegameSaveHook += Instance_BeforeSavegameSaveHook;
			ModHooks.Instance.AfterSavegameLoadHook += this.SaveGame;
			ModHooks.Instance.NewGameHook += Uuwuu.AddComponent;
			ModHooks.Instance.LanguageGetHook += this.OnLangGet;
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged += this.SceneChanged;
			ModHooks.Instance.GetPlayerVariableHook += this.GetVariableHook;
			ModHooks.Instance.GetPlayerBoolHook += this.OnGetPlayerBoolHook;
			ModHooks.Instance.SetPlayerBoolHook += this.OnSetPlayerBoolHook;
			ModHooks.Instance.GetPlayerIntHook += this.OnGetPlayerIntHook;
			Assembly asm = Assembly.GetExecutingAssembly();
			Sprites = new Dictionary<string, Sprite>();
			foreach (string res in asm.GetManifestResourceNames())
			{
				if (!res.EndsWith(".png"))
					continue;

				if (res.Contains("Uuwuu.png"))
				{
					using (Stream s = asm.GetManifestResourceStream(res))
					{
						if (s == null) continue;
						byte[] buffer = new byte[s.Length];
						s.Read(buffer, 0, buffer.Length);
						s.Dispose();

						//Create texture from bytes
						Texture2D tex = new Texture2D(1, 1);
						tex.LoadImage(buffer);

						//Create sprite from texture
						Sprites.Add(res, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
						Sprites2[1] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
						Log("Created sprite from embedded image: " + res);
					}
				}
			}
			this.ChHelper = new CharmHelper();
		}

		private void Instance_BeforeSavegameSaveHook(SaveGameData data)
		{
			PlayerData.instance.statueStateUumuu.usingAltVersion = false;
		}

		private void SetupSettings()
		{
			if (!File.Exists(Application.persistentDataPath + ModHooks.PathSeperator.ToString() + base.GetType().Name + ".GlobalSettings.json"))
			{
				SaveSettings globalSettings = base.GlobalSettings;
				if (globalSettings != null)
				{
					globalSettings.Reset();
				}
			}
			base.SaveGlobalSettings();
			customnums = base.GlobalSettings.customtimers;
			easynum = base.GlobalSettings.easytimer;
			hardnum = base.GlobalSettings.hardtimer;
		}
		private object GetVariableHook(Type t, string key, object orig)
		{
			if (!(key == "statueStateUuwuu"))
			{
				return orig;
			}
			return new BossStatue.Completion
			{
				completedTier1 = true,
				seenTier3Unlock = true,
				completedTier2 = true,
				completedTier3 = true,
				isUnlocked = true,
				hasBeenSeen = true,
				usingAltVersion = false
			};
		}

		// Token: 0x04000022 RID: 34
		private string _lastScene;

		// Token: 0x04000023 RID: 35
		public Dictionary<string, Sprite> dict;

		// Token: 0x04000024 RID: 36
		public static readonly Dictionary<string, GameObject> PreloadedGameObjects = new Dictionary<string, GameObject>();

		public bool customnums;

		public int easynum;

		public int hardnum;

		public int charmnum1 = 41;

		public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

		public Sprite[] Sprites2 = new Sprite[2];
		public CharmHelper ChHelper { get; private set; }
	}
}
