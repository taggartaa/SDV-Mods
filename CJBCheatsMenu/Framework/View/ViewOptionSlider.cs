using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.View
{
    internal class ViewOptionSlider : ViewOption<Menu.IOptionSlider>
    {
        private int sliderValue;
        public ViewOptionSlider(Menu.IOptionSlider sliderOption, int width = 48)
            : base(sliderOption, width * Game1.pixelZoom, 6 * Game1.pixelZoom, 0)
        {
            if (sliderOption.MinValue >= sliderOption.MaxValue)
            {
                throw new ArgumentException(String.Format("ISliderOption minValue cannot be greater than the maxValue. Got min({0}) max({1})", sliderOption.MinValue, sliderOption.MaxValue));
            }

            if (sliderOption.Step <= 0)
            {
                throw new ArgumentException(String.Format("ISliderOption step must be positive. Got {0})", sliderOption.Step));
            }

            this.SliderValue = sliderOption.Value;
        }

        private int SliderValue
        {
            get
            {
                return sliderValue;
            }
            set
            {
                /* Set value to the nearest valid step.
                int offestFromNextStep = (value - this.Option.MinValue) % this.Option.Step;
                if (offestFromNextStep != 0)
                {
                    if (offestFromNextStep >= this.Option.Step / 2)
                    {
                        value += offestFromNextStep;
                    }
                    else
                    {
                        value -= (this.Option.Step - offestFromNextStep);
                    }
                }
                */
                if (value < this.Option.MinValue)
                {
                    this.sliderValue = this.Option.MinValue;
                }
                else if (value > this.Option.MaxValue)
                {
                    this.sliderValue = this.Option.MaxValue;
                }
                else
                {
                    this.sliderValue = value;
                }
            }
        }

        private int SliderSegments
        {
            get
            {
                return ((this.Option.MaxValue - this.Option.MinValue) / this.Option.Step) + 1;
            }
        }

        private int CurrentSliderSegment
        {
            get
            {
                return (this.SliderValue - this.Option.MinValue) / (this.Option.Step);
            }
            set
            {
                this.SliderValue = this.Option.MinValue + value * this.Option.Step;
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);

            double xPositionInSlider = ((double)x - this.bounds.X) / this.bounds.Width;
            this.CurrentSliderSegment = (int) (xPositionInSlider * this.SliderSegments);
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        public override void leftClickReleased(int x, int y)
        {
            this.Option.Value = this.SliderValue;
        }

        public override string DrawnLabel => $"{this.Option.Label}: {this.SliderValue}";

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            base.draw(spriteBatch, slotX, slotY);
            IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, Game1.pixelZoom, false);
            spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X + (this.bounds.Width - 10 * Game1.pixelZoom) * this.CurrentSliderSegment / ((float)this.SliderSegments - 1), slotY + this.bounds.Y), OptionsSlider.sliderButtonRect, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.9f);
        }
    }
}
