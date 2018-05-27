using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionGroup : IOptionGroup
    {
        public OptionGroup(string title)
        {
            this.Title = title;
        }

        public string Title { get; private set; }

        public List<IOption> Options { get; private set; } = new List<IOption>();
    }
}
