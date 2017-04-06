using Nop.Core.Data;
using Nop.Core.Domain.Aero87;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Aero87
{
    public class CommonService : ICommonService
    {
        private readonly IDbContext _dbContext;
        private readonly IDataProvider _dataProvider;

        public CommonService(IDbContext dbContext, IDataProvider dataProvider)
        {
            this._dbContext = dbContext;
            this._dataProvider = dataProvider;
        }

        public IList<CF_Store> GetAllStore()
        {
            var rs = _dbContext.SqlQuery<CF_Store>(@"exec [usp_GetAllStore]").ToList();
            return rs;
        }

        public IList<CF_Carrier> GetAllCarrier()
        {
            var rs = _dbContext.SqlQuery<CF_Carrier>(@"exec [usp_GetAllCarrier]").ToList();
            return rs;
        }


        public IList<CF_SaleSource> GetAllSaleSource()
        {
            var rs = _dbContext.SqlQuery<CF_SaleSource>(@"exec [usp_GetAllSaleSource]").ToList();
            return rs;
        }

        public IList<CF_SaleReturnReason> GetAllSaleReturnReason()
        {
            var rs = _dbContext.SqlQuery<CF_SaleReturnReason>(@"exec [usp_GetAllSaleReturnReason]").ToList();
            return rs;
        }
    }
}
