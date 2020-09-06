using System;
using Modding;
using UnityEngine;

namespace Uuwuu
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public class SaveSettings : ModSettings
	{
		public void Reset()
		{
			this.BoolValues.Clear();
			this.StringValues.Clear();
			this.IntValues.Clear();
			this.customtimers = false;
			this.GotCharm = false;
			this.easytimer = 30;
			this.hardtimer = 45;
		}

		public int easytimer
		{
			get
			{
				return base.GetInt(30, "easytimer");
			}
			private set
			{
				base.SetInt(value, "easytimer");
			}
		}

		public int hardtimer
		{
			get
			{
				return base.GetInt(45, "easytimer");
			}
			private set
			{
				base.SetInt(value, "easytimer");
			}
		}
		public bool customtimers
		{
			get
			{
				return base.GetBool(false, "customtimers");
			}
			set
			{
				base.SetBool(value, "customtimers");
			}
		}
		public bool GotCharm
		{
			get
			{
				return base.GetBool(false, "GotCharm");
			}
			set
			{
				base.SetBool(value, "GotCharm");
			}
		}
	}
}
