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
	public class ChimeBase : RefTheItem
	{
		protected LegacySoundStyle useSound = new LegacySoundStyle(2, 101);
		//LegacySoundStyle useSound = new LegacySoundStyle(2, 72);
		//I don't know how, but I got the lightning spears to have multiple different sizes of WotG when they enter water.
		protected String[] Spells = new String[9]{"Lightning Spear","Great Lightning Spear","Sunlight Spear","Lightning Arrow","Wrath of the Gods","Lightning Storm","Heal","Blessed Weapon","Great Magic Barrier"};
		protected int spell = 0;
		protected float[] SpellDamage = new float[9]{1.33f,1.66f,2,0.85f,1,1.3f,75,0,0};
		protected int[] SpellAmmo = new int[9]{15,10,6,0,5,4,0,0,0};
		protected int[] SpellMana = new int[9]{5,7,10,15,40,60,20,35,20};
		protected float[] SpellSpeed = new float[9]{2f,1.85f,1.75f,1.25f,0.45f,0.45f,1.05f,1f,1f};

		public override bool Autoload(ref string name){
			return name!="ChimeBase";
		}

		public override void HoldItem(Player player){
			player.GetModPlayer<GunPlayer>().guninfo = Spells[spell];
			base.HoldItem(player);
		}
		
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
                item.UseSound = new LegacySoundStyle(2, 7);
				item.reuseDelay = 0;
				item.channel = false;
				if(Spells[spell]=="Way of White Corona"){
					for(int i = 0; i < Main.projectile.Length; i++){
						if(Main.projectile[i].owner == player.whoAmI && Main.projectile[i].Name=="Way of White Corona" && Main.projectile[i].active){
							Main.projectile[i].Kill();
							return false;
						}
					}
				}
            }else{
				item.useStyle = 5;
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
				}else if(Spells[spell].Contains("Heal")){
					return !player.HasBuff(BuffID.PotionSickness);
				}else if(Spells[spell]=="Tears of Denial"){
					return !player.HasBuff(ModContent.BuffType<ToDCD>());
				}
            }
			return base.CanUseItem(player)&&(player.altFunctionUse == 2 || player.CheckMana((int)(SpellMana[spell]*1.5f), true));
        }
		public override float UseTimeMultiplier(Player player){
			return player.altFunctionUse != 2 ? SpellSpeed[spell] : 1;
			//return Spells[spell]=="Fire Surge"&&player.altFunctionUse != 2 ? 2.5f : 1;
		}
		//*
		public override void restoredefaults(){
			MaxAmmo = SpellAmmo[spell];
			item.UseSound = useSound;
			Spread = 0;
		}//*/
		//*
		public override void SpecialReloadProj(Player player, int ammoleft){
			if(ammoleft>=SpellAmmo[spell]||Spells[spell]=="Blessed Weapon"||Spells[spell]=="Great Magic Barrier"){
				if(player.controlSmart){
					spell--;
					if(spell<0)spell=Spells.Length-1;
				}else{
					spell++;
					if(spell>=Spells.Length)spell=0;
				}
			}
		}//*/
		public override void OnHitEffect(Projectile projectile){
            if(projectile.Name=="Way of White Corona"){
				projectile.ai[0] = 0;
				projectile.velocity = projectile.oldVelocity;
				projectile.GetGlobalProjectile<GunGlobalProjectile>().timesincestop+=5;
				projectile.GetGlobalProjectile<GunGlobalProjectile>().stopping=2;
			}else if(projectile.Name=="Soul Geyser"){
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
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>();
			damage = (int)(damage*SpellDamage[spell]);
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
			if(Spells[spell].Contains("Spear")){
				type = ProjectileID.BulletHighVelocity;
				speedX*=1.5f;
				speedY*=1.5f;
			}else if(Spells[spell]=="Lightning Arrow"){
				type=ModContent.ProjectileType<LightningBow>();
				//position+=new Vector2(speedX, speedY).Normalized().RotatedBy(90*player.direction)*3;
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
			}else if(Spells[spell]=="Black Flame"){
				type = ProjectileID.BlackBolt;
				float rotation = MathHelper.ToRadians(360);
				position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
				for (int i = 0; i < 8; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy((rotation/7)*(i-3)); // Watch out for dividing by 0 if there is only 1 projectile.
					int a = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, ai1:1);
					Main.projectile[a].usesLocalNPCImmunity = true;
					Main.projectile[a].localNPCHitCooldown = 3;
					Main.projectile[a].scale+=0.5f;
					Main.projectile[a].timeLeft/=60;
					Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = true;
					Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().Color = new Color(255,215,50);
					if(storefiredshots)firedshots.Add(a);
            		PostShoot(a);
				}
            	Ammo--;
				return false;
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
					if(storefiredshots)firedshots.Add(a);
            		PostShoot(a);
				}
            	Ammo--;
				return false;
			}else if(Spells[spell]=="Lifehunt Scythe"){
				type=ModContent.ProjectileType<LifehuntScythe>();
			}else if(Spells[spell]=="Blessed Weapon"){
				player.AddBuff(ModContent.BuffType<BlessedBuff>(),2700);
				return false;
			}else if(Spells[spell]=="Great Magic Barrier"){
				player.AddBuff(ModContent.BuffType<BarrierBuff>(),3900);
				return false;
			}else if(Spells[spell]=="Tears of Denial"){
				player.AddBuff(ModContent.BuffType<ToDBuff>(),60);
				return false;
			}else if(Spells[spell].Contains("Heal")){
				Projectile.NewProjectile(player.Center, new Vector2(), ProjectileID.SpiritHeal, (int)SpellDamage[spell], 0, item.owner, ai1:(int)(SpellDamage[spell]*player.magicDamage)*(Main.rand.Next(0,99)<player.magicCrit?2:1));
				player.AddBuff(BuffID.PotionSickness, player.potionDelayTime/4);
				return false;
			}
            int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY).RotatedByRandom(MathHelper.ToRadians(Spread)), type, damage, knockBack, item.owner);
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].friendly = true;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 3;
			Main.projectile[proj].Name = Spells[spell];
			if(storefiredshots)firedshots.Add(proj);
            Spread = Math.Min(Spread+BloomPerShot, MaxSpread);
			if(type==ProjectileID.BallofFire)Main.projectile[proj].ai[0] = 4;
			if(Spells[spell].Contains("Spear")){
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
            PostShoot(proj);
            return false;
		}
	}
}
