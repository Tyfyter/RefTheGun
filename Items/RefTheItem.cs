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
	public class RefTheItem : ModItem
	{
        public static float basereloadspeed = 0.85f;
        protected int MaxAmmo = 6;
        public int Ammo = 6;
        protected float MinSpread = 0;
        protected float MaxSpread = 16;
        public float Spread = 0;
        protected float BloomPerShot = 1f;
        protected float SpreadLossSpeed = 0.05f;
        protected int ReloadTimeMax = 90;
        protected int Multishot = 1;
        protected bool specialreloadproj = false;
        protected bool storefiredshots = false;
        public List<int> firedshots = new List<int>();
        protected bool TrackHitEnemies = false;
        public List<NPC> hitenemies = new List<NPC>();
        public bool reloadwhenfull = false;
        public static float reloadmult = basereloadspeed;
        protected bool instantreload = false;
        public virtual bool isGun => true;
        public virtual bool ammoPerMult(int i){return false;}
        //protected int ReloadTime = 0;
        public override bool CloneNewInstances{
			get { return true; }
		}
        public override bool Autoload(ref string name){
            if(name == "RefTheItem")return false;
            return true;
        }
        public override void UpdateInventory(Player player){
            if(player.HeldItem!=item)Spread = Math.Min(Math.Max(Spread-SpreadLossSpeed, MinSpread),MaxSpread);
        }
        public override void HoldItem(Player player){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			modPlayer.roundsinmag = Ammo;
			modPlayer.magsize = /*(int)(*/MaxAmmo/**Main.player[item.owner].GetModPlayer<GunPlayer>(mod).MagMultiply)*/;
            if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded){
                ReloadFinishHook(player, Ammo);
                restoredefaults();
                Ammo = /*(int)(*/MaxAmmo/**Main.player[item.owner].GetModPlayer<GunPlayer>(mod).MagMultiply)*/;
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded = false;
            }
        }
        public override void HoldStyle(Player player){
            Spread = Math.Min(Math.Max(Spread-SpreadLossSpeed, MinSpread), MaxSpread);
        }
        public override bool AltFunctionUse(Player player){
            return true;
        }
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2 || (Ammo<=0&&MaxAmmo>0&&!Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading&&!Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded)){
                if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading)return false;
                if(reloadwhenfull && (Ammo >= MaxAmmo||MaxAmmo<=0)){
                    SpecialReloadProj(player, Ammo);
                    Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded = true;
                    return true;
                }
                if(Ammo >= /*(int)(*/MaxAmmo/**Main.player[item.owner].GetModPlayer<GunPlayer>(mod).MagMultiply)*/)return false;
                SpecialReloadProj(player, Ammo);
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading = true;
                int proj = Projectile.NewProjectile(player.Center, new Vector2(), mod.ProjectileType("ReloadProj"), 0, 0, player.whoAmI, (int)(ReloadTimeMax*reloadmult), player.selectedItem);
                Main.projectile[proj].timeLeft = Math.Max((int)(ReloadTimeMax*reloadmult),2);
                item.UseSound = new LegacySoundStyle(-1, -1);
                item.useAmmo = 0;
                return true;
            }else{
                restoredefaults();
                return (Ammo > 0 || MaxAmmo <= 0) && !Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading;
            }
        }
        public virtual void SpecialReloadProj(Player player, int ammoleft){}
        public virtual void ReloadFinishHook(Player player, int ammoleft){}
        public virtual void restoredefaults(){item.useAmmo = AmmoID.Bullet;}
		public virtual void OnHitEffect(Projectile projectile, NPC target){OnHitEffect(projectile);}
        public virtual void OnHitEffect(Projectile projectile){}
        public virtual void PostShoot(int p){}
        public virtual void PostShoot(int[] p){for (int i = 0; i < p.Length; i++)PostShoot(p[i]);}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
            if((player.altFunctionUse == 2 || modPlayer.Reloading) && !specialreloadproj)return false;
            if((ammoPerMult(0)?Math.Min(Multishot,Ammo):Multishot)*modPlayer.multishotmult==1){
                int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY).RotatedByRandom(Main.rand.NextFloat(0, Spread)/36), type, damage, knockBack, item.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].usesLocalNPCImmunity = true;
                Main.projectile[proj].localNPCHitCooldown = 9;
                if(Main.rand.NextFloat(0,1)<=modPlayer.bulletPoisonChance){
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).passives.Add(1);
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).GlowColor = Color.DarkGreen*0.1f;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).Color = Color.DarkGreen;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).OverrideColor = true;
                }
                if(Main.rand.NextFloat(0,1)<=modPlayer.bulletFreezeChance){
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).passives.Add(2);
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).GlowColor = Color.Aqua*0.1f;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).Color = Color.Aqua;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).OverrideColor = true;
                }
                if(Main.rand.NextFloat(0,1)<=modPlayer.bulletBurnChance){
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).passives.Add(3);
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).GlowColor = Color.DarkOrange*0.1f;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).Color = Color.DarkOrange;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).OverrideColor = true;
                }
                if(storefiredshots){
                    firedshots.Add(proj);
                }
                if(TrackHitEnemies){
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).TrackHitEnemies = true;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = (RefTheItem)item.modItem;
                }
                Spread = Math.Max(Math.Min(Spread+BloomPerShot, MaxSpread),MinSpread);
                if(player.altFunctionUse != 2&&MaxAmmo>0){
                    Ammo--;
                    modPlayer.roundsinmag = Ammo;
                    if(instantreload&&Ammo<=0)Reload();
                }
                PostShoot(proj);
            }else{
                List<int> projs = new List<int>();
                for (int i = 0; i < Multishot*modPlayer.multishotmult; i++)
                {
                    int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY).RotatedByRandom(Spread/36), type, damage, knockBack, item.owner);
                    Main.projectile[proj].friendly = true;
                    Main.projectile[proj].hostile = false;
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                    Main.projectile[proj].localNPCHitCooldown = 9;
                if(Main.rand.NextFloat(0,1)<=modPlayer.bulletPoisonChance){
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).passives.Add(1);
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).GlowColor = Color.DarkGreen*0.1f;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).Color = Color.DarkGreen;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).OverrideColor = true;
                }
                if(Main.rand.NextFloat(0,1)<=modPlayer.bulletFreezeChance){
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).passives.Add(2);
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).GlowColor = Color.Aqua*0.1f;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).Color = Color.Aqua;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).OverrideColor = true;
                }
                if(Main.rand.NextFloat(0,1)<=modPlayer.bulletBurnChance){
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).passives.Add(3);
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).GlowColor = Color.DarkOrange*0.1f;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).Color = Color.DarkOrange;
                    Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).OverrideColor = true;
                }
                    projs.Add(proj);
                    if(storefiredshots){
                        firedshots.Add(proj);
                    }
                    if(TrackHitEnemies){
                        Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).TrackHitEnemies = true;
                        Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = (RefTheItem)item.modItem;
                    }
                    Spread = Math.Max(Math.Min(Spread+BloomPerShot, MaxSpread),MinSpread);
                    if(ammoPerMult(i)){
                        if(player.altFunctionUse != 2&&MaxAmmo>0){
                            Ammo--;
                            modPlayer.roundsinmag = Ammo;
                            if(instantreload&&Ammo<=0)Reload();
                        }
                        if(Ammo<=0)break;
                    }
                }
                if(!ammoPerMult(0))if(player.altFunctionUse != 2&&MaxAmmo>0){
                    Ammo--;
                    modPlayer.roundsinmag = Ammo;
                    if(instantreload&&Ammo<=0)Reload();
                }
                PostShoot(projs.ToArray());
            }
            return false;
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
    }
}