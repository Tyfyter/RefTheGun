using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static RefTheGun.RefTheExtensions;

namespace RefTheGun.Projectiles
{
    public class WrathoftheGods : ModProjectile
    {
        public override String Texture{
            get {return "Terraria/Projectile_"+ProjectileID.Electrosphere;}
        }
        //int[] dustids = new int[4]{184,44};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath of the Gods");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Electrosphere);
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.timeLeft = 3;
            projectile.aiStyle = 0;
        }
        public override void AI(){
            if(((int)projectile.ai[0]&2)==0){
                projectile.scale = 1+(6/projectile.timeLeft);
            }else{
                projectile.scale = 1+(6/(float)projectile.timeLeft);
            }
            if(!projectile.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures)Main.player[projectile.owner].velocity*=0.92f;
            for(int i = 0; i < 100; i++){
                Dust a = Dust.NewDustPerfect(projectile.Center+new Vector2(0,(projectile.height/2)*projectile.scale).RotatedBy(i), projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor==Color.DarkRed?105:133);
                a.noGravity = true;
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            Rectangle hitbox = projectile.Hitbox;
            if(((int)projectile.ai[0]&1)==0)hitbox.Inflate((int)((hitbox.Width/2)*projectile.scale),(int)((hitbox.Height/2)*projectile.scale));
            if(((int)projectile.ai[0]&4)!=0)hitbox = target.Hitbox;
            damage = (int)(damage*((192-(Rectangle.Intersect(target.Hitbox, hitbox).Center()-projectile.Center).Length())*0.02f));
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            Rectangle hitbox = projectile.Hitbox;
            if(((int)projectile.ai[0]&1)==0)hitbox.Inflate((int)((hitbox.Width/2)*projectile.scale),(int)((hitbox.Height/2)*projectile.scale));
            if(((int)projectile.ai[0]&8)!=0)hitbox = target.Hitbox;
            target.velocity+=((Rectangle.Intersect(target.Hitbox, hitbox).Center()-projectile.Center).Normalized()*((192-(target.Center-projectile.Center).Length())*0.3f))/(projectile.ai[0]==-1?1:projectile.ai[0]+1);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            if(((int)projectile.ai[0]&1)==0)hitbox.Inflate((int)((hitbox.Width/2)*projectile.scale),(int)((hitbox.Height/2)*projectile.scale));
            foreach(Projectile target in Main.projectile){
                if(target.Hitbox.Intersects(hitbox)&&(projectile.owner!=target.owner||!target.friendly||projectile.whoAmI==target.whoAmI)&&target.type!=ModContent.ProjectileType<SoulArrow>()){
                    target.velocity+=((((int)projectile.ai[0]&16)!=0)?(Rectangle.Intersect(target.Hitbox, hitbox).Center()):target.Center-projectile.Center).Normalized()*((192-(target.Center-projectile.Center).Length())*0.3f);
                    target.friendly = true;
                    if(((int)projectile.ai[0]&32)==0&&target.whoAmI!=projectile.whoAmI)target.damage+=projectile.damage;
                }
            }
        }
    }
    /**
    ai:
        0:growth
        1:knockback
    aioverflow:
        0:damage
        1:size
        2:dust
        3:dust range
        4:damage
        5:knockback
        6:
            1:kill projectiles
            2:
            4:
            8:
    */
    public class WrathoftheTitans : ModProjectile
    {
        public override String Texture{
            get {return "Terraria/Projectile_"+ProjectileID.Electrosphere;}
        }
        //int[] dustids = new int[4]{184,44};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath of the Titans");
		}
        bool init = true;
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Electrosphere);
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            if(!init)return;
            projectile.timeLeft = 3;
            projectile.aiStyle = 0;
            projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0] = 6;
            projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[2] = 133;
            init = false;
        }
        public override void AI(){
            projectile.scale = 1+(projectile.ai[0]/(float)projectile.timeLeft)+(float)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[1];
            if(!projectile.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures)Main.player[projectile.owner].velocity*=0.95f;
            if(projectile.timeLeft<10||projectile.timeLeft%5==0)for(int i = 0; i < 100; i++){
                Dust a = Dust.NewDustPerfect(projectile.Center+new Vector2(0,(projectile.height/2)*projectile.scale*Main.rand.NextFloat(1-(float)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[3],1)).RotatedBy(i), (int)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[2], newColor:projectile.GetGlobalProjectile<GunGlobalProjectile>().OverrideColor?projectile.GetGlobalProjectile<GunGlobalProjectile>().Color:default(Color));
                a.noGravity = true;
            }
        }
        public override  void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            Rectangle hitbox = projectile.Hitbox;
            hitbox.Inflate((int)((hitbox.Width/2)*projectile.scale),(int)((hitbox.Height/2)*projectile.scale));
            damage = (int)(damage*(((projectile.height*projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[0])-(Rectangle.Intersect(target.Hitbox, hitbox).Center()-projectile.Center).Length())*0.02f)*(projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[4]==null?1:(float)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[4]));
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            Rectangle hitbox = projectile.Hitbox;
            hitbox.Inflate((int)((hitbox.Width/2)*projectile.scale),(int)((hitbox.Height/2)*projectile.scale));
            target.velocity+=(((target.Center-projectile.Center).Normalized()*(((projectile.height*projectile.ai[1])-(/*target.Center*/Rectangle.Intersect(target.Hitbox, hitbox).Center()-projectile.Center).Length())*0.3f))/(projectile.ai[0]==-1?1:projectile.ai[0]+1))*(projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[5]==null?1:(float)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[5]);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            hitbox.Inflate((int)((hitbox.Width/2)*projectile.scale),(int)((hitbox.Height/2)*projectile.scale));
            foreach(Projectile target in Main.projectile){
                if(target.Hitbox.Intersects(hitbox)&&(projectile.owner!=target.owner||!target.friendly||!projectile.Name.Contains("Lightning Arrow"))&&projectile.whoAmI!=target.whoAmI){
                    target.velocity+=((Rectangle.Intersect(target.Hitbox, hitbox).Center()-projectile.Center).Normalized()*(((projectile.height*projectile.ai[1])-(target.Center-projectile.Center).Length())*0.3f))*(projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[5]==null?1:(float)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[5]);
                    target.friendly = true;
                    if(target.owner!=projectile.owner&&((((projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[6]==null?0:(int)projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow[5])|1))!=0))target.Kill();
                    //Main.NewText(projectile.GetGlobalProjectile<GunGlobalProjectile>().aioverflow.Concat());
                }
            }
        }
    }
    public class ResonatingQuake : ModProjectile
    {
        public override String Texture{
            get {return "Terraria/Projectile_"+ProjectileID.Electrosphere;}
        }
        //int[] dustids = new int[4]{184,44};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Resonating Quake");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Electrosphere);
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 600;
            projectile.timeLeft = 3;
            projectile.aiStyle = 0;
        }
        public override void AI(){
            projectile.scale = 1+(29/projectile.timeLeft);
            for(int i = 0; i < 100; i++){
                Dust a = Dust.NewDustPerfect(projectile.Center+new Vector2(0,(projectile.height/2)*projectile.scale).RotatedBy(i), 132);
                a.noGravity = true;
            }
        }
        public override  void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            damage = (int)(damage*(((projectile.width*30)-(target.Center-projectile.Center).Length())*0.1f));
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit){
            target.velocity+=(target.Center-projectile.Center).Normalized()*(((projectile.width*30)-(target.Center-projectile.Center).Length())*0.3f);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            hitbox.Inflate((int)((hitbox.Width/2)*projectile.scale),(int)((hitbox.Height/2)*projectile.scale));
            foreach(Projectile target in Main.projectile){
                if(target.Hitbox.Intersects(hitbox))target.velocity+=(target.Center-projectile.Center).Normalized()*(((projectile.width*30)-(target.Center-projectile.Center).Length())*0.3f);
            }
        }
    }
}