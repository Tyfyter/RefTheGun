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
	public class Reverence_Single : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 89);
		public override bool Autoload(ref string name){
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Better Half");
			Tooltip.SetDefault(@"");
		}
		public override void SetDefaults()
		{
			item.damage = 60;
			item.ranged = true;
			item.noMelee = true;
			item.width = 28;
			item.height = 18;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.crit = 21;
			item.rare = ItemRarityID.Orange;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 12.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 1;
			MaxSpread = 0f;
			MinSpread = 0f;
			BloomPerShot = 0f;
			SpreadLossSpeed = 0.05f;
			ReloadTimeMax = 20;

			Ammo = MaxAmmo;
		}

		public override bool ConsumeAmmo(Player player){
			return false;
		}
		
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
				item.noUseGraphic = true;
				ReloadTimeMax = 4;
            }else{
				item.useStyle = 5;
				item.noUseGraphic = false;
				ReloadTimeMax = 40;
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
			recipe.AddIngredient(ItemID.PhoenixBlaster, 1);
			recipe.AddIngredient(ItemID.BloodWater, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
