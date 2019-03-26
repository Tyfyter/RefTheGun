using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class LightningBow : ModProjectile
    {
        public override string Texture{
            get {return "Terraria/Projectile_630";}
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Bow");
		}
        int charge = 0;
        int chargephase = 0;
        bool playerinvul = false;
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Arkhalis);
            projectile.width = 48;
            projectile.height = 48;
            projectile.penetrate = -1;
            projectile.timeLeft = 45;
            projectile.alpha = 25;
            projectile.tileCollide = false;
            projectile.ownerHitCheck = true;
            Main.instance.LoadProjectile(630);
        }
        public override void AI(){
            Player player = Main.player[projectile.owner];
            projectile.rotation = (float)Math.Atan2((projectile.Center - Main.MouseWorld).Y, (projectile.Center - Main.MouseWorld).X) + 3.1157f;
            projectile.Center = player.MountedCenter+new Vector2(0,-16).RotatedBy(-(new Vector2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X).ToRotation()))-new Vector2(9*player.direction,0);
            projectile.velocity = projectile.velocity.Normalized()/10;
            if(!player.channel){
                Projectile proj = Projectile.NewProjectileDirect(projectile.Center, new Vector2(17.5f, 0).RotatedBy(projectile.rotation+MathHelper.ToRadians(1f)), mod.ProjectileType<SoulArrow>(), (int)(projectile.damage*((((float)chargephase)/5)+1)), projectile.knockBack, projectile.owner);
                proj.Name = chargephase>9?"Great Lightning Arrow":"Lightning Arrow";
                if(chargephase>9)proj.penetrate=-1;
                proj.extraUpdates+=(int)(((float)chargephase)/5);
                proj.GetGlobalProjectile<GunGlobalProjectile>().GlowColor = projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor;
                projectile.Kill();
            }
            if(charge<200){
                charge++;
            }
            if(charge%20==0){
                Main.PlaySound(new LegacySoundStyle(SoundID.MaxMana, 0).WithVolume(0.5f), projectile.Center);
                Color color = new Color(255,215,50);
                if(projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor!=new Color()){
                color = projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor;
                }
                for (int i = 0; i < 5; i++)Dust.NewDust(projectile.Center, 0, 0, DustID.Electric, newColor:color);
                if(chargephase<20)chargephase++;
            }
            if(player.immune){
                if (!playerinvul){
                    charge-=60;
                    chargephase-=12;
                }
                playerinvul = true;
            }else{
                playerinvul = false;
            }
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor){
            int OverrideTextureMode = 0;
            if(projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor!=new Color()){
                spriteBatch.Draw(Main.projectileTexture[630], projectile.Center - Main.screenPosition, new Rectangle(0,0,Main.projectileTexture[630].Width,Main.projectileTexture[630].Height), projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor, projectile.rotation, OverrideTextureMode==0? new Rectangle(0,0,Main.projectileTexture[630].Width,Main.projectileTexture[630].Height).Center():OverrideTextureMode==1? new Rectangle(0,0,Main.projectileTexture[630].Width,0).Center():new Rectangle(0,0,0,Main.projectileTexture[630].Height).Center(), 1f, SpriteEffects.None, 0f);
            }else{
                spriteBatch.Draw(Main.projectileTexture[630], projectile.Center - Main.screenPosition, new Rectangle(0,0,Main.projectileTexture[630].Width,Main.projectileTexture[630].Height), new Color(255,215,50,25), projectile.rotation, OverrideTextureMode==0? new Rectangle(0,0,Main.projectileTexture[630].Width,Main.projectileTexture[630].Height).Center():OverrideTextureMode==1? new Rectangle(0,0,Main.projectileTexture[630].Width,0).Center():new Rectangle(0,0,0,Main.projectileTexture[630].Height).Center(), 1f, SpriteEffects.None, 0f);
            }
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