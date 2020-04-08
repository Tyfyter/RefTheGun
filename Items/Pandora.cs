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
using Terraria.GameInput;
using RefTheGun.Buffs;

namespace RefTheGun.Items{
	public class Pandora : RefTheItem{
		LegacySoundStyle useSound = new LegacySoundStyle(2, 5);
		public int mode = 0;
		public int lastfav = 0;
		List<SimpleProj> favs = new List<SimpleProj>();
        private int timesinceright;
        private static int searchtype;

        public override void SetStaticDefaults(){
			DisplayName.SetDefault("Pandora");
		}
		public override void SetDefaults(){
			item.damage = 41;
			item.noMelee = true;
			item.width = 10;
			item.height = 50;
			item.useTime = 7;
			item.useAnimation = 7;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 5000;
			item.rare = ItemRarityID.Green;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 12.5f;
			item.UseSound = null;
			item.autoReuse = true;
			Item.claw[item.type] = true;
			Item.staff[item.type] = true;
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
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>();
			modPlayer.roundsinmag = Ammo;
			modPlayer.magsize = /*(int)(*/MaxAmmo/**Main.player[item.owner].GetModPlayer<GunPlayer>().MagMultiply)*/;
			modPlayer.guninfo = GetProjName(mode);
			//if(!player.controlUseTile&&timesinceswitch>0)timesinceswitch--;
            if(Main.player[item.owner].GetModPlayer<GunPlayer>().Reloaded){
				Ammo = MaxAmmo;
                restoredefaults();
                Main.player[item.owner].GetModPlayer<GunPlayer>().Reloaded = player.HasBuff(ModContent.BuffType<ReloadBuff>());
            }
		}
        public override void HoldStyle(Player player){
            Spread = MaxSpread;
        }
		public override bool ConsumeAmmo(Player player){
			return false;
		}
        public override TagCompound Save(){
            TagCompound o = new TagCompound();
			o["mode"] = mode;
			o["favs"] = favs;
            return o;
        }
		public void tryFavScroll(int dir){
			searchtype = mode;
			if(!favs.Exists(search))mode = lastfav;
			favScroll(dir);
		}
		void favScroll(int dir){
			searchtype = mode;
			lastfav = favs.IndexOf(favs.Find(search));
			int fav = lastfav+dir<0?favs.Count-1:(lastfav+dir)%favs.Count;
			mode = favs[fav].type;
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
			item.melee = a.melee;
			item.ranged = a.ranged;
			item.magic = a.magic;
			item.summon = a.minion;
			item.thrown = a.thrown;
			return a.Name;
			//return ProjectileLoader.GetProjectile(type).DisplayName.GetTranslation(LanguageManager.Instance.ActiveCulture);
		}
		void Reload(){
			Player player = Main.player[item.owner];
			if(!Main.player[item.owner].GetModPlayer<GunPlayer>().Reloading&&!Main.player[item.owner].GetModPlayer<GunPlayer>().Reloaded){
                if(Main.player[item.owner].GetModPlayer<GunPlayer>().Reloading)return;
                Main.player[item.owner].GetModPlayer<GunPlayer>().Reloading = true;
                int proj = Projectile.NewProjectile(player.Center, new Vector2(), ModContent.ProjectileType<ReloadProj>(), 0, 0, player.whoAmI, (int)(ReloadTimeMax*reloadmult), player.selectedItem);
                Main.projectile[proj].timeLeft = Math.Max((int)(ReloadTimeMax*reloadmult),2);
			}
		}
        public override bool CanUseItem(Player player){
			if(Main.player[item.owner].GetModPlayer<GunPlayer>().Reloading)return false;
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
				item.noUseGraphic = false;
				ToggleMode();
				item.useTime = 10;
				item.useAnimation = 10;
				item.mana = 0;
            }else{
				item.useStyle = 5;
				item.noUseGraphic = true;
				item.useTime = 7;
				item.useAnimation = 7;
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
				if(a.aiStyle==20||a.aiStyle==26||a.minionSlots>0||a.aiStyle==99||a.bobber)goto Predec;
			}else{
				Preinc:
				if(++mode>ProjectileLoader.ProjectileCount){
					mode=1;
				}
				a = new Projectile();
				a.SetDefaults(mode);
				if(a.aiStyle==20||a.aiStyle==26||a.minionSlots>0||a.aiStyle==99||a.bobber)goto Preinc;
			}
		}
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronAnvil, 999);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadAnvil, 999);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateInventory(Player player){
            if(timesinceright>0){
                timesinceright--;
            }
		}
		public override void PostShoot(int p){
			Main.projectile[p].GetGlobalProjectile<PandoraGlobalProjectile>().useIt = true;
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
			if(mod.TextureExists("Items/Pandora"))spriteBatch.Draw(mod.GetTexture("Items/Pandora"), position/*+new Vector2(20,17)*/, new Rectangle(0,0,10,50), drawColor, 0, new Vector2(), scale, SpriteEffects.None, 0);
			}
			catch (System.Exception){
				return true;
			}
			return false;
		}
	}
}
