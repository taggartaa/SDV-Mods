using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewScroller<T>
    {
        public Rectangle Bounds { get; protected set; }
        public ClickableTextureComponent UpArrow { get; protected set; }
        public ClickableTextureComponent DownArrow { get; protected set; }
        public ClickableTextureComponent Scrollbar { get; protected set; }
        public Rectangle ScrollbarRunner { get; protected set; }

        private int currentVisibleStartIndex;
        private bool IsScrolling { get; set; } = false;

        public ViewScroller(List<T> items, int currentIndex, int itemsPerPage, int x, int y, int height)
        {
            const int arrowButtonHeight = 12 * Game1.pixelZoom;
            this.Bounds = new Rectangle(x, y, 11 * Game1.pixelZoom, height);
            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(x, y, this.Bounds.Width, arrowButtonHeight), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(x, y + height - arrowButtonHeight, this.Bounds.Width, arrowButtonHeight), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.Scrollbar = new ClickableTextureComponent("scrollbar", new Rectangle(this.UpArrow.bounds.X + Game1.pixelZoom * 3, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            this.ScrollbarRunner = new Rectangle(this.Scrollbar.bounds.X, this.Scrollbar.bounds.Y, this.Scrollbar.bounds.Width, height - arrowButtonHeight * 2 - Game1.pixelZoom * 2);

            this.Items = items;
            this.ItemsPerPage = itemsPerPage;
            this.CurrentVisibleStartIndex = currentIndex;
        }

        public IReadOnlyCollection<T> VisibleItems
        {
            get
            {
                return this.Items.GetRange(this.CurrentVisibleStartIndex, this.VisibleItemCount).AsReadOnly();
            }
        }

        public int ItemsPerPage { get; protected set; }

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
            if (!Items.Any())
                return;
            this.Scrollbar.bounds.Y = this.ScrollbarRunner.Height / (this.LastVisibleStartIndex + 1) * this.CurrentVisibleStartIndex + this.UpArrow.bounds.Bottom + Game1.pixelZoom;
            if (this.AtBottom())
            {
                this.Scrollbar.bounds.Y = this.DownArrow.bounds.Y - this.Scrollbar.bounds.Height - Game1.pixelZoom;
            }
        }

        public bool LeftClickHeld(int x, int y)
        {
            if (this.IsScrolling)
            {
                int oldIndex = this.CurrentVisibleStartIndex;
                this.ScrollToY(y);
                if (oldIndex != this.CurrentVisibleStartIndex)
                {
                    Game1.soundBank.PlayCue("shiny4");
                }

                return true;
            }
            return false;
        }

        public bool ReleaseLeftClick(int x, int y)
        {
            if (this.IsScrolling)
            {
                this.IsScrolling = false;
                return true;
            }
            return false;
        }

        public bool ReceiveLeftClick(int x, int y, bool playSound = true)
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
            this.UpArrow.draw(spriteBatch);
            this.DownArrow.draw(spriteBatch);
            if (this.Items.Count > this.ItemsPerPage)
            {
                IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.ScrollbarRunner.X, this.ScrollbarRunner.Y, this.ScrollbarRunner.Width, this.ScrollbarRunner.Height, Color.White, Game1.pixelZoom, false);
                this.Scrollbar.draw(spriteBatch);
            }
        }

        private List<T> Items { get; set; }

        private int LastVisibileIndex => Math.Max(0, this.Items.Count - 1);

        private int LastVisibleStartIndex => Math.Max(0, this.Items.Count - this.ItemsPerPage);
    }
}
