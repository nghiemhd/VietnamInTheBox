using Nop.Core.Domain.Aero87;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Aero87
{
    public interface ICommonService
    {
        IList<CF_Store> GetAllStore();

        IList<CF_Carrier> GetAllCarrier();

        IList<CF_SaleSource> GetAllSaleSource();

        IList<CF_SaleReturnReason> GetAllSaleReturnReason();
    }
}
