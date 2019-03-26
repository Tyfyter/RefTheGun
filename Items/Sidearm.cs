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
	public class Sidearm : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 89);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marine Issue");
			Tooltip.SetDefault(@"Always With You
Though this gun appears sturdy, it has been known to fail when it is most needed.");
		}
		public override void SetDefaults()
		{
			item.damage = 40;
			item.ranged = true;
			item.noMelee = true;
			item.width = 28;
			item.height = 18;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 6.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 10;
			MaxSpread = 3f;
			MinSpread = 0.2f;
			BloomPerShot = 1.35f;
			SpreadLossSpeed = 0.05f;
			ReloadTimeMax = 72;

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
			recipe.AddIngredient(ItemID.Handgun, 1);
			recipe.AddRecipeGroup("RefTheGun:EvilBars", 10);
			recipe.AddRecipeGroup("IronBar", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			Vector2 Vel = new Vector2(speedX, speedY).Normalized().RotatedBy(90*player.direction)*3;
			position-=Vel;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
