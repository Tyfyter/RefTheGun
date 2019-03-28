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

namespace RefTheGun.UI
{
	// This class represents the UIState for the "Gungeon Bandoleer"
	internal class GunItemsUI : UIState {
        public List<VanillaItemSlotWrapper> itemSlots = new List<VanillaItemSlotWrapper>(){};
        public Item item{
            get{return Main.item[itemId];}
            set{itemId = value.whoAmI;}
        }
        public int itemId;
        public override void OnInitialize(){
            for (int i = 0; i < 4; i++){
                if(i>=itemSlots.Count)itemSlots.Add(null);
                itemSlots[i] = new VanillaItemSlotWrapper(scale:0.575f){
				Left = { Pixels = 570 },
                Top = { Pixels = 105+(i*33) },
                ValidItemFunc = item => item.IsAir || !item.IsAir && ((item.modItem!=null?item.modItem.mod==RefTheGun.mod:false)?(!((RefTheItem)item.modItem).isGun):false)
				};
                if(item!=null){
                    Main.NewText(((GunItemBelt)item.modItem).passives.Count+";"+i);
                    if(((GunItemBelt)item.modItem).passives.Count>i){
                        Main.NewText(((GunItemBelt)item.modItem).passives.Count+";"+i);
                        itemSlots[i].Item = ((GunItemBelt)item.modItem).passives[i];
                    }
                }
                Append(itemSlots[i]);
            }
        }
        public override void Update(GameTime gameTime){
            base.Update(gameTime);
            if(item!=null)if(item.modItem!=null){
                item.modItem.UpdateInventory(Main.LocalPlayer);
            }
            for (int i = 0; i < itemSlots.Count; i++)if(itemSlots[i].Item.modItem!=null){
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
            List<Item>passives = new List<Item>(){};
            for(int i = 0; i < itemSlots.Count; i++)passives.Add(null);
			for(int i = 0; i < itemSlots.Count; i++)passives[i] = itemSlots[i].Item;
            ((GunItemBelt)item.modItem).passives = passives;
            ((GunItemBelt)item.modItem).pcount = passives.Count;
            //Main.NewText(";"+passives.Count+";"+((GunItemBelt)item.modItem).passives.Count+";"+((GunItemBelt)item.modItem).pcount);
		}
    }
}