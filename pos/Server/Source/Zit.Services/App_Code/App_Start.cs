using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zit.Services;
using Zit.Services.Maps;

namespace Zit.Services.App_Code
{
    public class App_Start
    {
        public static void AppInitialize()
        {
            log4net.Config.XmlConfigurator.Configure();

            UnityConfig.Init();
            Map.Boot();
        }
    }
}