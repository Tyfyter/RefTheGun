using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Text;
using System.Reflection;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.Projectiles;

namespace RefTheGun.Items
{
	public class GolemHeart : ModItem
	{
        public override bool CloneNewInstances => true;
        int time = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart of a Golem");
			Tooltip.SetDefault(@"It's not a small bell, it doesn't ring.");
		}
		public override void SetDefaults()
		{
            item.CloneDefaults(ItemID.GolemTrophy);
            item.consumable = false;
            item.createTile = 0;
            item.placeStyle = 0;
			item.damage = 60;
			item.magic = true;
			item.knockBack = 6;
			item.value = 3500;
            item.accessory = true;
		}
        
        public override void UpdateAccessory(Player player, bool hideVisual){
            if(!Array.Exists(player.hurtCooldowns, EmptyHurtness))if(time++>=120){
                int damage = item.damage;
                GetWeaponDamage(player, ref damage);
                Projectile a = Projectile.NewProjectileDirect(player.Center, new Vector2(), mod.ProjectileType<WrathoftheTitans>(), damage, 0, item.owner, 3, 46);
                a.timeLeft = 4;
                a.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures=true;
                //Main.PlaySound(new LegacySoundStyle(2, 35).WithVolume(0.1f).WithPitchVariance(0.1f), player.Center);
                time = 0;
            }
        }
        public static bool EmptyHurtness(int i){
            return i>0;
        }
	}
}
