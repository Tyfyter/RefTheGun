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
	public class GunItemBelt : RefTheItem
	{
        public List<Item> passives = new List<Item>(){};
        //public List<int> actives = new List<int>(){};
        public int active = 0;
        int timesinceright = 0;
        public List<string> synergies = new List<string>();
        public override bool isGun => false;
        public static int searchtype = 0;
        public int pcount(){
            int a = 0;
            int b = 1;
            bool maybesub = false;
            bool sub = false;
            foreach(Item i in passives){
                if(!i.IsAir){
                    a+=b;
                    b = 1;
                    sub = maybesub;
                }else{
                    b++;
                    maybesub = true;
                }
            }
            //Main.NewText("Items:"+a+"; Corrected:"+(a-(sub?1:0))+"; Slots:"+(Math.Ceiling((a-3)/4f)*4)+"; Slots (corrected):"+(Math.Ceiling((a-(sub?4:3))/4f)*4));
            return a-(sub?1:0);
        }
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Gungeon bandoleer");
			Tooltip.SetDefault(@"Great for storing everything you need... except weapons... and ammo...
Right click to open passive UI.");
		}
		public override void SetDefaults(){
            item.CloneDefaults(ItemID.GolemTrophy);
            item.createTile = -1;
            item.createWall = -1;
            item.placeStyle = -1;
            item.useStyle = 0;
            item.maxStack = 1;
            item.width = item.height = 20;
            item.consumable = false;
            item.noUseGraphic = true;
            item.ranged = true;
			item.value = 35000;
            item.accessory = false;
            item.useStyle = 5;
            item.useAnimation = 20;
            item.useTime = 20;
            item.uniqueStack = true;
		}
        public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Leather, 10);
			recipe.AddIngredient(ItemID.Bone, 6);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
        public override void UpdateInventory(Player player){
            /*
            if(pcount!=passives.Count){
                Main.NewText(pcount+"!="+passives.Count);
                pcount=passives.Count;
            }//*/
            foreach (Item i in passives)if(i!=null)if(!i.IsAir)if(i.modItem.mod!=ModLoader.GetMod("ModLoader")){
                try{
                    ((PassiveBase)i.modItem).Apply(player);
                }catch (Exception){}
            }else{
                i.TurnToAir();
            }
            if(timesinceright>0){
                timesinceright--;
            }
        }
        public override TagCompound Save(){
            TagCompound o = new TagCompound(){{"passives",passives}};
            return o;
        }
        public override void Load(TagCompound tagCompound){
            if(tagCompound.HasTag("passives")){
                passives = tagCompound.Get<List<Item>>("passives");
            }
        }
        static string string1;
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            tooltips.RemoveAll(PlaceableorFav);
            if(synergies.Count>0){
                tooltips.Add(new TooltipLine(mod, "Synergy0", "Synergies:"));
                int a = 0;
                foreach (string i in synergies){
                    string1 = i;
                    if(!tooltips.Exists(contains))tooltips.Add(new TooltipLine(mod, "Synergy"+(++a), i));
                }
            }
            synergies.RemoveAll(True);
        }
        static bool True(object s = null){return true;}
        static bool contains(TooltipLine t){
            return t.text.Contains(string1);
        }
        static bool PlaceableorFav(TooltipLine t){
            return t.Name.ToLower().Contains("placeable")||t.Name.ToLower().Contains("favorite");
        }
        public override bool CanRightClick(){
            if(timesinceright<=0){
                UseItem(Main.player[item.owner]);
                //Main.NewText(passives.Count);
            }
            timesinceright=7;
            return false;
        }
        public bool HasItem(int type){
            searchtype = type;
            return passives.Exists(hasitem);
        }
        bool hasitem(Item item){
            return item.type==searchtype;
        }
        public override bool UseItem(Player player){
            //if(actives.Count>0)Activate(actives[active]);
            RefTheGun.mod.gunItemUI = new GunItemsUI();
            RefTheGun.mod.gunItemUI.Activate();
            RefTheGun.mod.UI.SetState(RefTheGun.mod.gunItemUI);
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
        static public void ActivatePassive(Projectile projectile, NPC target, int type){
            switch (type){
                case 1://Poison
                target.AddBuff(BuffID.Poisoned, 600);
                break;
                case 2://Ice
                target.AddBuff(BuffID.Frozen, target.boss?30:120);
                break;
                case 3://Fire
                target.AddBuff(BuffID.OnFire, 450);
                break;
                default:
                break;
            }
        }
	}
}
