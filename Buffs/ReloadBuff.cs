using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace RefTheGun.Buffs
{
	public class ReloadBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Reloaded");
			Description.SetDefault("");
		}

		public override void Update(Player player, ref int buffIndex){
            ((GunPlayer)player).Reloaded = true;
        }
	}
}