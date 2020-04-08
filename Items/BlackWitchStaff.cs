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
	public class BlackWitchStaff : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 101);
		
		String[] Spells = new String[17]{"Farron Dart","Great Farron Dart","Soul Arrow","Great Soul Arrow","Great Heavy Soul Arrow","Soul Spear","Crystal Soul Spear","Soul Geyser","Dark Bead","Soul Greatsword","Farron Flashsword","Sunlight Spear","Lightning Arrow","Way of White Corona","Wrath of the Gods","Lightning Storm","Lifehunt Scythe"};
		int spell = 0;
		float[] SpellDamage = new float[17]{1,1.85f,2.17f,2.67f,2.85f,3.33f,3.67f,2.25f,0.75f,4,1,2,0.95f,1.15f,1,1.3f,1.75f};
		int[] SpellAmmo = new int[17]{60,30,40,20,15,5,4,3,6,0,0,6,0,0,5,4,0};
		int[] SpellMana = new int[17]{5,7,10,15,25,40,50,80,35,20,7,10,15,25,40,80,40};
		float[] SpellSpeed = new float[17]{2f,1.85f,1.75f,1.25f,0.75f,0.5f,0.45f,0.4f,0.55f,1,3,1.75f,1.25f,0.75f,0.45f,0.45f,0.8f};
		bool[] SpellAutoCast = new bool[17]{true,true,false,false,false,false,false,false,false,false,true,false,false,false,false,false,false};

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Black Witch's Staff");
			Tooltip.SetDefault("This thing is still hell to scroll through.");
			Item.staff[item.type] = true;
		}
		public override void SetDefaults()
		{
			item.damage = 85;
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
			item.rare = ItemRarityID.Green;
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
			player.GetModPlayer<GunPlayer>().guninfo = Spells[spell];
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
				item.channel = false;
				if(Spells[spell]=="Way of White Corona"){
					for(int i = 0; i < Main.projectile.Length; i++){
						if(Main.projectile[i].owner == player.whoAmI && Main.projectile[i].Name=="Way of White Corona"){
							Main.projectile[i].Kill();
							return false;
						}
					}
				}
            }else{
				item.useStyle = 5;
				item.autoReuse = SpellAutoCast[spell];
				item.noMelee = true;
				item.noUseGraphic = false;
				item.reuseDelay = 0;
				item.channel = false;
				if(Spells[spell]=="Lightning Arrow"){
					item.channel = true;
				}else if(Spells[spell]=="Way of White Corona"){
					player.GetModPlayer<GunPlayer>().magsize=-2;
					for(int i = 0; i < Main.projectile.Length; i++){
						if(Main.projectile[i].owner == player.whoAmI && Main.projectile[i].Name=="Way of White Corona" && Main.projectile[i].active){
							player.GetModPlayer<GunPlayer>().magsize=-1;
							return false;
						}
					}
					item.channel = true;
				}else if(Spells[spell]=="Soul Greatsword"){
					item.useStyle = 1;
					item.noMelee = true;
					item.noUseGraphic = true;
				}else if(Spells[spell]=="Farron Flashsword"){
					item.noUseGraphic = true;
					item.reuseDelay = 21;
				}
				//TextureInUse = Spells[spell]=="Soul Greatsword" ? SwordTexture : MainTexture;
            }
			return base.CanUseItem(player)&&(player.altFunctionUse == 2 || player.CheckMana((int)(SpellMana[spell]*1.11f), true));
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
			recipe.AddIngredient(ItemID.ShadowbeamStaff, 1);
			recipe.AddIngredient(ItemID.SpectreStaff, 1);
			recipe.AddIngredient(ItemID.CorruptionKey, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 20);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShadowbeamStaff, 1);
			recipe.AddIngredient(ItemID.SpectreStaff, 1);
			recipe.AddIngredient(ItemID.CrimsonKey, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 20);
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
			}else if(projectile.Name=="Way of White Corona"){
				projectile.ai[0] = 0;
				projectile.velocity = projectile.oldVelocity;
				projectile.GetGlobalProjectile<GunGlobalProjectile>().timesincestop+=5;
				projectile.GetGlobalProjectile<GunGlobalProjectile>().stopping=2;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>();
			damage = (int)(SpellDamage[spell]*damage);
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
				type=ModContent.ProjectileType<SoulArrow>();
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
				type=ModContent.ProjectileType<SoulGreatsword>();
				speedX= 0;
				speedY= 0;
			}else if(Spells[spell]=="Farron Flashsword"){
				type=ModContent.ProjectileType<FarronSlash>();
			}else if(Spells[spell]=="Sunlight Spear"){
				type = ProjectileID.BulletHighVelocity;
				speedX*=1.5f;
				speedY*=1.5f;
			}else if(Spells[spell]=="Lightning Arrow"){
				type=ModContent.ProjectileType<LightningBow>();
			}else if(Spells[spell]=="Way of White Corona"){
				type=ProjectileID.LightDisc;
				speedX*=1.5f;
				speedY*=1.5f;
			}else if(Spells[spell].Contains("Wrath of the Gods")){
				type=ModContent.ProjectileType<WrathoftheGods>();
				position = player.Center;
				knockBack=0;
				speedX=0;
				speedY=0;
			}else if(Spells[spell]=="Lightning Storm"){
				type = ProjectileID.BlackBolt;
				float rotation = MathHelper.ToRadians(360);
				position = player.Center;
				for (int i = 0; i < 12; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy((rotation/11)*(i-3)); // Watch out for dividing by 0 if there is only 1 projectile.
					int a = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, ai1:1);
					Main.projectile[a].usesLocalNPCImmunity = true;
					Main.projectile[a].localNPCHitCooldown = 3;
					Main.projectile[a].timeLeft/=2;
					Main.projectile[a].penetrate = -1;
					Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().OverrideTextureInt = type;
					Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = true;
					Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().Color = new Color(255,215,50)*10;
					Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().render = false;
					
				}
            	Ammo--;
				return false;
			}else if(Spells[spell]=="Lifehunt Scythe"){
				type=ModContent.ProjectileType<LifehuntScythe>();
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
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().firedwith = this;
				Main.projectile[proj].timeLeft = Main.projectile[proj].timeLeft/140;
            	Main.projectile[proj].usesLocalNPCImmunity = true;
            	Main.projectile[proj].localNPCHitCooldown = 0;
			}else if(Spells[spell]=="Farron Flashsword"){
				return false;
			}else if(Spells[spell]=="Sunlight Spear"){
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().Color = new Color(255,215,50,25);
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = true;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().OverrideTextureInt = ProjectileID.JavelinFriendly;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().OverrideTextureMode = 1;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().render = false;
				Main.projectile[proj].extraUpdates=1;
				Main.projectile[proj].penetrate=-1;
				Main.projectile[proj].ignoreWater = false;
			}else if(Spells[spell].Contains("Way of White Corona")){
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().firedwith = this;
				Main.projectile[proj].extraUpdates++;
			}
            if(MaxAmmo>0)Ammo--;
            return false;
		}
	}
}
