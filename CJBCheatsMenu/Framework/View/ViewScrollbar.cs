using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewScrollbar : StardewValley.Menus.OptionsElement, IScrollable
    {
        public ClickableTextureComponent UpArrow { get; protected set; }
        public ClickableTextureComponent DownArrow { get; protected set; }
        public ClickableTextureComponent Scrollbar { get; protected set; }
        public Rectangle ScrollbarRunner { get; protected set; }
        public int ItemCount { get; set; }
        public int ItemsPerPage { get; protected set; }

        private int currentVisibleStartIndex;
        private bool IsScrolling { get; set; } = false;

        public ViewScrollbar(int currentIndex, int itemsPerPage, int x, int y, int height, int itemCount = 0)
            : base("", new Rectangle(x, y, 11 * Game1.pixelZoom, height), -1)
        {
            const int arrowButtonHeight = 12 * Game1.pixelZoom;
            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(x, y, this.bounds.Width, arrowButtonHeight), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(x, y + height - arrowButtonHeight, this.bounds.Width, arrowButtonHeight), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.Scrollbar = new ClickableTextureComponent("scrollbar", new Rectangle(this.UpArrow.bounds.X + Game1.pixelZoom * 3, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            this.ScrollbarRunner = new Rectangle(this.Scrollbar.bounds.X, this.Scrollbar.bounds.Y, this.Scrollbar.bounds.Width, height - arrowButtonHeight * 2 - Game1.pixelZoom * 2);
            this.ItemsPerPage = itemsPerPage;
            this.ItemCount = itemCount;
            this.CurrentVisibleStartIndex = currentIndex;
        }

        public int CurrentVisibleStartIndex
        {
            get
            {
                return currentVisibleStartIndex;
            }
            protected set
            {
                currentVisibleStartIndex = Math.Max(0, Math.Min(value, this.LastVisibleStartIndex));
                updateScrollbarPosition();
            }
        }

        public int VisibleItemCount
        {
            get
            {
                if (this.currentVisibleStartIndex == this.CurrentVisibleEndIndex)
                {
                    return 0;
                }
                return this.CurrentVisibleEndIndex - this.currentVisibleStartIndex + 1;
            }
        }

        public int CurrentVisibleEndIndex => Math.Min(this.LastVisibileIndex, this.CurrentVisibleStartIndex + this.ItemsPerPage - 1);

        public bool AtBottom()
        {
            return CurrentVisibleEndIndex == LastVisibileIndex;
        }

        public void ScrollUp()
        {
            this.CurrentVisibleStartIndex--;
        }

        public void ScrollDown()
        {
            this.CurrentVisibleStartIndex++;
        }

        public void ScrollTo(int index)
        {
            this.CurrentVisibleStartIndex = index;
        }

        public void ScrollToY(int y)
        {
            this.ScrollTo((int)((y - this.ScrollbarRunner.Y) / (double)this.ScrollbarRunner.Height * (this.LastVisibleStartIndex + 1)));
        }

        private void updateScrollbarPosition()
        {
            if (this.ItemCount <= this.ItemsPerPage)
                return;

            this.Scrollbar.bounds.Y = this.ScrollbarRunner.Height / (this.LastVisibleStartIndex + 1) * this.CurrentVisibleStartIndex + this.UpArrow.bounds.Bottom + Game1.pixelZoom;
            if (this.AtBottom())
            {
                this.Scrollbar.bounds.Y = this.DownArrow.bounds.Y - this.Scrollbar.bounds.Height - Game1.pixelZoom;
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            if (this.IsScrolling)
            {
                int oldIndex = this.CurrentVisibleStartIndex;
                this.ScrollToY(y);
                if (oldIndex != this.CurrentVisibleStartIndex)
                {
                    Game1.soundBank.PlayCue("shiny4");
                }
            }
        }

        public override void leftClickReleased(int x, int y)
        {
            this.IsScrolling = false;
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.DownArrow.containsPoint(x, y))
            {
                this.ScrollDown();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.UpArrow.containsPoint(x, y))
            {
                this.ScrollUp();
                Game1.soundBank.PlayCue("shwip");
            }
            else
            {
                this.IsScrolling = true;
            };
        }

        public bool ReceiveScrollWheelAction(int direction)
        {
            if (direction > 0)
            {
                this.ScrollUp();
            }
            else if (direction < 0)
            {
                this.ScrollDown();
            }
            else
            {
                return false;
            }

            return true;
        }

        public void PerformHoverAction(int x, int y)
        {
            this.UpArrow.tryHover(x, y);
            this.DownArrow.tryHover(x, y);
            this.Scrollbar.tryHover(x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.ItemCount <= this.ItemsPerPage)
            {
                return;
            }

            this.UpArrow.draw(spriteBatch);
            this.DownArrow.draw(spriteBatch);
            IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.ScrollbarRunner.X, this.ScrollbarRunner.Y, this.ScrollbarRunner.Width, this.ScrollbarRunner.Height, Color.White, Game1.pixelZoom, false);
            this.Scrollbar.draw(spriteBatch);
        }

        private int LastVisibileIndex => Math.Max(0, this.ItemCount - 1);

        private int LastVisibleStartIndex => Math.Max(0, this.ItemCount - this.ItemsPerPage);
    }
}
