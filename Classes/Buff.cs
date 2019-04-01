using Terraria;

namespace RefTheGun.Classes {
    public class Buff {
        public bool isActive{
            get{return duration>0&&npc.active;}
        }
        public string Name{
            get{return this.GetType().Name;}
        }
        public int duration;
        public NPC npc;
        public Buff(NPC npc){
            this.npc = npc;
        }
        public virtual bool PreUpdate(NPC npc){return true;}
        public virtual void Update(NPC npc){duration--;}
        public virtual void OnDeath(NPC npc){duration=-1;}
        public static bool GC(Buff buff){return !buff.isActive;}
    }
}