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

namespace RefTheGun.Items
{
	public class Crossboom : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crossboom");
			Tooltip.SetDefault(@"Did I just out-pun the Enter the Gungeon devs?");
		}
		public override void SetDefaults()
		{
			item.damage = 60;
			item.ranged = true;
			item.noMelee = true;
			item.width = 28;
			item.height = 18;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 4;
			item.value = 5000;
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 8.5f;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = useSound;
			item.autoReuse = false;
			MaxAmmo = 6;
			BloomPerShot = 0f;
			ReloadTimeMax = 60;
			TrackHitEnemies = true;
			storefiredshots = true;

			Ammo = MaxAmmo;
		}
		
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
				item.noUseGraphic = true;
            }else{
				item.useStyle = 5;
				item.noUseGraphic = false;
            }
			return base.CanUseItem(player);
        }
		public override void restoredefaults(){
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = useSound;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}
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
		}

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
			//Vector2 Vel = new Vector2(speedX, speedY).Normalized().RotatedBy(90*player.direction)*3;
			//position-=Vel;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
