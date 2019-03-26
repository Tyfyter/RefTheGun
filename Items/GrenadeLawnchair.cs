
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
	public class GrenadeLawnchair : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 61);
		int time = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grenade Lawnchair");
			Tooltip.SetDefault(@"Bone apple tea");
		}
		public override void SetDefaults()
		{
			item.damage = 17;
			item.ranged = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.width = 28;
			item.height = 18;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 5;
			item.knockBack = 4;
			item.value = 5000;
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ProjectileID.GrenadeI;
			item.shootSpeed = 8.5f;
			item.useAmmo = AmmoID.Rocket;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 60;
			Spread = MaxSpread = 3f;
			MinSpread = 1.75f;
			SpreadLossSpeed = -0.1f;
			BloomPerShot = -0.05f;
			Multishot = 3;
			ReloadTimeMax = 60;

			Ammo = MaxAmmo;
		}
		public override void UpdateInventory(Player player){
			time = Math.Max(time-1,0);
			base.UpdateInventory(player);
		}
		public float SpeedMultiplier(Player player){
			return (4-Spread)/(player.GetModPlayer<GunPlayer>(mod).bullet20Slow?1f:1.75f);
		}
        public override bool CanUseItem(Player player){
			ReloadTimeMax = (player.GetModPlayer<GunPlayer>(mod).bullet20Slow?25:60);
            if(player.altFunctionUse == 2){
				item.UseSound = null;
				item.useStyle = 1;
            }else{
				item.UseSound = useSound;
				item.useStyle = 5;
            }
			return base.CanUseItem(player);
        }
		public override void restoredefaults(){
			item.useAmmo = AmmoID.Rocket;
			item.UseSound = null;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GrenadeLauncher, 1);
			recipe.AddIngredient(ItemID.LihzahrdChair, 1);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
            GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			if(modPlayer.bullet20Slow)type = 20;
            if(!modPlayer.Chairesy){
                Projectile.NewProjectile(position, new Vector2(speedX, speedY), mod.ProjectileType<DrawChair>(), damage, knockBack, player.whoAmI);
			}
            if(player.controlUseItem){
				modPlayer.channelsword = 5;
            	if(time<=0){
					time = (int)(12/SpeedMultiplier(player));
					Main.PlaySound(useSound, player.Center);
					damage = (int)(damage/1.5f);
					return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
				}
			}
			return false;
        }
	}
}
