using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.CheatMenus
{
    class FarmAndFishingMenu : CheatMenu
    {
        public FarmAndFishingMenu(ModConfig config, Cheats cheats, StardewModdingAPI.ITranslationHelper i18n, int rowWidth)
            : base(config, cheats, i18n, rowWidth)
        {
        }

        public override string Id
        {
            get
            {
                return "CBJCheatsMenu_FarmAndFishingMenu";
            }
        }

        public override string Title
        {
            get
            {
               return I18n.Get("tabs.farm-and-fishing");
            }
        }

        public override List<OptionsElement> OptionElements
        {
            get
            {
                Action<bool> waterAllFieldsAction = new Action<bool>(value =>
                {
                    StardewValley.Game1.soundBank.PlayCue("glug");
                    Cheats.WaterAllFields(CJB.GetAllLocations().ToArray());
                });

                Menu.OptionGroup farmGroup = new Menu.OptionGroup($"{I18n.Get("farm.title")}:");
                farmGroup.AddOption(new Menu.OptionSetButton<bool>(I18n.Get("farm.water-all-fields"), RowWidth, true, waterAllFieldsAction));
                farmGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("farm.durable-fences"), Config.DurableFences, value => Config.DurableFences = value));
                farmGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("farm.instant-build"), Config.InstantBuild, value => Config.InstantBuild = value));
                farmGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("farm.always-auto-feed"), Config.AutoFeed, value => Config.AutoFeed = value));
                farmGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("farm.infinite-hay"), Config.InfiniteHay, value => Config.InfiniteHay = value));

                Menu.OptionGroup fishingGroup = new Menu.OptionGroup($"{I18n.Get("fishing.title")}:");
                fishingGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fishing.instant-catch"), Config.InstantCatch, value => Config.InstantCatch = value));
                fishingGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fishing.instant-bite"), Config.InstantBite, value => Config.InstantBite = value));
                fishingGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fishing.always-throw-max-distance"), Config.ThrowBobberMax, value => Config.ThrowBobberMax = value));
                fishingGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fishing.always-treasure"), Config.AlwaysTreasure, value => Config.AlwaysTreasure = value));
                fishingGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fishing.durable-tackles"), Config.DurableTackles, value => Config.DurableTackles = value));

                Menu.OptionGroup fastMachinesGroup = new Menu.OptionGroup($"{I18n.Get("fast-machines.title")}:");
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.cask"), Config.FastCask, value => Config.FastCask = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.furnace"), Config.FastFurnace, value => Config.FastFurnace = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.recycling-machine"), Config.FastRecyclingMachine, value => Config.FastRecyclingMachine = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.crystalarium"), Config.FastCrystalarium, value => Config.FastCrystalarium = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.incubator"), Config.FastIncubator, value => Config.FastIncubator = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.slime-incubator"), Config.FastSlimeIncubator, value => Config.FastSlimeIncubator = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.keg"), Config.FastKeg, value => Config.FastKeg = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.preserves-jar"), Config.FastPreservesJar, value => Config.FastPreservesJar = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.cheese-press"), Config.FastCheesePress, value => Config.FastCheesePress = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.mayonnaise-machine"), Config.FastMayonnaiseMachine, value => Config.FastMayonnaiseMachine = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.loom"), Config.FastLoom, value => Config.FastLoom = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.oil-maker"), Config.FastOilMaker, value => Config.FastOilMaker = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.seed-maker"), Config.FastSeedMaker, value => Config.FastSeedMaker = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.charcoal-kiln"), Config.FastCharcoalKiln, value => Config.FastCharcoalKiln = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.slime-egg-press"), Config.FastSlimeEggPress, value => Config.FastSlimeEggPress = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.tapper"), Config.FastTapper, value => Config.FastTapper = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.lightning-rod"), Config.FastLightningRod, value => Config.FastLightningRod = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.bee-house"), Config.FastBeeHouse, value => Config.FastBeeHouse = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.mushroom-box"), Config.FastMushroomBox, value => Config.FastMushroomBox = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.worm-bin"), Config.FastWormBin, value => Config.FastWormBin = value));
                fastMachinesGroup.AddOption(new Menu.OptionCheckbox(I18n.Get("fast-machines.fruit-trees"), Config.FastFruitTree, value => Config.FastFruitTree = value));

                return farmGroup.OptionElements
                    .Concat(fishingGroup.OptionElements)
                    .Concat(fastMachinesGroup.OptionElements)
                    .ToList();
            }
        }
    }
}
