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
	public class NapalmGun : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightwatch Ogris");
			Tooltip.SetDefault("I think the Grineer might have messed this one up.");
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
			item.rare = ItemRarityID.Yellow;
			item.shoot = ProjectileID.RocketI;
			item.shootSpeed = 8.5f;
			item.useAmmo = AmmoID.Rocket;
			item.UseSound = useSound;
			item.autoReuse = false;
			MaxAmmo = 6;
			BloomPerShot = 0.1f;
			SpreadLossSpeed = 0.1f;
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
			item.useAmmo = AmmoID.Rocket;
			item.UseSound = useSound;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RocketLauncher, 1);
			recipe.AddIngredient(ItemID.InfernoFork, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void OnHitEffect(Projectile projectile){
			Projectile a = Projectile.NewProjectileDirect(projectile.Center, new Vector2(), mod.ProjectileType<WrathoftheTitans>(), (int)(projectile.damage/1.5), 0, projectile.owner, ai1:(projectile.type==ProjectileID.RocketI||projectile.type==ProjectileID.RocketII)?2:3);
			a.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]=(projectile.type==ProjectileID.RocketI||projectile.type==ProjectileID.RocketII)?2:3;
			a.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[1]=(projectile.type==ProjectileID.RocketI||projectile.type==ProjectileID.RocketII)?1:1.5f;
			a.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[2]=DustID.AmberBolt;
			a.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[3]=1;
			a.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[5]=-0.0625f;
			a.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures=true;
			projectile.GetGlobalProjectile<GunGlobalProjectile>().effectonhit=false;
			a.timeLeft=(int)(300*(projectile.type==ProjectileID.RocketI||projectile.type==ProjectileID.RocketII?1:2));
		}
		public override void PostShoot(int p){
            Main.projectile[p].usesLocalNPCImmunity = true;
            Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>(mod).effectonhit = true;
            Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = (RefTheItem)item.modItem;
		}
	}
}
