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
	public class WoFChime : ChimeBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Talisman");
			Tooltip.SetDefault("This looks kind of like a pepper, doesn't it?\nI'm bad at drawing cloth.");
		}
		public override void SetDefaults()
		{
			item.damage = 35;
			item.magic = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useTurn = false;
			item.width = 24;
			item.height = 26;
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 50000;
			item.rare = ItemRarityID.LightRed;
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
				if(Spells[i]=="Heal"){
					SpellsList.Add("Great Heal");
					SpellDamageList.Add(250);
					SpellAmmoList.Add(0);
					SpellManaList.Add(55);
					SpellSpeedList.Add(1);
				}else if(Spells[i]=="Lightning Arrow"){
					SpellsList.Add("Way of White Corona");
					SpellDamageList.Add(1.05f);
					SpellAmmoList.Add(0);
					SpellManaList.Add(25);
					SpellSpeedList.Add(0.7f);
				}
			}
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
		public override void PostShoot(int p){
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().OverrideColor = true;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().Color = Color.DarkRed;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().GlowColor = Color.DarkRed;
			Main.projectile[p].light = 0;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			for (int i = 0; i < 4; i++){
				Dust.NewDust(position, 0, 0, 183, speedX, speedY, newColor:Color.DarkRed);
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
