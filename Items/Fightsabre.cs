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
	public class Fightsabre : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 12);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fightsabre");
			Tooltip.SetDefault(@"Reload to reflect nearby bullets.
An ancient weapon, composed entirely of hardened light.");
		}
		public override void SetDefaults()
		{
			item.damage = 50;
			item.ranged = true;
			item.noMelee = true;
			item.width = 40;
			item.height = 18;
			item.useTime = 7;
			item.useAnimation = 7;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 50000;
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ProjectileID.GreenLaser;
			item.shootSpeed = 7.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 25;
			MaxSpread = 6.5f;
			BloomPerShot = 2.35f;
			SpreadLossSpeed = 0.2f;

			specialreloadproj = true;
			Ammo = MaxAmmo;
		}
		
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
				item.useAmmo = 0;
				item.shoot = mod.ProjectileType("FightsabreSlash");
				item.useStyle = 1;
				item.noUseGraphic = true;
            }else{
				item.useAmmo = AmmoID.Bullet;
				item.shoot = ProjectileID.GreenLaser;
				if(Ammo<=0)item.shoot = mod.ProjectileType("FightsabreSlash");
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
			return new Vector2(-6, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			int shoot =  type == ProjectileID.Bullet ? item.shoot : type;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref shoot, ref damage, ref knockBack);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GreenPhasesaber, 1);
			recipe.AddIngredient(ItemID.Uzi, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 10);
			recipe.AddIngredient(ItemID.SoulofSight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
