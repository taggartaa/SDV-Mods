using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Menu;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    internal class PlayersAndToolsCheatMenu : CheatMenu
    {
        public PlayersAndToolsCheatMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper I18n, int rowWidth)
            : base(config, cheats, I18n, rowWidth)
        {
        }

        public override string Id
        {
            get
            {
                return "CBJCheatsMenu_PlayersAndToolsCheatMenu";
            }
        }

        public override string Title
        {
            get
            {
                return I18n.Get("tabs.player-and-tools");
            }
        }

        public override List<OptionsElement> OptionElements
        {
            get
            {
                OptionGroup playerOptionGroup = new OptionGroup($"{I18n.Get("player.title")}:");
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(I18n.Get("player.infinite-stamina"), Config.InfiniteStamina, value => Config.InfiniteStamina = value));
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(I18n.Get("player.infinite-health"), Config.InfiniteHealth, value => Config.InfiniteHealth = value));
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(I18n.Get("player.increased-movement-speed"), Config.IncreasedMovement, value => Config.IncreasedMovement = value));
                playerOptionGroup.AddOption(new CheatsOptionsSlider(I18n.Get("player.movement-speed"), Config.MoveSpeed, 10, value => Config.MoveSpeed = value, disabled: () => !Config.IncreasedMovement));
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(I18n.Get("player.one-hit-kill"), Config.OneHitKill, value => Config.OneHitKill = value));
                playerOptionGroup.AddOption(new CheatsOptionsCheckbox(I18n.Get("player.max-daily-luck"), Config.MaxDailyLuck, value => Config.MaxDailyLuck = value));

                OptionGroup toolsOptionGroup = new OptionGroup($"{I18n.Get("tools.title")}:");
                toolsOptionGroup.AddOption(new CheatsOptionsCheckbox(I18n.Get("tools.infinite-water"), Config.InfiniteWateringCan, value => Config.InfiniteWateringCan = value));
                toolsOptionGroup.AddOption(new CheatsOptionsCheckbox(I18n.Get("tools.one-hit-break"), Config.OneHitBreak, value => Config.OneHitBreak = value));
                toolsOptionGroup.AddOption(new CheatsOptionsCheckbox(I18n.Get("tools.harvest-with-sickle"), Config.HarvestSickle, value => Config.HarvestSickle = value));

                
                Action<int> addMoneyAction = new Action<int>(value =>
                {
                    StardewValley.Game1.player.money += value;
                    StardewValley.Game1.soundBank.PlayCue("coin");
                });

                OptionGroup moneyOptionGroup = new OptionGroup($"{I18n.Get("money.title")}:");
                moneyOptionGroup.AddOption(new OptionSetButton<int>(I18n.Get("money.add-amount", new { amount = 100 }), RowWidth, 100, addMoneyAction));
                moneyOptionGroup.AddOption(new OptionSetButton<int>(I18n.Get("money.add-amount", new { amount = 1000 }), RowWidth, 1000, addMoneyAction));
                moneyOptionGroup.AddOption(new OptionSetButton<int>(I18n.Get("money.add-amount", new { amount = 10000 }), RowWidth, 10000, addMoneyAction));
                moneyOptionGroup.AddOption(new OptionSetButton<int>(I18n.Get("money.add-amount", new { amount = 100000 }), RowWidth, 100000, addMoneyAction));

                Action<int> addCasinoCoinsAction = new Action<int>(value =>
                {
                    StardewValley.Game1.player.clubCoins += value;
                    StardewValley.Game1.soundBank.PlayCue("coin");
                });

                OptionGroup casinoCoinsGroup = new OptionGroup($"{I18n.Get("casino-coins.title")}:");
                casinoCoinsGroup.AddOption(new OptionSetButton<int>(I18n.Get("casino-coins.add-amount", new { amount = 100 }), RowWidth, 100, addCasinoCoinsAction));
                casinoCoinsGroup.AddOption(new OptionSetButton<int>(I18n.Get("casino-coins.add-amount", new { amount = 1000 }), RowWidth, 1000, addCasinoCoinsAction));
                casinoCoinsGroup.AddOption(new OptionSetButton<int>(I18n.Get("casino-coins.add-amount", new { amount = 10000 }), RowWidth, 10000, addCasinoCoinsAction));

                return playerOptionGroup.OptionElements
                    .Concat(toolsOptionGroup.OptionElements)
                    .Concat(moneyOptionGroup.OptionElements)
                    .Concat(casinoCoinsGroup.OptionElements)
                    .ToList();
            }
        }
    }
}
