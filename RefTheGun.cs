using Terraria.ModLoader;
using Terraria.UI;
using RefTheGun.UI;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using RefTheGun.Items;
using Terraria.ModLoader.IO;
using RefTheGun.Classes;

namespace RefTheGun
{
	public class RefTheGun : Mod
	{
        public static RefTheGun mod;
		internal UserInterface UI;
		public GunItemsUI gunItemUI;
		//string[] ammotexts = new string[2]{"∞"};
		public RefTheGun()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
        public override void Load()
        {
            mod = this;
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
			if (!Main.dedServ){
				UI = new UserInterface();
			}
        }

        public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Evil Bars", new int[]
			{
				ItemID.DemoniteBar,
				ItemID.CrimtaneBar
			});
			RecipeGroup.RegisterGroup("RefTheGun:EvilBars", group);
		}
		public override void PostDrawInterface(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch){
            Player player = Main.player[Main.myPlayer];
			GunPlayer modPlayer = player.GetModPlayer<GunPlayer>(mod);
            if(player.HeldItem.modItem == null){
                return;
            }else if(player.HeldItem.modItem.mod!=mod){
                return;
            }else if(!((RefTheItem)player.HeldItem.modItem).isGun){
				return;
			}else if(Main.playerInventory){
				return;
			}
			if(modPlayer.magsize!=0)Utils.DrawBorderStringFourWay(spriteBatch, Main.fontCombatText[1], (modPlayer.magsize > 0 ? modPlayer.roundsinmag+"/"+modPlayer.magsize : (modPlayer.magsize == 0 ? "∞" : (modPlayer.magsize == -1 ? modPlayer.roundsinmag+"" : ""))), Main.screenWidth*0.90f, Main.screenHeight*0.85f, Color.White, Color.Black, new Vector2(0.3f), 1);
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontCombatText[1], modPlayer.guninfo, Main.screenWidth*0.90f, Main.screenHeight*0.90f, modPlayer.guninfocolor, new Color(0,0,0,modPlayer.guninfocolor.A), new Vector2(0.3f), 0.8f);
			modPlayer.guninfo="";
			modPlayer.guninfocolor=Color.White;
		}
		public override void UpdateUI(GameTime gameTime) {
            if(RefTheGun.mod.UI.CurrentState!=null&&!Main.playerInventory){
				UI.SetState(null);
			}
			UI?.Update(gameTime);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (inventoryIndex != -1) {
				layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
					"RefTheGun: TheOnlyRealUIInTheMod",
					delegate {
						// If the current UIState of the UserInterface is null, nothing will draw. We don't need to track a separate .visible value.
						UI.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
	public static class RefTheExtensions{
		public static Vector2 Normalized(this Vector2 input, bool errorcheck){
			input.Normalize();
			return input;
		}
		public static Vector2 Normalized(this Vector2 input){
			bool n = false;
			return input.Normalized(n);
		}
		public static Color toColor(this Vector4 i){
			return new Color(i.X,i.Y,i.Z,i.W);
		}
		public static Color toColor(this Vector3 i){
			return new Color(i.X,i.Y,i.Z);
		}
		public static Vector2 toVector2(this Vector3 i){
			return new Vector2(i.X,i.Y);
		}
        public static Rectangle ProperBox(Point a, Point b){
			int x1,x2,y1,y2;
			if(b.X>a.X){
				x1=a.X;
				x2=b.X-a.X;
			}else{
				x1=b.X;
				x2=a.X-b.X;
			}
			if(b.Y>a.Y){
				y1=a.Y;
				y2=b.Y-a.Y;
			}else{
				y1=b.Y;
				y2=a.Y-b.Y;
			}
			return new Rectangle(x1,y1,x2,y2);
        }
        public static Rectangle ProperBox(Vector2 a, Vector2 b){

			return ProperBox(a.ToPoint(),b.ToPoint());
        }
        public static Rectangle ProperBox(this Ray ray){
			return ProperBox(ray.Position.toVector2(), (ray.Position+ray.Direction).toVector2());
        }
		/**
		finds the nearest NPC to a Vector2
		 */
		public static int findNearestNPC(Vector2 i, int dist = -1, Func<NPC,bool> req = null){
			if(req==null)req=retrueNPC;
			int o = -1;
			for (int i1 = 0; i1 < Main.npc.Length; i1++){
				if (!Main.npc[i1].GivenOrTypeName.ToLower().Contains("dummy")&&(dist==-1||Math.Abs((Main.npc[i1].position-i).Length())<dist)&&req.Invoke(Main.npc[i1])){
					o=i1;
					dist=(int)Math.Abs((Main.npc[i1].position-i).Length());
				}
			}
			return o;
		}
        public static bool isWithin(this float input, float min, float max, ref float confined){
			if (input>max||input<min){
				confined = input>max?max:min;
				return false;
			}
            return true;
        }
        public static bool isWithin(this float input, float min, float max){
			float no = 0;
            return input.isWithin(min, max, ref no);
        }
		public static bool retrueNPC(NPC yes){
			return true;
		}
		public static bool hostNPC(NPC maybe){
			return !maybe.friendly;
		}
		public static bool frenPC(NPC maybe){
			return maybe.friendly;
		}
        public static string Concat<T>(this T[] i){
            string o = "[";
            for (int i2 = 0; i2 < i.Length; i2++) {
                o+=(i[i2]!=null?i[i2].ToString():"null")+(i2==i.Length-1?"":",");
            }
            return o+"]";
        }
        public static string Concat<T>(this IList<T> i){
            string o = "[";
            for (int i2 = 0; i2 < i.Count; i2++) {
                o+=(i[i2]!=null?i[i2].ToString():"null")+(i2==i.Count-1?"":",");
            }
            return o+"]";
        }
		public static List<T> Merge<T>(this List<T> a, List<T> b) where T : class{
			List<T> c = new List<T>(){};
			int d = Math.Max(a.Count,b.Count);
			for (int i = 0; i < d; i++)c.Add(null);
			for (int i = 0; i < d; i++){
				if(i>=a.Count){
					c[i] = b[i];
				}else if(i>=b.Count){
					c[i] = a[i];
				}else{
					c[i] = b[i]??a[i];
				}
			}
			return c;
		}
		public static void MergeIn<T>(this List<T> a, List<T> b) where T : class{
			List<T> c = new List<T>(){};
			int d = Math.Max(a.Count,b.Count);
			for (int i = 0; i < d; i++)c.Add(null);
			for (int i = 0; i < d; i++){
				if(i>=a.Count){
					c[i] = b[i];
				}else if(i>=b.Count){
					c[i] = a[i];
				}else{
					c[i] = b[i]==null?a[i]:b[i];
				}
			}
			a.Clear();
			for (int i = 0; i < d; i++)a.Add(null);
			for (int i = 0; i < d; i++)a[i] = c[i];
		}
		
		public class TrueNullable<T> {
			public T value;
			public TrueNullable(T value){
				this.value=value;
			}
			public static implicit operator T(TrueNullable<T> input){
				return input.value;
			}
			public static implicit operator TrueNullable<T>(T input){
				return new TrueNullable<T>(input);
			}
			public override string ToString(){
				return value.ToString()+"?";
			}
		}
	}
}
