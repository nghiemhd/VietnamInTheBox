using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Aero87
{
    public class CF_Obj : BaseEntity
    {
        public string ObjCode { get; set; }
        public string ObjName { get; set; }

        public string ObjFull
        {
            get
            {
                return ObjCode + " - " + ObjName;
            }
        }
    }
}
