using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace RefTheGun.Buffs
{
	public class PowerWithin : ModBuff
	{
        PlayerDeathReason deathreason = new PlayerDeathReason();
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Power Within");
			Description.SetDefault("");
            deathreason.SourceCustomReason = " Got a bit carried away";
		}

		public override void Update(Player player, ref int buffIndex){
            player.meleeDamage+=0.6f;
            player.rangedDamage+=0.6f;
            player.magicDamage+=0.6f;
            player.minionDamage+=0.6f;
            player.thrownDamage+=0.6f;
            //player.lifeRegen=Math.Min(player.lifeRegen-player.statLifeMax2/100, -player.statLifeMax2/100);
            int imm = player.immuneTime;
            player.immuneTime = 0;
            if(player.buffTime[buffIndex]%60==0)player.Hurt(deathreason, (player.statLifeMax2/75)+(int)(player.statDefense*(Main.expertMode?0.75:0.5)), 0, cooldownCounter:1);
            player.immuneTime = imm;
        }
	}
}