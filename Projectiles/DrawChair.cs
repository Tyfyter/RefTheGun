using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class DrawChair : ModProjectile
    {
        public override string Texture{
            get {return "RefTheGun/Items/GrenadeLawnchair";}
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DrawnChair (You shouldn't be seeing this in-game, but at least you got to see the pun)");
		}
        public override void SetDefaults()
        {
            projectile.width = mod.GetTexture("Items/GrenadeLawnchair").Width;
            projectile.height = mod.GetTexture("Items/GrenadeLawnchair").Height;
            projectile.penetrate = -1;
            projectile.timeLeft = 5;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
        }
        public override void AI(){
            Player player = Main.player[projectile.owner];
            projectile.rotation = (float)Math.Atan2((projectile.Center - Main.MouseWorld).Y, (projectile.Center - Main.MouseWorld).X) + 3.1157f;
            projectile.Center = player.MountedCenter+new Vector2(0,-16).RotatedBy(-(new Vector2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X).ToRotation()))-new Vector2(9*player.direction,0);
            projectile.velocity = projectile.velocity.Normalized()/10;
            player.GetModPlayer<GunPlayer>().Chairesy = true;
            if (player.controlUseItem){
                if(projectile.timeLeft<9)projectile.timeLeft = 24;
            }else{
                projectile.Kill();
            }
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor){
            spriteBatch.Draw(mod.GetTexture("Items/GrenadeLawnchair"), projectile.Center - Main.screenPosition, new Rectangle(0,0,mod.GetTexture("Items/GrenadeLawnchair").Width,mod.GetTexture("Items/GrenadeLawnchair").Height), lightColor, (float)(projectile.rotation-(Math.PI/2)), new Rectangle(0,0,mod.GetTexture("Items/GrenadeLawnchair").Width,mod.GetTexture("Items/GrenadeLawnchair").Height).Center(), 0.75f, ((int)(projectile.timeLeft/8))%2==0?SpriteEffects.FlipHorizontally:SpriteEffects.None, 1);
            return false;
        }
        public override bool? CanHitNPC(NPC target){
            return false;
        }
        public override bool CanHitPlayer(Player target){
            return false;
        }
        public override bool CanHitPvp(Player target){
            return false;
        }
    }
}