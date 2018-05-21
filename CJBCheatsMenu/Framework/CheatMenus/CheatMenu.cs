using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    abstract internal class CheatMenu : Menu.IMenu
    {
        protected ModConfig Config { get; }
        protected Cheats Cheats { get; }
        protected StardewModdingAPI.ITranslationHelper I18n { get; }
        protected int RowWidth { get; }

        public CheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n, int rowWidth)
        {
            Config = config;
            Cheats = cheats;
            I18n = i18n;
            RowWidth = rowWidth;
        }

        abstract public string Id { get; }

        abstract public string Title { get; }

        abstract public List<OptionsElement> OptionElements { get; }
    }
}
