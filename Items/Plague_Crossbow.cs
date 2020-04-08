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
	public class Plague_Crossbow : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Crossbow");
			Tooltip.SetDefault(@"Where did the tape come from?");
		}
		public override void SetDefaults()
		{
			item.damage = 37;
			item.ranged = true;
			item.noMelee = true;
			item.width = 52;
			item.height = 20;
			item.useTime = 1;
			item.useAnimation = 11;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Green;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 11.5f;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 6;
			MaxSpread = 3f;
			MinSpread = 0.2f;
			BloomPerShot = 0.75f;
			SpreadLossSpeed = 0.05f;
			ReloadTimeMax = 50;
			specialreloadproj = true;

			Ammo = MaxAmmo;
		}

		public override bool ConsumeAmmo(Player player){
			return !((GunPlayer)player).Reloading;
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
			recipe.AddIngredient(ModContent.ItemType<Crossbow>(), 1);
			recipe.AddIngredient(ItemID.JungleSpores, 10);
			recipe.AddIngredient(ItemID.QueenBeeTrophy, 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostShoot(int p){
			if(Main.projectile[p].type == ProjectileID.ToxicBubble){
				Main.projectile[p].aiStyle = 1;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().OverrideTextureInt = ProjectileID.ToxicBubble;
				Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
			}
		}
		public override void OnHitEffect(Projectile projectile, NPC target){
			projectile.timeLeft = projectile.timeLeft>6?6:projectile.timeLeft;
			projectile.damage = (int)(projectile.damage*0.55f);
			projectile.velocity = new Vector2();
			Rectangle hb = projectile.Hitbox;
			hb.Inflate(13,13);
			projectile.Hitbox = hb;
			projectile.penetrate+=10;
			projectile.tileCollide = false;
			projectile.GetGlobalProjectile<GunGlobalProjectile>().OverrideTextureInt = 0;
			target.AddBuff(BuffID.Venom, 300);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.itemAnimation==item.useAnimation-1){
				int a = type;
				TrackHitEnemies = true;
				type = ProjectileID.ToxicBubble;
				if(Ammo>0)base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
				type = a;
				if(!((GunPlayer)player).Reloading){
					TrackHitEnemies = false;
					Ammo++;
					base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
					Reload();
				}
			}else{
				if(!player.controlUseItem)player.itemAnimation = 0;
			}
			return false;
		}
	}
}
