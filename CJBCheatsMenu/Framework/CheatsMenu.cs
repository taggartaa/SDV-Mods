using System;
using System.Collections.Generic;
using System.Linq;
using CJBCheatsMenu.Framework.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    internal class CheatsMenu : IClickableMenu
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod settings.</summary>
        private readonly ModConfig Config;

        /// <summary>The cheats helper.</summary>
        private readonly Cheats Cheats;

        /// <summary>Provides translations for the mod.</summary>
        private readonly ITranslationHelper TranslationHelper;

        private readonly List<ClickableComponent> OptionSlots = new List<ClickableComponent>();
        private readonly List<OptionsElement> Options = new List<OptionsElement>();
        private readonly ClickableTextureComponent UpArrow;
        private readonly ClickableTextureComponent DownArrow;
        private readonly ClickableTextureComponent Scrollbar;
        private readonly List<ClickableComponent> Tabs = new List<ClickableComponent>();
        private readonly ClickableComponent Title;
        private const int ItemsPerPage = 10;

        private string HoverText = "";
        private int OptionsSlotHeld = -1;
        private int CurrentItemIndex;
        private bool IsScrolling;
        private Rectangle ScrollbarRunner;
        private bool CanClose;
        private string CurrentTabId { get; set; }

        private Dictionary<String, int> MenuIndecies { get; set; } = new Dictionary<string, int>();
        private readonly List<Menu.IMenu> Menus;


        /*********
        ** Public methods
        *********/
        public Menu.IMenu CurrentMenu
        {
            get
            {
                return Menus[CurrentTabIndex];
            }
        }

        private int CurrentTabIndex
        {
            get
            {
                return MenuIndecies[CurrentTabId];
            }
        }

        private int RowWidth
        {
            get
            {
                return this.width - Game1.tileSize / 2;
            }
        }

        private OptionsElement GetViewForOption(Menu.IOption option)
        {
            Menu.IOptionCheckbox checkboxOption = option as Menu.IOptionCheckbox;
            if (checkboxOption != null)
            {
                return new View.ViewOptionCheckbox(checkboxOption);
            }

            Menu.IOptionSetButton setButtonOption = option as Menu.IOptionSetButton;
            if (setButtonOption != null)
            {
                return new View.ViewOptionSetButton(setButtonOption, RowWidth);
            }

            Menu.IOptionSlider sliderOption = option as Menu.IOptionSlider;
            if (sliderOption != null)
            {
                return new View.ViewOptionSlider(sliderOption);
            }

            Menu.IOptionHeartPicker heartPickerOption = option as Menu.IOptionHeartPicker;
            if (heartPickerOption != null)
            {
                return new View.ViewOptionHeartPicker(heartPickerOption);
            }

            return new View.ViewOption<Menu.IOption>(option);
        }

        public CheatsMenu(string currentTabId, ModConfig config, Cheats cheats, ITranslationHelper i18n)
          : base(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2)
        {
            this.Config = config;
            this.Cheats = cheats;
            this.TranslationHelper = i18n;

            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), i18n.Get("title"));
            

            int labelX = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
            int labelY = (int)(this.yPositionOnScreen + Game1.tileSize * 1.5f);
            int labelHeight = (int)(Game1.tileSize * 0.9F);

            Menus = new List<Menu.IMenu>
            {
                new CheatMenus.PlayersAndToolsCheatMenu(config, cheats, i18n),
                new CheatMenus.FarmAndFishingCheatMenu(config, cheats, i18n),
                new CheatMenus.SkillsCheatMenu(config, cheats, i18n),
                new CheatMenus.WeatherCheatMenu(config, cheats, i18n),
                new CheatMenus.RelationshipsCheatMenu(config, cheats, i18n),
                new CheatMenus.WarpLocationsCheatMenu(config, cheats, i18n),
                new CheatMenus.TimeCheatMenu(config, cheats, i18n)
            };

            for (int i = 0; i < Menus.Count; i++)
            {
                Menu.IMenu menu = Menus.ElementAt(i);
                if (!MenuIndecies.ContainsKey(menu.Id))
                {
                    Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i, Game1.tileSize * 5, Game1.tileSize), menu.Id, menu.Title));
                    MenuIndecies.Add(menu.Id, i);
                }
                else
                {
                    throw new Exception("Error: Two menus with same id: " + menu.Id);
                }
            }

            if (MenuIndecies.ContainsKey(currentTabId))
            {
                CurrentTabId = currentTabId;
            }
            else
            {
                CurrentTabId = Menus.First().Id;
            }

            /*
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i++, Game1.tileSize * 5, Game1.tileSize), MenuTab.Time.ToString(), i18n.Get("tabs.time")));
                this.Tabs.Add(new ClickableComponent(new Rectangle(labelX, labelY + labelHeight * i, Game1.tileSize * 5, Game1.tileSize), MenuTab.Controls.ToString(), i18n.Get("tabs.controls")));
            */

            this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 459, 11, 12), Game1.pixelZoom);
            this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(421, 472, 11, 12), Game1.pixelZoom);
            this.Scrollbar = new ClickableTextureComponent("scrollbar", new Rectangle(this.UpArrow.bounds.X + Game1.pixelZoom * 3, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), "", "", Game1.mouseCursors, new Rectangle(435, 463, 6, 10), Game1.pixelZoom);
            this.ScrollbarRunner = new Rectangle(this.Scrollbar.bounds.X, this.UpArrow.bounds.Y + this.UpArrow.bounds.Height + Game1.pixelZoom, this.Scrollbar.bounds.Width, this.height - Game1.tileSize * 2 - this.UpArrow.bounds.Height - Game1.pixelZoom * 2);
            for (int i = 0; i < CheatsMenu.ItemsPerPage; i++)
                this.OptionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage + Game1.pixelZoom), string.Concat(i)));

            foreach (Menu.IOptionGroup group in CurrentMenu.OptionGroups)
            {
                this.Options.Add(new View.ViewOptionGroupHeader(group.Title));
                foreach (Menu.IOption option in group.Options)
                {
                    OptionsElement optionView = this.GetViewForOption(option);
                    this.Options.Add(optionView);
                }
            }

            /*
            switch (this.CurrentTab)
            {

                case MenuTab.Relationships:
                    {

                    }
                    break;

                case MenuTab.WarpLocations:

                    break;

                case MenuTab.Time:

                    break;

                case MenuTab.Controls:
                    this.Options.Add(new OptionsElement($"{i18n.Get("controls.title")}:"));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("controls.open-menu"), 1000, this.OptionSlots[0].bounds.Width, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("controls.freeze-time"), 1001, this.OptionSlots[0].bounds.Width, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("controls.grow-tree"), 1002, this.OptionSlots[0].bounds.Width, config, cheats, i18n));
                    this.Options.Add(new CheatsOptionsInputListener(i18n.Get("controls.grow-crops"), 1003, this.OptionSlots[0].bounds.Width, config, cheats, i18n));
                    break;
            }
            */
            this.SetScrollBarToCurrentIndex();
        }

        /// <summary>Get whether the player has the given profession.</summary>
        /// <param name="id">The profession ID.</param>
        private bool GetProfession(int id)
        {
            return Game1.player.professions.Contains(id);
        }

        /// <summary>Toggle a player profession.</summary>
        /// <param name="id">The profession ID.</param>
        /// <param name="enable">Whether to enable the profession (else disable).</param>
        private void SetProfession(int id, bool enable)
        {
            if (enable)
            {
                if (!this.GetProfession(id))
                    Game1.player.professions.Add(id);
            }
            else
                Game1.player.professions.Remove(id);
        }

        private void SetScrollBarToCurrentIndex()
        {
            if (!this.Options.Any())
                return;
            this.Scrollbar.bounds.Y = this.ScrollbarRunner.Height / Math.Max(1, this.Options.Count - CheatsMenu.ItemsPerPage + 1) * this.CurrentItemIndex + this.UpArrow.bounds.Bottom + Game1.pixelZoom;
            if (this.CurrentItemIndex != this.Options.Count - CheatsMenu.ItemsPerPage)
                return;
            this.Scrollbar.bounds.Y = this.DownArrow.bounds.Y - this.Scrollbar.bounds.Height - Game1.pixelZoom;
        }

        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.leftClickHeld(x, y);
            if (this.IsScrolling)
            {
                int num = this.Scrollbar.bounds.Y;
                this.Scrollbar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.Scrollbar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.UpArrow.bounds.Height + Game1.pixelZoom * 5));
                this.CurrentItemIndex = Math.Min(this.Options.Count - CheatsMenu.ItemsPerPage, Math.Max(0, (int)(this.Options.Count * (double)((y - this.ScrollbarRunner.Y) / (float)this.ScrollbarRunner.Height))));
                this.SetScrollBarToCurrentIndex();
                if (num == this.Scrollbar.bounds.Y)
                    return;
                Game1.soundBank.PlayCue("shiny4");
            }
            else
            {
                if (this.OptionsSlotHeld == -1 || this.OptionsSlotHeld + this.CurrentItemIndex >= this.Options.Count)
                    return;
                this.Options[this.CurrentItemIndex + this.OptionsSlotHeld].leftClickHeld(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
            }
        }

        public override void receiveKeyPress(Keys key)
        {
            if ((Game1.options.menuButton.Contains(new InputButton(key)) || key.ToString() == this.Config.OpenMenuKey) && this.readyToClose() && this.CanClose)
            {
                Game1.exitActiveMenu();
                Game1.soundBank.PlayCue("bigDeSelect");
                return;
            }

            this.CanClose = true;

            if (this.OptionsSlotHeld == -1 || this.OptionsSlotHeld + this.CurrentItemIndex >= this.Options.Count)
                return;
            this.Options[this.CurrentItemIndex + this.OptionsSlotHeld].receiveKeyPress(key);
        }

        public override void receiveGamePadButton(Buttons key)
        {
            if (key == Buttons.LeftShoulder || key == Buttons.RightShoulder)
            {
                // rotate tab index
                int index = CurrentTabIndex;
                if (key == Buttons.LeftShoulder)
                    index--;
                if (key == Buttons.RightShoulder)
                    index++;

                if (index >= this.Tabs.Count)
                    index = 0;
                if (index < 0)
                    index = this.Tabs.Count - 1;

                // open menu with new index
                Game1.activeClickableMenu = new CheatsMenu(Menus[index].Id, this.Config, this.Cheats, this.TranslationHelper);
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (direction > 0 && this.CurrentItemIndex > 0)
                this.UpArrowPressed();
            else
            {
                if (direction >= 0 || this.CurrentItemIndex >= Math.Max(0, this.Options.Count - CheatsMenu.ItemsPerPage))
                    return;
                this.DownArrowPressed();
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.releaseLeftClick(x, y);
            if (this.OptionsSlotHeld != -1 && this.OptionsSlotHeld + this.CurrentItemIndex < this.Options.Count)
                this.Options[this.CurrentItemIndex + this.OptionsSlotHeld].leftClickReleased(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
            this.OptionsSlotHeld = -1;
            this.IsScrolling = false;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            if (this.DownArrow.containsPoint(x, y) && this.CurrentItemIndex < Math.Max(0, this.Options.Count - CheatsMenu.ItemsPerPage))
            {
                this.DownArrowPressed();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.UpArrow.containsPoint(x, y) && this.CurrentItemIndex > 0)
            {
                this.UpArrowPressed();
                Game1.soundBank.PlayCue("shwip");
            }
            else if (this.Scrollbar.containsPoint(x, y))
                this.IsScrolling = true;
            else if (!this.DownArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
            {
                this.IsScrolling = true;
                this.leftClickHeld(x, y);
                this.releaseLeftClick(x, y);
            }
            this.CurrentItemIndex = Math.Max(0, Math.Min(this.Options.Count - CheatsMenu.ItemsPerPage, this.CurrentItemIndex));
            for (int index = 0; index < this.OptionSlots.Count; ++index)
            {
                if (this.OptionSlots[index].bounds.Contains(x, y) && this.CurrentItemIndex + index < this.Options.Count && this.Options[this.CurrentItemIndex + index].bounds.Contains(x - this.OptionSlots[index].bounds.X, y - this.OptionSlots[index].bounds.Y - 5))
                {
                    this.Options[this.CurrentItemIndex + index].receiveLeftClick(x - this.OptionSlots[index].bounds.X, y - this.OptionSlots[index].bounds.Y + 5);
                    this.OptionsSlotHeld = index;
                    break;
                }
            }

            foreach (var tab in this.Tabs)
            {
                if (tab.bounds.Contains(x, y))
                {
                    Game1.activeClickableMenu = new CheatsMenu(tab.name, this.Config, this.Cheats, this.TranslationHelper);
                    break;
                }
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        public override void performHoverAction(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            this.HoverText = "";
            this.UpArrow.tryHover(x, y);
            this.DownArrow.tryHover(x, y);
            this.Scrollbar.tryHover(x, y);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);

            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.dialogueFont, this.Title.name, 1);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
            for (int index = 0; index < this.OptionSlots.Count; ++index)
            {
                if (this.CurrentItemIndex >= 0 && this.CurrentItemIndex + index < this.Options.Count)
                    this.Options[this.CurrentItemIndex + index].draw(spriteBatch, this.OptionSlots[index].bounds.X, this.OptionSlots[index].bounds.Y + 5);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            if (!GameMenu.forcePreventClose)
            {

                for (int i = 0; i < Tabs.Count; i++)
                {
                    ClickableComponent tab = Tabs[i];
                    CJB.DrawTextBox(tab.bounds.X + tab.bounds.Width, tab.bounds.Y, Game1.smallFont, tab.label, 2, i == CurrentTabIndex ? 1F : 0.7F);
                }

                this.UpArrow.draw(spriteBatch);
                this.DownArrow.draw(spriteBatch);
                if (this.Options.Count > CheatsMenu.ItemsPerPage)
                {
                    IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.ScrollbarRunner.X, this.ScrollbarRunner.Y, this.ScrollbarRunner.Width, this.ScrollbarRunner.Height, Color.White, Game1.pixelZoom, false);
                    this.Scrollbar.draw(spriteBatch);
                }
            }
            if (this.HoverText != "")
                IClickableMenu.drawHoverText(spriteBatch, this.HoverText, Game1.smallFont);

            if (!Game1.options.hardwareCursor)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16), Color.White, 0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }


        /*********
        ** Private methods
        *********/
        private void DownArrowPressed()
        {
            this.DownArrow.scale = this.DownArrow.baseScale;
            ++this.CurrentItemIndex;
            this.SetScrollBarToCurrentIndex();
        }

        private void UpArrowPressed()
        {
            this.UpArrow.scale = this.UpArrow.baseScale;
            --this.CurrentItemIndex;
            this.SetScrollBarToCurrentIndex();
        }

        /// <summary>Get the tab constant represented by a tab component.</summary>
        /// <param name="tab">The component to check.</param>
        private MenuTab GetTabID(ClickableComponent tab)
        {
            if (!Enum.TryParse(tab.name, out MenuTab tabID))
                throw new InvalidOperationException($"Couldn't parse tab name '{tab.name}'.");
            return tabID;
        }
    }
}
