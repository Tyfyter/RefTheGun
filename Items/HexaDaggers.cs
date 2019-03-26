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
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.Projectiles;

namespace RefTheGun.Items
{
	public class HexaDaggers : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 19);
		int timesincereload = 10;
		Color mainColor = Color.OrangeRed;
		Color accentColor = Color.MediumPurple;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hex Daggers");
			Tooltip.SetDefault("This isn't even a reference.");
		}
		public override void SetDefaults()
		{
			item.damage = 40;
			item.ranged = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.shoot = mod.ProjectileType<HexDaggerProj>();
			item.shootSpeed = 12.5f;
			item.useAmmo = 0;
			item.UseSound = useSound;
			item.autoReuse = true;
			item.scale = 0.75f;
			MaxAmmo = 4;
			MaxSpread = 0.1f;
			MinSpread = 0.1f;
			BloomPerShot = 0f;
			SpreadLossSpeed = 0f;
			ReloadTimeMax = 5;
			storefiredshots = true;
			TrackHitEnemies = true;

			Ammo = MaxAmmo;
		}

		public override bool ConsumeAmmo(Player player){
			return false;
		}
		
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2||Ammo<1){
				item.useStyle = ItemUseStyleID.EatingUsing;
				item.noUseGraphic = true;
				if(timesincereload<10)return false;
				return base.CanUseItem(player);
            }else{
				item.useStyle = 5;
				item.noUseGraphic = true;
            }
			return base.CanUseItem(player);
        }
        public override void HoldItem(Player player){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			modPlayer.roundsinmag = Ammo;
			modPlayer.magsize = /*(int)(*/MaxAmmo/**Main.player[item.owner].GetModPlayer<GunPlayer>(mod).MagMultiply)*/;
			if(timesincereload<10)timesincereload++;
            if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded){
                ReloadFinishHook(player, Ammo);
                restoredefaults();
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded = false;
            }
        }
		public override void ReloadFinishHook(Player player, int ammoleft){
			foreach (int p in firedshots){
				if(Main.projectile[p].type==mod.ProjectileType<HexDaggerProj>())((HexDaggerProj)Main.projectile[p].modProjectile).PreKill(Main.projectile[p].timeLeft);
			}
			firedshots.Clear();
			timesincereload = 0;
		}
		public override void restoredefaults(){
			item.useAmmo = 0;
			item.UseSound = useSound;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CelestialMagnet, 1);
			recipe.AddRecipeGroup("IronBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.GetModPlayer<GunPlayer>().multishotmult>1&&player.altFunctionUse!=2)Ammo-=(int)player.GetModPlayer<GunPlayer>().multishotmult-1;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void PostShoot(int p){
			((HexDaggerProj)Main.projectile[p].modProjectile).color = Ammo%2==0?accentColor:mainColor;
		}
		public override void PostShoot(int[] p){
			for(int i = 0; i<p.Length; i++)((HexDaggerProj)Main.projectile[p[i]].modProjectile).color = i%2==0?accentColor:mainColor;
		}

		public override bool PreDrawInInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
			Player player = Main.player[item.owner];
			try{
			if(mod.TextureExists("Items/HexaDaggers_Accents"))spriteBatch.Draw(mod.GetTexture("Items/HexaDaggers_Accents"), position, new Rectangle(0,0,40,40), accentColor, 0, new Vector2(), item.scale*((player.HeldItem.type==item.type&&!Main.playerInventory)?1.1f:0.8f), SpriteEffects.None, 0);
			if(mod.TextureExists("Items/HexaDaggers_Main"))spriteBatch.Draw(mod.GetTexture("Items/HexaDaggers_Main"), position, new Rectangle(0,0,40,40), mainColor, 0, new Vector2(), item.scale*((player.HeldItem.type==item.type&&!Main.playerInventory)?1.1f:0.8f), SpriteEffects.None, 0);
			}
			catch (System.Exception){
				return true;
			}
			return false;
		}
	}
}
