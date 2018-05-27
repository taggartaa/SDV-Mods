using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Menu
{
    public interface IMenu
    {
        string Id { get; }
        string Title { get; }
        List<IOptionGroup> OptionGroups { get; }
    }
}
