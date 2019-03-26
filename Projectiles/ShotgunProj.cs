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

    public class ShotgunProj : ModProjectile
    {
        
        public override String Texture{
            get {return "Terraria/Item_"+ItemID.Boomstick;}
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shotgun");
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
        }
        public override void Kill(int timeLeft){
            Projectile a;
            for (int i = 0; i < Main.rand.Next(3,5); i++){
                a = Projectile.NewProjectileDirect(projectile.Center, new Vector2(projectile.oldVelocity.Length()*1.25f, 0).RotatedBy(projectile.rotation).RotatedByRandom(0.75f), (int)projectile.ai[0], (int)(projectile.damage/1.25), projectile.knockBack, projectile.owner);
                a.friendly = true;
                a.hostile = false;
            }
            Main.PlaySound(Shell.useSound.SoundId, projectile.Center, Shell.useSound.Style);
        }
    }
}