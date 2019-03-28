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
using RefTheGun.UI;
using RefTheGun.Items.Passives;

namespace RefTheGun.Items
{
	public class GunItemBelt : ModItem
	{
        public override bool CloneNewInstances => true;
        public List<Item> passives = new List<Item>(){};
        public List<int> actives = new List<int>(){};
        public int active = 0;
        int timesinceright = 0;
        public int pcount = 0;
        public static int total = 0;
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Gungeon bandoleer");
			Tooltip.SetDefault(@"Great for storing everything you need... except weapons... and ammo...");
		}
		public override void SetDefaults(){
            item.CloneDefaults(ItemID.GolemTrophy);
            item.createTile = 0;
            item.createWall = 0;
            item.placeStyle = 0;
            item.useStyle = 0;
            item.consumable = false;
            item.ranged = true;
			item.value = 35000;
            item.accessory = false;
            item.useStyle = 5;
            item.uniqueStack = true;
		}
        public override void UpdateInventory(Player player){
            if(pcount!=passives.Count){
                Main.NewText(pcount+"!="+passives.Count);
                pcount=passives.Count;
            }
            foreach (Item i in passives)if(i!=null)if(!i.IsAir)((PassiveBase)i.modItem).Apply(player);
            if(timesinceright>0){
                timesinceright--;
            }
        }
        public override bool CanRightClick(){
            if(timesinceright<=0){
                Main.NewText(passives.Count);
                RefTheGun.mod.gunItemUI = new GunItemsUI{item = item};
                RefTheGun.mod.gunItemUI.Activate();
                RefTheGun.mod.UI.SetState(RefTheGun.mod.gunItemUI);
            }
            timesinceright=7;
            return false;
        }
        public override bool UseItem(Player player){
            if(actives.Count>0)Activate(actives[active]);
            return true;
        }
        public void Activate(int i){
            switch (i){
                
                default:
                break;
            }
        }
        public void Passivate(int i){//totally a real word
            Player player = Main.player[item.owner];
            GunPlayer modPlayer = player.GetModPlayer<GunPlayer>();
            switch (i){
                case 1://Master Round
                player.statLifeMax2 = (int)(player.statLifeMax2*1.05);
                break;
                case 2://Master Round
                player.statLifeMax2 = (int)(player.statLifeMax2*1.05);
                break;
                case 3://Master Round
                player.statLifeMax2 = (int)(player.statLifeMax2*1.05);
                break;
                case 4://Master Round
                player.statLifeMax2 = (int)(player.statLifeMax2*1.05);
                break;
                case 5://Master Round
                player.statLifeMax2 = (int)(player.statLifeMax2*1.05);
                break;
                case 6://Master Round
                player.statLifeMax2 = (int)(player.statLifeMax2*1.05);
                break;
                case 7://+1 Bullets
                player.rangedDamage+=0.25f;
                break;
                case 8://TODO Rocket Bullets
                player.rangedDamage+=0.1f;
                break;
                case 9://TODO Heavy Bullets
                player.rangedDamage+=0.1f;
                break;
                case 10://Alpha Bullet
                if(modPlayer.roundsinmag==modPlayer.magsize)player.rangedDamage*=1.8f;
                break;
                case 11://Omega Bullet
                if(modPlayer.roundsinmag==1)player.rangedDamage*=2f;
                break;
                case 12://Irradiated Lead
                modPlayer.bulletPoisonChance+=0.1f;
                break;
                case 13://Liquid Valyrie
                modPlayer.bullet20Slow = true;
                break;
                default:
                break;
            }
        }
        public bool hasPassive(int type){
            //for (int i = 0; i < passives.Count; i++)if(passives[i]==type)return true;
            return false;
        }
        static public void ActivatePassive(Projectile projectile, NPC target, int type){
            switch (type){
                case 1://Irradiated Lead
                target.AddBuff(BuffID.Poisoned, 600);
                break;
                default:
                break;
            }
        }
	}
}
