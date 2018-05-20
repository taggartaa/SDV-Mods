using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Menu;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    internal class PlayersAndToolsCheatGroup : IMenu
    {
        private ModConfig config;
        private Cheats cheats;
        private StardewModdingAPI.ITranslationHelper i18n;
        private int slotWidth;
        

        public PlayersAndToolsCheatGroup(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n, int slotWidth)
        {
            this.config = config;
            this.cheats = cheats;
            this.i18n = i18n;
            this.slotWidth = slotWidth;
        }

        public string Id
        {
            get
            {
                return "CBJCheatsMenu_PlayersAndToolsCheatMenu";
            }
        }

        public string Title
        {
            get
            {
                return i18n.Get("tabs.player-and-tools");
            }
        }

        public List<OptionsElement> OptionElements
        {
            get
            {
                OptionGroup playerOptionGroup = new OptionGroup($"{i18n.Get("player.title")}:");
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(i18n.Get("player.infinite-stamina"), config.InfiniteStamina, value => config.InfiniteStamina = value));
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(i18n.Get("player.infinite-health"), config.InfiniteHealth, value => config.InfiniteHealth = value));
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(i18n.Get("player.increased-movement-speed"), config.IncreasedMovement, value => config.IncreasedMovement = value));
                playerOptionGroup.AddOption(new CheatsOptionsSlider(i18n.Get("player.movement-speed"), config.MoveSpeed, 10, value => config.MoveSpeed = value, disabled: () => !config.IncreasedMovement));
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(i18n.Get("player.one-hit-kill"), config.OneHitKill, value => config.OneHitKill = value));
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(i18n.Get("player.max-daily-luck"), config.MaxDailyLuck, value => config.MaxDailyLuck = value));

                OptionGroup toolsOptionGroup = new OptionGroup($"{i18n.Get("tools.title")}:");
                toolsOptionGroup.AddOption(new CheatsOptionsCheckbox(i18n.Get("tools.infinite-water"), config.InfiniteWateringCan, value => config.InfiniteWateringCan = value));
                toolsOptionGroup.AddOption(new CheatsOptionsCheckbox(i18n.Get("tools.one-hit-break"), config.OneHitBreak, value => config.OneHitBreak = value));
                toolsOptionGroup.AddOption(new CheatsOptionsCheckbox(i18n.Get("tools.harvest-with-sickle"), config.HarvestSickle, value => config.HarvestSickle = value));

                
                Action<int> addMoneyAction = new Action<int>(value =>
                {
                    StardewValley.Game1.player.money += value;
                    StardewValley.Game1.soundBank.PlayCue("coin");
                });

                OptionGroup moneyOptionGroup = new OptionGroup($"{i18n.Get("money.title")}:");
                moneyOptionGroup.AddOption(new OptionSetButton<int>(i18n.Get("money.add-amount", new { amount = 100 }), slotWidth, 100, addMoneyAction));
                moneyOptionGroup.AddOption(new OptionSetButton<int>(i18n.Get("money.add-amount", new { amount = 1000 }), slotWidth, 1000, addMoneyAction));
                moneyOptionGroup.AddOption(new OptionSetButton<int>(i18n.Get("money.add-amount", new { amount = 10000 }), slotWidth, 10000, addMoneyAction));
                moneyOptionGroup.AddOption(new OptionSetButton<int>(i18n.Get("money.add-amount", new { amount = 100000 }), slotWidth, 100000, addMoneyAction));

                Action<int> addCasinoCoinsAction = new Action<int>(value =>
                {
                    StardewValley.Game1.player.clubCoins += value;
                    StardewValley.Game1.soundBank.PlayCue("coin");
                });

                OptionGroup casinoCoinsGroup = new OptionGroup($"{i18n.Get("casino-coins.title")}:");
                casinoCoinsGroup.AddOption(new OptionSetButton<int>(i18n.Get("casino-coins.add-amount", new { amount = 100 }), slotWidth, 100, addCasinoCoinsAction));
                casinoCoinsGroup.AddOption(new OptionSetButton<int>(i18n.Get("casino-coins.add-amount", new { amount = 1000 }), slotWidth, 1000, addCasinoCoinsAction));
                casinoCoinsGroup.AddOption(new OptionSetButton<int>(i18n.Get("casino-coins.add-amount", new { amount = 10000 }), slotWidth, 10000, addCasinoCoinsAction));

                return playerOptionGroup.OptionElements
                    .Concat(toolsOptionGroup.OptionElements)
                    .Concat(moneyOptionGroup.OptionElements)
                    .Concat(casinoCoinsGroup.OptionElements)
                    .ToList();
            }
        }
    }
}
