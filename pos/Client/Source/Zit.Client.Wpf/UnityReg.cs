using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zit.Client.Proxy;
using Zit.Client.Proxy.ZitServices;
using Zit.Client.Wpf.Infractstructure;
using Zit.Client.Wpf.ViewModel;

namespace Zit.Client.Wpf
{
    public static class UnityReg
    {
        public static IUnityContainer Reg()
        {
            IUnityContainer ctn = new UnityContainer();

            #region System

            ctn.RegisterInstance<IUnityContainer>(ctn, new ContainerControlledLifetimeManager());

            ctn.RegisterType<IZitServices>(new ContainerControlledLifetimeManager(), new InjectionFactory(u =>
            {
#if DEBUG
                return ProxyFactory.CreateZitServices();
#else
                return ProxyFactory.CreateZitServices_Release();
#endif
            }));

            #endregion

            ctn.RegisterType<IScanner, Scanner_LS1203>(new ContainerControlledLifetimeManager());

            #region ViewModels

            ctn.RegisterType<MainViewModel>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<SaleOrderViewModel>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<InventoryTransferViewModel>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<CommonStaticModel>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<LoginViewModel>(new ContainerControlledLifetimeManager());

            #endregion

            #region Views

            ctn.RegisterType<SaleOrder>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<InventoryTransfer>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<Login>(new ContainerControlledLifetimeManager());

            #endregion

            return ctn;
        }
    }
}
