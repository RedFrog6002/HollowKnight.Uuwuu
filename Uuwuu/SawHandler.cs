using System;
using Modding;
using UnityEngine;

namespace Uuwuu
{
	// Token: 0x02000014 RID: 20
	internal class SawHandler : MonoBehaviour
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00005070 File Offset: 0x00003270
		private void Start()
		{
			base.gameObject.GetComponent<AudioSource>().volume = 0.2f;
			base.gameObject.GetComponent<DamageHero>().hazardType = 0;
			base.gameObject.GetComponent<DamageHero>().damageDealt = 2;
			base.gameObject.AddComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			base.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
			base.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
		}
		private void OnTriggerEnter2D(Collider2D col)
		{
			if ((col.name.StartsWith("Jelly") || col.name.StartsWith("jelly")) && col.name != "Mega Jellyfish GG")
			{
				col.gameObject.GetComponent<HealthManager>().Die(270f, AttackTypes.Nail, true);
			}
		}
		// Token: 0x060000D4 RID: 212 RVA: 0x00005125 File Offset: 0x00003325
		private static void Log(object obj)
		{
			Modding.Logger.Log("[Saws] " + ((obj != null) ? obj.ToString() : null));
		}
	}
}