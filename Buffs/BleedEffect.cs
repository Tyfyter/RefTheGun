using RefTheGun.Classes;
using Terraria;

namespace RefTheGun.Buffs {
    public class BleedEffect : Buff{
        int rate = 1;
        int damage = 1;
        public BleedEffect(NPC npc, int damage, int duration, int rate = 10) : base(npc){
            this.damage = damage;
            this.duration = duration;
            this.rate = rate;
        }
        public override void Update(NPC npc){
            if(duration%rate==0){
                int[] a = npc.immune;
                npc.StrikeNPC(damage, 0, 0, false, true, true);
                npc.immune = a;
            }
            base.Update(npc);
        }
    }
}