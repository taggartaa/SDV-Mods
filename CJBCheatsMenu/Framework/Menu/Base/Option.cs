namespace CJBCheatsMenu.Framework.Menu
{
    public class Option : IOption
    {
        public string Label { get; private set; }
        public bool Disabled { get; private set; }

        public Option(string label, bool disabled = false)
        {
            this.Label = label;
            this.Disabled = disabled;
        }
    }
}
