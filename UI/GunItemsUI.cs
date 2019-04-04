using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using RefTheGun.Items;
using RefTheGun.Items.Passives;
using System;

namespace RefTheGun.UI
{
	// This class represents the UIState for the "Gungeon Bandoleer"
	public class GunItemsUI : UIState {
        public List<VanillaItemSlotWrapper> itemSlots = new List<VanillaItemSlotWrapper>(){};
        public Item item{
            get{return ((GunPlayer)Main.LocalPlayer).GetBandoleer();}
            set{if(((GunPlayer)Main.LocalPlayer).GetBandoleerIndex()>=0)Main.LocalPlayer.inventory[((GunPlayer)Main.LocalPlayer).GetBandoleerIndex()] = value;}
        }//*/
        //public List<Item> passives;
        public int itemId;
        public override void OnInitialize(){
            int l = 4;
            if(item!=null){
                l+=(int)(Math.Ceiling((((GunItemBelt)item.modItem).pcount()-3)/4f)*4);
            }
            for (int i = 0; i < l; i++){
                if(i>=itemSlots.Count)itemSlots.Add(null);
                itemSlots[i] = new VanillaItemSlotWrapper(scale:0.575f){
				Left = { Pixels = 570+((i-(i%4))*8.25f) },
                Top = { Pixels = 105+((i%4)*33) },
                ValidItemFunc = item => item.IsAir || !item.IsAir && ((item.modItem!=null?item.modItem.mod==RefTheGun.mod:false)?(!((RefTheItem)item.modItem).isGun):false)
				};
                if(item!=null)if(!item.IsAir){
                    if(((GunItemBelt)item.modItem).passives.Count>i){
                        itemSlots[i].Item = ((GunItemBelt)item.modItem).passives[i];
                    }
                }
                Append(itemSlots[i]);
            }
            //Main.NewText(((GunItemBelt)RefTheGun.mod.gunItemUI.item.modItem).passives.Count+":"+passives.Count);
        }
        public override void Update(GameTime gameTime){
            base.Update(gameTime);
            if(item!=null)if(!item.IsAir)if(item.modItem!=null){
                item.modItem.UpdateInventory(Main.LocalPlayer);
            }
            for (int i = 0; i < itemSlots.Count; i++)if(item!=null)if(!item.IsAir)if(itemSlots[i].Item.modItem!=null){
                //((HeartItemBase)heartSlots[i].Item.modItem).index = i;
            }
        }
        public void a(ModItem b){
            item = b.item;
        }
		public override void OnDeactivate(){
			UpdateItem();
		}
		public void UpdateItem(){
            List<Item> passives = new List<Item>(){};
            for(int i = 0; i < itemSlots.Count; i++)passives.Add(null);
			for(int i = 0; i < itemSlots.Count; i++)passives[i] = itemSlots[i].Item;
            ((GunItemBelt)item.modItem).passives = passives;
            //((GunItemBelt)item.modItem).pcount = ((GunItemBelt)item.modItem).passives.Count;
            //Main.NewText(passives.Count+";"+((GunItemBelt)item.modItem).passives[0].Name+";"+((GunItemBelt)item.modItem).pcount);
		}
    }
}