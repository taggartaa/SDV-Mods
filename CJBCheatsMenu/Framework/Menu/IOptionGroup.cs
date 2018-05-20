using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJBCheatsMenu.Framework.Menu
{
    public interface IOptionGroup
    {
        string Title { get; }
        List<StardewValley.Menus.OptionsElement> OptionElements { get; }
    }
}
