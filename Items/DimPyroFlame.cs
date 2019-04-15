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
	public class DimPyroFlame : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 20);
		String[] Spells = new String[11]{"Fireball","Fire Orb","Great Fireball","Bursting Fireball","Forbidden Sun","Combustion","Great Combustion","Fire Surge","Toxic Mist","Acid Mist","Power Within"};
		int spell = 0;
		float[] SpellDamage = new float[11]{1.75f,2.5f,3.85f,1.25f,3f,1.5f,2.5f,0.85f,0.625f,0.625f,0};
		int[] SpellAmmo = new int[11]{8,4,3,6,1,10,6,10,3,2,1};
		int[] SpellMana = new int[11]{5,15,25,20,60,8,18,10,30,30,50};
		float[] SpellSpeed = new float[11]{1.5f,1,0.75f,0.79f,0.6f,1.9f,1.65f,2.4f,1,1,0.5f};
		bool[] SpellNeedsDry = new bool[11]{true,true,true,true,true,false,false,true,false,false,false};
		
		public override string Texture{
			get {return"RefTheGun/Items/PyroFlame";}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Faint Pyromancy Flame");
			Tooltip.SetDefault("Well done, you've made a dangerous sparkler.");
		}
		public override void SetDefaults()
		{
			item.damage = 10;
			item.magic = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.width = 28;
			item.height = 18;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.shoot = 1;//ProjectileID.BallofFire;
			item.shootSpeed = 11.5f;
			item.UseSound = useSound;
			item.autoReuse = false;
            item.color = (Color.Black.ToVector4()-new Vector4(0,0,0,0.35f)).toColor();
			MaxAmmo = SpellAmmo[spell];
			MinSpread = 0f;
			BloomPerShot = 0f;
			ReloadTimeMax = 50;
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
				item.shoot = 1;
            }else{
				item.useStyle = 5;
				item.autoReuse = Spells[spell]=="Fire Surge";
				item.shoot = SpellNeedsDry[spell] ? ProjectileID.BallofFire : 1;
            }
			return base.CanUseItem(player)&&(player.altFunctionUse == 2 || player.CheckMana((int)(SpellMana[spell]*1.75f), true));
        }
		public override float UseTimeMultiplier(Player player){
			return player.altFunctionUse != 2 ? SpellSpeed[spell] : 1;
			//return Spells[spell]=="Fire Surge"&&player.altFunctionUse != 2 ? 2.5f : 1;
		}
		//*
		public override void restoredefaults(){
			MaxAmmo = SpellAmmo[spell];
			item.UseSound = useSound;
		}//*/
		//*
		public override void SpecialReloadProj(Player player, int ammoleft){
			if(ammoleft>=SpellAmmo[spell]||Spells[spell]=="Power Within"){
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
			recipe.AddIngredient(ItemID.WaterCandle, 2);
			recipe.AddIngredient(ItemID.ManaCrystal, 5);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void OnHitEffect(Projectile projectile){
			if(projectile.Name=="Forbidden Sun"){
				Projectile a = Projectile.NewProjectileDirect(projectile.position, new Vector2(), ProjectileID.MolotovCocktail, projectile.damage, 0, projectile.owner);
				a.timeLeft=1;
				projectile.GetGlobalProjectile<GunGlobalProjectile>(mod).effectonhit = false;
				projectile.Kill();
			}else if(projectile.Name=="Bursting Fireball"){
				for (int i = 0; i < Main.rand.Next(5,7); i++)
				{
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X*(1+Main.rand.Next(1)), projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20));
					int a = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, projectile.type, projectile.damage/5, projectile.knockBack, projectile.owner, 4);
					Main.projectile[a].usesLocalNPCImmunity = true;
					Main.projectile[a].localNPCHitCooldown = 2;
				}
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			damage = (int)(damage*SpellDamage[spell]);
            if(player.altFunctionUse == 2)return false;
			type = ProjectileID.BallofFire;
			//damage += (int)(SpellDamage[spell]*player.magicDamage);
			float rot = 0;
			Vector2 vel = new Vector2(speedX,speedY);
			if(Spells[spell]=="Bursting Fireball"){
				speedX*= 1.5f;
				speedY*= 1.5f;
			}else if(Spells[spell]=="Combustion"){
				type = mod.ProjectileType<Combustion>();
				rot = (float)Math.Atan2((double)speedY, (double)speedX) + 1.57f; 
				knockBack=0;
				speedX/= 4;
				speedY/= 4;
			}else if(Spells[spell]=="Great Combustion"){
				type = mod.ProjectileType<GreatCombustion>();
				rot  = (float)Math.Atan2((double)speedY, (double)speedX) + 1.57f; 
				knockBack=0;
				speedX/= 5;
				speedY/= 5;
			}else if(Spells[spell]=="Fire Surge"){
				type = ProjectileID.Flames;
				knockBack=0;
			}else if(Spells[spell]=="Toxic Mist"){
				speedX = 0;
				speedY = 0;
				position=Main.MouseWorld;
				knockBack = 0;
				type=mod.ProjectileType<ToxinCloud>();
			}else if(Spells[spell]=="Acid Mist"){
				speedX = 0;
				speedY = 0;
				position=Main.MouseWorld;
				knockBack = 0;
				type=mod.ProjectileType<AcidCloud>();
			}else if(Spells[spell]=="Power Within"){
                Ammo--;
			    modPlayer.roundsinmag = Ammo;
				player.AddBuff(mod.BuffType("PowerWithin"), 6000);
				player.altFunctionUse = 2;
				CanUseItem(player);
				return false;
			}
            int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY), type, damage, knockBack, item.owner);
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].friendly = true;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 3;
			if(type==ProjectileID.BallofFire)Main.projectile[proj].ai[0] = 4;
			/*if(Spells[spell]=="Fire Surge"){
			}*/
			if(Spells[spell]=="Toxic Mist"){
				//Main.projectile[proj].scale+=3;
				Main.projectile[proj].penetrate = -1;
				Main.projectile[proj].tileCollide = false;
			}
			if(Spells[spell]=="Fire Orb"){
				Main.projectile[proj].scale+=0.33f;
			}else if(Spells[spell]=="Great Fireball"){
				Main.projectile[proj].scale+=0.66f;
			}else if(Spells[spell].Contains("Combustion")){
				Main.projectile[proj].rotation = rot;
				Main.projectile[proj].Center = player.MountedCenter+(vel*2);
				if(Spells[spell].Contains("Great")){
					Main.projectile[proj].Center = player.MountedCenter+(vel*4);
				}	
			}else if(Spells[spell]=="Bursting Fireball"){
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).effectonhit = true;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = this;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).render = false;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).bounce = true;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).destroyonbounce = true;
				Main.projectile[proj].Name = "Bursting Fireball";
				Main.projectile[proj].timeLeft = Main.projectile[proj].timeLeft/120;
            	Main.projectile[proj].usesLocalNPCImmunity = true;
            	Main.projectile[proj].localNPCHitCooldown = 0;
			}else if(Spells[spell]=="Forbidden Sun"){
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).effectonhit = true;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = this;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).render = false;
				Main.projectile[proj].Name = "Forbidden Sun";
				Main.projectile[proj].scale+=1;
				Main.projectile[proj].timeLeft = Main.projectile[proj].timeLeft/20;
			}
            /*if(storefiredshots){
                firedshots.Add(proj);
            }
            if(TrackHitEnemies){
                Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).TrackHitEnemies = true;
                Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = (RefTheItem)item.modItem;
            }*/
            Ammo--;
			//modPlayer.roundsinmag = Ammo;
            return false;
			//return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
