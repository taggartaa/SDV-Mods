namespace CJBCheatsMenu.Framework.Menu
{
    public class Option : IOption
    {
        public virtual string Label { get; protected set; }
        public virtual bool Disabled { get; protected set; }

        public Option(string label, bool disabled = false)
        {
            this.Label = label;
            this.Disabled = disabled;
        }
    }
}
