using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.Items {
    public class HeroBow2 : HeroBow {
        public override string Texture => "RefTheGun/Items/HeroBow";
		public override void SetStaticDefaults(){
			DisplayName.SetDefault("Hero's Bow");
			Tooltip.SetDefault("Magic damage boosts are applied at 25% effectiveness.");
		}
        public override void SetDefaults(){
            base.SetDefaults();
            item.damage*=2;
			item.rare = ItemRarityID.LightPurple;
            ReloadTimeMax-=ReloadTimeMax/4;
        }
		public override void AddRecipes(){
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HeroBow>(), 1);
			recipe.AddIngredient(ItemID.RangerEmblem, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}