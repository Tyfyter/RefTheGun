using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class Combustion : ModProjectile
    {
        public override String Texture{
            get {return "Terraria/Projectile_694";}
        }
        //int[] dustids = new int[4]{184,44};
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
			DisplayName.SetDefault("Combustion");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2ExplosiveTrapT1Explosion);
            projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            target.velocity+=projectile.velocity*0.75f;
        }
    }
    /*
    public class FireCloud : ModProjectile
    {
        public override String Texture{
            get {return "RefTheGun/Projectiles/ReloadProjTick";}
        }
        int[] dustids = new int[4]{6,35,127,158};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Cloud");
		}
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.penetrate = -1;
            projectile.timeLeft = 90;
            projectile.alpha = 25;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.friendly = true; 
        }
        public override void AI(){

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(projectile.Center+new Vector2(0,new Random().Next(0,64)).RotatedByRandom(50),0,0,dustids[new Random().Next(0,4)],newColor:lightColor);
            }
            return false;
        }
    } 
    //*/
}