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
	public class SorcererStaff : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 101);
		//LegacySoundStyle useSound = new LegacySoundStyle(2, 72);
		String[] Spells = new String[11]{"Farron Dart","Great Farron Dart","Soul Arrow","Great Soul Arrow","Great Heavy Soul Arrow","Soul Spear","Crystal Soul Spear","Soul Geyser","Dark Bead","Soul Greatsword","Farron Flashsword"};
		int spell = 0;
		int[] SpellDamage = new int[11]{0,50,65,95,110,140,160,75,-15,175,0};
		int[] SpellAmmo = new int[11]{60,30,40,20,15,5,4,3,6,0,0};
		int[] SpellMana = new int[11]{5,7,10,15,25,40,50,80,35,20,7};
		float[] SpellSpeed = new float[11]{2f,1.85f,1.75f,1.25f,0.75f,0.5f,0.45f,0.4f,0.55f,1,3};
		bool[] SpellAutoCast = new bool[11]{true,true,false,false,false,false,false,false,false,false,true};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sorcerer's Staff");
			Tooltip.SetDefault("This also really doesn't belong in this mod.");
			Item.staff[item.type] = true;
		}
		public override void SetDefaults()
		{
			item.damage = 40;
			item.magic = true;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.shoot = ProjectileID.MagicDagger;
			item.shootSpeed = 12.5f;
			item.UseSound = useSound;
			item.autoReuse = false;
			MaxAmmo = SpellAmmo[spell];
			MinSpread = 0f;
			BloomPerShot = 0f;
			ReloadTimeMax = 45;
			reloadwhenfull = true;

			Ammo = MaxAmmo;
		}

		public override void HoldItem(Player player){
			player.GetModPlayer<GunPlayer>(mod).guninfo = Spells[spell];
			base.HoldItem(player);
		}
		
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
				item.autoReuse = false;
				item.noMelee = false;
				item.noUseGraphic = false;
                item.UseSound = new LegacySoundStyle(2, 7);
				item.reuseDelay = 0;
            }else{
				item.useStyle = 5;
				item.autoReuse = SpellAutoCast[spell];
				item.noMelee = true;
				item.noUseGraphic = false;
				item.reuseDelay = 0;
				if(Spells[spell]=="Soul Greatsword"){
					item.useStyle = 1;
					item.noMelee = true;
					item.noUseGraphic = true;
				}
				if(Spells[spell]=="Farron Flashsword"){
					item.noUseGraphic = true;
					item.reuseDelay = 21;
				}
				//TextureInUse = Spells[spell]=="Soul Greatsword" ? SwordTexture : MainTexture;
            }
			return base.CanUseItem(player)&&(player.altFunctionUse == 2 || player.CheckMana(SpellMana[spell], true));
        }
		public override float UseTimeMultiplier(Player player){
			return player.altFunctionUse != 2 ? SpellSpeed[spell] : 1;
			//return Spells[spell]=="Fire Surge"&&player.altFunctionUse != 2 ? 2.5f : 1;
		}
		//*
		public override void restoredefaults(){
			MaxAmmo = SpellAmmo[spell];
			item.UseSound = Spells[spell].Contains("Faron Dart") ? new LegacySoundStyle(2, 63) : useSound;
			Spread = 0;
		}//*/
		//*
		public override void SpecialReloadProj(Player player, int ammoleft){
			if(ammoleft>=SpellAmmo[spell]){
				if(player.controlSmart){
					spell--;
					if(spell<0)spell=Spells.Length-1;
				}else{
					spell++;
					if(spell>=Spells.Length)spell=0;
				}
			}
		}//*/

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.UnholyTrident, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void OnHitEffect(Projectile projectile){
			if(projectile.Name=="Soul Geyser"){
				for (int i = 0; i < 4; i++)
				{
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X*(1+Main.rand.Next(1)), projectile.velocity.Y).RotatedBy(MathHelper.ToRadians((10*i)-15));
					int a = Projectile.NewProjectile(projectile.position.X+perturbedSpeed.X, projectile.position.Y+perturbedSpeed.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.LostSoulFriendly, projectile.damage, projectile.knockBack, projectile.owner);
					Main.projectile[a].usesLocalNPCImmunity = true;
					Main.projectile[a].localNPCHitCooldown = 2;
				}
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			damage += (int)(SpellDamage[spell]*player.magicDamage);
            if(player.altFunctionUse == 2)return false;
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			//position+=new Vector2(speedX, speedY).Normalized().RotatedBy(90*player.direction)*3;
            Spread = Math.Min(Spread, MaxSpread);
			MinSpread = 0;
			MaxSpread = 0;
			if(Spells[spell].Contains("Farron Dart")){
				type = ProjectileID.Blizzard;
				//speedX*= 1.5f;
				//speedY*= 1.5f;
				MinSpread = Spells[spell].Contains("Great") ? 0.5f : 3.5f;
				MaxSpread = Spells[spell].Contains("Great") ? 10f : 17.5f;
				BloomPerShot = Spells[spell].Contains("Great") ? 2.5f : 4.5f;
				SpreadLossSpeed = Spells[spell].Contains("Great") ? 0.03f : 0.029f;
			}else if(Spells[spell].Contains("Soul Arrow")){
				type=mod.ProjectileType<SoulArrow>();
				position+=new Vector2(speedX, speedY).Normalized().RotatedBy(90*player.direction)*3;
			}else if(Spells[spell].Contains("Soul Spear")){
				type=ProjectileID.SkyFracture;
			}else if(Spells[spell].Contains("Soul Geyser")){
				type=ProjectileID.ChargedBlasterOrb;
				speedX/= 1.5f;
				speedY/= 1.5f;
			}else if(Spells[spell]=="Dark Bead"){
				type = ProjectileID.ShadowFlameArrow;
				float rotation = MathHelper.ToRadians(30);
				position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
				for (int i = 0; i < 7; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy((rotation/7)*(i-3)); // Watch out for dividing by 0 if there is only 1 projectile.
					int a = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
					Main.projectile[a].hostile = false;
					Main.projectile[a].friendly = true;
					Main.projectile[a].usesLocalNPCImmunity = true;
					Main.projectile[a].localNPCHitCooldown = 3;
				}
            	Ammo--;
				return false;
			}else if(Spells[spell]=="Soul Greatsword"){
				type=mod.ProjectileType<SoulGreatsword>();
				speedX= 0;
				speedY= 0;
			}else if(Spells[spell]=="Farron Flashsword"){
				type=mod.ProjectileType<FarronSlash>();
			}
            int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY).RotatedByRandom(MathHelper.ToRadians(Spread)), type, damage, knockBack, item.owner);
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].friendly = true;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 3;
				Main.projectile[proj].Name = Spells[spell];
            Spread = Math.Min(Spread+BloomPerShot, MaxSpread);
			if(type==ProjectileID.BallofFire)Main.projectile[proj].ai[0] = 4;
			if(Spells[spell].Contains("Faron Dart")){
				Main.projectile[proj].extraUpdates+=2;
			}else if(Spells[spell].Contains("Soul Spear")){
				Main.projectile[proj].extraUpdates+=1;
				if(Spells[spell].Contains("Crystal")){
					Main.projectile[proj].penetrate = -1;
				}
			}else if(Spells[spell].Contains("Soul Geyser")){
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).effectonhit = true;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = this;
				Main.projectile[proj].timeLeft = Main.projectile[proj].timeLeft/140;
            	Main.projectile[proj].usesLocalNPCImmunity = true;
            	Main.projectile[proj].localNPCHitCooldown = 0;
			}else if(Spells[spell]=="Farron Flashsword"){
				return false;
			}
            if(MaxAmmo>0)Ammo--;
            return false;
		}
	}
}
