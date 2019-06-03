using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class MallNinjaStab : ModProjectile
    {
        public override string Texture{
            get {return "RefTheGun/Items/MallNinjaGun";}
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mall Ninja Stab");
		}
        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 16;
            projectile.height = 16;
            projectile.penetrate = -1;
            projectile.timeLeft = 5;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.hostile = false;
        }
        public override void AI(){
            Player player = Main.player[projectile.owner];
            projectile.rotation = (float)Math.Atan2((projectile.Center - Main.MouseWorld).Y, (projectile.Center - Main.MouseWorld).X) + 3.1157f;
            projectile.Center = player.MountedCenter+new Vector2(0,((GunPlayer)player).Reloading?-12:-32).RotatedBy(-(new Vector2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X).ToRotation()))-new Vector2(9*player.direction,0);
            projectile.velocity = projectile.velocity.Normalized()/10;
            if(!((GunPlayer)player).Reloading){
                if(player.itemAnimation>0){
                    projectile.timeLeft = 16;
                }else{
                    projectile.Kill();
                }
            }
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor){
            try{
                if(!((GunPlayer)Main.player[projectile.owner]).Reloading){
                    spriteBatch.Draw(mod.GetTexture("Items/MallNinjaGun"), projectile.Center - Main.screenPosition, new Rectangle(37,22,21,6), lightColor, projectile.rotation, new Rectangle(0,0,10,10).Center(), 1f, Main.player[projectile.owner].direction==1?SpriteEffects.None:SpriteEffects.FlipVertically, 0f);
                }else{
                    switch (projectile.timeLeft%4){
                        case 0:
                        spriteBatch.Draw(mod.GetTexture("Items/MallNinjaSpin4"), projectile.Center - Main.screenPosition, new Rectangle(0,0,18,18), lightColor, projectile.rotation, new Rectangle(0,0,18,18).Center(), 1f, Main.player[projectile.owner].direction==1?SpriteEffects.None:SpriteEffects.FlipVertically, 0f);
                        break;
                        case 3:
                        spriteBatch.Draw(mod.GetTexture("Items/MallNinjaSpin3"), projectile.Center - Main.screenPosition, new Rectangle(0,0,18,18), lightColor, projectile.rotation, new Rectangle(0,0,18,18).Center(), 1f, Main.player[projectile.owner].direction==1?SpriteEffects.None:SpriteEffects.FlipVertically, 0f);
                        break;
                        case 2:
                        spriteBatch.Draw(mod.GetTexture("Items/MallNinjaSpin2"), projectile.Center - Main.screenPosition, new Rectangle(0,0,18,18), lightColor, projectile.rotation, new Rectangle(0,0,18,18).Center(), 1f, Main.player[projectile.owner].direction==1?SpriteEffects.None:SpriteEffects.FlipVertically, 0f);
                        break;
                        case 1:
                        spriteBatch.Draw(mod.GetTexture("Items/MallNinjaSpin1"), projectile.Center - Main.screenPosition, new Rectangle(0,0,18,18), lightColor, projectile.rotation, new Rectangle(0,0,18,18).Center(), 1f, Main.player[projectile.owner].direction==1?SpriteEffects.None:SpriteEffects.FlipVertically, 0f);
                        break;
                        default:
                        break;
                    }
                }
            }
            catch (System.Exception){return true;}
            return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            Player player = Main.player[projectile.owner];
            hitbox.X+=(int)(hitbox.Center.X-player.MountedCenter.X);
            hitbox.Y+=(int)(hitbox.Center.Y-player.MountedCenter.Y);
        }
    }
}