using System.Collections.Generic;
using CJBCheatsMenu.Framework.Menu;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    abstract internal class CheatMenu : Menu.IMenu
    {
        protected ModConfig Config { get; }
        protected Cheats Cheats { get; }
        protected StardewModdingAPI.ITranslationHelper I18n { get; }

        public CheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
        {
            Config = config;
            Cheats = cheats;
            I18n = i18n;
        }

        abstract public string Id { get; }

        abstract public string Title { get; }

        abstract public List<IOptionGroup> OptionGroups { get; }
    }
}
