using Terraria;

namespace RefTheGun.Items.Passives {
    public class HotBullets : PassiveBase {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Hot Lead");
            base.SetStaticDefaults();
        }
        public override void Apply(Player player){
            ((GunPlayer)player).bulletBurnChance+=0.1f;
        }
    }
}