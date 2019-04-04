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
using Terraria.Localization;
using RefTheGun.Classes;

namespace RefTheGun.Items{
	public class Pandora : RefTheItem{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);
		int mode = 0;
		List<SimpleProj> favs = new List<SimpleProj>();
		int timesinceswitch = 0;
        private int timesinceright;
        private static int searchtype;

        public override void SetStaticDefaults(){
			DisplayName.SetDefault("Pandora");
		}
		public override void SetDefaults(){
			item.damage = 21;
			item.ranged = true;
			item.noMelee = true;
			item.width = 10;
			item.height = 50;
			item.useTime = 1;
			item.useAnimation = 7;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Green;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 12.5f;
			item.UseSound = null;
			item.autoReuse = true;
			MaxAmmo = 0;
			MaxSpread = 4.5f;
			MinSpread = 0f;
			BloomPerShot = 0f;
			SpreadLossSpeed = 0.1f;
			ReloadTimeMax = 30;
			TrackHitEnemies = true;
			specialreloadproj = true;

			Ammo = MaxAmmo;
		}
		public override void HoldItem(Player player){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
			modPlayer.roundsinmag = Ammo;
			modPlayer.magsize = /*(int)(*/MaxAmmo/**Main.player[item.owner].GetModPlayer<GunPlayer>(mod).MagMultiply)*/;
			modPlayer.guninfo = GetProjName(mode);
			if(!player.controlUseTile&&timesinceswitch>0)timesinceswitch--;
            if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded){
				Ammo = MaxAmmo;
                restoredefaults();
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded = false;
            }
		}
        public override void HoldStyle(Player player){
            Spread = MaxSpread;
        }
		public override bool ConsumeAmmo(Player player){
			return false;
		}
        public override TagCompound Save(){
            TagCompound o = new TagCompound(){{"mode",mode},{"favs",favs}};
            return o;
        }
        public override void Load(TagCompound tagCompound){
            if(tagCompound.HasTag("mode")){
                mode = tagCompound.GetInt("mode");
            }
            if(tagCompound.HasTag("favs")){
                favs = tagCompound.Get<List<SimpleProj>>("favs");
            }
        }
        public override bool CanRightClick(){
            if(timesinceright<=0){
				searchtype = mode;
				if(favs.Exists(search)){
					favs.RemoveAll(search);
				}else{
					favs.Add(new SimpleProj(mode,GetProjName(mode)));
				}
            }
            timesinceright=7;
            return false;
        }
		bool search(SimpleProj input){
			return input.type == searchtype;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips){
            if(favs.Count>0){
                tooltips.Add(new TooltipLine(mod, "Proj0", "Favorites:"));
                int a = 0;
                foreach (SimpleProj i in favs){
                    searchstring = i.name;
                    if(!tooltips.Exists(contains))tooltips.Add(new TooltipLine(mod, "Proj"+(++a), i.name));
                }
            }
        }
		static string searchstring = "";
        static bool contains(TooltipLine t){
            return t.text.Contains(searchstring);
        }
		string GetProjName(int type){
			Projectile a = new Projectile();
			a.SetDefaults(type);
			return a.Name;
			//return ProjectileLoader.GetProjectile(type).DisplayName.GetTranslation(LanguageManager.Instance.ActiveCulture);
		}
		void Reload(){
			Player player = Main.player[item.owner];
			if(!Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading&&!Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloaded){
                if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading)return;
                Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading = true;
                int proj = Projectile.NewProjectile(player.Center, new Vector2(), mod.ProjectileType("ReloadProj"), 0, 0, player.whoAmI, (int)(ReloadTimeMax*reloadmult), player.selectedItem);
                Main.projectile[proj].timeLeft = Math.Max((int)(ReloadTimeMax*reloadmult),2);
			}
		}
        public override bool CanUseItem(Player player){
			if(Main.player[item.owner].GetModPlayer<GunPlayer>(mod).Reloading)return false;
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
				item.noUseGraphic = true;
				if(timesinceswitch==0)ToggleMode();
				timesinceswitch = 2;
				item.mana = 0;
            }else{
				item.useStyle = 5;
				item.noUseGraphic = false;
            }
			return true;
        }
		public override void restoredefaults(){
			item.UseSound = null;
		}
		public override Vector2? HoldoutOffset(){
			return new Vector2(-2, 0);
		}
		void ToggleMode(){
			Projectile a;
			if(Main.player[item.owner].controlSmart){
				Predec:
				if(--mode<0){
					mode=ProjectileLoader.ProjectileCount-1;
				}
				a = new Projectile();
				a.SetDefaults(mode);
				if(a.aiStyle==20||a.aiStyle==26||a.aiStyle==62)goto Predec;
			}else{
				Preinc:
				if(++mode>ProjectileLoader.ProjectileCount){
					mode=1;
				}
				a = new Projectile();
				a.SetDefaults(mode);
				if(a.aiStyle==20||a.aiStyle==26||a.aiStyle==62)goto Preinc;
			}
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenBow, 1);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			if(player.altFunctionUse==2||player.GetModPlayer<GunPlayer>().Reloading)return false;
			type = mode;
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale){
			Player player = Main.player[item.owner];
			//float scale = 0.7f;
			try{
			if(mod.TextureExists("Items/Pandora"))spriteBatch.Draw(mod.GetTexture("Items/Pandora"), position/*+new Vector2(20,17)*/, new Rectangle(0,0,28,50), drawColor, 0, new Vector2(), scale, SpriteEffects.None, 0);
			}
			catch (System.Exception){
				return true;
			}
			return false;
		}
	}
}
