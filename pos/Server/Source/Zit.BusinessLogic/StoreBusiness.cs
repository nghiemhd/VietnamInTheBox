using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;
using Zit.DataObjects;
using Zit.EntLib.Extensions;

namespace Zit.BusinessLogic
{
    public class StoreBusiness : IStoreBusiness
    {
        public List<CF_Store> GetAllStore()
        {
            var rp = IoC.Get<IStoreRepository>();
            return rp.GetAll().Where(m => m.Id != 0).ToList();
        }
    }
}
