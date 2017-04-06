using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.DataObjects
{
    public interface ICommonRepository
    {
        string GetPOSNextNumber(DateTime date);
        string GetITNextNumber(DateTime date);
    }
}
