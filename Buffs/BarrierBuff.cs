using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace RefTheGun.Buffs
{
	public class BarrierBuff : ModBuff
	{
        PlayerDeathReason deathreason = new PlayerDeathReason();
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Great Magic Barrier");
			Description.SetDefault("");
		}

		public override void Update(Player player, ref int buffIndex){
            player.endurance+=0.15f;
        }
	}
}