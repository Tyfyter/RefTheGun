using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class NeedlerProj : ModProjectile
    {
        public override String Texture{
            get {return "Terraria/Projectile_"+ProjectileID.BoneArrow;}
        }
        public override bool CloneNewInstances => true;
        bool embedded = false;
        bool tileembedded = false;
        Vector2 embeddedpos = new Vector2();
        int embeddedin = -1;
        //bool explode = false;
        bool explode {
            get{return explodes>0;}
            set{explodes=explodes+(value?1:-1);}
        }
        int explodes = 0;
        public int target = -1;
        public Color color = Color.DarkMagenta;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Needle");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Shuriken);
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.timeLeft = 600;
            projectile.aiStyle = 0;
            projectile.width = 1;
            projectile.height = 1;
            projectile.extraUpdates+=4;
        }
        public override void AI(){
            if(projectile.velocity.Length()>0)projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X)+MathHelper.ToRadians(60);
            int a = Dust.NewDust(projectile.Center, 0, 0, 62, 0f, 0f, 0, default(Color), 0.85f);
            Main.dust[a].velocity *= 0f;
            Main.dust[a].noGravity = true;
            if(embedded){
                if(!Main.npc[embeddedin].active){
                    explode = true;
                    embedded = false;
                    tileembedded = false;
                    Rectangle hb = projectile.Hitbox;
                    hb.Inflate(15,15);
                    projectile.Hitbox = hb;
                }
                projectile.Center = Main.npc[embeddedin].Center+embeddedpos;
            }else if(target>=0){
                homing();
            }
            if(!explode){
                if(embedded||tileembedded){
                    List<int> needles = new List<int>(){};
                    for(int i = 0; i < projectile.GetGlobalProjectile<GunGlobalProjectile>().firedwith.firedshots.Count; i++){
                        Projectile p = Main.projectile[projectile.GetGlobalProjectile<GunGlobalProjectile>().firedwith.firedshots[i]];
                        if(p.type != projectile.type||!p.active)continue;
                        if(embedded&&((NeedlerProj)p.modProjectile).embeddedin==embeddedin){
                            needles.Add(p.whoAmI);
                            continue;
                        }
                        if(tileembedded&&(p.Center-projectile.Center).Length()<24){
                            needles.Add(p.whoAmI);
                            continue;
                        }
                    }
                    if(needles.Count>6){
                        for (int i = 0; i < needles.Count; i++){
                            ((NeedlerProj)Main.projectile[needles[i]].modProjectile).explode = true;
                            ((NeedlerProj)Main.projectile[needles[i]].modProjectile).embedded = false;
                            ((NeedlerProj)Main.projectile[needles[i]].modProjectile).tileembedded = false;
                            Rectangle hb = Main.projectile[needles[i]].Hitbox;
                            hb.Inflate(15,15);
                            Main.projectile[needles[i]].Hitbox = hb;
                            Main.projectile[needles[i]].damage=(int)(Main.projectile[needles[i]].damage*(3-Math.Min(((NeedlerProj)Main.projectile[needles[i]].modProjectile).explodes,1.95f)));
                            //if(Main.projectile[needles[i]].damage>=10000)Main.NewText(explodes);
                        }
                    }
                }
            }else {
                if(explodes>2){
                    for (int i = 0; i < Main.projectile.Length; i++){
                        if(Main.projectile[i].hostile){
                            if(Main.projectile[i].Hitbox.Intersects(projectile.Hitbox))Main.projectile[i].Kill();
                        }
                    }
                }
                if(projectile.timeLeft > 1)projectile.timeLeft = 1;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            //spriteBatch.Draw(mod.GetTexture("Projectiles/HexDaggerProj"), projectile.Center - Main.screenPosition + new Vector2(5, 8), new Rectangle(0, 0, 20, 32), Color.Lerp(color, lightColor, 0.2f), projectile.rotation, new Vector2(20,0), 0.65f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity){
            projectile.position+=oldVelocity;
            projectile.velocity = new Vector2();
            tileembedded = true;
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            if(!embedded){
                projectile.velocity = new Vector2();
                embedded = true;
                embeddedpos = projectile.Center-target.Center;
                embeddedin = target.whoAmI;
                projectile.tileCollide = false;
            }
            if (explode){
                damage+=(int)(target.defense*0.55f);
                if(explodes>3)target.GetGlobalNPC<GunGlobalNPC>().DMGBuffs.Add(new float[]{1.05f,200,6,projectile.type});
            }
        }
        public override bool? CanHitNPC(NPC target){
            if(embedded||tileembedded){
                return false;
            }else{
                return null;
            }
        }
        public override void Kill(int timeLeft){
            for(int i = 0; i<(projectile.width+projectile.height)/2; i++){
                Vector2 off = new Vector2(Main.rand.Next((projectile.width+projectile.height)/2),0).RotatedByRandom(MathHelper.ToRadians(180));
                int a = Dust.NewDust(projectile.Center+off, 0, 0, 62, 0f, 0f, 0, default(Color), 0.95f);
                Main.dust[a].velocity = off*0.1f;
                Main.dust[a].noGravity = true;
            }
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI){
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough){
            width/=40;
            height/=40;
            return true;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            hitbox.Width/=2;
            hitbox.Height/=2;
            hitbox.X = hitbox.X + hitbox.Width/2;
            hitbox.Y = hitbox.Y + hitbox.Height/2;
        }
        void homing(){
            if (projectile.alpha < 170){
                int num;
                for (int num164 = 0; num164 < 10; num164 = num + 1){
                    float x2 = projectile.Center.X;// - projectile.velocity.X / 10f * (float)num164;
                    float y2 = projectile.Center.Y;// - projectile.velocity.Y / 10f * (float)num164;
                    int num165 = Dust.NewDust(new Vector2(x2, y2), 0, 0, 62, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[num165].alpha = projectile.alpha;
                    Main.dust[num165].position.X = x2;
                    Main.dust[num165].position.Y = y2;
                    Dust dust3 = Main.dust[num165];
                    dust3.velocity *= 0f;
                    Main.dust[num165].noGravity = true;
                    num = num164;
                }
            }
            float dist = (float)Math.Sqrt((double)(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y));
            float num167 = projectile.localAI[0];
            if (num167 == 0f)
            {
                projectile.localAI[0] = dist;
                num167 = dist;
            }
            if (projectile.alpha > 0){
                projectile.alpha -= 25;
            }
            if (projectile.alpha < 0){
                projectile.alpha = 0;
            }
            float posX = projectile.position.X;
            float posY = projectile.position.Y;
            bool flag4 = false;
            int num171 = 0;
            if (projectile.ai[1] == 0f)
            {
                if (target>=0){
                    if(!Main.npc[target].active){
                        target = -1;
                        return;
                    }
                    if (Main.npc[target].CanBeChasedBy(projectile, false) && (projectile.ai[1] == 0f || projectile.ai[1] == (float)(target + 1)))
                    {
                        float num173 = Main.npc[target].position.X + (float)(Main.npc[target].width / 2);
                        float num174 = Main.npc[target].position.Y + (float)(Main.npc[target].height / 2);
                        if (Collision.CanHit(new Vector2(projectile.position.X + (float)(projectile.width / 2), projectile.position.Y + (float)(projectile.height / 2)), 1, 1, Main.npc[target].position, Main.npc[target].width, Main.npc[target].height))
                        {
                            posX = num173;
                            posY = num174;
                            flag4 = true;
                            num171 = target;
                        }
                    }
                }
                if (flag4)
                {
                    projectile.ai[1] = (float)(num171 + 1);
                }
                flag4 = false;
            }
            if (projectile.ai[1] > 0f)
            {
                int num176 = (int)(projectile.ai[1] - 1f);
                if (Main.npc[num176].active && Main.npc[num176].CanBeChasedBy(projectile, true) && !Main.npc[num176].dontTakeDamage)
                {
                    float num177 = Main.npc[num176].position.X + (float)(Main.npc[num176].width / 2);
                    float num178 = Main.npc[num176].position.Y + (float)(Main.npc[num176].height / 2);
                    float num179 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num177) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num178);
                    if (num179 < 1000f)
                    {
                        flag4 = true;
                        posX = Main.npc[num176].position.X + (float)(Main.npc[num176].width / 2);
                        posY = Main.npc[num176].position.Y + (float)(Main.npc[num176].height / 2);
                    }
                }
                else
                {
                    projectile.ai[1] = 0f;
                }
            }
            if (!projectile.friendly)
            {
                flag4 = false;
            }
            if (flag4)
            {
                float num180 = num167;
                Vector2 vector19 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num181 = posX - vector19.X;
                float num182 = posY - vector19.Y;
                float num183 = (float)Math.Sqrt((double)(num181 * num181 + num182 * num182));
                num183 = num180 / num183;
                num181 *= num183;
                num182 *= num183;
                int num184 = 8;
                projectile.velocity.X = (projectile.velocity.X * (float)(num184 - 1) + num181) / (float)num184;
                projectile.velocity.Y = (projectile.velocity.Y * (float)(num184 - 1) + num182) / (float)num184;
            }
        }
    }
}