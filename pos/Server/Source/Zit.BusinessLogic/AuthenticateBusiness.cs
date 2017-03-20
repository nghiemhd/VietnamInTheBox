using System.Linq;
using System;
using System.Diagnostics;
using System.Collections;
using System.Security.Permissions;
using System.Threading;
using log4net;
using System.Globalization;
using Zit.Core;
using Zit.BusinessObjects;
using Zit.EntLib.Extensions;
using Zit.DataObjects;
using Zit.BusinessObjects.Enums;
using Zit.Security;
using Zit.Configurations;
using System.Collections.Generic;
using Zit.DataObjects.Views;
using Sendo.BusinessLogic;


namespace Zit.BusinessLogic
{
    public class AuthenticateBusiness : BusinessBase, IAuthenticateBusiness
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(AuthenticateBusiness));

        #region Vars

        IEnumerable<SYS_Menu> listMenuSource = null;
        IEnumerable<SYS_View> listViewSource = null;
        IEnumerable<SYS_Function> functionInfoSource = null;
        int[] roles = null;
        IEnumerable<SYS_RoleFunction> permissionSource = null;

        #endregion

        #region Public

        public UserContext Login(int storeId, string userName, string password, string appKey)
        {
            int? appID = null;
            appID = ValidateApp(appKey);
            if (appID == null)
            {
                this.AddError("Zit không hổ trợ ứng dụng này");
                return null;
            }

            var userRepo = IoC.Get<ISysUserRepository>();
            SYS_User user = userRepo.GetUserByUserName(userName);

            if (user == null)
            {
                this.AddError("Mật khẩu sai hoặc tên đăng nhập không tồn tại trong hệ thống");
                return null;
            }
            else
            {
                //Check App
                if (user.AppID != appID.Value)
                {
                    this.AddError("App không hợp lệ");
                    return null;
                }

                if (user.UserStatus == UserStatus.Stop)
                {
                    this.AddError("Tên đăng nhập đã ngừng sử dụng");
                    return null;
                }

                if (!VerifyPassword(user.Password, user.UserName, password))
                {
                    this.AddError("Mật khẩu sai hoặc tên đăng nhập không tồn tại trong hệ thống");
                    return null;
                }

                //Check StoreId
                var storeRp = IoC.Get<IStoreRepository>();
                var store = storeRp.GetByID(storeId);
                if (store == null)
                {
                    this.AddError("Cửa hàng không tồn tại");
                    return null;
                }

                WorkContext.UserContext = new UserContext();

                WorkContext.UserContext.StoreId = store.Id;
                WorkContext.UserContext.UserName = userName;
                WorkContext.UserContext.StoreCode = store.Code;
                WorkContext.UserContext.StoreName = store.Name;
                WorkContext.UserContext.StoreAdress = store.Address;
            }

            if (!this.HasError)
            {
                ZitSession.Current.Principal = new ZitPrincipal(this.GetRoles(appID.Value, userName).ToList(), userName, user.FullName, "A87Id");
                ZitSession.Current.AppType = (AppTypeEnum)appID.Value;
                Thread.CurrentPrincipal = ZitSession.Current.Principal;
                return WorkContext.UserContext;
            }

            return null;
        }

        public bool TryOverrideSecurityContextByToken(string token)
        {
            var session = ZitSession.GetSession(token);
            if (session != null)
            {
                //Update Token Container
                ZitSession.Current = session;
                //Override Currrent Security Context
                Thread.CurrentPrincipal = session.Principal;
                return true;
            }
            else return false;
        }

        public bool VerifyPassword(string passwordHashed, string userName, string password)
        {
            var forHash = (userName ?? "") + (password ?? "");
            return passwordHashed.VerifyHash(forHash, AppConfig.Current.HashSalt);
        }

        public string HashPassword(string userName, string password)
        {
            var forHash = (userName??"") + (password??"");
            return forHash.Hash(AppConfig.Current.HashSalt);
        }

        [PrincipalPermission(SecurityAction.Demand,Authenticated=true)]
        public SortedSet<ZitMenu> GetMenus()
        {
            var userName = ZitSession.Current.Principal.Identity.Name;
            if (listMenuSource == null) listMenuSource = IoC.Get<ISysMenuRepository>().GetAll();
            if (listViewSource == null) listViewSource = IoC.Get<ISysViewRepository>().GetAll();
            if (functionInfoSource == null) functionInfoSource = IoC.Get<ISysFunctionRepository>().GetAll();
            if (roles == null) roles = IoC.Get<ISysUserRoleRepository>().GetRoles(userName);
            var menuSource = IoC.Get<IViewRoleMenuRepository>().GetByRoles(roles);

            var menus = from p in menuSource
                             join m in listMenuSource on p.MenuID equals m.MenuID
                             select p.MenuID;
            SortedSet<ZitMenu> listMenu = new SortedSet<ZitMenu>();
            menus
                .ToList().ForEach(item =>
                {
                    __createMenu(listMenu, item, listMenuSource.ToList(), listViewSource.ToList());
                });

            return listMenu;
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        public string[] GetRoles()
        {
            return ZitSession.Current.Principal.Roles.ToArray();
        }

        #endregion

        #region Private

        private int? ValidateApp(string appKey)
        {
            var app = IoC.Get<ISysAppsRepository>().GetByKey(appKey);
            if (app == null) return null;
            Debug.Assert(Enum.GetValues(typeof(AppTypeEnum)).Cast<int>().Contains(app.AppID));
            return app.AppID;
        }

        private IEnumerable<string> GetRoles(int appID, string userName)
        {
            if (roles == null) roles = IoC.Get<ISysUserRoleRepository>().GetRoles(userName);
            if (permissionSource == null) permissionSource = IoC.Get<ISysRoleFunctionRepository>().GetByRoles(roles);
            if (functionInfoSource == null) functionInfoSource = IoC.Get<ISysFunctionRepository>().GetAll();
            var listFunction = (from p in permissionSource
                                join f in functionInfoSource on p.FunctionID equals f.FunctionID
                                select f.FunctionCode).Distinct();
            var app = IoC.Get<ISysAppsRepository>().GetByID(appID);
            if (!app.AllFunctions)
            {
                var appFunctions =
                    from p in IoC.Get<ISysAppFunctionRepository>().GetByAppID(appID).Select(m => m.FunctionID)
                    join f in functionInfoSource on p equals f.FunctionID
                    select f.FunctionCode;
                listFunction = listFunction.Intersect(appFunctions);
            }

            return listFunction;
        }

        private ZitMenu __findMenu(SortedSet<ZitMenu> listMenu, int menuid)
        {
            //Find current root list
            var menuFind = listMenu.Where(m => m.MenuID == menuid).FirstOrDefault();
            if (menuFind == null)
            {
                //Try find in child
                foreach (var menu in listMenu)
                {
                    if (menu.Menus != null || menu.Menus.Any())
                    {
                        menuFind = __findMenu(menu.Menus,menuid);
                        if (menuFind != null) break;
                    }
                }
            }
            return menuFind;
        }

        private void __createMenu(SortedSet<ZitMenu> listMenu, int menuID, List<SYS_Menu> listMenuSource, List<SYS_View> listTypeInfoSource)
        {
            if (__findMenu(listMenu, menuID) != null) return;
            //Exist Not Exist
            var sysmenu = listMenuSource.Where(m => m.MenuID == menuID).First();
            var typeInfo = listTypeInfoSource.Where(m => m.ViewID == sysmenu.ViewID).FirstOrDefault();
            ZitMenu menu = new ZitMenu()
            {
                MenuID = menuID,
                MenuCode = sysmenu.MenuName,
                MenuUrl = typeInfo!=null?typeInfo.Url:null,
                MenuName = sysmenu.MenuName,
                Order = sysmenu.MenuOrder,
                Visible = sysmenu.Enable??false
            };

            if (sysmenu.ParentID == null)
                listMenu.Add(menu);
            else
            {
                __createMenu(listMenu, sysmenu.ParentID.Value, listMenuSource, listTypeInfoSource);
                var parent = __findMenu(listMenu, sysmenu.ParentID.Value);
                parent.Menus.Add(menu);
            }
        }

        #endregion
    }
}