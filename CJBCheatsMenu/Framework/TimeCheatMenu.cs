using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    class TimeCheatMenu : CheatMenu
    {
        public override string Id => "CBJCheatsMenu_TimeCheatMenu";

        public override string Title => this.I18n.Get("tabs.time");

        public TimeCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
            : base(config, cheats, i18n)
        {

        }

        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();

                Menu.IOptionGroup timeOptionGroup = new Menu.OptionGroup($"{this.I18n.Get("time.title")}:");
                timeOptionGroup.Options.Add(new Menu.OptionCheckbox(this.I18n.Get("time.freeze-inside"), this.Config.FreezeTimeInside, value => this.Config.FreezeTimeInside = value));
                timeOptionGroup.Options.Add(new Menu.OptionCheckbox(this.I18n.Get("time.freeze-caves"), this.Config.FreezeTimeCaves, value => this.Config.FreezeTimeCaves = value));
                timeOptionGroup.Options.Add(new Menu.OptionCheckbox(this.I18n.Get("time.freeze-everywhere"), this.Config.FreezeTime, value => this.Config.FreezeTime = value));
                timeOptionGroup.Options.Add(new Menu.OptionSliderTimePicker(this.I18n.Get("time.time"), StardewValley.Game1.timeOfDay, newTime => this.SafelySetTime(newTime), 600, 2500, 60));
                optionGroups.Add(timeOptionGroup);

                return optionGroups;
            }
        }

        /// <summary>Safely transition to the given time, allowing NPCs to update their schedule.</summary>
        /// <param name="time">The time of day.</param>
        private void SafelySetTime(int time)
        {
            // define conversion between game time and TimeSpan
            TimeSpan ToTimeSpan(int value) => new TimeSpan(0, value / 100, value % 100, 0);
            int FromTimeSpan(TimeSpan span) => (span.Days * 2400) + (span.Hours * 100) + span.Minutes;

            // transition to new time
            int intervals = (int)((ToTimeSpan(time) - ToTimeSpan(StardewValley.Game1.timeOfDay)).TotalMinutes / 10);
            if (intervals > 0)
            {
                for (int i = 0; i < intervals; i++)
                    StardewValley.Game1.performTenMinuteClockUpdate();
            }
            else if (intervals < 0)
            {
                for (int i = 0; i > intervals; i--)
                {
                    StardewValley.Game1.timeOfDay = FromTimeSpan(ToTimeSpan(StardewValley.Game1.timeOfDay).Subtract(TimeSpan.FromMinutes(20))); // offset 20 mins so game updates to next interval
                    StardewValley.Game1.performTenMinuteClockUpdate();
                }
            }
        }
    }
}
