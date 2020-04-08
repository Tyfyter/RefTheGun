using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class ToxinCloud : ModProjectile
    {
        public override String Texture{
            get {return "RefTheGun/Projectiles/ReloadProjTick";}
        }
        //int[] dustids = new int[4]{184,44};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Cloud");
		}
        public override void SetDefaults()
        {
            projectile.width = 0;
            projectile.height = 0;
            projectile.penetrate = -1;
            projectile.timeLeft = 90;
            projectile.alpha = 25;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.friendly = true; 
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 3;
        }
        public override void AI(){
            for (int i = 0; i < 2; i++)
            {
                //Vector2 vect = new Vector2(0,/*Main.rand.Next(0,64)*/64).RotatedByRandom(50);
                if(projectile.ai[0]!=1){
                    Projectile proj = Projectile.NewProjectileDirect(projectile.Center+new Vector2(0,Main.rand.Next(0,64)).RotatedByRandom(50), new Vector2(), projectile.type, projectile.damage, 0, projectile.owner, 1);
                    proj.timeLeft = 3;
                }else{
                    Dust a = Dust.NewDustDirect(projectile.Center,0,0,184);
                    a.alpha = 200;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            if(!target.HasBuff(BuffID.Venom)){
                target.AddBuff(BuffID.Venom, 240);
            }else{
                target.buffTime[target.FindBuffIndex(BuffID.Venom)]+=30;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
    public class AcidCloud : ModProjectile
    {
        public override String Texture{
            get {return "RefTheGun/Projectiles/ReloadProjTick";}
        }
        //int[] dustids = new int[4]{184,44};
        int[] dustids = new int[4]{35,127,158,184};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Cloud");
		}
        public override void SetDefaults()
        {
            projectile.width = 0;
            projectile.height = 0;
            projectile.penetrate = -1;
            projectile.timeLeft = 90;
            projectile.alpha = 25;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.friendly = true; 
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 3;
        }
        public override void AI(){
            for (int i = 0; i < 2; i++)
            {
                //Vector2 vect = new Vector2(0,/*Main.rand.Next(0,64)*/64).RotatedByRandom(50);
                if(projectile.ai[0]!=1){
                    Projectile proj = Projectile.NewProjectileDirect(projectile.Center+new Vector2(0,Main.rand.Next(0,64)).RotatedByRandom(50), new Vector2(0,Main.rand.Next(0,64)).RotatedByRandom(50), projectile.type, projectile.damage, 0, projectile.owner, 1);
                    proj.timeLeft = 3;
                }else{
                    Dust a = Dust.NewDustDirect(projectile.Center,0,0,dustids[Main.rand.Next(0,dustids.Length-1)], newColor:new Color(Color.YellowGreen.R/2,Color.YellowGreen.G/2,Color.YellowGreen.B/2, 100));
                    a.alpha = 200;
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            if(Main.rand.Next(0,9)==0){
                target.defense-=Main.rand.Next((int)(damage*0.9),(int)(damage*1.1));
            }if(Main.rand.Next(0,14)==0){
                for(int i = Main.rand.Next(0,14); i<20 ;i++)Dust.NewDustDirect(projectile.Center,0,0,4, newColor:new Color(Color.YellowGreen.R,Color.YellowGreen.G,Color.YellowGreen.B, 100));
                target.GetGlobalNPC<GunGlobalNPC>().dmgmult += (float)Main.rand.NextDouble();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            return false;
        }
    }
    /*
    public class FireCloud : ModProjectile
    {
        public override String Texture{
            get {return "RefTheGun/Projectiles/ReloadProjTick";}
        }
        int[] dustids = new int[4]{6,35,127,158};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxic Cloud");
		}
        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.penetrate = -1;
            projectile.timeLeft = 90;
            projectile.alpha = 25;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.friendly = true; 
        }
        public override void AI(){

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(projectile.Center+new Vector2(0,Main.rand.Next(0,64)).RotatedByRandom(50),0,0,dustids[Main.rand.Next(0,4)],newColor:lightColor);
            }
            return false;
        }
    } 
    //*/
}