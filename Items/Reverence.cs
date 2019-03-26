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
	public class Reverence : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 89);
		bool poorlyTimed = false;
		bool boosted = false;
		float boostglowfade = 0;
		public override string Texture => "RefTheGun/Items/Reverence_Single";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reverence");
			Tooltip.SetDefault(@"Blessed by Kaliber");
		}
		public override void SetDefaults()
		{
			item.damage = 60;
			item.ranged = true;
			item.noMelee = true;
			item.width = 30;
			item.height = 32;
			item.useTime = 1;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Cyan;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 12.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 2;
			MaxSpread = 0f;
			MinSpread = 0f;
			BloomPerShot = 0f;
			SpreadLossSpeed = 0.05f;
			ReloadTimeMax = 35;
			TrackHitEnemies = true;

			Ammo = MaxAmmo;
		}
		public override void GetWeaponDamage(Player player, ref int damage){
			if(boosted)damage=(int)(damage/0.4f);
		}
		public override void GetWeaponCrit(Player player, ref int crit){
			if(boosted)crit+=100;
		}
		public override void HoldItem(Player player){
			if(boosted)boostglowfade = 1;
			if(player.GetModPlayer<GunPlayer>().Reloading)boosted = false;
			if(!boosted&&boostglowfade>0)boostglowfade-=0.02f;
			if(boostglowfade>0)Lighting.AddLight(player.Center, boostglowfade, 0, 0);
			base.HoldItem(player);
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
			item.UseSound = null;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType<Reverence_Single>(), 1);
			recipe.AddIngredient(ItemID.VenusMagnum, 1);
			recipe.AddIngredient(ItemID.FragmentVortex, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostShoot(int p){
			Main.projectile[p].extraUpdates++;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().effectreqs = 1;
			if(boosted){
				Main.projectile[p].extraUpdates+=2;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.Red*0.4f;
			}
		}
		public override void OnHitEffect(Projectile projectile){
			if(boostglowfade>0)Projectile.NewProjectile(projectile.Center, new Vector2(), ProjectileID.VampireHeal, projectile.damage, 0, projectile.owner, projectile.owner, projectile.damage/7);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2||player.GetModPlayer<GunPlayer>().Reloading)return false;
			if(player.itemAnimation==11||player.itemAnimation==14){
				Main.PlaySound(useSound, position);
				if(player.controlUseTile){
					poorlyTimed = true;
				}
			}else if(player.itemAnimation>1&&player.itemAnimation<11){
				if(player.controlUseTile&&!poorlyTimed){
					for(int i = 0; i < 6; i++){
						//DustID.PortalBoltTrail DustID.FlameBurst
						Dust.NewDustDirect(position, 0, 0, DustID.PortalBoltTrail, -speedX/0.8f, -speedY/0.8f, 0, Color.Red, 1.33f).noGravity = true;
						boosted = true;
						player.GetModPlayer<GunPlayer>().Reloaded = true;
					}
				}
				return false;
			}else{
				if((player.itemAnimation>=12||player.itemAnimation<=13)&&player.controlUseTile){
					poorlyTimed = false;
				}else{
					poorlyTimed = false;
				}
				return false;
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
			Player player = Main.player[item.owner];
			try{
			if(mod.TextureExists("Items/Reverence"))spriteBatch.Draw(mod.GetTexture("Items/Reverence"), position+new Vector2(4,-2), new Rectangle(0,0,30,32), drawColor, 0, new Vector2(), item.scale*(player.HeldItem.type==item.type?0.75f:0.6f), SpriteEffects.None, 0);
			if(boostglowfade>0){
				if(mod.TextureExists("Items/Reverence_Glow"))spriteBatch.Draw(mod.GetTexture("Items/Reverence_Glow"), position+new Vector2(4,-2), new Rectangle(0,0,30,32), new Color(255*boostglowfade,255*boostglowfade,255*boostglowfade,100*boostglowfade), 0, new Vector2(), item.scale*(player.HeldItem.type==item.type?0.75f:0.6f), SpriteEffects.None, 0);
			}else{
				if(mod.TextureExists("Items/Reverence_NoGlow"))spriteBatch.Draw(mod.GetTexture("Items/Reverence_NoGlow"), position+new Vector2(4,-2), new Rectangle(0,0,30,32), new Color(255,255,255), 0, new Vector2(), item.scale*(player.HeldItem.type==item.type?0.75f:0.6f), SpriteEffects.None, 0);
			}
			}
			catch (System.Exception){
				return true;
			}
			return false;
		}
	}
}
