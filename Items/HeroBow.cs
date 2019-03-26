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

namespace RefTheGun.Items{
	public class HeroBow : RefTheItem{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);
		//hummingbirds are the sharks of the sky - Jardon
		int charge = 0;
		int lastcharge = 0;
		int mode = 0;
		int timesinceswitch = 0;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Old Hero's Bow");
			Tooltip.SetDefault("Magic damage boosts are applied at 25% effectiveness.");
		}
		public override void SetDefaults(){
			item.damage = 21;
			item.ranged = true;
			item.noMelee = true;
			item.width = 28;
			item.height = 50;
			item.useTime = 1;
			item.useAnimation = 7;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Green;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 12.5f;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = null;
			item.autoReuse = false;
			MaxAmmo = 0;
			MaxSpread = 4.5f;
			MinSpread = 0f;
			BloomPerShot = 0f;
			SpreadLossSpeed = 0.1f;
			ReloadTimeMax = 30;
			TrackHitEnemies = true;
			specialreloadproj = true;

			Ammo = MaxAmmo;
		}
		public override void HoldItem(Player player){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			modPlayer.roundsinmag = Ammo;
			modPlayer.magsize = /*(int)(*/MaxAmmo/**Main.player[item.owner].GetModPlayer<GunPlayer>(mod).MagMultiply)*/;
			modPlayer.guninfo = GetArrowName(mode);
			if(!player.controlUseTile&&timesinceswitch>0)timesinceswitch--;
            if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded){
				Ammo = MaxAmmo;
                restoredefaults();
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded = false;
            }
		}
        public override void HoldStyle(Player player){
            Spread = MaxSpread;
        }
		public override bool ConsumeAmmo(Player player){
			return false;
		}
		string GetArrowName(int type){
			switch (type){
				case 1:
				return "Fire Arrow";
				case 2:
				return "Ice Arrow";
				case 3:
				return "Light Arrow";
				default:
				return "Arrow";
			}
		}
		void Reload(){
			Player player = Main.player[item.owner];
			if(!Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading&&!Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded){
                if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading)return;
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading = true;
                int proj = Projectile.NewProjectile(player.Center, new Vector2(), mod.ProjectileType("ReloadProj"), 0, 0, player.whoAmI, (int)(ReloadTimeMax*reloadmult), player.selectedItem);
                Main.projectile[proj].timeLeft = Math.Max((int)(ReloadTimeMax*reloadmult),2);
			}
		}
        public override bool CanUseItem(Player player){
			if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading)return false;
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
				item.noUseGraphic = true;
				if(timesinceswitch==0)ToggleMode();
				timesinceswitch = 2;
				item.mana = 0;
            }else{
				item.useStyle = 5;
				item.noUseGraphic = false;
				if(mode>0){
					item.mana = 25*(int)Math.Pow(2, mode-1);
				}else{
					item.mana = 0;
				}
				
				switch (mode){
				case 1:
				Lighting.AddLight(item.Center, Color.OrangeRed.ToVector3()*0.4f);
				break;
				case 2:
				Lighting.AddLight(item.Center, Color.LightCyan.ToVector3()*0.4f);
				break;
				case 3:
				Lighting.AddLight(item.Center, Color.Wheat.ToVector3()*0.4f);
				break;
				default:
				break;
				}
            }
			return true;
        }
		public override void restoredefaults(){
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = null;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}
		void ToggleMode(){
			int maxmode = 0;
			if(NPC.downedBoss1)maxmode++;
			if(Main.hardMode)maxmode++;
			if(NPC.downedMechBoss1||NPC.downedMechBoss2||NPC.downedMechBoss3)maxmode++;
			if (++mode>maxmode)mode=0;
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenBow, 1);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostShoot(int p){
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0] = mode;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().aioverflow[1] = lastcharge;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
			Main.projectile[p].extraUpdates++;
			Main.projectile[p].extraUpdates+=(lastcharge/60);
			Main.projectile[p].Name = GetArrowName(mode);
			switch (mode){
				case 1:
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.OrangeRed*0.4f;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = false;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().Color = Color.OrangeRed;
				break;
				case 2:
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.LightCyan*0.4f;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = false;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().Color = Color.LightCyan;
				break;
				case 3:
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.Wheat*0.4f;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = false;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().Color = Color.Wheat;
				break;
				default:
				break;
			}
		}
		public override void OnHitEffect(Projectile projectile, NPC target){
			switch ((int)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]){
				case 1:
				Projectile.NewProjectileDirect(projectile.Center, projectile.velocity/2, mod.ProjectileType<Combustion>(), projectile.damage, 0, projectile.owner).rotation = (float)Math.Atan2((double)projectile.velocity.X, (double)projectile.velocity.Y);
				target.AddBuff(BuffID.OnFire, (int)(600*projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[1]/45));
				break;
				case 2:
				target.AddBuff(BuffID.Frozen, (int)((target.boss?60:6400)*projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[1]/45));
				break;
				case 3:
				if(projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[1]>=30){
					float c = Main.rand.Next(5,7);
					for(float i = 0; i < c; i++){
						Projectile.NewProjectileDirect(projectile.Center-new Vector2(128,0).RotatedBy((i/c)*6.28319), new Vector2(projectile.velocity.Length(),0).RotatedBy((i/c)*6.28319), ProjectileID.HolyArrow, projectile.damage/3, 0, projectile.owner).tileCollide = false;
					}
				}
				break;
				default:
				break;
			}
		}
		public override void GetWeaponDamage(Player player, ref int damage){
			damage = (int)((damage*player.rangedDamage)*(1+((player.rangedDamage-1)*0.25f)));
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2||player.GetModPlayer<GunPlayer>().Reloading)return false;
			if(player.itemAnimation>2){
				if(player.controlUseItem){
					player.itemAnimation = item.useAnimation-1;
					if(Spread>0){
						Spread = Math.Max(Spread-SpreadLossSpeed, MinSpread);
						if(Spread<=0)for(int i = 0; i < 7; i++){
							int a = Dust.NewDust(position+new Vector2(speedX,speedY), 0, 0, DustID.GoldFlame);
							Main.dust[a].noGravity = true;
							Main.dust[a].noLight = true;
						}
					}
					if(charge<45){
						charge++;
						if(charge>=45)for(int i = 0; i < 15; i++){
							int a = Dust.NewDust(position+new Vector2(speedX,speedY), 0, 0, DustID.GoldFlame);
							Main.dust[a].noGravity = true;
							Main.dust[a].noLight = true;
						}
					}else if(Main.time%12<=1){
						int a = Dust.NewDust(position+new Vector2(speedX,speedY), 0, 0, DustID.GoldFlame);
						Main.dust[a].noGravity = true;
						Main.dust[a].noLight = true;
					}
				}
				return false;
			}else{
				damage=(int)((charge/9f)*damage);
				speedX*=(charge/45)+1;
				speedY*=(charge/45)+1;
				lastcharge = (int)charge;
				charge = 0;
				Main.PlaySound(useSound, position);
				player.itemAnimation = 0;
				Reload();
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
			Player player = Main.player[item.owner];
			//float scale = 0.7f;
			try{
			if(mod.TextureExists("Items/HeroBow"))spriteBatch.Draw(mod.GetTexture("Items/HeroBow"), position/*+new Vector2(20,17)*/, new Rectangle(0,0,28,50), drawColor, 0, new Vector2(), scale, SpriteEffects.None, 0);
			}
			catch (System.Exception){
				return true;
			}
			return false;
		}
	}
}
