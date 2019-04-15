using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RefTheGun.Classes;
using RefTheGun.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RefTheGun.NPCs
{
	public class GunGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public float dmgmult = 1;
		/*
		0:
		multiplier

		1:
		duration

		2:
		human readable:
		1: single use
		2: type or id
		4: time based
		stylized:
		0 0 0 0
		1 2 4 8
		s t t n
		i y i o
		n p m t
		g e e h
		l     i
		e o b n
		  r a g
		u   s  
		s i e y
		e d d e
		      t
		
		3:
		type/id (see bit 2 of 2)
		*/
        public List<float[]> DMGBuffs = new List<float[]>{};
		public List<Buff> Buffs = new List<Buff>{};
        public override void AI(NPC npc){
            for(int i = 0; i<DMGBuffs.Count; i++){
				if((((int)DMGBuffs[i][2])&4)!=0)continue;
                DMGBuffs[i][1]--;
                if(DMGBuffs[i][1]<=0){
                    DMGBuffs.RemoveAt(i);
                }
            }
            Buffs.RemoveAll(Buff.GC);
			for(int i = 0; i<Buffs.Count; i++){
				Buffs[i].Update(npc);
            }
        }
        public override bool PreAI(NPC npc){
			bool a = true;
			bool b = !npc.HasBuff(BuffID.Frozen);
			if(b)npc.color=Color.Aqua*0.5f;
            Buffs.RemoveAll(Buff.GC);
			if(Buffs.Count>0)for(int i = 0; i<Buffs.Count; i++){
				a = a&&Buffs[i].PreUpdate(npc);
            }
			if(a&&!b)AI(npc);
			return a&&b;
        }
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit){
			damage = (int)(damage*dmgmult);
		}
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection){
			if(DMGBuffs.Count>0){
				int index = 0;
				List<int> toRemove = new List<int>{};
				foreach (var item in DMGBuffs){
					index++;
					if(item.Length>2){
						if(item.Length>3){
							if((projectile.whoAmI==(int)item[3]&&(((int)item[2])&2)==0)||(projectile.type==(int)item[3]&&(((int)item[2])&2)!=0)){
								damage = (int)(damage*item[0]);
								if((((int)item[2])&1)!=0)toRemove.Add(index);
							}
						}else{
							damage = (int)(damage*item[0]);
							if((((int)item[2])&1)!=0)toRemove.Add(index);
						}
					}/*else{
						damage = (int)(damage*item[0]);
						if(item.Length>2){
							toRemove.Add(index);
						}
					}*/
				}
				toRemove.Reverse();
				foreach (int i in toRemove){
					DMGBuffs.RemoveAt(i-1);
				}
			}
			damage = (int)(damage*dmgmult);
		}
		public override void NPCLoot(NPC npc){
			if(npc.type==NPCID.AngryBones||npc.type==NPCID.DarkCaster){
				if(Main.rand.NextFloat(0,10)<= (Main.expertMode ? 3 : 1.5)){
					Item.NewItem(npc.position, new Vector2(npc.width,npc.height), mod.ItemType<LittleBell>());
				}
			}else if(npc.type==NPCID.SkeletronHead||npc.type==NPCID.SkeletronHand){
				if(Main.rand.NextFloat(0,10)<= (Main.expertMode ? 10 : 0.75)){
					Item.NewItem(npc.position, new Vector2(npc.width,npc.height), mod.ItemType<ClericChime>());
				}
			}else if(npc.type==NPCID.WallofFlesh||npc.type==NPCID.WallofFleshEye){
				if(Main.rand.NextFloat(0,10)<= (Main.expertMode ? 10 : 0.75)){
					Item.NewItem(npc.position, new Vector2(npc.width,npc.height), mod.ItemType<WoFChime>());
				}
			}else if(npc.type==NPCID.Golem){
				if(Main.rand.NextFloat(0,10)<= (Main.expertMode ? 7.5 : 4)){
					Item.NewItem(npc.position, new Vector2(npc.width,npc.height), mod.ItemType<GolemHeart>());
				}
			}
			for(int i = 0; i<Buffs.Count; i++){
				Buffs[i].OnDeath(npc);
            }
			Buffs = new List<Buff>();
		}
		public static void AddBuff(Buff buff){buff.npc.GetGlobalNPC<GunGlobalNPC>().Buffs.Add(buff);}
	}
}