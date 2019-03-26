using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.Items;
using RefTheGun.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class GunProj : ModProjectile
    {
        
        public override String Texture{
            get {return "RefTheGun/Items/Sidearm";}
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gun");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Shuriken);
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 3;
            projectile.aiStyle = 0;
            projectile.penetrate = 1;
        }
        public override void AI(){
            projectile.rotation+=0.2f;
            if(projectile.timeLeft%((int)projectile.ai[1]==0?15:(int)projectile.ai[1])==0){
                Projectile a = Projectile.NewProjectileDirect(projectile.Center, new Vector2(projectile.velocity.Length()*1.5f, 0).RotatedBy(projectile.rotation), (int)projectile.ai[0], (int)(projectile.damage/2.5), projectile.knockBack, projectile.owner);
                a.friendly = true;
                a.hostile = false;
                Main.PlaySound(Bullet.useSound.SoundId, projectile.Center, Bullet.useSound.Style);
            }
        }
        /*public override bool PreKill(int timeLeft){
            aiType = (int)projectile.ai[0];
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity){
            if(aiType != (int)projectile.ai[0]){
                aiType = (int)projectile.ai[0];
                projectile.velocity = oldVelocity;
                return false;
            }else{
                return base.OnTileCollide(oldVelocity);
            }
        }*/
    }
}