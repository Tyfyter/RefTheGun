using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Text;
using System.Reflection;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.Projectiles;

namespace RefTheGun.Items
{
	public class LittleBell : ModItem
	{
        public override bool CloneNewInstances => true;
		LegacySoundStyle useSound = new LegacySoundStyle(2, 89);
        int time = 0;
        public override string Texture{
            get {return "Terraria/Item_"+ItemID.FairyBell;}
        }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Little Bell");
			Tooltip.SetDefault(@"It's a small bell, it rings.");
		}
		public override void SetDefaults()
		{
            item.CloneDefaults(ItemID.Bell);
			item.damage = 10;
			item.magic = true;
			item.knockBack = 6;
			item.value = 3500;
            item.accessory = true;
            item.color = Color.Wheat;
		}
        
        public override void UpdateAccessory(Player player, bool hideVisual){
            if(player.velocity.Length()==0)return;
            if(time++*(player.velocity.Length())>=260){
                int damage = item.damage;
                float add = 0,mult = 1,flat = 0;
                ModifyWeaponDamage(player, ref add, ref mult, ref flat);
                damage = (int)((damage+add)*mult+flat);
                Projectile a = Projectile.NewProjectileDirect(player.Center, new Vector2(), ModContent.ProjectileType<WrathoftheGods>(), damage, 0, item.owner, 3, 46);
                a.timeLeft = 8;
                a.GetGlobalProjectile<GunGlobalProjectile>().ignorespecialfeatures=true;
                Main.PlaySound(new LegacySoundStyle(2, 35).WithVolume(0.1f).WithPitchVariance(0.1f), player.Center);
                time = 0;
            }
        }
        public override bool PreDrawInInventory(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
            spriteBatch.Draw(Main.itemTexture[ItemID.Bell], position+new Vector2(item.width/2,item.height/2), null, Color.Wheat, 0.5f, new Vector2(item.width/2,item.height/2), scale*0.75f, SpriteEffects.None, 0);
            return false;
        }
	}
}
