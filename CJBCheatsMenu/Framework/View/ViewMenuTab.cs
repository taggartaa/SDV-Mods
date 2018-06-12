using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewMenuTab : StardewValley.Menus.OptionsElement
    {
        public Menu.IMenu Menu { get; private set; }

        private Action<Menu.IMenu> OnPressedAction;

        public bool Intensify { get; set; } = false;

        public ViewMenuTab(Menu.IMenu menu, int width, Action<Menu.IMenu> pressAction)
            : base(menu.Title, new Rectangle(0, 0, width, StardewValley.Game1.tileSize), -1)
        {
            this.Menu = menu;
            this.OnPressedAction = pressAction;
        }

        public override void receiveLeftClick(int x, int y)
        {
            this.OnPressedAction(this.Menu);
        }

        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            CJB.DrawTextBox(slotX + this.bounds.Width, slotY, StardewValley.Game1.smallFont, this.Menu.Title, 2, this.Intensify ? 1F : 0.7F);
        }
    }
}
