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
using RefTheGun.Classes;

namespace RefTheGun.Items
{
	public class Coalescence : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);
		//public List<CoalEff<int>> postshooteffects = new List<CoalEff<int>>(){};
		public List<CoalEffNew<Projectile>> effects = new List<CoalEffNew<Projectile>>(){};//new CoalEff<Projectile>(new NapalmGun().OnHitEffect)
		public int shoot = ProjectileID.Bullet;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coalescence");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 40;
			item.ranged = true;
			item.noMelee = true;
			item.width = 28;
			item.height = 18;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.knockBack = 4;
			item.value = 5000;
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 12f;
			item.autoReuse = true;
			MaxAmmo = 100;
			BloomPerShot = 0f;
			ReloadTimeMax = 60;
			TrackHitEnemies = true;
			storefiredshots = true;

			Ammo = MaxAmmo;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips){
			int i = 0;
			foreach (CoalEffNew<Projectile> e in effects){
				tooltips.Add(new TooltipLine(mod, "Effect"+i, e.name));
				i++;
			}
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
		public override void restoredefaults(){}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-20, -20);
		}
		public Coalescence Getit(){
			return this;
		}
		public void CopyShoot(int s){
			item.shoot = s;
			shoot = s;
		}
		public override void OnHitEffect(Projectile projectile){
			foreach (CoalEffNew<Projectile> i in effects){
				i.InvokeOnHit(projectile);
			}
		}
		/*public override void PostShoot(int p){
			foreach (CoalEff<int> i in postshooteffects){
				i.effect.Invoke(p);
			}
		}*/

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentVortex, 15);
			recipe.AddIngredient(ItemID.SoulofMight, 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			type = shoot;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
