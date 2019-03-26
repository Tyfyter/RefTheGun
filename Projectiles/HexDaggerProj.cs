using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class HexDaggerProj : ModProjectile
    {
        public override String Texture{
            get {return "Terraria/Projectile_"+ProjectileID.BoneArrow;}
        }
        public override bool CloneNewInstances => true;
        bool returning = false;
        bool embedded = false;
        Vector2 embeddedpos = new Vector2();
        int embeddedin = 0;
        public Color color = Color.OrangeRed;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dagger");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Shuriken);
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.aiStyle = 0;
            projectile.width = 17;
            projectile.height = 17;
            projectile.extraUpdates+=4;
        }
        public override void AI(){
            if(projectile.velocity.Length()>0)projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X)+MathHelper.ToRadians(60);
            if(embedded){
                if(!Main.npc[embeddedin].active){
                    PreKill(projectile.timeLeft);
                    goto postembed;
                }
                projectile.Center = Main.npc[embeddedin].Center+embeddedpos;
            }
            if(projectile.timeLeft<5){
                projectile.timeLeft = 600;
                PreKill(projectile.timeLeft);
            }
            postembed:
            if(returning){
                projectile.velocity = (Main.player[projectile.owner].Center-projectile.Center);
                if(projectile.velocity.Length()<16){
                    projectile.Kill();
                }
                projectile.velocity.Normalize();
                projectile.velocity*=3;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            spriteBatch.Draw(mod.GetTexture("Projectiles/HexDaggerProj"), projectile.position - Main.screenPosition + new Vector2(5, 8), new Rectangle(0, 0, 20, 32), Color.Lerp(color, lightColor, 0.2f), projectile.rotation, new Vector2(20,0), 0.65f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity){
            projectile.position+=oldVelocity;
            projectile.velocity = new Vector2();
            return false;
        }
        public override bool PreKill(int timeLeft){
            projectile.timeLeft = 600;
            if (!returning){
                returning = true;
                embedded = false;
                projectile.tileCollide = false;
                projectile.localNPCHitCooldown = 15;
                try{    
                    for (int i = 0; i < projectile.localNPCImmunity.Length; i++){
                        projectile.localNPCImmunity[i] = 0;
                    }
                }catch (System.Exception){}
                return false;
            }
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            if(!embedded&&!returning){
                //projectile.position+=projectile.velocity;
                projectile.velocity = new Vector2();
                projectile.damage = (int)(projectile.damage*1.5);
                embedded = true;
                embeddedpos = projectile.Center-target.Center;
                embeddedin = target.whoAmI;
                projectile.tileCollide = false;
                projectile.localNPCHitCooldown = 60;
            }else if(embedded){
                damage = (int)(damage*0.2);
                knockback*=0.1f;
            }
        }
        public override void Kill(int timeLeft){
            projectile.GetGlobalProjectile<GunGlobalProjectile>().firedwith.Ammo++;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI){
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough){
            width/=40;
            height/=40;
            return true;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            hitbox.Width/=2;
            hitbox.Height/=2;
            hitbox.X = hitbox.X + hitbox.Width/2;
            hitbox.Y = hitbox.Y + hitbox.Height/2;
        }
    }
}