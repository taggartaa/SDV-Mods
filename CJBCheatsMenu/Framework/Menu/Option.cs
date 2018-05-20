using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJBCheatsMenu.Framework.Menu
{
    public abstract class Option<T> : StardewValley.Menus.OptionsElement, IOption
    {
        protected Action<T> SetValue { get; set; }

        public Option(string label, int width, int height, Action<T> setValue)
            : base(label, -1, -1, width, height)
        {
            SetValue = setValue;
        }
    }
}
