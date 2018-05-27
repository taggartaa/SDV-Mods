namespace CJBCheatsMenu.Framework.Menu
{
    public enum OptionType { SetButton, CheckBox };

    public interface IOption
    {
        string Label { get; }
        bool Disabled { get; }
    }
}
