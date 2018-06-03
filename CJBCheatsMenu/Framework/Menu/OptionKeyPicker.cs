using System;
using Microsoft.Xna.Framework.Input;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionKeyPicker : OptionWithValue<Keys>, IOptionKeyPicker
    {
        public OptionKeyPicker(string label, Keys initialValue, Action<Keys> onKeyChangedCallback) 
            : base(label, initialValue, onKeyChangedCallback)
        {

        }
    }
}
