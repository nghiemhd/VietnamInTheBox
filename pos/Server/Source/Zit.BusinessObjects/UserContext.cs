using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.BusinessObjects
{
    public class UserContext
    {
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string UserName { get; set; }
        public string StoreAdress { get; set; }
    }
}
