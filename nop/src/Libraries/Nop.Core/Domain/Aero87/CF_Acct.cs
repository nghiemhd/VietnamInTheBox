using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Aero87
{
    public class CF_Acct : BaseEntity
    {
        public string AcctCode { get; set; }
        public string AcctName { get; set; }

        public bool RequireObj { get; set; }

        public string AcctFull
        {
            get
            {
                return AcctCode + " - " + AcctName;
            }
        }
    }
}
