using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Enums;

namespace RefTheGun.Projectiles
{

    public class LifehuntScythe : ModProjectile
    {
        //I remembered to make this spell, right?
        public override string Texture{
            get {return "Terraria/Item_"+ItemID.DeathSickle;}
        }
        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 3600;
            projectile.light = 0.75f;
            projectile.extraUpdates = 0;
            projectile.ignoreWater = true;   
			projectile.aiStyle = 0;
            projectile.ownerHitCheck = true;
            projectile.scale = 1;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lifehunt Scythe");
		}
        public override void AI(){
            Player player = Main.player[projectile.owner];
            if(player.itemAnimation<=0){
                projectile.Kill();
            }else{
                projectile.rotation = (((player.itemAnimationMax-player.itemAnimation)*-0.5f)-1.51f)*player.direction;
                projectile.Center = player.Center+(projectile.velocity*2)+new Vector2(0, -48).RotatedBy(projectile.rotation*100);
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
			Projectile.NewProjectile(target.Center, new Vector2(), ProjectileID.SpiritHeal, 0, 0, projectile.owner, ai1:damage/7);
        }
        public override void OnHitPvp(Player target, int damage, bool crit){
			Projectile.NewProjectile(target.Center, new Vector2(), ProjectileID.SpiritHeal, 0, 0, projectile.owner, ai1:damage/7);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit){
			Projectile.NewProjectile(target.Center, new Vector2(), ProjectileID.SpiritHeal, 0, 0, projectile.owner, ai1:damage/7);
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor){
            int OverrideTextureMode = 1;
            spriteBatch.Draw(Main.itemTexture[ItemID.DeathSickle], projectile.Center - Main.screenPosition, new Rectangle(0,0,Main.itemTexture[ItemID.DeathSickle].Width,Main.itemTexture[ItemID.DeathSickle].Height), new Color(72,61,139,175), -projectile.rotation+Main.player[projectile.owner].direction, OverrideTextureMode==0? new Rectangle(0,0,Main.itemTexture[ItemID.DeathSickle].Width,Main.itemTexture[ItemID.DeathSickle].Height).Center():OverrideTextureMode==1? new Rectangle(0,0,Main.itemTexture[ItemID.DeathSickle].Width,0).Center():new Rectangle(0,0,0,Main.itemTexture[ItemID.DeathSickle].Height).Center(), 1f, SpriteEffects.FlipHorizontally, 0f);
            Dust.NewDustPerfect(projectile.Center, 186, newColor:new Color(72,61,139,175));
            return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            Player player = Main.player[projectile.owner];
            hitbox.Inflate(hitbox.Width,hitbox.Height);
        }
    }
}