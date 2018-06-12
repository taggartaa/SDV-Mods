using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewScrollableContents<T> : StardewValley.Menus.OptionsElement where T : StardewValley.Menus.OptionsElement
    {
        private const int INVALID_INDEX = -1;

        private IReadOnlyList<T> Rows { get; set; }

        public ViewScrollbar Scrollbar { get; private set; }

        public ViewScrollableContents(ViewScrollbar scrollbar, IReadOnlyList<T> rows, Rectangle bounds, string label)
            : base(label, bounds, -1)
        {
            this.Rows = rows;
            this.Scrollbar = scrollbar;
            this.Scrollbar.ItemCount = this.Rows.Count;
        }

        private int RowHeight => this.bounds.Height / this.Scrollbar.ItemsPerPage;

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            for (int i = this.Scrollbar.CurrentVisibleStartIndex; i <= this.Scrollbar.CurrentVisibleEndIndex; i++)
            {
                Rows[i].draw(spriteBatch, slotX, slotY + i * this.RowHeight);
            }
        }

        private int GetItemIndexForY(int y)
        {
            int itemIndex = y / RowHeight + this.Scrollbar.CurrentVisibleStartIndex;
            if (itemIndex < this.Rows.Count && itemIndex >= 0)
            {
                return itemIndex;
            }
            else
            {
                return INVALID_INDEX;
            }
        }

        public override void receiveLeftClick(int x, int y)
        {
            int itemIndex = this.GetItemIndexForY(y);
            if (itemIndex != INVALID_INDEX)
            {
                this.Rows[itemIndex].receiveLeftClick(x, y - itemIndex * RowHeight);
            }
        }

        public override void leftClickReleased(int x, int y)
        {
            int itemIndex = this.GetItemIndexForY(y);
            if (itemIndex != INVALID_INDEX)
            {
                this.Rows[itemIndex].leftClickReleased(x, y - itemIndex * RowHeight);
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            int itemIndex = this.GetItemIndexForY(y);
            if (itemIndex != INVALID_INDEX)
            {
                this.Rows[itemIndex].leftClickHeld(x, y - itemIndex * RowHeight);
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            for (int i = this.Scrollbar.CurrentVisibleStartIndex; i <= this.Scrollbar.CurrentVisibleStartIndex; i++)
            {
                this.Rows[i].receiveKeyPress(key);
            }
        }
    }
}
