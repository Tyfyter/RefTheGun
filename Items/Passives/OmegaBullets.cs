using Terraria;

namespace RefTheGun.Items.Passives {
    public class OmegaBullets : PassiveBase {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Omega Bullets");
        }
        public override void Apply(Player player){
            if(player.HeldItem.modItem != null)
            if(player.HeldItem.modItem.mod==mod)
            if(((RefTheItem)player.HeldItem.modItem).isGun)
            if(((GunPlayer)player).roundsinmag<=1)player.rangedDamage*=2f;
        }
    }
}