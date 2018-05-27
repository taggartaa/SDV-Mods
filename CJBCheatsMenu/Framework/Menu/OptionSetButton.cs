using System;

namespace CJBCheatsMenu.Framework.Menu
{

    public class OptionSetButton<T> : Option, IOptionSetButton
    {
        private T Value { get; set; }
        private Action<T> OnPressAction { get; set; }
        public OptionSetButton(string label, T value, Action<T> onPressAction) :
            base(label)
        {
            this.Value = value;
            this.OnPressAction = onPressAction;
        }

        public void OnPressed()
        {
            this.OnPressAction(Value);
        }
    }
}
