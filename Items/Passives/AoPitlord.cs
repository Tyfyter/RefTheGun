using Terraria;

namespace RefTheGun.Items.Passives {
    public class AoPitlord : PassiveBase {
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Amulet of the Pit Lord");
            base.SetStaticDefaults();
        }
        public override void Apply(Player player){
            player.noFallDmg = true;
        }
    }
}