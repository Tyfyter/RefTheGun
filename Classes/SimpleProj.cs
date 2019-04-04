using Terraria;
using Terraria.ModLoader.IO;

namespace RefTheGun.Classes {
    public class SimpleProj {
        public int type;
        public string name = "";
        public SimpleProj(int type){
			Projectile a = new Projectile();
			a.SetDefaults(type);
			new SimpleProj(type,a.Name);
        }
        public SimpleProj(int type, string name){
            this.type = type;
            this.name = name;
        }
    }
	public class ProjSerializer : TagSerializer<SimpleProj, TagCompound>
	{
		public override TagCompound Serialize(SimpleProj value)
		{
			TagCompound expr_05 = new TagCompound();
			expr_05["type"] = value.type;
			expr_05["name"] = value.name;
			return expr_05;
		}

		public override SimpleProj Deserialize(TagCompound tag)
		{
			return new SimpleProj(tag.GetInt("type"), tag.GetString("name"));
		}
	}
}