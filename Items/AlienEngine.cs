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

namespace RefTheGun.Items
{
	public class AlienEngine : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alien Engine");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 60;
			item.ranged = true;
			item.noMelee = true;
			item.width = 28;
			item.height = 18;
			item.useTime = 3;
			item.useAnimation = 3;
			item.useStyle = 5;
			item.knockBack = 4;
			item.value = 5000;
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 2f;
			item.useAmmo = AmmoID.Gel;
			item.autoReuse = true;
			MaxAmmo = 100;
			BloomPerShot = 0f;
			ReloadTimeMax = 60;
			TrackHitEnemies = true;
			storefiredshots = true;

			Ammo = MaxAmmo;
		}
		
        /*public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
				item.noUseGraphic = true;
            }else{
				item.useStyle = 5;
				item.noUseGraphic = false;
            }
			return base.CanUseItem(player);
        }*/
		public override void restoredefaults(){
			item.useAmmo = AmmoID.Gel;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}
		/*
		public override void SpecialReloadProj(Player player, int ammoleft){
			int dmg = item.damage;
			GetWeaponDamage(player, ref dmg);
			foreach(NPC Targ in hitenemies){
				//NPC Targ = Main.npc[targ];
				//Projectile.NewProjectileDirect(Proj.position, new Vector2(), ProjectileID.SolarWhipSwordExplosion, Proj.damage*3, Proj.knockBack*2, Proj.owner);
				Projectile a = Projectile.NewProjectileDirect(Targ.Center, new Vector2(), ProjectileID.SolarWhipSwordExplosion, dmg*2, item.knockBack*2, item.owner);
				a.position-=new Vector2(a.width/2,a.height/2);
				a.usesLocalNPCImmunity = true;
				a.scale+=2;
			}
			hitenemies.Clear();
			foreach(int proj in firedshots){
				Projectile Targ = Main.projectile[proj];
				//Projectile.NewProjectileDirect(Proj.position, new Vector2(), ProjectileID.SolarWhipSwordExplosion, Proj.damage*3, Proj.knockBack*2, Proj.owner);
				Projectile a = Projectile.NewProjectileDirect(Targ.Center, new Vector2(), ProjectileID.SolarWhipSwordExplosion, dmg*2, item.knockBack*2, item.owner);
				a.position-=new Vector2(a.width/2,a.height/2);
				a.usesLocalNPCImmunity = true;
				a.scale+=2;
				Targ.Kill();
			}
			firedshots.Clear();
		}*/

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedRepeater, 1);
			recipe.AddIngredient(ItemID.Grenade, 2);
			recipe.AddIngredient(ItemID.SoulofMight, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2)return false;
			Vector2 Vel = new Vector2(speedX, speedY);
			//player.velocity += (Vel.Normalized()*(float)Math.Min(Vel.Length(),((player.velocity+Vel).Length())-12.5));
			player.velocity = ((player.velocity-Vel).Normalized())*(float)Math.Min((player.velocity-Vel).Length(),750);
			type = ModContent.ProjectileType<Combustion>();
			float rot = (float)Math.Atan2((double)speedY, (double)speedX) + 1.57f;
            int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY), type, damage, knockBack, item.owner);
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].friendly = true;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 3;
			Main.projectile[proj].rotation = rot;
			Main.projectile[proj].Center = player.MountedCenter+(Vel*(6.25f))+(Vel.Normalized()*(28));
			Ammo--;
			return false;
			//return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
