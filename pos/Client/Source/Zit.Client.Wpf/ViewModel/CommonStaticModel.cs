using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.BusinessObjects.Enums;
using Zit.Utils;

namespace Zit.Client.Wpf.ViewModel
{
    public class CommonStaticModel
    {
        private  Dictionary<SaleChanel, string> _saleChanelList = null;
        public Dictionary<SaleChanel, string> SaleChanelList
        {
            get
            {
                if (_saleChanelList == null)
                {
                    _saleChanelList = EnumHelpers.ToDictionary<SaleChanel>();
                }
                return _saleChanelList;
            }
        }
    }
}
