using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core;

namespace Zit.DataObjects
{
    public interface ISysFunctionRepository
    {
        IEnumerable<SYS_Function> GetAll();
    }
}
