using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class FarronSlash : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 28;
			DisplayName.SetDefault("Farron Slash");
		}
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.penetrate = -1;
            projectile.timeLeft = 7;
            projectile.frame = (Main.rand.Next(0, 3)*7);
            projectile.alpha = 25;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.ownerHitCheck = true;
            projectile.hide = false;
            projectile.friendly = true;  
			projectile.usesLocalNPCImmunity = true;
        }
        public override void AI(){
            Player player = Main.player[projectile.owner];
            projectile.rotation = (float)Math.Atan2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X) + 3.1157f;
            projectile.Center = player.MountedCenter+new Vector2(0,-16).RotatedBy(-(new Vector2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X).ToRotation()));
            projectile.velocity = new Vector2();
            foreach (Projectile proj in Main.projectile)
            {
                if(projectile.Hitbox.Intersects(proj.Hitbox)&&proj!=projectile&&proj.hostile){
                    proj.Kill();
                }
            }
            projectile.frame++;
        }
    }
}