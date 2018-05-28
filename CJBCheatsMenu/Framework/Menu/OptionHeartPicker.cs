using System;

namespace CJBCheatsMenu.Framework.Menu
{
    public class OptionHeartPicker : OptionWithValue<int>, IOptionHeartPicker
    {
        public StardewValley.NPC NPC { get; private set; }
        public OptionHeartPicker(StardewValley.NPC npc, int initialHeartValue, Action<int> valueChangedCallback)
            : base("", initialHeartValue, valueChangedCallback)
        {
            this.NPC = npc;
        }
    }
}
