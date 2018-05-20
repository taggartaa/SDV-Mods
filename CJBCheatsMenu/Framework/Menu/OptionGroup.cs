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
            OptionElements.Add(new OptionsElement(title));
        }

        public string Title
        {
            get
            {
                return OptionElements.First().label;
            }
        }

        public void AddOption(OptionsElement element)
        {
            OptionElements.Add(element);
        }

        public List<StardewValley.Menus.OptionsElement> OptionElements { get; private set; } = new List<OptionsElement>();
    }
}
