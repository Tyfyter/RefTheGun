using Terraria;

namespace RefTheGun.Items.Passives {
    public class PassiveBase : RefTheItem {
        public override bool isGun => false;
        public override bool Autoload(ref string name){
            if(name == "PassiveBase")return false;
            return true;
        }
        public virtual void Apply(Player player){}
    }
}