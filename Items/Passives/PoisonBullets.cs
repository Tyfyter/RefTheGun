using Terraria;

namespace RefTheGun.Items.Passives {
    public class PoisonBullets : PassiveBase {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Irradiated Lead");
            base.SetStaticDefaults();
        }
        public override void Apply(Player player){
            ((GunPlayer)player).bulletPoisonChance+=0.1f;
        }
    }
}