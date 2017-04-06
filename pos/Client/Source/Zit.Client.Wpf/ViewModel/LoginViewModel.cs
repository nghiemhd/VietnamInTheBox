using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zit.Client.Wpf.Infractstructure;
using Zit.Client.Wpf.Messages;
using Zit.Client.Proxy.ZitServices;
using AutoMapper;
using Zit.BusinessObjects.Enums;
using Zit.DataTransferObjects;
using Zit.BusinessObjects.BusinessModels;
using Zit.Entity;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Reporting.WinForms;
using System.Drawing;
using ZXing;
using System.Threading;
using System.Globalization;
using Sendo.Web.Mvc4.Infractstructure;

namespace Zit.Client.Wpf.ViewModel
{
    public class LoginViewModel : FormViewModelBase
    {
        private Func<IZitServices> _service = () =>
        {
            return ServiceLocator.Current.GetInstance<IZitServices>();
        };

        public LoginViewModel(IZitServices service)
        {
            Title = "Đăng nhập";

            if (IsInDesignMode)
            {

            }
            else
            {

            }

            Login = new RelayCommand(__login);
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value);}
        }

        private string _password;
        public string Password
        {   
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public ICommand Login { get; set; }

        private void __login()
        {
            var rp = _service().Login(AppConfig.StoreId, UserName, Password , "76205a984a9845b2ee0a9d2aa96e7149c6ec2270");
            if (rp.HasError)
            {
                MessageBox.Show(rp.ToErrorMsg(), "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                WorkContext.UserContext = rp.Data;

                var mainvm = ServiceLocator.Current.GetInstance<MainViewModel>();

                mainvm.Title = string.Format("Hệ thống bán lẻ Aero87 - Mã : {0} <-> Tên : {1} - Nhân viên : {2}", rp.Data.StoreCode, rp.Data.StoreName, rp.Data.UserName);
                mainvm.CurrentView = ViewLocator.Saleorder;

                mainvm.Users.Clear();
                mainvm.Users.Add(WorkContext.UserContext.UserName);
                mainvm.IsAuthenticate = true;
            }
        }
    }
}
