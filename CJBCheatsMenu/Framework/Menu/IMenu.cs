using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJBCheatsMenu.Framework.Menu
{
    public interface IMenu : IOptionGroup
    {
        string Id { get; }
    }
}
