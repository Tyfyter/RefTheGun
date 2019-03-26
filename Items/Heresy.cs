using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RefTheGun;
using RefTheGun.Projectiles;
using RefTheGun.NPCs;

namespace RefTheGun.Items
{
	public class Heresy : RefTheItem
	{
		public override void SetDefaults()
		{
			//item.name = "Ice Gauntlet";
			item.damage = 100;
            //item.magic = true;
            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.channel = false;
            item.width = 26;
			item.height = 26;
			item.useTime = 4;
			item.useAnimation = 4;
            item.useStyle = 5;
            item.knockBack = 10;        
			item.value = 10000;
			item.rare = 2;
            item.useStyle = 5;
            item.shoot = mod.ProjectileType<HeresySword>();
            item.shootSpeed = 1f;
            item.autoReuse = true;
            reloadwhenfull = true;
            specialreloadproj = true;
            MaxAmmo = 0;
        }
        public override void restoredefaults(){}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Heresy");
		  Tooltip.SetDefault("");
		}
        public override void UpdateInventory(Player player){
            if(firedshots.Count>0)if(!Main.projectile[firedshots[0]].active){
                firedshots.Clear();
            }
        }

        public override void HoldStyle(Player player)
        {
            GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
            if ((!(player.velocity.Y > 0)) || player.sliding)
            {
                int dust3 = Dust.NewDust(player.Center, 0, 0, 90, 0f, 0f, 25, Color.Goldenrod, 0.5f);
                Main.dust[dust3].noGravity = true;
                Main.dust[dust3].velocity = new Vector2(0, 0);
            }
            else
            {
                int dust3 = Dust.NewDust(player.Top + new Vector2(player.direction * -10, 0), 0, 0, 90, 0f, 0f, 25, Color.Goldenrod, 0.5f);
                Main.dust[dust3].noGravity = true;
                Main.dust[dust3].velocity = new Vector2(0, 0);
            }
            //item.toolTip = modPlayer.stone + "";
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 50);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
        public override bool CanRightClick(){
            Player player = Main.player[item.owner];
            GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
            return !modPlayer.Heresy&&!(firedshots.Count>0);
        }
        public override void PostShoot(int p){
            Main.projectile[p].GetGlobalProjectile<GunGlobalProjectile>(mod).firedwith = (RefTheItem)item.modItem;
        }
        public override void OnHitEffect(Projectile projectile){
            Main.PlaySound(SoundID.DD2_GoblinBomb, projectile.Center);
            foreach (NPC t in hitenemies){
                ((GunGlobalNPC)t.GetGlobalNPC<GunGlobalNPC>()).DMGBuffs.Add(new float[]{1.05f,20,7,mod.ProjectileType<HeresySword>()});
            }
            hitenemies.Clear();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack){
            GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
            if(player.altFunctionUse==2&&!modPlayer.Heresy){
                PostShoot(Projectile.NewProjectile(position, new Vector2(speedX, speedY), mod.ProjectileType<HeresyAlt>(), damage, knockBack, player.whoAmI));
                return false;
            }
            if(!modPlayer.Heresy&&!(firedshots.Count>0)){
                firedshots.Add(Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI));
            }
            modPlayer.channelsword = 5;
            return false;
        }
    }
}
