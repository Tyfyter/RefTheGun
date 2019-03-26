using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class FightsabreSlash : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 28;
			DisplayName.SetDefault("Fightsabre Slash");
		}
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.penetrate = -1;
            projectile.timeLeft = 45;
            projectile.alpha = 25;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.ownerHitCheck = true;
            projectile.hide = false;
            projectile.friendly = true;  
        }
        public override void AI(){
            Player player = Main.player[projectile.owner];
            projectile.rotation = (float)Math.Atan2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X) + 3.1157f;
            projectile.Center = player.MountedCenter+new Vector2(0,-16).RotatedBy(-(new Vector2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X).ToRotation()))-new Vector2(8,0);
            projectile.velocity = new Vector2();
            foreach (Projectile proj in Main.projectile)
            {
                if(projectile.Hitbox.Intersects(proj.Hitbox)&&proj!=projectile){
                    proj.velocity += (proj.Center-player.Center).SafeNormalize(proj.velocity*-2)*proj.velocity.Length();
                    proj.friendly = true;
                    proj.owner = projectile.owner;
                }
            }
            //if(projectile.timeLeft%2==0)projectile.frame++;
            //projectile.frame = (int)(projectile.timeLeft*0.62f);
            projectile.frame = projectile.timeLeft % 28;
            /*if (++projectile.frame >= 14)
            {
                projectile.Kill();
                //projectile.frame = 0;
            }*/
        }

        /*public override void DrawBehind(int index, System.Collections.Generic.List<int> drawCacheProjsBehindNPCsAndTiles, System.Collections.Generic.List<int> drawCacheProjsBehindNPCs, System.Collections.Generic.List<int> drawCacheProjsBehindProjectiles, System.Collections.Generic.List<int> drawCacheProjsOverWiresUI){
            //drawCacheProjsBehindNPCs.Clear();
            //drawCacheProjsBehindNPCsAndTiles.Clear();
            //drawCacheProjsBehindProjectiles.Clear();
            drawHeldProjInFrontOfHeldItemAndArms = true;
        }*/
    }
}