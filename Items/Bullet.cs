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
	public class Bullet : RefTheItem
	{
		public static LegacySoundStyle useSound = new LegacySoundStyle(2, 89);
		int bullettype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet");
			Tooltip.SetDefault(@"");
		}
		public override void SetDefaults()
		{
			item.damage = 60;
			item.ranged = true;
			item.noMelee = true;
			item.width = 32;
			item.height = 16;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.LightRed;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 10.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 6;
			MaxSpread = 0f;
			BloomPerShot = 0f;
			ReloadTimeMax = 60;

			Ammo = MaxAmmo;
		}

		public override bool ConsumeAmmo(Player player){
			return false;
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
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EmptyBullet, 5);
			recipe.AddIngredient(ItemID.SoulofMight, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void PostShoot(int p){
			Main.projectile[p].ai[0] = bullettype;
			Main.projectile[p].ai[1] = 9;
            Main.projectile[p].rotation = Main.projectile[p].velocity.RotatedByRandom(0.1f).ToRotation();
			if(bullettype != ProjectileID.ChlorophyteBullet){
                ((GunProj)Main.projectile[p].modProjectile).aiType = bullettype;
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			bullettype = type;
			type = ModContent.ProjectileType<GunProj>();
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
