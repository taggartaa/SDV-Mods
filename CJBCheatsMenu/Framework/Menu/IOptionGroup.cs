using System.Collections.Generic;

namespace CJBCheatsMenu.Framework.Menu
{
    public interface IOptionGroup
    {
        string Title { get; }
        List<IOption> Options { get; }
    }
}
