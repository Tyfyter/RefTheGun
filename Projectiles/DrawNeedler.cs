using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class DrawNeedler : ModProjectile
    {
        public override string Texture{
            get {return "RefTheGun/Items/Needler";}
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DrawNeedler (You shouldn't be seeing this in-game, tell me if you do)");
		}
        public override void SetDefaults()
        {
            projectile.width = mod.GetTexture("Items/Needler").Width;
            projectile.height = mod.GetTexture("Items/Needler").Height;
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
            if (player.controlUseItem){
                projectile.timeLeft = 5;
            }else{
                projectile.Kill();
            }
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor){
            spriteBatch.Draw(mod.GetTexture("Items/Needler"), projectile.Center - Main.screenPosition, new Rectangle(0,0,mod.GetTexture("Items/Needler").Width,mod.GetTexture("Items/Needler").Height), lightColor, (float)(projectile.rotation%Math.PI), new Rectangle(0,0,mod.GetTexture("Items/Needler").Width,mod.GetTexture("Items/Needler").Height).Center(), 1f, SpriteEffects.None, 0f);
            return true;
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