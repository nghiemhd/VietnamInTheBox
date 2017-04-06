using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Zit.BusinessLogic;
using Zit.Cache;
using Zit.Configurations;
using Zit.Core;
using Zit.Core.Repository;
using Zit.Data;
using Zit.Security;
using Zit.Wcf.Libs;
using Zit.DataObjects;
using Zit.DataObjects.Views;

namespace Zit.Services.Maps
{
    public static class UnityConfig
    {
        public static void Init()
        {
            IUnityContainer ctn = Reg();

            ICacheManager cache = CacheFactory.GetCacheManager(ZitSessionContainer.CACHENAME);
            ctn.RegisterInstance<ICacheManager>(ZitSessionContainer.CACHENAME, cache,
                                                new ContainerControlledLifetimeManager());

            ICacheManager cacheGlobal = CacheFactory.GetCacheManager(GlobalCache.CACHENAME);
            ctn.RegisterInstance<ICacheManager>(GlobalCache.CACHENAME, cache, new ContainerControlledLifetimeManager());

            IServiceLocator locator = new UnityServiceLocator(ctn);

            ServiceLocator.SetLocatorProvider(() => locator);
        }

        private static IUnityContainer Reg()
        {
            IUnityContainer ctn = new UnityContainer();

            #region System

            ctn.RegisterType<GlobalCache>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<SessionCache>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<ZitCacheManager>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<ISessionContainer, ZitSessionContainer>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<ITokenContainer, ZitTokenContainer>(new ContainerControlledLifetimeManager());
            ctn.RegisterType<IRequestContainer, RequestContainer>(new ContainerControlledLifetimeManager());

            ctn.RegisterType<UserConfig>(new PerSessionLifetimeManager());
            ctn.RegisterType<BusinessProcess>(new PerRequestLifetimeManager());
            ctn.RegisterType<IObjectContext, DataEntities>(new PerRequestLifetimeManager(), new InjectionConstructor());
            ctn.RegisterType<IUnitOfWork, EFUnitOfWork>(new PerRequestLifetimeManager());

            #endregion

            #region Repositories

            ctn.RegisterType<ISysAppFunctionRepository, SysAppFunctionRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysAppsRepository, SysAppsRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysConfigAppRepository, SysConfigAppRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysConfigUserRepository, SysConfigUserRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysFunctionRepository, SysFunctionRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysMenuRepository, SysMenuRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysRoleFunctionRepository, SysRoleFunctionRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysUserRepository, SysUserRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysUserRoleRepository, SysUserRoleRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISysViewRepository, SysViewRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<IViewRoleMenuRepository, ViewRoleMenuRepository>(new PerRequestLifetimeManager());

            ctn.RegisterType<IProductRepository, ProductRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ICommonRepository, CommonRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<IPOSSaleOrderRepository, POSSaleOrderRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<IPOSSaleOrderDetailRepository, POSSaleOrderDetailRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<IIVMTransactionRepository, IVMTransactionRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<IStoreRepository, StoreRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<IObjectRepository, ObjectRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ICarrierRepository, CarrierRepository>(new PerRequestLifetimeManager());

            ctn.RegisterType<IIVMTransferDetailRepository, IVMTransferDetailRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<IIVMTransferRepository, IVMTransferRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<IHrUserCheckTimeRepository, HrUserCheckTimeRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISaleSourceRepository, SaleSourceRepository>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISaleReturnReasonRepository, SaleReturnReasonRepository>(new PerRequestLifetimeManager());

            #endregion

            #region Business

            ctn.RegisterType<IAuthenticateBusiness, AuthenticateBusiness>(new PerRequestLifetimeManager());
            ctn.RegisterType<IProductBusiness, ProductBusiness>(new PerRequestLifetimeManager());
            ctn.RegisterType<ISaleOrderBusiness, SaleOrderBusiness>(new PerRequestLifetimeManager());
            ctn.RegisterType<IInventoryBusiness, InventoryBusiness>(new PerRequestLifetimeManager());
            ctn.RegisterType<IStoreBusiness, StoreBusiness>(new PerRequestLifetimeManager());
            ctn.RegisterType<IObjectBusiness, ObjectBusiness>(new PerRequestLifetimeManager());
            ctn.RegisterType<IHrBusiness, HrBusiness>(new PerRequestLifetimeManager());
            ctn.RegisterType<ICommonBusiness, CommonBusiness>(new PerRequestLifetimeManager());

            #endregion

            return ctn;
        }
    }
}