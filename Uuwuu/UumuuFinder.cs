using System;
using System.Collections;
using System.Reflection;
using ModCommon;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uuwuu
{
	// Token: 0x02000011 RID: 17
	internal class UumuuFinder : MonoBehaviour
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000023A2 File Offset: 0x000005A2
		private void Start()
		{
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged += this.SceneChanged;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000370C File Offset: 0x0000190C
		private void SceneChanged(Scene arg0, Scene arg1)
		{
			if (arg1.name == "GG_Workshop")
			{
				UumuuFinder.SetStatue();
			}
			if (arg0.name != "GG_Workshop")
			{
				return;
			}
			if (PlayerData.instance.statueStateUumuu.usingAltVersion)
			{
				if (!Uuwuu.Instance.Settings.EquippedCharm)
				{
					if (arg1.name == "GG_Uumuu")
						base.StartCoroutine(UumuuFinder.AddComponent());
					if (arg1.name == "GG_Uumuu_V")
						base.StartCoroutine(UumuuFinder.AddComponent2());
				}
				else
					base.StartCoroutine(UumuuFinder.AddComponent3());
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003768 File Offset: 0x00001968
		private static void SetStatue()
		{
			GameObject gameObject = GameObject.Find("GG_Statue_Uumuu");
			//BossScene bossScene = ScriptableObject.CreateInstance<BossScene>();
			//bossScene.sceneName = "GG_Uumuu";
			BossStatue component = gameObject.GetComponent<BossStatue>();
			//component.dreamBossScene = bossScene;
			component.dreamBossScene = component.bossScene;
			component.dreamStatueStatePD = "statueStateUuwuu";
			BossStatue.Completion statueState = new BossStatue.Completion
			{
				completedTier1 = true,
				seenTier3Unlock = true,
				completedTier2 = true,
				completedTier3 = true,
				isUnlocked = true,
				hasBeenSeen = true,
				usingAltVersion = false
			};
			component.DreamStatueState = statueState;
			component.SetPlaquesVisible((component.StatueState.isUnlocked && component.StatueState.hasBeenSeen) || component.isAlwaysUnlocked);
			UnityEngine.Object.Destroy(gameObject.FindGameObjectInChildren("StatueAlt"));
			GameObject statueDisplay = component.statueDisplay;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(statueDisplay, statueDisplay.transform.parent, true);
			gameObject2.SetActive(component.UsingDreamVersion);
			//SpriteRenderer componentInChildren = gameObject2.GetComponentInChildren<SpriteRenderer>();
			//componentInChildren.sprite = SuperUumuu.Instance.dict["Statue"];
			//componentInChildren.transform.position += Vector3.up * 1.8f;
			gameObject2.name = "StatueAlt";
			gameObject2.GetComponentInChildren<SpriteRenderer>(true).flipX = true;
			gameObject2.GetComponentInChildren<SpriteRenderer>(true).flipY = true;
			gameObject2.GetComponentInChildren<SpriteRenderer>(true).transform.SetScaleX(gameObject2.GetComponentInChildren<SpriteRenderer>(true).transform.GetScaleX() + 0.07f);
			gameObject2.GetComponentInChildren<SpriteRenderer>(true).transform.SetScaleY(gameObject2.GetComponentInChildren<SpriteRenderer>(true).transform.GetScaleY() + 0.07f);
			component.statueDisplayAlt = gameObject2;
			BossStatue.BossUIDetails dreamBossDetails = default(BossStatue.BossUIDetails);
			dreamBossDetails.nameKey = (dreamBossDetails.nameSheet = "Uuwuu_Name");
			dreamBossDetails.descriptionKey = (dreamBossDetails.descriptionSheet = "Uuwuu_Desc");
			component.dreamBossDetails = dreamBossDetails;
			GameObject gameObject3 = gameObject.FindGameObjectInChildren("alt_lever");
			gameObject3.SetActive(true);
			gameObject3.transform.position = new Vector3(177.5f, 7.5f, 1f);
			gameObject3.FindGameObjectInChildren("GG_statue_switch_bracket").SetActive(true);
			gameObject3.FindGameObjectInChildren("GG_statue_switch_lever").SetActive(true);
			BossStatueLever componentInChildren2 = gameObject.GetComponentInChildren<BossStatueLever>();
			componentInChildren2.SetOwner(component);
			componentInChildren2.SetState(true);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000023B5 File Offset: 0x000005B5
		private static IEnumerator AddComponent()
		{
			yield return null;
			GameObject.Find("Mega Jellyfish GG").AddComponent<Uumuu>();
			yield break;
		}
		private static IEnumerator AddComponent2()
		{
			yield return null;
			GameObject.Find("Mega Jellyfish GG").AddComponent<Uumuu>().hardmode = true;
			yield break;
		}
		private static IEnumerator AddComponent3()
		{
			yield return null;
			GameObject.Find("Mega Jellyfish GG").AddComponent<Uumuu>().easymode = true;
			yield break;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000023BD File Offset: 0x000005BD
		private void OnDestroy()
		{
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= this.SceneChanged;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000023D0 File Offset: 0x000005D0
		public static void Log(object o)
		{
			Modding.Logger.Log(string.Format("[{0}]: ", Assembly.GetExecutingAssembly().GetName().Name) + o);
		}
	}
}
