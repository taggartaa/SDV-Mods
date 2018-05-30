using System;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionSlider : OptionWithValue<int>, IOptionSlider
    {
        public int MinValue { get; protected set; }

        public int MaxValue { get; protected set; }

        public int Step { get; protected set; }

        public virtual string ConvertValueToString(int value)
        {
            return value.ToString();
        }

        public OptionSlider(string label, int initialValue, Action<int> valueChangedCallback, int minValue = 0, int maxValue = 10, int step = 1)
            : base(label, initialValue, valueChangedCallback)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Step = step;
        }
    }
}
