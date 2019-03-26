using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Text;
using System.Reflection;
using Terraria.Audio;
using RefTheGun.Projectiles;
using RefTheGun.Items;

namespace RefTheGun.Classes{
    public class CoalEff<T>{
        public string name;
        public Action<T> effect;
        public CoalEff(Action<T> effect, string name = "unnamed"){
            this.effect = effect;
            this.name = name;
        }
    }
    public class CoalEffNew<T>{
        public string name;
        public string effect;
        public CoalEffNew(string internalname, string name = "unnamed"){
            this.effect = internalname;
            this.name = name;
        }
        public void InvokeOnHit(Projectile input){
            int a = Item.NewItem(new Vector2(), 0, 0, RefTheGun.mod.ItemType(effect), 0);
			((RefTheItem)Main.item[a].modItem).OnHitEffect(input);
			Main.item[a].TurnToAir();
        }
    }
}