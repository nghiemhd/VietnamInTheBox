using System;
using System.Collections.Generic;
using System.Linq;

using Zit.BusinessObjects;
using Zit.BusinessObjects.Enums;
using Zit.Core;
using Zit.Core.Repository;
using Zit.Utils;

namespace Zit.DataObjects
{
    public class HrUserCheckTimeRepository : EFRepository<HR_UserCheckTime>, IHrUserCheckTimeRepository
    {
        public HrUserCheckTimeRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork) {}
    }
}