using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using RefTheGun.UI;
using RefTheGun.Buffs;
using RefTheGun.Items;
using RefTheGun.Classes;
using Terraria.ModLoader.IO;

namespace RefTheGun {
    internal class GunPlayer : ModPlayer {
        int hotslot = 0;
        public int reloaddelay = 0;
        public bool Reloading = false;
        public bool Reloaded = false;
        public int roundsinmag = 0;
        public int magsize = 0;
        public float MagMultiply = 1;
        public String guninfo = "";
        public int channelsword = 0;
        public bool Heresy = false;
        public bool Chairesy = false;
        public float bulletPoisonChance = 0;
        public bool bullet20Slow = false;
        public float multishotmult = 1;

        public override bool Autoload(ref string name) {
            return true;
        }

        public override void ResetEffects(){
            if(hotslot != player.HotbarOffset)Reloading = false;
            if(hotslot != player.HotbarOffset)Reloaded = false;
            if(hotslot != player.HotbarOffset)reloaddelay = 3;
            if(Reloaded)Reloading = false;
            hotslot = player.HotbarOffset;
            reloaddelay = Math.Max(reloaddelay-1, 0);
            MagMultiply = 1;
            bulletPoisonChance = 0;
            bullet20Slow = false;
            if(channelsword<=0)Heresy = false;
            if(channelsword<=0)Chairesy = false;
            if(channelsword>0)channelsword--;
        }
        public override bool ShiftClickSlot(Item[] inventory, int context, int slot){
            if(player.HeldItem!=null){if(inventory[slot].modItem!=null&&player.HeldItem.modItem!=null){if(inventory[slot].modItem is RefTheItem&&player.HeldItem.modItem is Coalescence&&!(inventory[slot].modItem is Coalescence)){
                        ((Coalescence)inventory[player.selectedItem].modItem).effects.Add(new CoalEffNew<Projectile>(inventory[slot].modItem.Name,inventory[slot].modItem.DisplayName.GetDefault()));//new CoalEff<Projectile>(((RefTheItem)player.HeldItem.modItem).OnHitEffect
                        return true;
                    }
                }
            }
            return base.ShiftClickSlot(inventory, context, slot);
        }
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource){
            if(player.HasBuff(mod.BuffType<ToDBuff>())){
                player.DelBuff(player.FindBuffIndex(mod.BuffType<ToDBuff>()));
                player.statLife+=player.statLifeMax2/25;
                player.immuneTime+=120;
                for(int i = 0; i < player.buffType.Length-1; i++){
                    if(Main.debuff[player.buffType[i]])player.DelBuff(i);
                }
                player.AddBuff(mod.BuffType<ToDCD>(), 7200);
                return false;
            }
            return true;
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
            if(damageSource.SourceCustomReason==" Got a bit carried away")playSound = false;
			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}
        public override void Load(TagCompound tag){
            multishotmult = tag.GetFloat("Multishot");
        }
        public override void FrameEffects(){
            multishotmult = 1;
        }
        public override void ModifyScreenPosition(){
            if(player.channel&&!(player.itemAnimation>player.itemAnimation*0.85f)){
                foreach(Projectile proj in Main.projectile){
                    if(proj.owner == player.whoAmI && (proj.Name=="Way of White Corona" || proj.Name=="Blazing Chakram") && proj.active){
						if(player.controlJump){
							player.Teleport(proj.Center);
							proj.Kill();
						}
                        Main.screenPosition = proj.Center-new Vector2(Main.screenWidth/2,Main.screenHeight/2);
                        return;
                    }
                }
            }
        }
    }
}
