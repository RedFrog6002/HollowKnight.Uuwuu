using System;
using GlobalEnums;
using UnityEngine;

namespace Uuwuu
{
	// Token: 0x02000002 RID: 2
	internal class DamageOnCollision : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public int Damage { private get; set; } = 1;

		// Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		private void OnParticleCollision(GameObject other)
		{
			if (other != HeroController.instance.gameObject)
			{
				return;
			}
			HeroController.instance.TakeDamage(base.gameObject, CollisionSide.other, this.Damage, 1);
		}
	}
}
