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
using RefTheGun.Buffs;

namespace RefTheGun.Items
{
	public class ShellNBullet : RefTheItem
	{
		public static LegacySoundStyle useSound = new LegacySoundStyle(2, 36);
		int bullettype = 0;
		int[] baseUseTimes = new int[2]{11,46};
		int[] useTimes = new int[2]{0,0};
		int[] maxAmmos = new int[2]{22,6};
		int[] ammos = new int[2]{0,0};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("\"Bullet Shell\"");
			Tooltip.SetDefault(@"Blammo!
Maybe I should stop with the puns.");
		}
		public override void SetDefaults()
		{
			item.damage = 30;
			item.ranged = true;
			item.noMelee = true;
			item.width = 26;
			item.height = 26;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.LightRed;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 10.5f;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = useSound;
			item.autoReuse = true;
			MaxAmmo = 6;
			MinSpread = 12f;
			MaxSpread = 12f;
			BloomPerShot = 0f;
			ReloadTimeMax = 75;
			
			ammos = (int[])maxAmmos.Clone();
			Ammo = MaxAmmo-1;
		}
        public override void HoldItem(Player player){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			modPlayer.roundsinmag = ammos[0];
			modPlayer.magsize = maxAmmos[0];
			modPlayer.guninfo = ammos[1]+"/"+maxAmmos[1];
            if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded){
                //ReloadFinishHook(player, Ammo);
                restoredefaults();
                Ammo = MaxAmmo;
                ammos = (int[])maxAmmos.Clone();
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded = player.HasBuff(mod.BuffType<ReloadBuff>());
            }
        }

		public override bool ConsumeAmmo(Player player){
			return false;
		}
		
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
                if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading)return false;
                if(ammos[0] >= maxAmmos[0] && ammos[1] >= maxAmmos[1])return false;
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading = true;
                int proj = Projectile.NewProjectile(player.Center, new Vector2(), mod.ProjectileType("ReloadProj"), 0, 0, player.whoAmI, (int)(ReloadTimeMax*reloadmult), player.selectedItem);
                Main.projectile[proj].timeLeft = Math.Max((int)(ReloadTimeMax*reloadmult),2);
                item.UseSound = new LegacySoundStyle(-1, -1);
                item.useAmmo = 0;
				item.useStyle = 1;
				item.noUseGraphic = true;
                return true;
            }else{
                restoredefaults();
				item.useStyle = 5;
				item.noUseGraphic = true;
                return !Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading;
            }
        }
		public override void UpdateInventory(Player player){
			if(useTimes[0] > 0){
				useTimes[0]--;
			}
			if(useTimes[1] > 0){
				useTimes[1]--;
			}
		}
		public override void restoredefaults(){
			item.useAmmo = AmmoID.Bullet;
            item.UseSound = new LegacySoundStyle(-1, -1);
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Shotgun, 3);
			recipe.AddIngredient(ItemID.EmptyBullet, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void PostShoot(int[] p){
			foreach (int i in p){
				Main.projectile[i].ai[0] = bullettype;
            	Main.projectile[i].rotation = Main.projectile[i].velocity.RotatedByRandom(MathHelper.ToRadians(90)).ToRotation();
			}
		}
		public override void PostShoot(int p){
			Main.projectile[p].ai[0] = bullettype;
			Main.projectile[p].ai[1] = 12;
            Main.projectile[p].rotation = Main.projectile[p].velocity.RotatedByRandom(MathHelper.ToRadians(90)).ToRotation();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
            if(player.altFunctionUse == 2)return false;
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			bullettype = type;
            if(useTimes[0]==0&&ammos[0]>0){
                int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY).RotatedByRandom(0.5), mod.ProjectileType<GunProj>(), damage, knockBack, item.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].usesLocalNPCImmunity = true;
                Main.projectile[proj].localNPCHitCooldown = 3;
                Spread = Math.Min(Spread+BloomPerShot, MaxSpread);
                ammos[0]--;
                modPlayer.roundsinmag = ammos[0];
				Ammo = MaxAmmo-1;
				useTimes[0]=baseUseTimes[0];
				Main.PlaySound(2, player.Center, 89);
                PostShoot(proj);
            }
			if(useTimes[1]==0&&ammos[1]>0){
                List<int> projs = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY).RotatedByRandom(1/3), mod.ProjectileType<ShotgunProj>(), damage, knockBack, item.owner);
                    Main.projectile[proj].friendly = true;
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                    Main.projectile[proj].localNPCHitCooldown = 3;
                    projs.Add(proj);
                    Spread = Math.Min(Spread+BloomPerShot, MaxSpread);
                }
                ammos[1]--;
                modPlayer.magsize = ammos[1];
				Ammo = MaxAmmo-1;
				useTimes[1]=baseUseTimes[1];
				Main.PlaySound(2, player.Center, 36);
                PostShoot(projs.ToArray());
            }
            return false;
			/*bullettype = type;
			type = mod.ProjectileType<ShotgunProj>();
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);*/
		}
	}
}
