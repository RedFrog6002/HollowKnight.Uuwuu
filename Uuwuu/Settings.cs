using System;
using Modding;
using UnityEngine;

namespace Uuwuu
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public class Settings : ModSettings
	{
		public bool EquippedCharm
		{
			get
			{
				return base.GetBool(false);
			}
			set
			{
				base.SetBool(value);
			}
		}
		public bool haskilled = false;
		public bool newentry = true;
		public int killsleft = 3;
	}
}