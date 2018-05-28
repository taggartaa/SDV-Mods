namespace CJBCheatsMenu.Framework.Menu
{
    public interface IOptionHeartPicker : IOptionWithValue<int>
    {
        StardewValley.NPC NPC { get; }
    }
}
