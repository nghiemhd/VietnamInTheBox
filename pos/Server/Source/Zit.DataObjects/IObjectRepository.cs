using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zit.BusinessObjects;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public interface IObjectRepository : IRepository<CF_Obj>
    {
        List<CF_Obj> GetAll();
    }
}
