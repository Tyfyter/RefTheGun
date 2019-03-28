using Terraria;

namespace RefTheGun.Items.Passives {
    public class Plus1Bullets : PassiveBase {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("+1 Bullets");
        }
        public override void Apply(Player player){
            player.rangedDamage*=1.25f;
        }
    }
}