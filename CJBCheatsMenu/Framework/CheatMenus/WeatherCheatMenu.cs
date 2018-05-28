using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    internal class WeatherCheatMenu : CheatMenu
    {
        public override string Id => "CBJCheatsMenu_WeatherCheatMenu";

        public override string Title => I18n.Get("tabs.weather");

        public WeatherCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n)
            : base(config, cheats, i18n)
        {

        }

        public override List<Menu.IOptionGroup> OptionGroups
        {
            get
            {
                List<Menu.IOptionGroup> optionGroups = new List<Menu.IOptionGroup>();

                Menu.IOptionGroup weatherOptionGroup = new Menu.OptionGroup($"{I18n.Get("weather.title")}:");
                
                
                Action<int> setTomorrowsWeatherAction = new Action<int>(weatherId =>
                {
                    this.Cheats.SetWeatherForNextDay(weatherId);
                });

                weatherOptionGroup.Options.Add(new CurrentWeatherLabel(I18n));
                weatherOptionGroup.Options.Add(new Menu.OptionSetButton<int>(I18n.Get("weather.sunny"), StardewValley.Game1.weather_sunny, setTomorrowsWeatherAction));
                weatherOptionGroup.Options.Add(new Menu.OptionSetButton<int>(I18n.Get("weather.raining"), StardewValley.Game1.weather_rain, setTomorrowsWeatherAction));
                weatherOptionGroup.Options.Add(new Menu.OptionSetButton<int>(I18n.Get("weather.lightning"), StardewValley.Game1.weather_lightning, setTomorrowsWeatherAction));
                weatherOptionGroup.Options.Add(new Menu.OptionSetButton<int>(I18n.Get("weather.snowing"), StardewValley.Game1.weather_snow, setTomorrowsWeatherAction));
                optionGroups.Add(weatherOptionGroup);
                return optionGroups;
            }
        }

        private class CurrentWeatherLabel : Menu.IOption
        {
            public CurrentWeatherLabel(StardewModdingAPI.ITranslationHelper i18n)
            {
                this.I18n = i18n;
            }

            private StardewModdingAPI.ITranslationHelper I18n { get; set; }

            public string Label => $"{this.I18n.Get("weather.title")}: {CJB.GetWeatherNexDay(I18n)}";

            public bool Disabled => false;
        }
    }
}
