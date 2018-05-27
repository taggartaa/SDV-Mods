namespace CJBCheatsMenu.Framework.Menu
{
    interface IOptionSlider : IOptionWithValue<int>
    {
        int MinValue { get; }
        int MaxValue { get; }
        int Step { get;  }
    }
}
