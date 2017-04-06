using AutoMapper;
using Sendo.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Zit.BusinessObjects;
using Zit.BusinessObjects.BusinessModels;
using Zit.BusinessObjects.Constants;
using Zit.BusinessObjects.Enums;
using Zit.Configurations;
using Zit.Core;
using Zit.Core.Repository;
using Zit.DataObjects;
using Zit.Entity;
using Zit.EntLib.Extensions;
using Zit.Security;

namespace Zit.BusinessLogic
{
    public class HrBusiness : BusinessBase, IHrBusiness
    {
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public void UserCheckTime(string userName, string password)
        {
            var userRepo = IoC.Get<ISysUserRepository>();
            SYS_User user = userRepo.GetUserByUserName(userName);

            if (!VerifyPassword(user.Password, user.UserName, password))
            {
                this.AddError("Mật khẩu sai hoặc tên đăng nhập không tồn tại trong hệ thống");
                return;
            }

            var hrRepo = IoC.Get<IHrUserCheckTimeRepository>();
            hrRepo.Add(new HR_UserCheckTime() { 
                CheckInOutTime = DateTime.Now,
                UserName = user.UserName
            });

            IoC.Get<IUnitOfWork>().Commit();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.PS)]
        public string GetUserByName(string userName)
        {
            var userRepo = IoC.Get<ISysUserRepository>();
            SYS_User user = userRepo.GetUserByUserName(userName);
            if (user != null) return user.UserName;
            else return null;
        }

        private bool VerifyPassword(string passwordHashed, string userName, string password)
        {
            var forHash = (userName ?? "") + (password ?? "");
            return passwordHashed.VerifyHash(forHash, AppConfig.Current.HashSalt);
        }

        private string HashPassword(string userName, string password)
        {
            var forHash = (userName ?? "") + (password ?? "");
            return forHash.Hash(AppConfig.Current.HashSalt);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = Functions.UE)]
        public void UpdateUser(string userName, string fullName, string password, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(userName)
                || string.IsNullOrWhiteSpace(fullName)
                || string.IsNullOrWhiteSpace(password)
                )
            {
                this.AddError("Dữ liệu không hợp lệ");
                return;
            }

            var userRepo = IoC.Get<ISysUserRepository>();
            var user = userRepo.GetUserByUserName(userName);
            if (user == null)
            {
                user = new SYS_User()
                {
                    AppID = 1,
                    FullName = fullName,
                    UserName = userName,
                    Password = HashPassword(userName, password),
                    Status = (int)(isActive ? UserStatus.Active : UserStatus.Stop)
                };
                userRepo.Add(user);

                IoC.Get<ISysUserRoleRepository>().Add(new SYS_UserRole() { 
                    RoleID = 1,
                    SYS_User = user
                });
            }
            else
            {
                user.FullName = fullName;
                user.UserName = userName;
                user.Status = (int)(isActive ? UserStatus.Active : UserStatus.Stop);
                user.Password = HashPassword(userName, password);
                userRepo.Update(user, m => m.FullName, m => m.UserName, m => m.Status, m => m.Password);
            }

            IoC.Get<IUnitOfWork>().Commit();
        }
    }
}
