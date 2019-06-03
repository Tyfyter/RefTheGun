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
                StatusPlayer(projectile, player);
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
        public void StatusPlayer(Projectile proj, Player player)
        {
            if (proj.type == 472)
            {
                player.AddBuff(149, Main.rand.Next(30, 150), true);
            }
            if (proj.type == 467)
            {
                player.AddBuff(24, Main.rand.Next(30, 150), true);
            }
            if (proj.type == 581)
            {
                if (Main.expertMode)
                {
                    player.AddBuff(164, Main.rand.Next(300, 540), true);
                }
                else if (Main.rand.Next(2) == 0)
                {
                    player.AddBuff(164, Main.rand.Next(360, 720), true);
                }
            }
            if (proj.type == 687)
            {
                player.AddBuff(24, 60 * Main.rand.Next(7, 11), true);
            }
            if (proj.type == 258 && Main.rand.Next(2) == 0)
            {
                player.AddBuff(24, 60 * Main.rand.Next(5, 8), true);
            }
            if (proj.type == 572 && Main.rand.Next(3) != 0)
            {
                player.AddBuff(20, Main.rand.Next(120, 240), true);
            }
            if (proj.type == 276)
            {
                if (Main.expertMode)
                {
                    player.AddBuff(20, Main.rand.Next(120, 540), true);
                }
                else if (Main.rand.Next(2) == 0)
                {
                    player.AddBuff(20, Main.rand.Next(180, 420), true);
                }
            }
            if (proj.type == 436 && Main.rand.Next(5) >= 2)
            {
                player.AddBuff(31, 300, true);
            }
            if (proj.type == 435 && Main.rand.Next(3) != 0)
            {
                player.AddBuff(144, 300, true);
            }
            if (proj.type == 682)
            {
                player.AddBuff(196, 300, true);
            }
            if (proj.type == 437)
            {
                player.AddBuff(144, 60 * Main.rand.Next(4, 9), true);
            }
            if (proj.type == 348)
            {
                if (Main.rand.Next(2) == 0)
                {
                    player.AddBuff(46, 600, true);
                }
                else
                {
                    player.AddBuff(46, 300, true);
                }
                if (Main.rand.Next(3) != 0)
                {
                    if (Main.rand.Next(16) == 0)
                    {
                        player.AddBuff(47, 60, true);
                    }
                    else if (Main.rand.Next(12) == 0)
                    {
                        player.AddBuff(47, 40, true);
                    }
                    else if (Main.rand.Next(8) == 0)
                    {
                        player.AddBuff(47, 20, true);
                    }
                }
            }
            if (proj.type == 349)
            {
                if (Main.rand.Next(3) == 0)
                {
                    player.AddBuff(46, 600, true);
                }
                else if (Main.rand.Next(2) == 0)
                {
                    player.AddBuff(46, 300, true);
                }
            }
            if (proj.type >= 399 && proj.type <= 402)
            {
                player.AddBuff(24, 60 * Main.rand.Next(3, 7), false);
            }
            if (proj.type == 55)
            {
                if (Main.rand.Next(3) == 0)
                {
                    player.AddBuff(20, 600, true);
                }
                else if (Main.expertMode)
                {
                    player.AddBuff(20, Main.rand.Next(60, 300), true);
                }
            }
            if (proj.type == 44 && Main.rand.Next(3) == 0)
            {
                player.AddBuff(22, 900, true);
            }
            if (proj.type == 293)
            {
                player.AddBuff(80, 60 * Main.rand.Next(2, 7), true);
            }
            if (proj.type == 82 && Main.rand.Next(3) == 0)
            {
                player.AddBuff(24, 420, true);
            }
            if (proj.type == 285)
            {
                if (Main.rand.Next(3) == 0)
                {
                    player.AddBuff(31, 180, true);
                }
                else
                {
                    player.AddBuff(31, 60, true);
                }
            }
            if (proj.type == 96 || proj.type == 101)
            {
                if (Main.rand.Next(6) == 0)
                {
                    player.AddBuff(39, 480, true);
                }
                else if (Main.rand.Next(4) == 0)
                {
                    player.AddBuff(39, 300, true);
                }
                else if (Main.rand.Next(2) == 0)
                {
                    player.AddBuff(39, 180, true);
                }
            }
            else if (proj.type == 288)
            {
                player.AddBuff(69, 900, true);
            }
            else if (proj.type == 253 && Main.rand.Next(2) == 0)
            {
                player.AddBuff(44, 600, true);
            }
            if (proj.type == 291 || proj.type == 292)
            {
                player.AddBuff(24, 60 * Main.rand.Next(8, 16), true);
            }
            if (proj.type == 98)
            {
                player.AddBuff(20, 600, true);
            }
            if (proj.type == 184)
            {
                player.AddBuff(20, 900, true);
            }
            if (proj.type == 290)
            {
                player.AddBuff(32, 60 * Main.rand.Next(5, 16), true);
            }
            if (proj.type == 174)
            {
                player.AddBuff(46, 1200, true);
                if (!player.frozen && Main.rand.Next(20) == 0)
                {
                    player.AddBuff(47, 90, true);
                }
                else if (!player.frozen && Main.expertMode && Main.rand.Next(20) == 0)
                {
                    player.AddBuff(47, 60, true);
                }
            }
            if (proj.type == 257)
            {
                player.AddBuff(46, 2700, true);
                if (!player.frozen && Main.rand.Next(5) == 0)
                {
                    player.AddBuff(47, 60, true);
                }
            }
            if (proj.type == 177)
            {
                player.AddBuff(46, 1500, true);
                if (!player.frozen && Main.rand.Next(10) == 0)
                {
                    player.AddBuff(47, Main.rand.Next(30, 120), true);
                }
            }
            if (proj.type == 176)
            {
                if (Main.rand.Next(4) == 0)
                {
                    player.AddBuff(20, 1200, true);
                    return;
                }
                if (Main.rand.Next(2) == 0)
                {
                    player.AddBuff(20, 300, true);
                }
            }
        }

    }
}