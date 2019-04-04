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
using RefTheGun.NPCs;
using RefTheGun.Classes;
using RefTheGun.Buffs;

namespace RefTheGun.Items
{
	public class Multifire : RefTheItem
	{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 19);
		int mode;
        int timesinceswitch;
		int charge;
        int maxcharge;
		int chargemax{
			get{return mode==1?Ammo*9:21;}
		}
		string name{
			get{
				switch (mode){
					case 1:
					return "spread";
					case 2:
					return "double";
					default:
					return "normal";
				}
			}
		}
		public override bool ammoPerMult(int i){
			//if you have a better algorithim for spread mode please tell me
			return mode!=1?false:i<Multishot-0.1f;
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cheimaros");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 50;
			item.thrown = true;
			item.noMelee = true;
			item.width = 38;
			item.height = 38;
			item.useTime = 1;
			item.useAnimation = 7;
			item.scale = 0.5f;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Blue;
			item.shoot = ProjectileID.ThrowingKnife;
			item.shootSpeed = 12.5f;
			item.UseSound = useSound;
			item.autoReuse = false;
			MaxAmmo = 6;
			MaxSpread = 10f;
			MinSpread = 0.2f;
			BloomPerShot = 10f;
			SpreadLossSpeed = 10f;
			ReloadTimeMax = 32;
			instantreload = true;
			TrackHitEnemies = true;

			Ammo = MaxAmmo;
		}
		public override void restoredefaults(){
			item.UseSound = useSound;
		}
		public override void HoldItem(Player player){
			if(player.itemAnimation<=0)if(!player.controlUseTile&&timesinceswitch>0)timesinceswitch--;
			((GunPlayer)player).guninfo = name;
			base.HoldItem(player);
		}
		public override bool CanUseItem(Player player){
			if(player.altFunctionUse==2&&Ammo>=MaxAmmo){
				if(timesinceswitch==0)if(++mode>2)mode=0;
				timesinceswitch = 2;
			}
			bool o = base.CanUseItem(player);
			item.useStyle = 1;
			return o;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 99);
			recipe.AddIngredient(ItemID.BoneArrow, 20);
			recipe.AddIngredient(ItemID.SoulofMight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostShoot(int p){
            Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
            Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().nullprecull = true;
            Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().render = false;
			Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>().OverrideTexture = mod.GetTexture("Items/Multifire_Proj");
		}
		public override void OnHitEffect(Projectile projectile, NPC target){
			//target.GetGlobalNPC<GunGlobalNPC>().Buffs.Add(new BleedEffect(target, projectile.damage/7, 70));
			if(target.active)GunGlobalNPC.AddBuff(new BleedEffect(target, projectile.damage/7, 70));
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
            if((player.altFunctionUse == 2 || ((GunPlayer)player).Reloading))return false;
			if(mode!=0){
				if(player.controlUseItem){
					player.itemAnimation = item.useAnimation-1;
					if(charge<chargemax){
						maxcharge=++charge;
						if(charge>=chargemax)for(int i = 0; i < 3; i++){
							int a = Dust.NewDust(position-new Vector2(speedX,speedY), 0, 0, 92);
							Main.dust[a].noGravity = true;
						}
					}else if(Main.time%12<=1){
						int a = Dust.NewDust(position-new Vector2(speedX,speedY), 0, 0, 92);
						Main.dust[a].noGravity = true;
					}
					if(mode == 2){
						Vector2 vel1 = new Vector2(speedX,speedY).RotatedBy((1.02f-(charge/((float)chargemax))));
						Vector2 vel2 = new Vector2(speedX,speedY).RotatedBy(((charge/((float)chargemax))-1.02f));
						for(int i = 0; i < 12; i++){
							int a = Dust.NewDust(position+(vel1*i*0.75f), 0, 0, 92);
							Main.dust[a].noGravity = true;
							Main.dust[a].velocity*=0;
							int b = Dust.NewDust(position+(vel2*i*0.75f), 0, 0, 92);
							Main.dust[b].noGravity = true;
							Main.dust[b].velocity*=0;
						}
					}
					return false;
				}else{
					player.itemAnimation = 0;
				}
			}
			Multishot = 1;
			switch (mode){
				case 1:
				damage=(int)(damage*1.1f);
				Multishot = 1+((charge*5)/chargemax);
				bool a = base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
				charge = 0;
				return a;
				case 2:
				damage=(int)(damage*2.1f);
				Vector2 vel1 = new Vector2(speedX,speedY).RotatedBy((1.02f-(charge/((float)chargemax))))*2;
				Vector2 vel2 = new Vector2(speedX,speedY).RotatedBy(((charge/((float)chargemax))-1.02f))*2;
				base.Shoot(player, ref position, ref vel1.X, ref vel1.Y, ref type, ref damage, ref knockBack);
				Spread = 0;
				base.Shoot(player, ref position, ref vel2.X, ref vel2.Y, ref type, ref damage, ref knockBack);
				charge = 0;
				if(player.itemAnimation != item.useAnimation/2)return false;
				break;
				default:
				charge = 0;
				if(player.itemAnimation != item.useAnimation/2)return false;
				break;
			}
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
	}
}
