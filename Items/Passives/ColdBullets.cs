using Terraria;

namespace RefTheGun.Items.Passives {
    public class ColdBullets : PassiveBase {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Frost Bullets");
            base.SetStaticDefaults();
        }
        public override void Apply(Player player){
            ((GunPlayer)player).bulletFreezeChance+=0.1f;
        }
    }
}