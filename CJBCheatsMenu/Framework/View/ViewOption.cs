using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewOption<T> : StardewValley.Menus.OptionsElement where T : Menu.IOption
    {
        public T Option { get; private set; }
        public virtual string DrawnLabel => this.Option.Label;

        public ViewOption(T option, int width, int height, int whichOption = -1)
            : base(option.Label, -1, -1, width, height, whichOption)
        {
            this.Option = option;
            this.greyedOut = this.Option.Disabled;
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            this.label = this.DrawnLabel;
            this.greyedOut = this.Option.Disabled;
            base.draw(spriteBatch, slotX, slotY);
        }
    }
}
