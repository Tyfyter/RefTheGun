using System;
using Terraria;
using Terraria.ModLoader.IO;

namespace RefTheGun.Classes {
    public class SimpleProj : TagSerializable{
        public int type;
        public string name = "";
        public static readonly Func<TagCompound, SimpleProj> DESERIALIZER = new Func<TagCompound, SimpleProj>(Deserialize);
        public SimpleProj(){
			new SimpleProj(1, "error");
        }
        public SimpleProj(int type){
			Projectile a = new Projectile();
			a.SetDefaults(type);
			new SimpleProj(type,a.Name);
        }
        public SimpleProj(int type, string name){
            this.type = type;
            this.name = name;
        }
        public TagCompound SerializeData(){
			TagCompound expr_05 = new TagCompound();
			expr_05["type"] = type;
			expr_05["name"] = name;
			return expr_05;
        }
		public static SimpleProj Deserialize(TagCompound tag)
		{
            if(!tag.HasTag("type"))return new SimpleProj();
            if(!tag.HasTag("name"))return new SimpleProj(tag.GetInt("type"));
			return new SimpleProj(tag.GetInt("type"), tag.GetString("name"));
		}
    }
}