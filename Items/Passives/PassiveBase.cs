using Terraria;

namespace RefTheGun.Items.Passives {
    public class PassiveBase : RefTheItem {
        public override bool isGun => false;
        public override bool Autoload(ref string name){
            if(name == "PassiveBase")return false;
            return true;
        }
        public virtual void Apply(Player player){}

        public override void SetStaticDefaults(){
            Tooltip.SetDefault("Equip this using the Gungeon bandoleer (because I couldn't find a better way to store passives).");
        }
        public override void SetDefaults(){
            item.rare+=3;
        }
    }
}