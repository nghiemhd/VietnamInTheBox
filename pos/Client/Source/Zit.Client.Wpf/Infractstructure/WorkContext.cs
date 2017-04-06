using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zit.BusinessObjects;

namespace Sendo.Web.Mvc4.Infractstructure
{
    public static class WorkContext
    {
        public static UserContext UserContext { get; set; }
    }
}