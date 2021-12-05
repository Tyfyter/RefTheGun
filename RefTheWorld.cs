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

			for (int chestIndex = 0; chestIndex < Main.chest.Length; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the nth chest is the ____ Chest. Since we are counting from 0, this is where n comes from. 36 comes from the width of each tile including padding. 
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
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && (Main.tile[chest.x, chest.y].frameX == 13 * 36||(Main.tile[chest.x, chest.y].frameX == 13 * 36)))
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
			int ch = Main.rand.Next(DungeonChests);
			if(DungeonChests.Count>0&&ch>=0&&ch<Main.chest.Length){
            Chest chestb = Main.chest[ch];
				for (int inventoryIndex = 0; inventoryIndex < 39; inventoryIndex++)
				{
					if (chestb.item[inventoryIndex].type == 0)
					{
						chestb.item[inventoryIndex].SetDefaults(ModContent.ItemType<ClericChime>());
						break;
					}
				}
			}

			bool flag = false;
			List<int> DungeonChests2 = DungeonChests;
			for(int i = 0; i<DungeonChests.Count; i++){
				if(DungeonChests2.Count<1)break;
				int c = Main.rand.Next(DungeonChests2);
				if(c<0||c>=Main.chest.Length)break;
				Chest chestc = Main.chest[c];//Main.chest[Main.rand.Next(DungeonChests)];
				for (int inventoryIndex = 0; inventoryIndex < 39; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(ModContent.ItemType<GunItemBelt>());
						flag = true;
						break;
					}
				}
				DungeonChests2.Remove(c);
				if(flag)break;
			}
			ch = Main.rand.Next(DungeonChests);
			if(IceChests.Count>0&&ch>=0&&ch<Main.chest.Length){
				Chest chestd = Main.chest[ch];
				for (int inventoryIndex = 0; inventoryIndex < 39; inventoryIndex++)
				{
					if (chestd.item[inventoryIndex].type == 0)
					{
						chestd.item[inventoryIndex].SetDefaults(ModContent.ItemType<Plus1Bullets>());
						break;
					}
				}
			}

			for(int i = Main.rand.Next(10); i<10; i++){
				if(JungleChests.Count<1)break;
				int c = Main.rand.Next(JungleChests);
				if(c<0||c>=Main.chest.Length)break;
				Chest chestc = Main.chest[c];
				for (int inventoryIndex = 0; inventoryIndex < 39; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(ModContent.ItemType<PoisonBullets>());
						break;
					}
				}
			}

			for(int i = Main.rand.Next(10); i<10; i++){
				if(IceChests.Count<1)break;
				int c = Main.rand.Next(IceChests);
				if(c<0||c>=Main.chest.Length)break;
				Chest chestc = Main.chest[c];
				for (int inventoryIndex = 0; inventoryIndex < 39; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(ModContent.ItemType<ColdBullets>());
						break;
					}
				}
			}
			for(int i = Main.rand.Next(10); i<10; i++){
				if(HellChests.Count<1)break;
				int c = Main.rand.Next(HellChests);
				if(c<0||c>=Main.chest.Length)break;
				Chest chestc = Main.chest[c];
				for (int inventoryIndex = 0; inventoryIndex < 39; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(ModContent.ItemType<HotBullets>());
						break;
					}
				}
			}
			for(int i = Main.rand.Next(10); i<10; i++){
				if(SkyChests.Count<1)break;
				int c = Main.rand.Next(SkyChests);
				if(c<0||c>=Main.chest.Length)break;
				Chest chestc = Main.chest[c];
				for (int inventoryIndex = 0; inventoryIndex < 39; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(ModContent.ItemType<Scattershot>());
						break;
					}
				}
			}

			for(int i = Main.rand.Next(10); i<10; i++){
				if(SkyChests.Count<1)break;
				int c = Main.rand.Next(SkyChests);
				if(c<0||c>=Main.chest.Length)break;
				Chest chestc = Main.chest[c];
				for (int inventoryIndex = 0; inventoryIndex < 39; inventoryIndex++)
				{
					if (chestc.item[inventoryIndex].type == 0)
					{
						chestc.item[inventoryIndex].SetDefaults(ModContent.ItemType<AoPitlord>());
						break;
					}
				}
			}
		}
	}
}