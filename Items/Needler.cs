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
	public class Needler : RefTheItem
	{
		LegacySoundStyle useSound {
			get{return new LegacySoundStyle(42, Main.rand.Next(165,168));}
		}
		bool indevkindofsatisfyingmode = false;
		//do you agree that this is pretty well balanced?
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Needler");
			Tooltip.SetDefault("You thought I'd make a mod with pretty much nothing but guns and not reference my favorite FPS series?");
		}
		public override void SetDefaults()
		{
			item.damage = 20;
			item.ranged = true;
			item.noMelee = true;
			item.width = 64;
			item.height = 48;
			item.scale = 0.65f;
			item.useTime = 1;
			item.useAnimation = 6;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.LightRed;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 12.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 22;
			MaxSpread = 3.3f;
			MinSpread = 0.3f;
			BloomPerShot = 0.9f;
			SpreadLossSpeed = 0.09f;
			ReloadTimeMax = 45;
			storefiredshots = true;
			TrackHitEnemies = true;

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
				item.noUseGraphic = (player.GetModPlayer<GunPlayer>().bulletPoisonChance>0);
            }
			return base.CanUseItem(player);
        }
		public override void restoredefaults(){
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = null;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-12, 0);
		}
		public override void HoldStyle(Player player){
			base.HoldStyle(player);
			firedshots.RemoveAll(GC);
		}
		bool GC(int p){
			if(!Main.projectile[p].active)return true;
			if(Main.projectile[p].modProjectile==null)return true;
			if(Main.projectile[p].type!=mod.ProjectileType<NeedlerProj>())return true;
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Uzi, 1);
			recipe.AddIngredient(ItemID.CrystalShard, 10);
			recipe.AddTile(TileID.LihzahrdFurnace);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostShoot(int p){
			if(indevkindofsatisfyingmode){
				Main.projectile[p].aiStyle = 207;
			}else{
				for(int i = 0; i < Main.npc.Length; i++){
					if(!Main.npc[i].active)continue;
					if(!Main.npc[i].CanBeChasedBy(Main.projectile[p]))continue;
					if((Main.npc[i].Center-Main.MouseWorld).Length()<96+((Main.npc[i].width+Main.npc[i].height)/2)){
						((NeedlerProj)Main.projectile[p].modProjectile).target = i;
						break;
					}
				}
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2||player.GetModPlayer<GunPlayer>().Reloading)return false;
            /*if(player.GetModPlayer<GunPlayer>().bulletPoisonChance>0&&!firedshots.Exists(hasActiveDraw))*/firedshots.Add(Projectile.NewProjectile(position, new Vector2(), mod.ProjectileType<DrawNeedler>(), damage, knockBack, item.owner));
			if(player.itemAnimation==item.useAnimation-1){
				type = mod.ProjectileType<NeedlerProj>();
				if(indevkindofsatisfyingmode){
					type = mod.ProjectileType<HexDaggerProj>();
				}
				Main.PlaySound(useSound, position);
			}else{
				if(!player.controlUseItem)player.itemAnimation = 0;
				return false;
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
			Player player = Main.player[item.owner];
			//float scale = 0.7f;
			if (player.GetModPlayer<GunPlayer>().bulletPoisonChance>0){
				if(mod.TextureExists("Items/Needle_Rifle")){
					spriteBatch.Draw(mod.GetTexture("Items/Needle_Rifle"), position/*+new Vector2(25,17)*/, new Rectangle(0,0,item.width,item.height), drawColor, 0, new Vector2(), scale, SpriteEffects.None, 0);
					return false;
				}
			}
			return true;
		}
		public bool hasActiveDraw(int i){
			if(Main.projectile[i].type==mod.ProjectileType<DrawNeedler>()&&Main.projectile[i].active)return true;
			return false;
		}
	}
}
