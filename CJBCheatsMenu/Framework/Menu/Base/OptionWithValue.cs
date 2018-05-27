using System;
using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionWithValue<T> : Option, IOptionWithValue<T> where T : IComparable
    {
        protected Action<T> ValueChangedCallback { get; set; }
        protected T value;

        public OptionWithValue(string label, T initialValue, Action<T> valueChangedCallback)
            : base(label)
        {
            this.value = initialValue;
            this.ValueChangedCallback = valueChangedCallback;
        }

        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(this.value, value))
                {
                    this.value = value;
                    ValueChangedCallback(value);
                }
            }
        }
    }
}
