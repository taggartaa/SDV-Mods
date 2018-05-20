using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionSetButton<T> : Option<T>
    {
        private Rectangle SetButtonBounds { get; set; }
        private readonly Rectangle SetButtonSprite = new Rectangle(294, 428, 21, 11);
        private T Value { get; set; }

        public OptionSetButton(string label, int width, T value, Action<T> setValue) :
            base(label, width + 1, 11 * StardewValley.Game1.pixelZoom, setValue)
        {
            Value = value;
            SetButtonBounds = new Rectangle(width - 28 * StardewValley.Game1.pixelZoom, -1 + StardewValley.Game1.pixelZoom * 3, 21 * StardewValley.Game1.pixelZoom, 11 * StardewValley.Game1.pixelZoom);
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            StardewValley.Utility.drawTextWithShadow(spriteBatch, this.label, StardewValley.Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? StardewValley.Game1.textColor * 0.33f : StardewValley.Game1.textColor, 1f, 0.15f);
            StardewValley.Utility.drawWithShadow(spriteBatch, StardewValley.Game1.mouseCursors, new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), this.SetButtonSprite, Color.White, 0.0f, Vector2.Zero, StardewValley.Game1.pixelZoom, false, 0.15f);
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut || !this.SetButtonBounds.Contains(x, y))
            {
                return;
            }


            SetValue(Value);
        }
    }
}
