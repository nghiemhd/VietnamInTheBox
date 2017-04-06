using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Aero87
{
    public class CF_Carrier : BaseEntity
    {
        public string Name { get; set; }
        public int AcctId { get; set; }
        public string Code { get; set; }
    }
}
