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
using RefTheGun.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.Buffs;

namespace RefTheGun.Items
{
	public class BlazingChakram : RefTheItem
	{
		protected LegacySoundStyle useSound = new LegacySoundStyle(2, 101);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Chakram");
			Tooltip.SetDefault("What did you expect, something from Enter the Gungeon?\nDo not use near sharp objects.");
		}
		public override void SetDefaults()
		{
			item.damage = 105;
			item.melee = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useTurn = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = ItemRarityID.Yellow;
			item.shoot = ProjectileID.LightDisc;//ProjectileID.Hook;
			item.shootSpeed = 12.5f;
			item.UseSound = useSound;
			MaxAmmo = 0;
			reloadwhenfull = true;
		}
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
                item.UseSound = new LegacySoundStyle(2, 7);
				item.reuseDelay = 0;
				item.channel = false;
				for(int i = 0; i < Main.projectile.Length; i++){
					if(Main.projectile[i].owner == player.whoAmI && Main.projectile[i].Name=="Blazing Chakram"){
						Main.player[Main.projectile[i].owner].Teleport(Main.projectile[i].Center);
						Main.projectile[i].Kill();
						return false;
					}
				}
            }else{
				item.useStyle = 5;
				player.GetModPlayer<GunPlayer>(mod).magsize=-2;
				for(int i = 0; i < Main.projectile.Length; i++){
					if(Main.projectile[i].owner == player.whoAmI && Main.projectile[i].Name=="Blazing Chakram" && Main.projectile[i].active){
						player.GetModPlayer<GunPlayer>(mod).magsize=-1;
						return false;
					}
				}
				item.channel = true;
            }
			return base.CanUseItem(player);
        }
		public override void restoredefaults(){
			item.UseSound = useSound;
		}
		public override void OnHitEffect(Projectile projectile){
            if(projectile.Name=="Blazing Chakram"){
				projectile.ai[0] = 0;
				//projectile.velocity = projectile.oldVelocity;
				projectile.GetGlobalProjectile<GunGlobalProjectile>(mod).timesincestop+=5;
				projectile.GetGlobalProjectile<GunGlobalProjectile>(mod).stopping=2;
				Vector2 diff = projectile.oldVelocity - projectile.velocity;
				projectile.velocity -= diff*0.9f;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
            if(player.altFunctionUse == 2)return false;
			type=ProjectileID.LightDisc;
			speedX*=1.5f;
			speedY*=1.5f;
            int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY).RotatedByRandom(MathHelper.ToRadians(Spread)), type, damage, knockBack, item.owner);
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].friendly = true;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 3;
			Main.projectile[proj].Name = "Blazing Chakram";
			Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).effectonhit = true;
			Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = this;
			Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).OverrideTexture = mod.GetTexture("Items/BlazingChakram");
			Main.projectile[proj].extraUpdates++;
			Main.projectile[proj].scale*=2f;
			GunGlobalProjectile.SetDefaults2(ref Main.projectile[proj]);
            return false;
		}
	}
}
