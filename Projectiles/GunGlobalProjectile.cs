using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using RefTheGun.Items;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using static RefTheGun.RefTheExtensions;

namespace RefTheGun.Projectiles
{
	public class GunGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
        public List<int> HitEnemies = new List<int>();
        public bool TrackHitEnemies = false;
        public RefTheItem firedwith = new RefTheItem();
        public bool effectonhit = false;
        public bool render = true;
        public bool bounce = false;
        public bool destroyonbounce = false;
        public bool OverrideColor = false;
        public Color Color = Color.White;
        public int timesincestop = 60;
        public int stopping = 2;
        public Texture2D OverrideTexture = null;
        public int OverrideTextureInt = 0;
        public int OverrideTextureMode = 0;
        public bool ignorespecialfeatures = false;
        public List<TrueNullable<float>> aioverflow = new List<TrueNullable<float>>{0,0,0,0,null,null,null};
        public Color GlowColor = new Color();
        public int? effectreqs = null;
        public List<int> passives = new List<int>(){};
        public bool nullprecull = false;
        public static void SetDefaults2(ref Projectile projectile){
            if(projectile.Name=="Blazing Chakram"){
                projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow=new List<TrueNullable<float>>{};
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit){
            if(passives.Count>0)for (int i = 0; i < passives.Count; i++){
                GunItemBelt.ActivatePassive(projectile, target, passives[i]);
            }
            if(TrackHitEnemies){
                //projectile.Center = target.Center;
                //HitEnemies.Add(target.whoAmI);
                firedwith.hitenemies.Add(target);
            }
            if(effectonhit){
                if(effectreqs==null){
                    firedwith.OnHitEffect(projectile, target);
                }else if ((effectreqs&1)!=0){
                    firedwith.OnHitEffect(projectile, target);
                }
            }
        }
        public override void AI(Projectile projectile){
            if(projectile.Name=="Way of White Corona" && projectile.ai[0]==1){
                if(timesincestop-->0 && stopping--<=0)projectile.velocity = new Vector2();
            }
            if(projectile.Name=="Ice Arrow" && aioverflow[0]==2 && projectile.wet){
                Vector2 offset = projectile.velocity.RotatedBy(MathHelper.ToRadians(45)).Normalized()*32;
                //Projectile.NewProjectileDirect(projectile.position-offset, projectile.velocity/16, ProjectileID.IceBlock, 1000, 0, projectile.owner, (int)((projectile.Center-offset).X/16), (int)((projectile.Center-offset).Y/16)).GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]=1;
                for (int i = 0; i < 5; i++)Projectile.NewProjectileDirect(projectile.position, projectile.velocity/16, ProjectileID.IceBlock, 1000, 0, projectile.owner, (int)(projectile.Center.X/16), (int)(projectile.Center.Y/16));//.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]=1;
                //Projectile.NewProjectileDirect(projectile.position+offset, projectile.velocity/16, ProjectileID.IceBlock, 1000, 0, projectile.owner, (int)((projectile.Center+offset).X/16), (int)((projectile.Center+offset).Y/16)).GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0]=1;
                projectile.Kill();
            }
            if(projectile.Name=="Blazing Chakram"){
                Player player = Main.player[projectile.owner];
                projectile.timeLeft++;
                if(player.channel){
                    projectile.ai[0] = 0;
                    projectile.velocity = new Vector2(12.5f*((projectile.Center - Main.MouseWorld).Length()/100), 0).RotatedBy((projectile.Center - Main.MouseWorld).ToRotation()+MathHelper.ToRadians(180));
                    projectile.GetGlobalProjectile<GunGlobalProjectile>().timesincestop+=5;
                    projectile.GetGlobalProjectile<GunGlobalProjectile>().stopping=2;
                }
                if(aioverflow.Count>=1)for(int i = 0; i < aioverflow.Count; i++){
                    if(aioverflow[i]!=null)if(Main.item[(int)aioverflow[i]].active){
                        Main.item[(int)aioverflow[i]].Center = projectile.Center;
                    }else{
                        aioverflow.RemoveAt(i);
                    }
                }
            }
            if(GlowColor!=new Color())Lighting.AddLight(projectile.Center, GlowColor.R/100, GlowColor.G/100, GlowColor.B/100);
            if((projectile.Name.Contains("Lightning Spear")||projectile.Name=="Sunlight Spear")&&projectile.wet){
                switch (projectile.Name)
                {
                    case "Lightning Spear":
                    Projectile a = Projectile.NewProjectileDirect(projectile.Center, new Vector2(), ModContent.ProjectileType<WrathoftheGods>(), projectile.damage, 0, projectile.owner, 2);
                    a.timeLeft = 10;
                    a.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures=true;
                    a.GetGlobalProjectile<GunGlobalProjectile>().GlowColor = GlowColor;
                    break;
                    case "Great Lightning Spear":
                    Projectile b = Projectile.NewProjectileDirect(projectile.Center, new Vector2(), ModContent.ProjectileType<WrathoftheGods>(), projectile.damage, 0, projectile.owner, 1);
                    b.timeLeft = 7;
                    b.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures=true;
                    b.GetGlobalProjectile<GunGlobalProjectile>().GlowColor = GlowColor;
                    break;
                    case "Sunlight Spear":
                    Projectile c = Projectile.NewProjectileDirect(projectile.Center, new Vector2(), ModContent.ProjectileType<WrathoftheGods>(), projectile.damage, 0, projectile.owner, 0);
                    c.timeLeft = 4;
                    c.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures=true;
                    c.GetGlobalProjectile<GunGlobalProjectile>().GlowColor = GlowColor;
                    break;
                    default:
                    break;
                }
                projectile.Kill();
            }
        }
        public override void Kill(Projectile projectile, int timeLeft){
            if(effectonhit){
                if(effectreqs==null){
                    firedwith.OnHitEffect(projectile);
                }else if ((effectreqs&2)!=0){
                    
                }
            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity){
            if(bounce){
                Vector2 diff = oldVelocity - projectile.velocity;
                projectile.velocity -= diff*0.8f;
            }
            if(effectonhit){
                if(effectreqs==null){
                    firedwith.OnHitEffect(projectile);
                }else if ((effectreqs&4)!=0){
                    firedwith.OnHitEffect(projectile);
                }
            }
            return bounce ? !destroyonbounce : base.OnTileCollide(projectile, oldVelocity);
        }
        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox){
            if(projectile.Name=="Way of White Corona"){
                hitbox.Inflate(hitbox.Width/2,hitbox.Height/2);
            }if(projectile.Name=="Blazing Chakram"){
                hitbox.Inflate(hitbox.Width/2,hitbox.Height/2);
                for(int i1 = 0; i1 < Main.item.Length; i1++){
                    Item i = Main.item[i1];
                    if(hitbox.Intersects(new Rectangle((int)i.position.X,(int)i.position.Y,i.width,i.height))&&!aioverflow.Contains(i.whoAmI))aioverflow.Add(i1);
                }
            }
        }
        public override bool PreKill(Projectile projectile, int timeLeft){
            if(nullprecull)projectile.type=0;
            return true;
        }
        public override bool PreDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor){
            if(OverrideColor){
                lightColor = Color;
            }
            if(OverrideTexture!=null){
                spriteBatch.Draw(OverrideTexture, projectile.Center - Main.screenPosition, new Rectangle(0,0,OverrideTexture.Width,OverrideTexture.Height), lightColor, projectile.rotation, OverrideTextureMode==0? new Rectangle(0,0,OverrideTexture.Width,OverrideTexture.Height).Center():OverrideTextureMode==1? new Rectangle(0,0,OverrideTexture.Width,0).Center():new Rectangle(0,0,0,OverrideTexture.Height).Center(), 1f, SpriteEffects.None, 0f);
            }
            if(OverrideTextureInt!=0){
                Main.instance.LoadProjectile(OverrideTextureInt);
                spriteBatch.Draw(Main.projectileTexture[OverrideTextureInt], projectile.Center - Main.screenPosition, new Rectangle(0,0,Main.projectileTexture[OverrideTextureInt].Width,Main.projectileTexture[OverrideTextureInt].Height), lightColor, projectile.rotation, OverrideTextureMode==0? new Rectangle(0,0,Main.projectileTexture[OverrideTextureInt].Width,Main.projectileTexture[OverrideTextureInt].Height).Center():OverrideTextureMode==1? new Rectangle(0,0,Main.projectileTexture[OverrideTextureInt].Width,0).Center():new Rectangle(0,0,0,Main.projectileTexture[OverrideTextureInt].Height).Center(), 1f, SpriteEffects.None, 0f);
            }
            if(projectile.Name=="Way of White Corona"){
                for(int i = 0; i < 5; i++){
                    /*
                    Dust a = Dust.NewDustPerfect(projectile.Center+new Vector2(0,projectile.height).RotatedBy((projectile.timeLeft%20)+(20*i)), 133);
                    a.noGravity = true;
                    a.velocity = new Vector2();
                    //*/
                    //*
                    Dust a = Dust.NewDustDirect(projectile.Center+new Vector2(0,projectile.height).RotatedBy((projectile.timeLeft%21)+(21*i)), 0, 0, 133);
                    a.velocity/=2;
                    //*/
                }
                return false;
            }else if(projectile.Name=="Blazing Chakram"){
                for(int i = 0; i < 5; i++){
                    /*
                    Dust a = Dust.NewDustPerfect(projectile.Center+new Vector2(0,projectile.height).RotatedBy((projectile.timeLeft%20)+(20*i)), 133);
                    a.noGravity = true;
                    a.velocity = new Vector2();
                    //*/
                    //*
                    Dust a = Dust.NewDustDirect(projectile.Center+new Vector2(0,projectile.height).RotatedBy(((Main.GameUpdateCount%21)*2)+(21*i)), 0, 0, 134);
                    a.velocity/=2;
                    //*/
                }
                return false;
            }
            return render;
        }
    }
}