using Terraria;

namespace RefTheGun.Items.Passives {
    public class Scattershot : PassiveBase {
        bool syn = false;
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Scattershot");
        }
        public override void Apply(Player player){
            player.rangedDamage*=0.4f;
            ((GunPlayer)player).multishotmult*=3;
            GunItemBelt b = (GunItemBelt)((GunPlayer)player).GetBandoleer().modItem;
            if(b.HasItem(mod.ItemType<PoisonBullets>())&&b.HasItem(mod.ItemType<ColdBullets>())&&b.HasItem(mod.ItemType<HotBullets>())){
                ((GunPlayer)player).multishotmult*=1.35f;
                ((GunPlayer)player).bulletPoisonChance+=0.05f;
                ((GunPlayer)player).bulletFreezeChance+=0.05f;
                ((GunPlayer)player).bulletBurnChance+=0.05f;
                b.synergies.Add("\"It's bound to do something\"");
                if(!syn){
                    Main.PlaySound(42, player.Center, 24);
                }
                syn = true;
            }else{
                syn = false;
            }
        }
    }
}