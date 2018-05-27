using System;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionSlider : OptionWithValue<int>, IOptionSlider
    {
        public int MinValue { get; private set; }

        public int MaxValue { get; private set; }

        public int Step { get; private set; }

        public OptionSlider(string label, int initialValue, Action<int> valueChangedCallback, int minValue, int maxValue, int step = 1)
            : base(label, initialValue, valueChangedCallback)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Step = step;
        }
    }
}
