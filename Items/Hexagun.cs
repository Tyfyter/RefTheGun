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
using static RefTheGun.RefTheExtensions;
using RefTheGun.Buffs;

namespace RefTheGun.Items
{
	public class Hexagun : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 67);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hexagun");
			Tooltip.SetDefault(@"1d10");
		}
		public override void SetDefaults()
		{
			item.damage = 67;
			item.ranged = true;
			item.noMelee = true;
			item.width = 52;
			item.height = 20;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Red;
			item.shoot = ProjectileID.ShadowBeamFriendly;
			item.shootSpeed = 9.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = false;
			MaxAmmo = 1;
			MaxSpread = 0f;
			MinSpread = 0f;
			BloomPerShot = 0f;
			SpreadLossSpeed = 0f;
			ReloadTimeMax = 31;
			TrackHitEnemies = true;
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

		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShadowbeamStaff, 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Hexagun>(), 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(ItemID.ShadowbeamStaff);
			recipe.AddRecipe();
		}
		public override void PostShoot(int p){
			rng = Main.rand.Next(0,max);
			Main.projectile[p].ai[1] = 3;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0] = rng;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().destroyonbounce = true;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().bounce = true;
			switch ((int)Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]){
				case 3:
				Main.projectile[p].aiStyle = 297;
				Main.projectile[p].damage*=4;
				Main.projectile[p].tileCollide = false;
				break;
				default:
				break;
			}
			//Main.NewText(Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]);
			Main.player[item.owner].chatOverhead.NewMessage(((int)Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]+1).ToString(), 30);
		}
		int rng = 0;
		int max = 10;//20;
		public override void OnHitEffect(Projectile projectile){
			switch ((int)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]){
				case 2:
				int a = Projectile.NewProjectile(projectile.position, new Vector2(), ModContent.ProjectileType<WrathoftheTitans>(), 85, 0, projectile.owner);
				//Main.projectile[a].SetDefaults(Main.projectile[a].type);
				Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().aioverflow.MergeIn(new List<TrueNullable<float>>(){null,null,null,null,0,0.15f,1});
				Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = true;
				Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.White;
				Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().Color = Color.White;
				Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures = true;
				Main.projectile[a].Name = "Blank";
				break;
				case 3:
				int b = Projectile.NewProjectile(projectile.position, new Vector2(), ModContent.ProjectileType<WrathoftheTitans>(), 0, 0, projectile.owner);
				//Main.projectile[b].SetDefaults(Main.projectile[b].type);
				Main.projectile[b].GetGlobalProjectile<GunGlobalProjectile>().aioverflow.MergeIn(new List<TrueNullable<float>>(){0,4,182,0.9f,0,0,0});
				Main.projectile[b].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = true;
				Main.projectile[b].GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.Red;
				Main.projectile[b].GetGlobalProjectile<GunGlobalProjectile>().Color = Color.Red;
				Main.projectile[b].GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures = true;
				break;
				case 4:
				if(projectile.timeLeft>0){
					projectile.timeLeft = 0;
					break;
				}
				int c = Projectile.NewProjectile(projectile.position, new Vector2(), ModContent.ProjectileType<WrathoftheTitans>(), 46, 0, projectile.owner);
				//Main.projectile[b].SetDefaults(Main.projectile[b].type);
				Main.projectile[c].GetGlobalProjectile<GunGlobalProjectile>().aioverflow.MergeIn(new List<TrueNullable<float>>(){null,0.85f,264,0.1f,0,0.2f,0});
				Main.projectile[c].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = true;
				Main.projectile[c].GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.LightSkyBlue;
				Main.projectile[c].GetGlobalProjectile<GunGlobalProjectile>().Color = Color.LightSkyBlue;
				Main.projectile[c].GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures = true;
				Main.projectile[c].timeLeft*=100;
				break;
				case 7:
				Projectile.NewProjectileDirect(projectile.position, projectile.velocity*3, ProjectileID.ChlorophyteBullet, projectile.damage/4, 3, projectile.owner).GetGlobalProjectile<GunGlobalProjectile>().bounce = true;
				break;
				default:
				break;
			}
		}
		public override void OnHitEffect(Projectile projectile, NPC target){
			base.OnHitEffect(projectile, target);
			switch ((int)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]){
				case 1:
				target.AddBuff(BuffID.Frozen, 10);
				break;
				case 5:
				Projectile.NewProjectile(target.Center, new Vector2(), ProjectileID.VampireHeal, 0, 0, projectile.owner, ai1:projectile.damage/3);
				break;
				case 6:
				int a = 0;
				int b = projectile.damage/3;
				Vector2 c = target.Center;
				specialreloadproj = true;
				Ammo++;
				Shoot(Main.player[projectile.owner], ref c, ref projectile.velocity.X, ref projectile.velocity.Y, ref a, ref b, ref projectile.knockBack);
				specialreloadproj = false;
				break;
				case 8:
				Main.player[projectile.owner].AddBuff(ModContent.BuffType<ReloadBuff>(), 120);
				break;
				case 9:
				target.StrikeNPC((target.lifeMax/13)+(int)(target.defense*0.49f),0,0);
				break;
				default:
				break;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			type = item.shoot;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
