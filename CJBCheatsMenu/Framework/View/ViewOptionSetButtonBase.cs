using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewOptionSetButtonBase<T> : ViewOption<T> where T : Menu.IOption
    {
        protected Rectangle SetButtonBounds { get; set; }
        protected readonly Rectangle SetButtonSprite = new Rectangle(294, 428, 21, 11);

        public ViewOptionSetButtonBase(T setButtonOption, int containerWidth) :
            base(setButtonOption, containerWidth + 1, 11 * StardewValley.Game1.pixelZoom)
        {
            SetButtonBounds = new Rectangle(containerWidth - 28 * StardewValley.Game1.pixelZoom, -1 + StardewValley.Game1.pixelZoom * 3, 21 * StardewValley.Game1.pixelZoom, 11 * StardewValley.Game1.pixelZoom);
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            // base draw isn't called, so have to set this here
            this.label = this.Option.Label;
            this.greyedOut = this.Option.Disabled;

            StardewValley.Utility.drawWithShadow(spriteBatch, StardewValley.Game1.mouseCursors, new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), this.SetButtonSprite, Color.White, 0.0f, Vector2.Zero, StardewValley.Game1.pixelZoom, false, 0.15f);
        }
    }
}
