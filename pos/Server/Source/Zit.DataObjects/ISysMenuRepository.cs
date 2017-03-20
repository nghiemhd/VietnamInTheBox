using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core;

namespace Zit.DataObjects
{
    public interface ISysMenuRepository
    {
        IEnumerable<SYS_Menu> GetAll();
    }
}
