using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using RefTheGun.Items;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using static RefTheGun.RefTheExtensions;

namespace RefTheGun.Projectiles
{
	public class PandoraGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
        public bool useIt = false;
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit){
            if (useIt){
                Player player = new Player();
                player.statLife = target.life;
                player.statLifeMax2 = target.lifeMax;
                for (int i = 0; i < target.buffType.Length; i++){
                    player.buffType[i] = target.buffType[i];
                    player.buffTime[i] = target.buffTime[i];
                }
                player.buffImmune = target.buffImmune;
                player.statDefense = target.defense;
                player.velocity = target.velocity;
                player.Center = target.Center;
                ProjectileLoader.OnHitPlayer(projectile, player, damage, crit);
                target.life = player.statLife;
                target.lifeMax = player.statLifeMax2;
                target.buffType = player.buffType;
                target.buffTime = player.buffTime;
                target.buffImmune = player.buffImmune;
                target.defense = player.statDefense;
                target.velocity = player.velocity;
                target.Center = player.Center;
            }
        }
    }
}