using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewOptionSetButton : ViewOptionSetButtonBase<Menu.IOptionSetButton>
    {

        public ViewOptionSetButton(Menu.IOptionSetButton setButtonOption, int containerWidth) :
            base(setButtonOption, containerWidth)
        {
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut || !this.SetButtonBounds.Contains(x, y))
            {
                return;
            }
            this.Option.OnPressed();
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            base.draw(spriteBatch, slotX, slotY);
            StardewValley.Utility.drawTextWithShadow(spriteBatch, this.label, StardewValley.Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? StardewValley.Game1.textColor * 0.33f : StardewValley.Game1.textColor, 1f, 0.15f);
        }
    }
}
