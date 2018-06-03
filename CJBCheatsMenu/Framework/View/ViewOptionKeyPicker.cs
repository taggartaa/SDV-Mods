using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewOptionKeyPicker : ViewOptionSetButtonBase<Menu.IOptionKeyPicker>
    {
        private StardewModdingAPI.ITranslationHelper I18n { get; set; }
        private bool Listening { get; set; } = false;
        private string ListenerMessage => this.I18n.Get("messages.press-new-key");
        public ViewOptionKeyPicker(Menu.IOptionKeyPicker keyPicker, int containerWidth, StardewModdingAPI.ITranslationHelper i18n)
            : base(keyPicker, containerWidth)
        {
            this.I18n = i18n;
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (!this.greyedOut && !this.Listening && this.SetButtonBounds.Contains(x, y))
            {
                this.Listening = true;
                StardewValley.Game1.soundBank.PlayCue("breathin");
                StardewValley.Menus.GameMenu.forcePreventClose = true;
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if (this.greyedOut || !this.Listening)
                return;
            if (key == Keys.Escape)
            {
                StardewValley.Game1.soundBank.PlayCue("bigDeSelect");
                this.Listening = false;
                StardewValley.Menus.GameMenu.forcePreventClose = false;
            }
            else
            {
                this.Option.Value = key;
                StardewValley.Game1.soundBank.PlayCue("coin");
                this.Listening = false;
                StardewValley.Menus.GameMenu.forcePreventClose = false;
            }
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            base.draw(spriteBatch, slotX, slotY);
            StardewValley.Utility.drawTextWithShadow(spriteBatch, this.label + ": " + this.Option.Value.ToString(), StardewValley.Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? StardewValley.Game1.textColor * 0.33f : StardewValley.Game1.textColor, 1f, 0.15f);
            if (this.Listening)
            {
                spriteBatch.Draw(StardewValley.Game1.staminaRect, new Rectangle(0, 0, StardewValley.Game1.graphics.GraphicsDevice.Viewport.Width, StardewValley.Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle(0, 0, 1, 1), Color.Black * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.999f);
                spriteBatch.DrawString(StardewValley.Game1.dialogueFont, this.ListenerMessage, StardewValley.Utility.getTopLeftPositionForCenteringOnScreen(StardewValley.Game1.tileSize * 3, StardewValley.Game1.tileSize), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
            }
        }
    }
}
