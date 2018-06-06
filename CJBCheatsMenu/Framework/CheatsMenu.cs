using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly MenuManager MenuManager;
        private View.ViewScroller<OptionsElement> ScrollableOptions;
        private readonly List<ClickableComponent> OptionSlots = new List<ClickableComponent>();
        private readonly List<ClickableComponent> Tabs = new List<ClickableComponent>();
        private readonly ClickableComponent Title;
        private const int ItemsPerPage = 10;

        private string HoverText = "";
        private int OptionsSlotHeld = -1;
        private bool CanClose;
        private string CurrentTabId { get; set; }

        private Dictionary<String, int> MenuIndecies { get; set; } = new Dictionary<string, int>();

        private IReadOnlyCollection<Menu.IMenu> Menus => MenuManager.Menus;

        /// <summary>
        /// The currently selected menu.
        /// </summary>
        public Menu.IMenu CurrentMenu
        {
            get
            {
                return Menus.ElementAt(CurrentTabIndex);
            }
        }

        /// <summary>
        /// The tab index of the currently selected menu.
        /// </summary>
        private int CurrentTabIndex
        {
            get
            {
                return MenuIndecies[CurrentTabId];
            }
        }

        /// <summary>
        /// The width of a row where options are rendered.
        /// </summary>
        private int RowWidth
        {
            get
            {
                return this.width - Game1.tileSize / 2;
            }
        }

        /*********
        ** Public methods
        *********/
        /// <summary>
        /// Reterns a view for the passed in option.
        /// </summary>
        /// <remarks>
        /// I wish I didn't have to upcast here, see stack overflow question for reasoning: https://stackoverflow.com/q/50472035/3154314
        /// </remarks>
        /// <param name="option">The option to get the view for.</param>
        /// <returns>A view to render for this option.</returns>
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
                return new View.ViewOptionSetButton(setButtonOption, this.RowWidth);
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

            Menu.IOptionKeyPicker keyPickerOption = option as Menu.IOptionKeyPicker;
            if (keyPickerOption != null)
            {
                return new View.ViewOptionKeyPicker(keyPickerOption, this.RowWidth, this.TranslationHelper);
            }

            return new View.ViewOption<Menu.IOption>(option);
        }

        public CheatsMenu(string currentTabId, MenuManager menuManager, ModConfig config, Cheats cheats, ITranslationHelper i18n)
          : base(Game1.viewport.Width / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2)
        {
            this.Config = config;
            this.Cheats = cheats;
            this.TranslationHelper = i18n;
            this.MenuManager = menuManager;

            this.Title = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen, Game1.tileSize * 4, Game1.tileSize), i18n.Get("title"));

            int labelX = (int)(this.xPositionOnScreen - Game1.tileSize * 4.8f);
            int labelY = (int)(this.yPositionOnScreen + Game1.tileSize * 1.5f);
            int labelHeight = (int)(Game1.tileSize * 0.9F);

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

            for (int i = 0; i < CheatsMenu.ItemsPerPage; i++)
            {
                this.OptionSlots.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + i * ((this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 2) / CheatsMenu.ItemsPerPage + Game1.pixelZoom), string.Concat(i)));
            }

            List<OptionsElement> options = new List<OptionsElement>();
            foreach (Menu.IOptionGroup group in CurrentMenu.OptionGroups)
            {
                options.Add(new View.ViewOptionGroupHeader(group.Title));
                foreach (Menu.IOption option in group.Options)
                {
                    OptionsElement optionView = this.GetViewForOption(option);
                    options.Add(optionView);
                }
            }
            this.ScrollableOptions = new View.ViewScroller<OptionsElement>(options, 0, CheatsMenu.ItemsPerPage, this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, this.height - Game1.tileSize * 2);
        }

        public OptionsElement GetOptionElementForSlotHeld()
        {
            if (this.OptionsSlotHeld == -1)
            {
                return null;
            }

            IReadOnlyCollection<OptionsElement> visibleOptions = this.ScrollableOptions.VisibleItems;
            if (this.OptionsSlotHeld >= visibleOptions.Count)
            {
                return null;
            }

            return visibleOptions.ElementAt(this.OptionsSlotHeld);
        }

        public override void leftClickHeld(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.leftClickHeld(x, y);
            bool handled = this.ScrollableOptions.LeftClickHeld(x, y);
            if (handled)
            {
                return;
            }

            OptionsElement optionHeld = this.GetOptionElementForSlotHeld();
            if (optionHeld == null)
                return;
            optionHeld.leftClickHeld(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
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
            OptionsElement optionHeld = this.GetOptionElementForSlotHeld();
            if (optionHeld == null)
                return;
            optionHeld.receiveKeyPress(key);
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
                Game1.activeClickableMenu = new CheatsMenu(Menus.ElementAt(index).Id, this.MenuManager, this.Config, this.Cheats, this.TranslationHelper);
            }
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.receiveScrollWheelAction(direction);
            if (direction > 0)
                this.ScrollableOptions.ScrollUp();
            else if (direction < 0)
            {
                this.ScrollableOptions.ScrollDown();
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            base.releaseLeftClick(x, y);
            OptionsElement optionHeld = this.GetOptionElementForSlotHeld();
            if (optionHeld != null)
                optionHeld.leftClickReleased(x - this.OptionSlots[this.OptionsSlotHeld].bounds.X, y - this.OptionSlots[this.OptionsSlotHeld].bounds.Y);
            this.OptionsSlotHeld = -1;
            this.ScrollableOptions.ReleaseLeftClick(x, y);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (GameMenu.forcePreventClose)
                return;
            if (this.ScrollableOptions.Bounds.Contains(x, y))
            {
                bool handled = this.ScrollableOptions.ReceiveLeftClick(x, y);
                if (handled)
                {
                    return;
                }
            }

            IReadOnlyCollection<OptionsElement> visibleOptions = this.ScrollableOptions.VisibleItems;
            for (int index = 0; index < visibleOptions.Count; ++index)
            {
                OptionsElement option = visibleOptions.ElementAt(index);
                if (this.OptionSlots[index].bounds.Contains(x, y) && option.bounds.Contains(x - this.OptionSlots[index].bounds.X, y - this.OptionSlots[index].bounds.Y - 5))
                {
                    option.receiveLeftClick(x - this.OptionSlots[index].bounds.X, y - this.OptionSlots[index].bounds.Y + 5);
                    this.OptionsSlotHeld = index;
                    return;
                }
            }

            foreach (var tab in this.Tabs)
            {
                if (tab.bounds.Contains(x, y))
                {
                    Game1.activeClickableMenu = new CheatsMenu(tab.name,this.MenuManager, this.Config, this.Cheats, this.TranslationHelper);
                    return;
                }
            }
        }

        public override void receiveRightClick(int x, int y, bool playSound = true) { }

        public override void performHoverAction(int x, int y)
        {
            if (GameMenu.forcePreventClose)
                return;
            this.HoverText = "";
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!Game1.options.showMenuBackground)
                spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);

            Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true);
            CJB.DrawTextBox(this.Title.bounds.X, this.Title.bounds.Y, Game1.dialogueFont, this.Title.name, 1);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);

            IReadOnlyCollection<OptionsElement> visibleOptions = this.ScrollableOptions.VisibleItems;
            for (int index = 0; index < visibleOptions.Count; ++index)
            {
                visibleOptions.ElementAt(index).draw(spriteBatch, this.OptionSlots[index].bounds.X, this.OptionSlots[index].bounds.Y + 5);
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

                this.ScrollableOptions.Draw(spriteBatch);
            }
            if (this.HoverText != "")
                IClickableMenu.drawHoverText(spriteBatch, this.HoverText, Game1.smallFont);

            if (!Game1.options.hardwareCursor)
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()), Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16), Color.White, 0f, Vector2.Zero, Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }
    }
}
