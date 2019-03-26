using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using System.Linq;
using Terraria.Localization;
using ReLogic.Graphics;

namespace RefTheGun.UI
{
	// ExampleUIs visibility is toggled by typing "/coin" in chat. (See CoinCommand.cs)
	// ExampleUI is a simple UI example showing how to use UIPanel, UIImageButton, and even a custom UIElement.
	class AmmoDisplay : UIState
	{
		public DragableUIPanel coinCounterPanel;
		public UIMoneyDisplay moneyDiplay;
		public static bool visible = false;

		// In OnInitialize, we place various UIElements onto our UIState (this class).
		// UIState classes have width and height equal to the full screen, because of this, usually we first define a UIElement that will act as the container for our UI.
		// We then place various other UIElement onto that container UIElement positioned relative to the container UIElement.
		public override void OnInitialize()
		{
			// Here we define our container UIElement. In DragableUIPanel.cs, you can see that DragableUIPanel is a UIPanel with a couple added features.
			coinCounterPanel = new DragableUIPanel();
			coinCounterPanel.SetPadding(0);
			// We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(coinCounterPanel);`. 
			// This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
			coinCounterPanel.Left.Set(400f, 0f);
			coinCounterPanel.Top.Set(100f, 0f);
			coinCounterPanel.Width.Set(170f, 0f);
			coinCounterPanel.Height.Set(70f, 0f);
			coinCounterPanel.BackgroundColor = new Color(73, 94, 171);

			// UIMoneyDisplay is a fairly complicated custom UIElement. UIMoneyDisplay handles drawing some text and coin textures.
			// Organization is key to managing UI design. Making a contained UIElement like UIMoneyDisplay will make many things easier.
			moneyDiplay = new UIMoneyDisplay();
			moneyDiplay.Left.Set(15, 0f);
			moneyDiplay.Top.Set(20, 0f);
			moneyDiplay.Width.Set(100f, 0f);
			moneyDiplay.Height.Set(0, 1f);
			coinCounterPanel.Append(moneyDiplay);

			base.Append(coinCounterPanel);

			// As a recap, ExampleUI is a UIState, meaning it covers the whole screen. We attach coinCounterPanel to ExampleUI some distance from the top left corner.
			// We then place playButton, closeButton, and moneyDiplay onto coinCounterPanel so we can easily place these UIElements relative to coinCounterPanel.
			// Since coinCounterPanel will move, this proper organization will move playButton, closeButton, and moneyDiplay properly when coinCounterPanel moves.
		}

		public void updateValue(int pickedUp)
		{
			moneyDiplay.coins += pickedUp;
			moneyDiplay.addCPM(pickedUp);
		}
	}

	public class UIMoneyDisplay : UIElement
	{
		public long coins;

		public UIMoneyDisplay()
		{
			Width.Set(100, 0f);
			Height.Set(40, 0f);

			for (int i = 0; i < 60; i++)
			{
				coinBins[i] = -1;
			}
		}

		//DateTime dpsEnd;
		//DateTime dpsStart;
		//int dpsDamage;
		public bool dpsStarted;
		public DateTime dpsLastHit;

		// Array of ints 60 long.
		// "length" = seconds since reset
		// reset on button or 20 seconds of inactivity?
		// pointer to index so on new you can clear previous
		int[] coinBins = new int[60];
		int coinBinsIndex;

		public void addCPM(int coins)
		{
			int second = DateTime.Now.Second;
			if (second != coinBinsIndex)
			{
				coinBinsIndex = second;
				coinBins[coinBinsIndex] = 0;
			}
			coinBins[coinBinsIndex] += coins;
		}

		public int getCPM()
		{
			int second = DateTime.Now.Second;
			if (second != coinBinsIndex)
			{
				coinBinsIndex = second;
				coinBins[coinBinsIndex] = 0;
			}

			long sum = coinBins.Sum(a => a > -1 ? a : 0);
			int count = coinBins.Count(a => a > -1);
			if(count == 0)
			{
				return 0;
			}
			return (int)((sum * 60f) / count);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 drawPos = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 30f);

			float shopx = innerDimensions.X;
			float shopy = innerDimensions.Y;
			for (int j = 0; j < 4; j++)
			{
				spriteBatch.Draw(Main.itemTexture[3], new Vector2(100, 100), null, Color.White, 0f, Main.itemTexture[3].Size() / 2f, 1f, SpriteEffects.None, 0f);
				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontItemStack, "test", 100, 100/* + 75f*/, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
			}
			//Utils.DrawBorderStringFourWay(spriteBatch, /*ExampleMod.exampleFont*/ Main.fontItemStack, "CPM", shopx + (float)(24 * 4), shopy + 25f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
		}
	}
}