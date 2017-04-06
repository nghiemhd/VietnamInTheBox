using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Catalog
{
    public class CFStoreModel : BaseNopEntityModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
    }
}