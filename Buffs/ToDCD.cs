using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace RefTheGun.Buffs
{
	public class ToDCD : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tears of Denial Cooldown");
			Description.SetDefault("");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
		}
	}
}