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

namespace RefTheGun.Items
{
	public class Nailgun : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 89);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nailgun");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 30;
			item.ranged = true;
			item.noMelee = true;
			item.width = 40;
			item.height = 34;
			item.useTime = 1;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.LightRed;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 12.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 80;
			MaxSpread = 5f;
			MinSpread = 0.5f;
			BloomPerShot = 1f;
			SpreadLossSpeed = 0.05f;
			ReloadTimeMax = 80;
			storefiredshots = true;

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
				item.noUseGraphic = (player.GetModPlayer<GunPlayer>().bulletPoisonChance>0);
            }
			return base.CanUseItem(player);
        }
		public override void restoredefaults(){
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = null;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(0, -2);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.VenusMagnum, 1);
			recipe.AddIngredient(ItemID.FragmentVortex, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2||player.GetModPlayer<GunPlayer>().Reloading)return false;
            /*if(player.GetModPlayer<GunPlayer>().bulletPoisonChance>0&&!firedshots.Exists(hasActiveDraw))*/firedshots.Add(Projectile.NewProjectile(position, new Vector2(), mod.ProjectileType<DrawNailgun>(), damage, knockBack, item.owner));
			if(player.itemAnimation==item.useAnimation-1){
				type = ProjectileID.NailFriendly;
				Main.PlaySound(useSound, position);
			}else{
				if(!player.controlUseItem)player.itemAnimation = 0;
				return false;
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
			Player player = Main.player[item.owner];
			//float scale = 0.7f;
			try{
				if (player.GetModPlayer<GunPlayer>().bulletPoisonChance>0){
					if(mod.TextureExists("Items/Nailgun_Fallout"))spriteBatch.Draw(mod.GetTexture("Items/Nailgun_Fallout"), position/*+new Vector2(25,17)*/, new Rectangle(0,0,50,36), drawColor, 0, new Vector2(), scale, SpriteEffects.None, 0);
				}else if(mod.TextureExists("Items/Nailgun"))spriteBatch.Draw(mod.GetTexture("Items/Nailgun"), position/*+new Vector2(20,17)*/, new Rectangle(0,0,40,34), drawColor, 0, new Vector2(), scale, SpriteEffects.None, 0);
			}
			catch (System.Exception){
				return true;
			}
			return false;
		}
		public bool hasActiveDraw(int i){
			if(Main.projectile[i].type==mod.ProjectileType<DrawNailgun>()&&Main.projectile[i].active)return true;
			return false;
		}
	}
}
