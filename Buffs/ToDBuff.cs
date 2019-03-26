using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace RefTheGun.Buffs
{
	public class ToDBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tears of Denial");
			Description.SetDefault("");
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
		}
        public override void Update(Player player, ref int buffIndex){
            player.buffTime[buffIndex]++;
        }
	}
}