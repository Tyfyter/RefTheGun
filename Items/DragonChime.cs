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
using RefTheGun.Projectiles;
using Microsoft.Xna.Framework.Graphics;

namespace RefTheGun.Items
{
	public class DragonChime : ChimeBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon Chime");
			Tooltip.SetDefault("\"It's probably only been about 50 years since the first flame was last linked.\"");
		}
		public override void SetDefaults()
		{
			item.damage = 95;
			item.magic = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useTurn = false;
			item.width = 40;
			item.height = 40;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 150000;
			item.rare = ItemRarityID.Yellow;
			item.shoot = ProjectileID.MagicDagger;
			item.shootSpeed = 12.5f;
			item.UseSound = useSound;
			item.autoReuse = false;
			MaxAmmo = SpellAmmo[spell];
			MinSpread = 0f;
			BloomPerShot = 0f;
			ReloadTimeMax = 45;
			reloadwhenfull = true;

			Ammo = MaxAmmo;
			List<string> SpellsList = new List<string>{};
			List<float> SpellDamageList = new List<float>{};
			List<int> SpellAmmoList = new List<int>{};
			List<int> SpellManaList = new List<int>{};
			List<float> SpellSpeedList = new List<float>{};
			for (int i = 0; i < Spells.Length; i++)
			{
				SpellsList.Add(Spells[i]);
				SpellDamageList.Add(SpellDamage[i]);
				SpellAmmoList.Add(SpellAmmo[i]);
				SpellManaList.Add(SpellMana[i]);
				SpellSpeedList.Add(SpellSpeed[i]);
				if(Spells[i]=="Lightning Storm"){
					SpellsList.Add("Lifehunt Scythe");
					SpellDamageList.Add(1.75f);
					SpellAmmoList.Add(0);
					SpellManaList.Add(40);
					SpellSpeedList.Add(0.8f);
				}else if(Spells[i]=="Heal"){
					SpellsList.Add("Great Heal");
					SpellDamageList.Add(250);
					SpellAmmoList.Add(0);
					SpellManaList.Add(55);
					SpellSpeedList.Add(1);
				}else if(Spells[i]=="Lightning Arrow"){
					SpellsList.Add("Way of White Corona");
					SpellDamageList.Add(1.15f);
					SpellAmmoList.Add(0);
					SpellManaList.Add(25);
					SpellSpeedList.Add(0.75f);
				}
			}
			SpellsList.Add("Tears of Denial");
			SpellDamageList.Add(0);
			SpellAmmoList.Add(0);
			SpellManaList.Add(100);
			SpellSpeedList.Add(0.15f);
			Spells = SpellsList.ToArray();
			SpellDamage = SpellDamageList.ToArray();
			SpellAmmo = SpellAmmoList.ToArray();
			SpellMana = SpellManaList.ToArray();
			SpellSpeed = SpellSpeedList.ToArray();
			/*Spells = new String[11]{"Lightning Spear","Great Lightning Spear","Sunlight Spear","Lightning Arrow","Way of White Corona","Wrath of the Gods","Lightning Storm","Lifehunt Scythe","Blessed Weapon","Great Magic Barrier","Tears of Denial"};
			SpellDamage = new float[11]{1,1.25f,1.75f,1.5f,1.15f,0.85f,1.3f,2,0,0,0};
			SpellAmmo = new int[11]{15,10,6,0,0,5,4,0,0,0,0};
			SpellMana = new int[11]{5,7,10,15,25,40,80,80,35,20,100};
			SpellSpeed = new float[11]{2f,1.85f,1.75f,1.25f,0.75f,0.45f,0.45f,0.4f,1f,1f,0.15f};*/
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofMight, 25);
			recipe.AddIngredient(ItemID.Bell, 1);
			recipe.AddTile(TileID.LihzahrdFurnace);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
