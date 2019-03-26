using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace RefTheGun.Buffs
{
	public class BlessedBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Blessed Weapon");
			Description.SetDefault("");
		}
        public override void Update(Player player, ref int buffIndex){
            player.meleeDamage+=0.15f;
            player.rangedDamage+=0.15f;
            player.magicDamage+=0.15f;
            player.minionDamage+=0.15f;
            player.thrownDamage+=0.15f;
            player.lifeRegenCount+=10;
        }
	}
}