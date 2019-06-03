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
	public class Crossbow : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crossbow");
			Tooltip.SetDefault(@"");
		}
		public override void SetDefaults()
		{
			item.damage = 45;
			item.ranged = true;
			item.noMelee = true;
			item.width = 52;
			item.height = 20;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 9.5f;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = useSound;
			item.autoReuse = false;
			MaxAmmo = 1;
			MaxSpread = 3f;
			MinSpread = 0.2f;
			BloomPerShot = 1.35f;
			SpreadLossSpeed = 0.05f;
			ReloadTimeMax = 50;
			specialreloadproj = true;
			instantreload = true;

			Ammo = MaxAmmo;
		}

		public override bool ConsumeAmmo(Player player){
			return Ammo>0;
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

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenBow, 1);
			recipe.AddRecipeGroup("Wood", 10);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(Ammo>0)return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			return false;
		}
	}
}
