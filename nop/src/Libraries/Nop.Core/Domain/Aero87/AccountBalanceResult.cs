using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Aero87
{
    public class AccountBalanceResult
    {
        public int AcctId { get; set; }
        public string AcctCode { get; set; }
        public string AcctName { get; set; }
        public decimal BeforeDebit { get; set; }
        public decimal BeforeCredit { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal EndDebit { get; set; }
        public decimal EndCredit { get; set; }
    }
}
