namespace CJBCheatsMenu.Framework.Menu
{
    public interface IOptionWithValue<T> : IOption
    {
        T Value { get; set; }
    }
}
