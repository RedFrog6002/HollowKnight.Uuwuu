using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using UnityEngine;
using Modding;
using ModCommon;
using ModCommon.Util;
using UnityEngine.UI;
using System.IO;
using System.Reflection;
using GlobalEnums;
using Language;
using On;
using Uuwuu;

namespace Uuwuu
{
	// Token: 0x0200001B RID: 27
	internal class Uumuu : MonoBehaviour
	{
		private static ParticleSystem AddTrail(GameObject go, float offset = 0f, Color? c = null)
		{
			ParticleSystem particleSystem = go.AddComponent<ParticleSystem>();
			ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
			Material material = new Material(Shader.Find("Particles/Additive"));
			material.mainTexture = Resources.FindObjectsOfTypeAll<Texture>().FirstOrDefault((Texture x) => x.name == "Default-Particle");
			material.color = (c ?? Color.white);
			Material material2 = material;
			component.trailMaterial = material;
			component.material = material2;
			ParticleSystem.MainModule main = particleSystem.main;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			main.startSpeed = 0f;
			main.startLifetime = 1.2f;
			main.startColor = (c ?? Color.white);
			main.maxParticles *= 3;
			ParticleSystem.ShapeModule shape = particleSystem.shape;
			shape.shapeType = ParticleSystemShapeType.Donut;
			shape.radius *= 4.5f;
			shape.radiusSpeed = 0.01f;
			Vector3 position = shape.position;
			position.y -= offset;
			shape.position = position;
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.rateOverTime = 0f;
			emission.rateOverDistance = 30f;
			ParticleSystem.CollisionModule collision = particleSystem.collision;
			collision.type = ParticleSystemCollisionType.World;
			collision.sendCollisionMessages = true;
			collision.mode = ParticleSystemCollisionMode.Collision2D;
			collision.enabled = true;
			collision.quality = ParticleSystemCollisionQuality.High;
			collision.maxCollisionShapes = 256;
			collision.dampenMultiplier = 0f;
			collision.radiusScale = 0.3f;
			collision.collidesWith = 512;
			go.AddComponent<DamageOnCollision>().Damage = 2;
			return particleSystem;
		}
		private Vector2 box;
		private Vector2 circle;
		// Token: 0x06000090 RID: 144 RVA: 0x0000253E File Offset: 0x0000073E
		private IEnumerator Start()
		{
			yield return null;
			while (HeroController.instance == null)
			{
				yield return null;
			}
			if (!PlayerData.instance.statueStateUumuu.usingAltVersion)
			{
				yield break;
			}
			Modding.Logger.Log("SUPERUUMUU");
			ModHooks.Instance.TakeHealthHook += this.TookDamge;
			DamageHero[] componentsInChildren = base.gameObject.GetComponentsInChildren<DamageHero>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].damageDealt *= 2;
			}
			foreach (Vector2 position in sawpositions)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Uuwuu.PreloadedGameObjects["saw"]);
				gameObject.GetComponent<SpriteRenderer>().color = new Color(0.25f, 1.7f, 0.5f, 1.65f);
				//gameObject.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(new byte[Assembly.GetExecutingAssembly().GetManifestResourceStream("Uuwuu.Resources.saw.png").Length]);
				gameObject.transform.SetPosition2D(position);
				gameObject.AddComponent<SawHandler>();
				gameObject.SetActive(true);
			}
			base.gameObject.GetComponent<global::tk2dSprite>().color = new Color(1f, 2f, 1f, 1f);
			base.gameObject.GetComponent<global::tk2dSprite>().FlipX = true;
			base.gameObject.GetComponent<global::tk2dSprite>().FlipY = true;
			base.gameObject.transform.SetScaleX(base.gameObject.transform.GetScaleX() + 0.07f);
			base.gameObject.transform.SetScaleY(base.gameObject.transform.GetScaleY() + 0.07f);
			box = base.gameObject.GetComponent<BoxCollider2D>().offset;
			circle = base.gameObject.GetComponent<CircleCollider2D>().offset;
			base.gameObject.GetComponent<BoxCollider2D>().offset += Vector2.up * -1.6f;
			base.gameObject.GetComponent<CircleCollider2D>().offset += Vector2.up * -1.6f;
			this._hm.hp = 3000;
			this.phase1 = true;
			this.phase2 = true;
			this.phase3 = true;
			this._trail = AddTrail(base.gameObject, 0f, Color.green);
			this._trail.Stop();
			this._control.CopyState("Multizap", "Multizap2");
			this._control.CopyState("Roar", "PhaseRoar");
			this._control.CopyState("Roar", "PhaseRoar2");
			this._control.CopyState("Zapping", "Attack21");
			this._control.CopyState("Gen", "Attack22");
			this._control.CopyState("First?", "Attack23");
			this._control.CopyState("Zapping", "Attack31");
			this._control.CopyState("Gen", "Attack32");
			this._control.CopyState("First?", "Attack33");
			this._control.ChangeTransition("Choice", "CHASE", "Pattern Choice");
			this._control.ChangeTransition("Multizap", "FINISHED", "Zapping");
			this._control.ChangeTransition("Gen", "END", "Multizap2");
			this._control.ChangeTransition("Attack21", "FINISHED", "Attack22");
			this._control.ChangeTransition("Attack22", "FINISHED", "Attack23");
			this._control.ChangeTransition("Attack23", "FINISHED", "Attack21");
			this._control.ChangeTransition("Attack31", "FINISHED", "Attack32");
			this._control.ChangeTransition("Attack32", "FINISHED", "Attack33");
			this._control.ChangeTransition("Attack33", "FINISHED", "Attack31");
			if (!easymode)
			{
				this._control.GetAction<Wait>("Multizap", 2).time = 1f;
				this._control.GetAction<Wait>("Attack Recover", 4).time = 0.5f;
				this._control.GetAction<Wait>("Zapping", 0).time = 0.25f;
				this._control.GetAction<Wait>("First?", 4).time = 0.25f;
				this._control.GetAction<Wait>("Attack21", 0).time = 0.70f;
				this._control.GetAction<Wait>("Attack23", 4).time = 0.70f;
				this._control.GetAction<Wait>("Wounded", 2).time = 2.5f;
			}
			else
			{
				this._control.GetAction<Wait>("Multizap", 2).time = 1.25f;
				this._control.GetAction<Wait>("Attack Recover", 4).time = 0.75f;
				this._control.GetAction<Wait>("Zapping", 0).time = 0.5f;
				this._control.GetAction<Wait>("First?", 4).time = 0.5f;
				this._control.GetAction<Wait>("Attack21", 0).time = 1f;
				this._control.GetAction<Wait>("Attack23", 4).time = 1f;
				this._control.GetAction<Wait>("Wounded", 2).time = 3f;
			}
			this._control.GetAction<Wait>("PhaseRoar", 1).time = 2.5f;
			this._control.GetAction<Wait>("PhaseRoar2", 1).time = 2.5f;
			this._control.GetAction<IntCompare>("Gen", 1).integer2 = 12;
			this._control.InsertMethod("PhaseRoar", 10, new Action(fixhitbox2));
			this._control.InsertMethod("PhaseRoar2", 10, new Action(fixhitbox2));
			this._control.InsertMethod("Idle", 2, new Action(fixhitbox2));
			FsmutilExt.RemoveAction(this._control, "PhaseRoar", 3);
			FsmutilExt.RemoveAction(this._control, "PhaseRoar2", 3);
			FsmutilExt.RemoveAction(this._control, "Attack22", 2);
			this._control.InsertMethod("Explode", 13, new Action(fixhitbox1));
			this._control.InsertMethod("Attack22", 2, new Action(Placetargets));
			this._control.AddAction("PhaseRoar", this._control.GetAction("Recover", 1));
			this._control.AddAction("PhaseRoar", this._control.GetAction("Recover", 2));
			this._control.AddAction("PhaseRoar", this._control.GetAction("Recover", 3));
			this._control.AddAction("PhaseRoar", this._control.GetAction("Recover", 4));
			this._control.AddAction("PhaseRoar", this._control.GetAction("Recover", 6));
			this._control.AddAction("PhaseRoar", this._control.GetAction("Recover", 7));
			this._control.AddAction("PhaseRoar2", this._control.GetAction("Recover", 1));
			this._control.AddAction("PhaseRoar2", this._control.GetAction("Recover", 3));
			this._control.AddAction("PhaseRoar2", this._control.GetAction("Recover", 4));
			this._control.AddAction("PhaseRoar2", this._control.GetAction("Recover", 6));
			this._control.AddAction("PhaseRoar2", this._control.GetAction("Recover", 7));
			yield break;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000254D File Offset: 0x0000074D
		private void Awake()
		{
			this._hm = base.gameObject.GetComponent<HealthManager>();
			this._control = base.gameObject.LocateMyFSM("Mega Jellyfish");
		}

		private void fixhitbox1()
		{
			base.gameObject.GetComponent<BoxCollider2D>().offset = box;
			base.gameObject.GetComponent<CircleCollider2D>().offset = circle;
		}
		private void fixhitbox2()
		{
			base.gameObject.GetComponent<BoxCollider2D>().offset = box + Vector2.up * -1.6f;
			base.gameObject.GetComponent<CircleCollider2D>().offset = circle + Vector2.up * -1.6f;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003C24 File Offset: 0x00001E24
		private void Update()
		{
			if (PlayerData.instance.statueStateUumuu.usingAltVersion)
			{
				MakeCanvas();
				if (this._hm.hp < 2500 && this.phase1)
				{
					this.phase1 = false;
					this._control.SetState("PhaseRoar");
					base.gameObject.GetComponent<global::tk2dSprite>().color = new Color(1f, 1.75f, 0f, 1.5f);
					this._control.ChangeTransition("Choice", "MULTIZAP", "Attack21");
					if (!easymode)
					{
						if (hardmode)
							this._control.AddAction("Idle", this._control.GetAction("Gen", 2));
						this._control.AddAction("Attack Antic", this._control.GetAction("Gen", 2));
						this._control.GetAction<ChaseObjectV2>("Attack Antic", 3).speedMax = 5f;
						this._control.GetAction<ChaseObjectV2>("Idle", 0).speedMax = 6f;
						this._control.GetAction<ChaseObjectV2>("Wounded", 1).speedMax = 3f;
					}
					this._control.GetAction<ChaseObjectV2>("Wounded", 1).accelerationForce = -1.5f;
					this._control.GetAction<SetRecoilSpeed>("Wounded", 3).newRecoilSpeed = 20f;
				}
				if (this._hm.hp < 1800 && this.phase2)
				{
					this.phase2 = false;
					this._control.SetState("PhaseRoar2");
					base.gameObject.GetComponent<global::tk2dSprite>().color = new Color(0f, 2f, 1f, 2f);
					this._control.ChangeTransition("Choice", "SPAWN", "Attack31");
					this._control.ChangeTransition("Explode", "FINISHED", "Recover");
					this._control.GetAction<SpawnObjectFromGlobalPool>("Attack32", 2).spawnPoint = base.gameObject;
					this._control.GetAction<IntCompare>("Attack32", 1).integer2 = 15;
					this._control.GetAction<Wait>("Attack31", 0).time = 0.1f;
					this._control.GetAction<Wait>("Attack33", 4).time = 0.2f;
					FsmutilExt.RemoveAction(this._control, "Explode", 7);
					FsmutilExt.RemoveAction(this._control, "Recover", 2);
					this._control.InsertMethod("Explode", 0, new Action(this.Retaliate));
					this._control.InsertMethod("Attack32", 2, new Action(this.LargeAttack));
					if (!easymode)
					{
						this._control.GetAction<ChaseObjectV2>("Idle", 0).speedMax = 10f;
						this._control.GetAction<ChaseObjectV2>("Idle", 0).accelerationForce = 5.5f;
						this._control.GetAction<ChaseObjectV2>("Attack Antic", 3).speedMax = 8f;
						this._control.GetAction<ChaseObjectV2>("Attack Antic", 3).accelerationForce = 4.5f;
						this._control.GetAction<WaitRandom>("Idle", 1).timeMin = 1.25f;
						this._control.GetAction<WaitRandom>("Idle", 1).timeMax = 1.75f;
					}
					else
					{
						this._control.GetAction<ChaseObjectV2>("Attack Antic", 3).speedMax = 5f;
						this._control.GetAction<ChaseObjectV2>("Idle", 0).speedMax = 6f;
						this._control.GetAction<SetRecoilSpeed>("Wounded", 3).newRecoilSpeed = 20f;
					}
				}
				if (_hm.hp < 1000 && this.phase3)
				{
					if (!easymode)
					{
						if (Uuwuu.Instance.customnums)
							HeroController.instance.StartCoroutine(Countdown(this.hardmode ? Uuwuu.Instance.hardnum : Uuwuu.Instance.easynum));
						else
							HeroController.instance.StartCoroutine(Countdown(this.hardmode ? 45 : 30));
					}
					else
					{

						if (Uuwuu.Instance.customnums)
							HeroController.instance.StartCoroutine(Countdown(this.hardmode ? Uuwuu.Instance.hardnum - 25 : Uuwuu.Instance.easynum - 10));
						else
							HeroController.instance.StartCoroutine(Countdown(20));
					}
					this.phase3 = false;
					this._control.SetState("PhaseRoar");
					base.gameObject.GetComponent<global::tk2dSprite>().color = new Color(0f, 2f, 0f, 2f);
					this._trail.Play();
					if (hardmode)
					this._control.AddAction("Attack Antic", this._control.GetAction("Roar", 3));
					this._control.AddAction("Attack Antic", this._control.GetAction("Roar", 3));
					if (!easymode)
					{
						this._control.GetAction<WaitRandom>("Idle", 1).timeMin = 1.1f;
						this._control.GetAction<WaitRandom>("Idle", 1).timeMax = 1.65f;
						this._control.GetAction<Wait>("Attack21", 0).time = 0.55f;
						this._control.GetAction<Wait>("Attack23", 4).time = 0.60f;
						this._control.GetAction<IntCompare>("Attack32", 1).integer2 = 20;
						this._control.GetAction<Wait>("Attack31", 0).time = 0.05f;
						this._control.GetAction<Wait>("Attack33", 4).time = 0.1f;
					}
					else
					{
						this._control.GetAction<WaitRandom>("Idle", 1).timeMin = 1.6f;
						this._control.GetAction<WaitRandom>("Idle", 1).timeMax = 1.8f;
						this._control.GetAction<Wait>("Attack21", 0).time = 0.60f;
						this._control.GetAction<Wait>("Attack23", 4).time = 0.70f;
						this._control.GetAction<IntCompare>("Attack32", 1).integer2 = 17;
						this._control.GetAction<Wait>("Attack31", 0).time = 0.10f;
						this._control.GetAction<Wait>("Attack33", 4).time = 0.2f;
					}
					if (TextObject == null)
						MakeCanvas();
				}
			}
		}

		private int TookDamge(int damage)
		{
			int rand = UnityEngine.Random.Range(1, 20);
			if (hardmode && rand == 10)
				return damage + 1;
			return damage;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003FC0 File Offset: 0x000021C0
		private void LargeAttack()
		{
			int num = this._control.FsmVariables.GetFsmInt("Zaps").Value * 2;
			int num2 = num / 2;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject7 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject8 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			gameObject.transform.SetPositionX(base.gameObject.transform.GetPositionX());
			gameObject.transform.SetPositionY(base.gameObject.transform.GetPositionY() + (float)num);
			gameObject.SetActive(true);
			gameObject2.transform.SetPositionX(base.gameObject.transform.GetPositionX());
			gameObject2.transform.SetPositionY(base.gameObject.transform.GetPositionY() - (float)num);
			gameObject2.SetActive(true);
			gameObject3.transform.SetPositionX(base.gameObject.transform.GetPositionX() + (float)num);
			gameObject3.transform.SetPositionY(base.gameObject.transform.GetPositionY());
			gameObject3.SetActive(true);
			gameObject4.transform.SetPositionX(base.gameObject.transform.GetPositionX() - (float)num);
			gameObject4.transform.SetPositionY(base.gameObject.transform.GetPositionY());
			gameObject4.SetActive(true);
			gameObject5.transform.SetPositionX(base.gameObject.transform.GetPositionX() + (float)num2);
			gameObject5.transform.SetPositionY(base.gameObject.transform.GetPositionY() + (float)num2);
			gameObject5.SetActive(true);
			gameObject6.transform.SetPositionX(base.gameObject.transform.GetPositionX() - (float)num2);
			gameObject6.transform.SetPositionY(base.gameObject.transform.GetPositionY() - (float)num2);
			gameObject6.SetActive(true);
			gameObject7.transform.SetPositionX(base.gameObject.transform.GetPositionX() + (float)num2);
			gameObject7.transform.SetPositionY(base.gameObject.transform.GetPositionY() - (float)num2);
			gameObject7.SetActive(true);
			gameObject8.transform.SetPositionX(base.gameObject.transform.GetPositionX() - (float)num2);
			gameObject8.transform.SetPositionY(base.gameObject.transform.GetPositionY() + (float)num2);
			gameObject8.SetActive(true);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000433C File Offset: 0x0000253C
		private void Retaliate()
		{
			int num = 3;
			int num2 = num / 2;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject7 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			GameObject gameObject8 = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
			gameObject.transform.SetPositionX(base.gameObject.transform.GetPositionX());
			gameObject.transform.SetPositionY(base.gameObject.transform.GetPositionY() + (float)num);
			gameObject.SetActive(true);
			gameObject2.transform.SetPositionX(base.gameObject.transform.GetPositionX());
			gameObject2.transform.SetPositionY(base.gameObject.transform.GetPositionY() - (float)num);
			gameObject2.SetActive(true);
			gameObject3.transform.SetPositionX(base.gameObject.transform.GetPositionX() + (float)num);
			gameObject3.transform.SetPositionY(base.gameObject.transform.GetPositionY());
			gameObject3.SetActive(true);
			gameObject4.transform.SetPositionX(base.gameObject.transform.GetPositionX() - (float)num);
			gameObject4.transform.SetPositionY(base.gameObject.transform.GetPositionY());
			gameObject4.SetActive(true);
			gameObject5.transform.SetPositionX(base.gameObject.transform.GetPositionX() + (float)num2);
			gameObject5.transform.SetPositionY(base.gameObject.transform.GetPositionY() + (float)num2);
			gameObject5.SetActive(true);
			gameObject6.transform.SetPositionX(base.gameObject.transform.GetPositionX() - (float)num2);
			gameObject6.transform.SetPositionY(base.gameObject.transform.GetPositionY() - (float)num2);
			gameObject6.SetActive(true);
			gameObject7.transform.SetPositionX(base.gameObject.transform.GetPositionX() + (float)num2);
			gameObject7.transform.SetPositionY(base.gameObject.transform.GetPositionY() - (float)num2);
			gameObject7.SetActive(true);
			gameObject8.transform.SetPositionX(base.gameObject.transform.GetPositionX() - (float)num2);
			gameObject8.transform.SetPositionY(base.gameObject.transform.GetPositionY() + (float)num2);
			gameObject8.SetActive(true);
		}
		private void MakeCanvas()
		{
			if (CanvasObject == null)
				CanvasObject = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1920f, 1080f));

			if (TextCanvas == null)
			{
				CanvasUtil.CreateFonts();
				TextCanvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1920f, 1080f));
				GameObject TextPanel = CanvasUtil.CreateTextPanel(TextCanvas, "", 35, TextAnchor.MiddleCenter,
					new CanvasUtil.RectData(
						new Vector2(0, 50),
						new Vector2(0, 45),
						new Vector2(0, 0),
						new Vector2(1, 0),
						new Vector2(0.5f, 0.5f)));
				TextObject = TextPanel.GetComponent<Text>();
				TextObject.font = CanvasUtil.TrajanBold;
				TextObject.text = "";
				TextObject.fontSize = 50;
				TextObject.color = Color.red;
			}
		}
		private IEnumerator Countdown(int countdown)
		{
			yield return new WaitForSeconds(0.35f);
			TextObject.text = "Countdown Initiated";
			TextObject.CrossFadeAlpha(1f, 0f, false);
			yield return new WaitForSeconds(2f);
			do
			{
				TextObject.text = countdown.ToString();
				countdown--;
				yield return new WaitForSeconds(1f);
			} while (countdown > 0);
			TextObject.CrossFadeAlpha(0f, 1f, false);
			base.gameObject.GetComponent<HealthManager>().Die(0f, AttackTypes.Nail, true);
		}

		private void Placetargets()
		{
			for (int i = 1; i < 17; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._control.GetAction<SpawnObjectFromGlobalPool>("Gen", 2).gameObject.Value.gameObject);
				gameObject.transform.SetPosition2D(UnityEngine.Random.Range(36f, 69f), UnityEngine.Random.Range(100f, 133f));
				gameObject.SetActive(true);
			}
		}

		private void OnDestroy()
		{
			ModHooks.Instance.TakeHealthHook -= this.TookDamge;
		}

		private PlayMakerFSM _control;

		private HealthManager _hm;

		private bool phase1;

		private bool phase2;

		private bool phase3;

		private ParticleSystem _trail;

		private GameObject CanvasObject;

		private GameObject TextCanvas;

		private Text TextObject;

		private Vector2[] sawpositions = new Vector2[] { new Vector2(35f, 134.5f), new Vector2(40f, 134.5f), new Vector2(45f, 134.5f), new Vector2(50f, 134.5f), new Vector2(55f, 134.5f), new Vector2(60f, 134.5f), new Vector2(65f, 134.5f), new Vector2(70f, 134.5f) };

		public bool hardmode = false;

		public bool easymode = false;
	}
}
