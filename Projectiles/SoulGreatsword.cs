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

    public class SoulGreatsword : ModProjectile
    {
        public override void SetDefaults()
        {
            //projectile.name = "Ice Shield";  //projectile name
            projectile.width = 25;       //projectile width
            projectile.height = 25;  //projectile height
            projectile.friendly = true;      //make that the projectile will not damage you
            projectile.magic = true;         // 
            projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = -1;      //how many npc will penetrate
            projectile.timeLeft = 3600;   //how many time this projectile has before disepire
            projectile.light = 0.75f;    // projectile light
            projectile.extraUpdates = 0;
            projectile.ignoreWater = true;   
			projectile.aiStyle = 0;
            projectile.ownerHitCheck = true;
            projectile.scale = 2;
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Some Kind of Sword");
		}
        public override void AI(){
            Player player = Main.player[projectile.owner];
            if(player.itemAnimation<=0){
                projectile.Kill();
            }else{
                projectile.rotation = (((player.itemAnimationMax/player.itemAnimation)*0.25f)-1.5f)*player.direction;
                projectile.Center = player.Center+new Vector2(0, 48).RotatedBy(projectile.rotation*100);
            }
        }
        public override bool? CanHitNPC(NPC target){
            Player player = Main.player[projectile.owner];
            if(player.itemAnimation>12)return false;
            return true;
        }
        public override bool CanHitPlayer(Player target){
            Player player = Main.player[projectile.owner];
            if(player.itemAnimation>12)return false;
            return true;
        }
        /*public override void ModifyDamageHitbox(ref Rectangle hitbox){
            hitbox.Inflate(hitbox.Width,hitbox.Height);
        }*/
    }
}