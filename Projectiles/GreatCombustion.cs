using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class GreatCombustion : ModProjectile
    {
        public override String Texture{
            get {return "Terraria/Projectile_695";}
        }
        //int[] dustids = new int[4]{184,44};
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 4;
			DisplayName.SetDefault("Great Combustion");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2ExplosiveTrapT2Explosion);
            projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            target.velocity+=projectile.velocity*1.25f;
        }
    }
}