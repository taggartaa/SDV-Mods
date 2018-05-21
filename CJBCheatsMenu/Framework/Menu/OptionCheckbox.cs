using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionCheckbox : Option<bool>
    {
        public bool IsChecked { get; private set; }

        public OptionCheckbox(string label, bool initialValue, Action<bool> setValue)
            : base(label, 9 * StardewValley.Game1.pixelZoom, 9 * StardewValley.Game1.pixelZoom, setValue)
        {
            IsChecked = initialValue;
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (greyedOut)
                return;

            StardewValley.Game1.soundBank.PlayCue("drumkit6");
            base.receiveLeftClick(x, y);
            IsChecked = !IsChecked;
            SetValue(IsChecked);
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            spriteBatch.Draw(StardewValley.Game1.mouseCursors, new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y), this.IsChecked ? StardewValley.Menus.OptionsCheckbox.sourceRectChecked : StardewValley.Menus.OptionsCheckbox.sourceRectUnchecked, Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, StardewValley.Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(spriteBatch, slotX, slotY);
        }
    }
}
