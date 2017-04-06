using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zit.Client.Wpf.Infractstructure
{
    public class AppConfig
    {
        public static int StoreId = int.Parse(ConfigurationSettings.AppSettings["StoreId"]);
    }
}
