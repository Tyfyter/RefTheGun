using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static RefTheGun.RefTheExtensions;

namespace RefTheGun.Projectiles
{

    public class HeresyAlt : ModProjectile
    {
        public override bool CloneNewInstances => true;
        int start = 0;
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 28;
			DisplayName.SetDefault("Heresy");
		}
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.penetrate = -1;
            projectile.timeLeft = 28;
            projectile.frame = (Main.rand.Next(0, 6)*4);
            start = projectile.frame;
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
            GunPlayer modPlayer = player.GetModPlayer<GunPlayer>();
            modPlayer.Heresy = true;
            projectile.rotation = (float)Math.Atan2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X) + 3.1157f;
            projectile.Center = player.MountedCenter+new Vector2(-12,0)+new Vector2(0,-16).RotatedBy(-(new Vector2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X).ToRotation()));
            projectile.velocity = new Vector2();
            Vector2 off = new Vector2(0,-1.3f).RotatedBy(-(new Vector2((player.Center - Main.MouseWorld).Y, (player.Center - Main.MouseWorld).X).ToRotation()));
            player.velocity+=(player.velocity.Y==0&&!RefTheExtensions.isWithin((Main.MouseWorld-player.Center).ToRotation(),105,255))?new Vector2(off.Length()*(off.X/Math.Abs(off.X)),0):new Vector2(off.X*(RefTheExtensions.isWithin((Main.MouseWorld-player.Center).ToRotation(),135,225)?1.5f:0.5f),off.Y*(RefTheExtensions.isWithin((Main.MouseWorld-player.Center).ToRotation(),135,225)?3:1));
            int targ = findNearestNPC(Main.MouseWorld, 1600, hostNPC);
            for(int i = 0; i<Main.projectile.Length; i++){
                //Projectile proj = Main.projectile[i];
                if(!Main.projectile[i].active)continue;
                Vector2 Pos = Main.projectile[i].Center;
                int dmg = Main.projectile[i].damage;
                if(projectile.Hitbox.Intersects(Main.projectile[i].Hitbox)&&Main.projectile[i].hostile){
                    Main.projectile[i].Kill();
                    if(targ==-1)continue;
                    int a = Projectile.NewProjectile(Pos, new Vector2(), ProjectileID.SpectreWrath, dmg, projectile.knockBack*0.85f, projectile.owner, targ);
                    Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().firedwith = projectile.GetGlobalProjectile<GunGlobalProjectile>().firedwith;
                    Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
                    Main.projectile[a].GetGlobalProjectile<GunGlobalProjectile>().TrackHitEnemies = true;
                }
            }
            if(projectile.timeLeft%2==0)if(++projectile.frame-start==4){
            }else if(projectile.frame-start==8){
                modPlayer.channelsword = 5;
                int b = Projectile.NewProjectile(projectile.Center-new Vector2(25), new Vector2(), mod.ProjectileType<HeresySword>(), projectile.damage, 0, projectile.owner);
                if(player.velocity.Y==0)Main.projectile[b].GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures = true;
                projectile.Kill();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return projectile.frame-start<4;
        }
        public override bool? CanHitNPC(NPC target){
            return projectile.frame-start<4&&!target.friendly;
        }
    }
}