using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Enums;
using static RefTheGun.RefTheExtensions;

namespace RefTheGun.Projectiles
{

    public class HeresySword : ModProjectile
    {
        public override bool CloneNewInstances => true;
        Ray HB = new Ray();
        Vector2 oldPos2;
        Vector2 oldPos;
        int stab = 0;
        public override void SetDefaults()
        {
            //projectile.name = "Ice Shield";  //projectile name
            projectile.width = 30;       //projectile width
            projectile.height = 30;  //projectile height
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;         // 
            projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = -1;      //how many npc will penetrate
            projectile.timeLeft = 600;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;   
			projectile.aiStyle = 0;
            oldPos = projectile.position;
            oldPos2 = projectile.position;
            projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.OrangeRed;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heresy");
		}
        public override void AI(){
            Player player = Main.player[projectile.owner];
            GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
            modPlayer.Heresy = true;
            Vector2 mousePos = Main.MouseWorld;
            oldPos2 = oldPos;
            oldPos = projectile.position;
            projectile.rotation = (float)Math.Atan2((player.Center - mousePos).Y, (player.Center - mousePos).X)+2.35619f;
            if(projectile.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures)projectile.rotation=player.direction==-1?2.35619f:-0.785398f;
            projectile.position = player.position + (new Vector2(0,12)) + new Vector2(0, (player.controlUseTile&&stab<=0)?(stab<0?47:52):42).RotatedBy(projectile.rotation - 0.79919);
            projectile.velocity = new Vector2();
            /*for (int i = 0; i<Main.projectile.Length; i++){
                if (projectile.Hitbox.Intersects(Main.projectile[i].Hitbox)&&Main.projectile[i].type != mod.ProjectileType<HeresySword>()){
                    //projectile.damage += (int)(Main.projectile[i].damage*0.1f);
                    //Main.projectile[i].velocity.;
                }
            }*/
            if (modPlayer.channelsword > 0 && projectile.timeLeft < 590){
                projectile.timeLeft = 575;
            }
            if(player.controlUseTile){
                if(stab==0){
                    for (int i = 0; i < 5; i++)Dust.NewDust((new Vector2(0, 42).RotatedBy(projectile.rotation-MathHelper.ToRadians(45))), 0, 0, 90, newColor:Color.Goldenrod, Scale:0.3f);
                    stab--;
                }else if(stab<0){
                    if(stab--<-3)stab=8;
                }else{
                    stab--;
                }
            }else{
                stab=0;
            }
            if (modPlayer.channelsword == 0){
                projectile.Kill();
            }
            //ray display
            //Vector2 pos2d = (new Vector2(0,50).RotatedBy(projectile.rotation-MathHelper.ToRadians(45)));
            /*for (int i = 0; i < 10; i++){
                Dust a = Dust.NewDustPerfect(((pos2d)*(2.5f*i/10))+player.MountedCenter, 90, newColor:Color.Goldenrod);
                a.noGravity = true;
                a.velocity*=0;
            }*/
            Vector2 pos2d2 = (new Vector2(0, (player.controlUseTile&&stab<=0)?62:42).RotatedBy(projectile.rotation-MathHelper.ToRadians(45)));
            HB.Position = new Vector3(((pos2d2)*0.25f)+player.MountedCenter,0);
            HB.Direction = new Vector3(((pos2d2)*2.45f),0);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            Player player = Main.player[projectile.owner];
            GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
            if(projectile.timeLeft>=599){
                oldPos = projectile.position;
                crit = true;
                knockback=0;
                modPlayer.channelsword+=5;
                stab--;
                oldPos2 = oldPos-(HB.Direction.toVector2()/12);
                target.velocity += (projectile.position-oldPos2)*0.85f;
                projectile.GetGlobalProjectile<GunGlobalProjectile>().firedwith.PostShoot(Projectile.NewProjectile(projectile.Center, new Vector2(), mod.ProjectileType<HeresyAlt>(), damage, 0, player.whoAmI));
                projectile.Kill();
                return;
            }
            if(projectile.timeLeft>=598)oldPos2 = oldPos;
            damage=(int)(damage*(projectile.position-oldPos2).Length()/16);
            knockback=0;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            if(projectile.timeLeft>=599)oldPos = projectile.position;
            if(projectile.timeLeft>=598)oldPos2 = oldPos;
            target.velocity += (projectile.position-oldPos2)*0.85f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            spriteBatch.Draw(mod.GetTexture("Items/Heresy"), projectile.Center-Main.screenPosition-new Vector2(1,0), null, Color.White, projectile.rotation+MathHelper.ToRadians(90), new Vector2(25f), 1, SpriteEffects.None, 0);
            return false;
        }
        public override bool? CanHitNPC(NPC target){
            if(!projectile.Hitbox.Intersects(target.Hitbox))return false;
            Player player = Main.player[projectile.owner];
            BoundingBox THB = new BoundingBox();
            THB.Min = new Vector3(target.Hitbox.X, target.Hitbox.Y, -1);
            THB.Max = new Vector3(target.Hitbox.X+target.Hitbox.Width, target.Hitbox.Y+target.Hitbox.Height, 1);
            return (HB.Intersects(THB)!=null)&&(!target.friendly)&&projectile.position!=oldPos;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            //hitbox.X-=Main.player[projectile.owner].width;
            hitbox = HB.ProperBox();
        }
    }
}