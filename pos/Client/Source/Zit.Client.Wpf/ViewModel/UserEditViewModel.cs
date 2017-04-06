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
    public class UserEditViewModel : FormViewModelBase
    {
        private Func<IZitServices> _service = () =>
        {
            return ServiceLocator.Current.GetInstance<IZitServices>();
        };

        public UserEditViewModel(IZitServices service)
        {
            Title = "Tạo/Thay đổi thông tin tài khoản";

            if (IsInDesignMode)
            {

            }
            else
            {

            }

            IsActive = true;

            Change = new RelayCommand(__change);
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

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }

        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        public ICommand Change { get; set; }

        private void __change()
        {
            var rp = _service().UpdateUser(UserName,FullName, Password,IsActive);
            if (rp.HasError)
            {
                MessageBox.Show(rp.ToErrorMsg(), "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Thay đổi / tạo mới thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            UserName = null;
            Password = null;
        }
    }
}
