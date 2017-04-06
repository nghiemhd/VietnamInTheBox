using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects;
using Zit.BusinessObjects.SqlResultModel;
using Zit.Core.Repository;

namespace Zit.DataObjects
{
    public class ProductRepository : EFRepository<Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork unitOfWork)
            :base(unitOfWork)

        {

        }

        
    }
}
