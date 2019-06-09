using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using RefTheGun.Items;
using RefTheGun.Items.Passives;

namespace RefTheGun
{
	public class GungeonWorld : ModWorld
	{
		public override void PostWorldGen()
		{
            List<int> DungeonChests = new List<int>{};
            List<int> HellChests = new List<int>{};
            List<int> JungleChests = new List<int>{};
            List<int> SkyChests = new List<int>{};
            List<int> IceChests = new List<int>{};
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Ice Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. 
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 2 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == 0)
						{
                            DungeonChests.Add(chestIndex);
							break;
						}
					}
				}
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 4 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == 0)
						{
                            HellChests.Add(chestIndex);
							break;
						}
					}
				}
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 12 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == 0)
						{
                            IceChests.Add(chestIndex);
							break;
						}
					}
				}
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && (Main.tile[chest.x, chest.y].frameX == 11 * 36||(Main.tile[chest.x, chest.y].frameX == 13 * 36)))
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == 0)
						{
                            JungleChests.Add(chestIndex);
							break;
						}
					}
				}
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 14 * 36)
				{
					for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
					{
						if (chest.item[inventoryIndex].type == 0)
						{
                            SkyChests.Add(chestIndex);
							break;
						}
					}
				}
			}
            Chest chestb = Main.chest[Main.rand.Next(DungeonChests)];
            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
            {
                if (chestb.item[inventoryIndex].type == 0)
                {
                    chestb.item[inventoryIndex].SetDefaults(mod.ItemType<ClericChime>());
                    break;
                }
            }
			bool flag = false;
			List<int> DungeonChests2 = DungeonChests;
			for(int i = 0; i<DungeonChests.Count; i++){
				int c = Main.rand.Next(DungeonChests2);
				Chest chestc = Main.chest[c];//Main.chest[Main.rand.Next(DungeonChests)];
				for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(mod.ItemType<GunItemBelt>());
						flag = true;
						break;
					}
				}
				DungeonChests2.Remove(c);
				if(flag)break;
			}
			Chest chestd = Main.chest[Main.rand.Next(DungeonChests)];
			for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
			{
				if (chestd.item[inventoryIndex].type == 0)
				{
					chestd.item[inventoryIndex].SetDefaults(mod.ItemType<Plus1Bullets>());
					break;
				}
			}
			for(int i = Main.rand.Next(10); i<10; i++){
				Chest chestc = Main.chest[Main.rand.Next(JungleChests)];
				for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(mod.ItemType<PoisonBullets>());
						break;
					}
				}
			}
			for(int i = Main.rand.Next(10); i<10; i++){
				Chest chestc = Main.chest[Main.rand.Next(IceChests)];
				for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(mod.ItemType<ColdBullets>());
						break;
					}
				}
			}
			for(int i = Main.rand.Next(10); i<10; i++){
				Chest chestc = Main.chest[Main.rand.Next(HellChests)];
				for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(mod.ItemType<HotBullets>());
						break;
					}
				}
			}
			for(int i = Main.rand.Next(10); i<10; i++){
				Chest chestc = Main.chest[Main.rand.Next(SkyChests)];
				for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(mod.ItemType<Scattershot>());
						break;
					}
				}
			}
			for(int i = Main.rand.Next(10); i<10; i++){
				Chest chestc = Main.chest[Main.rand.Next(SkyChests)];
				for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(mod.ItemType<AoPitlord>());
						break;
					}
				}
			}
		}
	}
}