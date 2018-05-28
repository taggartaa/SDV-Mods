using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewOptionHeartPicker : ViewOption<Menu.IOptionHeartPicker>
    {
        private static int MAX_HEARTS = 10;

        private StardewValley.NPC npcForCurrentMugshot;
        StardewValley.Menus.ClickableTextureComponent cachedMugshot;

        private StardewValley.Menus.ClickableTextureComponent Mugshot
        {
            get
            {
                if (this.Option.NPC != npcForCurrentMugshot)
                {
                    this.npcForCurrentMugshot = this.Option.NPC;
                    cachedMugshot = new StardewValley.Menus.ClickableTextureComponent("Mugshot", this.bounds, "", "", this.Option.NPC.Sprite.Texture, this.Option.NPC.getMugShotSourceRect(), 0.7f * StardewValley.Game1.pixelZoom);
                }
                return cachedMugshot;
            }
        }

        public ViewOptionHeartPicker(Menu.IOptionHeartPicker heartPickerOption)
            : base(heartPickerOption, 80 * StardewValley.Game1.pixelZoom, 6 * StardewValley.Game1.pixelZoom, 0)
        {
            this.bounds.X = 96;
        }

        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);
            this.Option.Value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * StardewValley.Game1.pixelZoom ? (int)((x - this.bounds.X) / (this.bounds.Width - 10d * StardewValley.Game1.pixelZoom) * MAX_HEARTS) : MAX_HEARTS) : 0;
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            base.draw(spriteBatch, slotX, slotY);

            if (this.Mugshot != null)
            {
                this.Mugshot.bounds = new Rectangle(slotX + 32, slotY, StardewValley.Game1.tileSize, StardewValley.Game1.tileSize);
                this.Mugshot.draw(spriteBatch);
            }

            for (int i = 0; i < 10; i++)
            {
                Rectangle sourceRectangle = i < this.Option.Value
                    ? new Rectangle(211, 428, 7, 6)
                    : new Rectangle(218, 428, 7, 6);
                spriteBatch.Draw(StardewValley.Game1.mouseCursors, new Vector2(slotX + this.bounds.X + i * (8 * StardewValley.Game1.pixelZoom), slotY + this.bounds.Y), sourceRectangle, Color.White, 0f, Vector2.Zero, StardewValley.Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }
    }
}
