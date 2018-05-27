using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewOptionCheckbox : ViewOption<Menu.IOptionCheckbox>
    {

        public ViewOptionCheckbox(Menu.IOptionCheckbox checkboxOption)
            : base(checkboxOption, 9 * StardewValley.Game1.pixelZoom, 9 * StardewValley.Game1.pixelZoom, 0)
        {
        }

        public bool Checked
        {
            get
            {
                return this.Option.Value;
            }
            set
            {
                this.Option.Value = value;
            }
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;

            StardewValley.Game1.soundBank.PlayCue("drumkit6");
            base.receiveLeftClick(x, y);
            this.Checked = !this.Checked;
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            Vector2 position = new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y);
            Rectangle sourceRect = this.Checked ? StardewValley.Menus.OptionsCheckbox.sourceRectChecked : StardewValley.Menus.OptionsCheckbox.sourceRectUnchecked;
            Color color = Color.White * (this.greyedOut ? 0.33f : 1f);
            spriteBatch.Draw(StardewValley.Game1.mouseCursors, position,  sourceRect, color, 0.0f, Vector2.Zero, StardewValley.Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(spriteBatch, slotX, slotY);
        }
    }
}
