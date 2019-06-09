using Terraria;

namespace RefTheGun.Items.Passives {
    public class AlphaBullets : PassiveBase {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Alpha Bullets");
            base.SetStaticDefaults();
        }
        public override void Apply(Player player){
            if(player.HeldItem.modItem != null)
            if(player.HeldItem.modItem.mod==mod)
            if(((RefTheItem)player.HeldItem.modItem).isGun)
            if(((GunPlayer)player).roundsinmag>=((GunPlayer)player).magsize)player.rangedDamage*=1.8f;
        }
    }
}