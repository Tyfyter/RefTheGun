using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class ReloadProj : ModProjectile
    {
        public override bool CloneNewInstances => true;
        public bool reloaded = false;
        public override void SetDefaults()
        {
            //projectile.name = "Wind Shot";  //projectile name
            projectile.width = 64;       //projectile width
            projectile.height = 12;  //projectile height
            projectile.penetrate = -1;      //how many npcs will penetrate
            projectile.timeLeft = (int)projectile.ai[0];   //how many time this projectile has before it expipires
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("\"If you see this I made a mistake\"");
		}
        public override void AI()           //this make that the projectile will face the corect way
        {                                                           // |
            projectile.velocity = new Vector2();
            Player player = Main.player[projectile.owner];
            projectile.rotation = 0;
            if(!Main.player[projectile.owner].GetModPlayer<GunPlayer>(mod).Reloading)projectile.Kill();
            if(projectile.ai[1]!=Main.player[projectile.owner].selectedItem)projectile.Kill();
            projectile.Center = player.MountedCenter - new Vector2(0, player.height*0.75f);
            if(projectile.timeLeft <= 1){
                Main.player[projectile.owner].GetModPlayer<GunPlayer>(mod).Reloaded = true;
                reloaded = true;
            }
        }
        public override void Kill(int timeLeft){
            if(!reloaded)Main.player[projectile.owner].GetModPlayer<GunPlayer>(mod).Reloading = false;
        }

        
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            spriteBatch.Draw(mod.GetTexture("Projectiles/ReloadProjTick"), new Vector2(projectile.position.X - Main.screenPosition.X+64-((projectile.timeLeft/(projectile.ai[0])*0.9f)*64), projectile.position.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, 2, 12), lightColor, 0,
					new Vector2(), 1f, SpriteEffects.None, 0f);
            lightColor = Color.White;
            return true;
        }
    }
}