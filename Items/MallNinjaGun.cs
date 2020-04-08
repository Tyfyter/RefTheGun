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
using RefTheGun.Buffs;

namespace RefTheGun.Items
{
	public class MallNinjaGun : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 89);
		int ammotime = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("\"Tactical\" Pistol");
			Tooltip.SetDefault("Seems like it would be ineffective.");
		}
		public override void SetDefaults()
		{
			item.damage = 40;
			item.ranged = true;
			item.noMelee = true;
			item.width = 58;
			item.height = 42;
			item.useTime = 1;
			item.useAnimation = 17;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.LightRed;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 12.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = null;
			item.autoReuse = false;
			MaxAmmo = 16;
			MaxSpread = 5f;
			MinSpread = 0.5f;
			BloomPerShot = 1f;
			SpreadLossSpeed = 0.05f;
			ReloadTimeMax = 16;
			specialreloadproj = true;

			Ammo = MaxAmmo;
		}
		public override bool ConsumeAmmo(Player player){
			return false;
		}
		
        public override bool CanUseItem(Player player){
			item.useStyle = 5;
            if(player.altFunctionUse == 2){
				item.noUseGraphic = true;
				item.autoReuse = false;
				return !Main.player[item.owner].GetModPlayer<GunPlayer>().Reloading;
            }
			item.noUseGraphic = false;
			if(player.altFunctionUse == 1)item.autoReuse = false;
			restoredefaults();
			return (Ammo > 0 || MaxAmmo <= 0) && !Main.player[item.owner].GetModPlayer<GunPlayer>().Reloading;
        }
		public override void restoredefaults(){
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = null;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Revolver, 1);
			recipe.AddIngredient(ItemID.SilverShortsword, 1);
			recipe.AddIngredient(ItemID.Actuator, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Revolver, 1);
			recipe.AddIngredient(ItemID.TungstenShortsword, 1);
			recipe.AddIngredient(ItemID.Actuator, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostShoot(int p){
			if(Main.projectile[p].type!=ModContent.ProjectileType<MallNinjaStab>()){
				ammotime = Ammo==0?250:300;
			}else{
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().firedwith = (RefTheItem)item.modItem;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
			}
		}
		public override void HoldStyle(Player player){
			base.HoldStyle(player);
			if(ammotime>0){
				ammotime--;
			}else if(ammotime==0){
				((GunPlayer)player).guninfo = "reload!";
				Color c = Main.DiscoColor;
				c = new Color(c.R,Math.Min(c.G,c.R/2),Math.Min(c.B,c.R/8));
				((GunPlayer)player).guninfocolor = new Color(c.R,c.G,c.B,(int)((c.R+c.G+c.B)/1.5f));
			}
		}
		public override void OnHitEffect(Projectile projectile, NPC target){
			if(!((GunPlayer)Main.player[item.owner]).Reloading)Reload();
		}
		public override void ReloadFinishHook(Player player, int ammoleft){
			ammotime = -1;
			player.AddBuff(ModContent.BuffType<ReloadStabBuff>(), 60);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.GetModPlayer<GunPlayer>().Reloading)return false;
			if(player.itemAnimation==item.useAnimation-1){
				if(player.altFunctionUse==2){
					type = ModContent.ProjectileType<MallNinjaStab>();
					base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
					if(ammotime==0){
						Reload();
						ammotime = -1;
					}
					return false;
				}
				Main.PlaySound(useSound, position);
			}else{
				if(!player.controlUseItem&&player.itemAnimation<13)player.itemAnimation = 0;
				return false;
			}
			if(player.HasBuff(ModContent.BuffType<ReloadStabBuff>())){
				damage = (int)(damage*2.45f);
				knockBack*=5;
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
			Player player = Main.player[item.owner];
			//float scale = 0.7f;
			try{
				if(mod.TextureExists("Items/MallNinjaGun"))spriteBatch.Draw(mod.GetTexture("Items/MallNinjaGun"), position/*+new Vector2(20,17)*/, new Rectangle(0,0,58,42), drawColor, 0, new Vector2(), scale, SpriteEffects.FlipHorizontally, 0);
			}
			catch (System.Exception){
				return true;
			}
			return false;
		}
	}
}
