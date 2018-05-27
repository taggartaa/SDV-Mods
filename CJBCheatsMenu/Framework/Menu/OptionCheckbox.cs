using System;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionCheckbox : OptionWithValue<bool>, IOptionCheckbox
    {
        public OptionCheckbox(string label, bool initialValue, Action<bool> valueChangedCallback)
            : base(label, initialValue, valueChangedCallback)
        {
        }
    }
}
