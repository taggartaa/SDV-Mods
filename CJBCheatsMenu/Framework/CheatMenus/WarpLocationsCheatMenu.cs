using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    internal class WarpLocationsCheatMenu : CheatMenu
    {
        public override string Id => "CBJCheatsMenu_WarpLocationsCheatMenu";

        public override string Title => this.I18n.Get("tabs.warp");

        public WarpLocationsCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
            : base(config, cheats, i18n)
        {

        }

        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();

                Action<WarpLocation> warpToAction = new Action<WarpLocation>(warpLocation =>
                {
                    StardewValley.Game1.warpFarmer(warpLocation.Name, warpLocation.X, warpLocation.Y, false);
                    StardewValley.Game1.exitActiveMenu();
                });

                Menu.IOptionGroup warpLocationsOptionGroup = new Menu.OptionGroup($"{this.I18n.Get("warp.title")}:"); 
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.farm"), new WarpLocation("Farm", 64, 15), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.pierre-shop"), new WarpLocation("Town", 43, 57), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.blacksmith"), new WarpLocation("Town", 94, 82), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.museum"), new WarpLocation("Town", 102, 90), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.saloon"), new WarpLocation("Town", 45, 71), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.community-center"), new WarpLocation("Town", 52, 20), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.carpenter"), new WarpLocation("Mountain", 12, 26), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.adventurers-guild"), new WarpLocation("Mountain", 76, 9), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.ranch"), new WarpLocation("Forest", 90, 16), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.mines"), new WarpLocation("Mine", 13, 10), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.willy-shop"), new WarpLocation("Beach", 30, 34), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.wizard-tower"), new WarpLocation("Forest", 5, 27), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.hats"), new WarpLocation("Forest", 34, 96), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.desert"), new WarpLocation("Desert", 18, 28), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.sandy-shop"), new WarpLocation("SandyHouse", 4, 8), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.casino"), new WarpLocation("Club", 8, 11), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.quarry"), new WarpLocation("Mountain", 127, 12), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.new-beach"), new WarpLocation("Beach", 87, 26), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.secret-woods"), new WarpLocation("Woods", 58, 15), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.sewer"), new WarpLocation("Sewer", 3, 48), warpToAction));
                warpLocationsOptionGroup.Options.Add(new Menu.OptionSetButton<WarpLocation>(this.I18n.Get("warp.bathhouse"), new WarpLocation("Railroad", 10, 57), warpToAction));
                optionGroups.Add(warpLocationsOptionGroup);

                return optionGroups;
            }
        }

        private class WarpLocation
        {
            public string Name { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }
            
            public WarpLocation(string name, int x, int y)
            {
                this.Name = name;
                this.X = x;
                this.Y = y;
            }
        }
    }
}
