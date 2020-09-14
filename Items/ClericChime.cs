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
	public class ClericChime : ChimeBase
	{
		/*protected LegacySoundStyle useSound = new LegacySoundStyle(2, 101);
		//LegacySoundStyle useSound = new LegacySoundStyle(2, 72);
		protected String[] Spells = new String[10]{"Lightning Spear","Great Lightning Spear","Sunlight Spear","Lightning Arrow","Way of White Corona","Wrath of the Gods","Lighnting Storm","Lifehunt Scythe","Blessed Weapon","Great Magic Barrier"};
		protected int spell = 0;
		protected float[] SpellDamage = new float[10]{1,1.25f,1.75f,1.5f,1.85f,0.75f,1.3f,2,0,0};
		protected int[] SpellAmmo = new int[10]{60,30,40,0,0,5,4,3,6,0};
		protected int[] SpellMana = new int[10]{5,7,10,15,25,40,50,80,35,20};
		protected float[] SpellSpeed = new float[10]{2f,1.85f,1.75f,1.25f,0.75f,0.5f,0.45f,0.4f,1f,1f};
		*/
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cleric's Sacred Chime");
			Tooltip.SetDefault("This also really doesn't belong in this mod.");
			Item.claw[item.type] = true;
		}
		public override void SetDefaults()
		{
			item.damage = 15;
			item.magic = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useTurn = true;
			item.width = 20;
			item.height = 20;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.shoot = ProjectileID.MagicDagger;
			item.shootSpeed = 12.5f;
			item.UseSound = useSound;
			MaxAmmo = SpellAmmo[spell];
			MinSpread = 0f;
			BloomPerShot = 0f;
			ReloadTimeMax = 45;
			reloadwhenfull = true;

			Ammo = MaxAmmo;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient(ModContent.ItemType<LittleBell>(), 4);
			recipe.AddTile(TileID.Furnaces);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		/* 

		public override void HoldItem(Player player){
			player.GetModPlayer<GunPlayer>().guninfo = Spells[spell];
			base.HoldItem(player);
		}
		
        public override bool CanUseItem(Player player){
            if(player.altFunctionUse == 2){
				item.useStyle = 1;
                item.UseSound = new LegacySoundStyle(2, 7);
				item.reuseDelay = 0;
				item.channel = false;
            }else{
				item.useStyle = 5;
				item.channel = false;
				if(Spells[spell]=="Way of White Corona"){
					player.GetModPlayer<GunPlayer>().magsize=-2;
					for(int i = 0; i < Main.projectile.Length; i++){
						if(Main.projectile[i].owner == player.whoAmI && Main.projectile[i].Name=="Way of White Corona" && Main.projectile[i].active){
							player.GetModPlayer<GunPlayer>().magsize=-1;
							return false;
						}
					}
					item.channel = true;
				}
            }
			return base.CanUseItem(player)&&(player.altFunctionUse == 2 || player.CheckMana(SpellMana[spell], true));
        }
		public override float UseTimeMultiplier(Player player){
			return player.altFunctionUse != 2 ? SpellSpeed[spell] : 1;
			//return Spells[spell]=="Fire Surge"&&player.altFunctionUse != 2 ? 2.5f : 1;
		}
		
		public override void restoredefaults(){
			MaxAmmo = SpellAmmo[spell];
			item.UseSound = Spells[spell].Contains("Faron Dart") ? new LegacySoundStyle(2, 63) : useSound;
			Spread = 0;
		}
		
		public override void SpecialReloadProj(Player player, int ammoleft){
			if(ammoleft>=SpellAmmo[spell]){
				spell++;
				if(spell>=Spells.Length)spell=0;
			}
		}
		public override void OnHitEffect(Projectile projectile){
            if(projectile.Name=="Way of White Corona"){
				projectile.ai[0] = 0;
				projectile.velocity = projectile.oldVelocity;
				projectile.GetGlobalProjectile<GunGlobalProjectile>().timesincestop=60;
				projectile.GetGlobalProjectile<GunGlobalProjectile>().stopping=2;
			}else if(projectile.Name=="Soul Geyser"){
				for (int i = 0; i < 4; i++)
				{
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X*(1+Main.rand.Next(1)), projectile.velocity.Y).RotatedBy(MathHelper.ToRadians((10*i)-15));
					int a = Projectile.NewProjectile(projectile.position.X+perturbedSpeed.X, projectile.position.Y+perturbedSpeed.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.LostSoulFriendly, projectile.damage, projectile.knockBack, projectile.owner);
					Main.projectile[a].usesLocalNPCImmunity = true;
					Main.projectile[a].localNPCHitCooldown = 2;
				}
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>();
			damage = (int)(damage*SpellDamage[spell]);
            if(player.altFunctionUse == 2)return false;
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			//position+=new Vector2(speedX, speedY).Normalized().RotatedBy(90*player.direction)*3;
            Spread = Math.Min(Spread, MaxSpread);
			MinSpread = 0;
			MaxSpread = 0;
			if(Spells[spell].Contains("Spear")){
				type = ProjectileID.JavelinFriendly;
			}else if(Spells[spell]=="Lightning Arrow"){
				type=ModContent.ProjectileType<SoulArrow>();
				position+=new Vector2(speedX, speedY).Normalized().RotatedBy(90*player.direction)*3;
			}else if(Spells[spell]=="Way of White Corona"){
				type=ProjectileID.LightDisc;
				speedX*=1.5f;
				speedY*=1.5f;
			}else if(Spells[spell].Contains("Wrath of the Gods")){
				type=ModContent.ProjectileType<SoulArrow>();
				position = player.Center;
				speedX=0;
				speedY=0;
			}else if(Spells[spell]=="Lightning Storm"){
				type = ProjectileID.ChargedBlasterOrb;
				float rotation = MathHelper.ToRadians(360);
				position += Vector2.Normalize(new Vector2(speedX, speedY)) * 30f;
				for (int i = 0; i < 8; i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy((rotation/7)*(i-3)); // Watch out for dividing by 0 if there is only 1 projectile.
					int a = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 1);
					Main.projectile[a].hostile = false;
					Main.projectile[a].friendly = true;
					Main.projectile[a].usesLocalNPCImmunity = true;
					Main.projectile[a].localNPCHitCooldown = 3;
				}
            	Ammo--;
				return false;
			}else if(Spells[spell]=="Lifehunt Scythe"){
			}else{
				return false;
			}
            int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY).RotatedByRandom(MathHelper.ToRadians(Spread)), type, damage, knockBack, item.owner);
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].friendly = true;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 3;
			Main.projectile[proj].Name = Spells[spell];
            Spread = Math.Min(Spread+BloomPerShot, MaxSpread);
			if(type==ProjectileID.BallofFire)Main.projectile[proj].ai[0] = 4;
			if(Spells[spell].Contains("Way of White Corona")){
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().effectonhit = true;
				Main.projectile[proj].GetGlobalProjectile<GunGlobalProjectile>().firedwith = this;
				Main.projectile[proj].extraUpdates++;
			}
            if(MaxAmmo>0)Ammo--;
            return false;
		}*/
	}
}
