using Terraria;

namespace RefTheGun.Items.Passives {
    public class Scattershot : PassiveBase {
        bool syn = false;
        public override void SetStaticDefaults(){
            DisplayName.SetDefault("Scattershot");
        }
        public override void Apply(Player player){
            player.rangedDamage*=0.4f;
            ((GunPlayer)player).multishotmult+=2;
            GunItemBelt b = (GunItemBelt)((GunPlayer)player).GetBandoleer().modItem;
            if(b.HasItem(mod.ItemType<PoisonBullets>())&&b.HasItem(mod.ItemType<ColdBullets>())&&b.HasItem(mod.ItemType<HotBullets>())){
                ((GunPlayer)player).multishotmult+=1;
                ((GunPlayer)player).bulletPoisonChance+=0.05f;
                ((GunPlayer)player).bulletFreezeChance+=0.05f;
                ((GunPlayer)player).bulletBurnChance+=0.05f;
                if(!syn){
                    player.chatOverhead.NewMessage("\"it's bound to do something\"",75);
                    Main.PlaySound(42, player.Center, 24);
                }
                syn = true;
            }else{
                syn = false;
            }
        }
    }
}