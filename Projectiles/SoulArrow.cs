using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Projectiles
{

    public class SoulArrow : ModProjectile
    {
        public override String Texture{
            get {return "Terraria/Projectile_"+ProjectileID.BoneArrow;}
        }
        //int[] dustids = new int[4]{184,44};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Arrow");
		}
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.BoneArrow);
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 3;
            projectile.aiStyle = 0;
            Main.instance.LoadProjectile(ProjectileID.BoneArrow);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
            if(projectile.Name=="Great Lightning Arrow"){
                crit=true;
                Projectile a = Projectile.NewProjectileDirect(projectile.Center, new Vector2(), ModContent.ProjectileType<WrathoftheGods>(), (int)(projectile.damage*0.75), 0, projectile.owner, 3);
                a.timeLeft = 10;
                a.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures=true;
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void AI(){
            if(projectile.velocity.Length()>0)projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f; 
            Color color = new Color();
            if(projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor!=new Color()){
                color = projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor;
            }else switch(projectile.Name){
                case "Soul Arrow":
                color = new Color(Color.Aqua.R*0.75f,Color.Aqua.G*0.75f,Color.Aqua.B);
                break;
                case "Great Soul Arrow":
                color = Color.Blue;
                break;
                case "Lightning Arrow":
                color = projectile.GetGlobalProjectile<GunGlobalProjectile>().OverrideColor? projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor : new Color(255,215,50);
                break;
                case "Great Lightning Arrow":
                color = projectile.GetGlobalProjectile<GunGlobalProjectile>().OverrideColor? projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor : new Color(255,215,50);
                break;
                default:
                break;
            } 
            Lighting.AddLight(projectile.Center, new Vector3(color.R,color.G,color.B)/200);
            Dust.NewDust(projectile.position, projectile.width, projectile.height, projectile.Name.Contains("Soul")? 135 : projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor==Color.DarkRed?264:133, newColor:color);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor){
            Color color = new Color();
            if(projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor!=new Color()){
                color = projectile.GetGlobalProjectile<GunGlobalProjectile>().GlowColor;
            }else switch(projectile.Name){
                case "Soul Arrow":
                color = new Color(Color.Aqua.R*0.75f,Color.Aqua.G*0.75f,Color.Aqua.B);
                break;
                case "Great Soul Arrow":
                color = Color.Blue;
                break;
                case "Great Heavy Soul Arrow":
                color = Color.Blue;
                break;
                case "Lightning Arrow":
                color = new Color(255,215,50);
                break;
                case "Great Lightning Arrow":
                color = new Color(255,215,50);
                break;
                default:
                break;
            }
            spriteBatch.Draw(Main.projectileTexture[ProjectileID.BoneArrow], projectile.position - Main.screenPosition, new Rectangle(0, 0, 14, 32), color, projectile.rotation, new Vector2(3,8), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}