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
    public class CheckInViewModel : FormViewModelBase
    {
        private Func<IZitServices> _service = () =>
        {
            return ServiceLocator.Current.GetInstance<IZitServices>();
        };

        public CheckInViewModel(IZitServices service)
        {
            Title = "Chấm công nhân viên";

            if (IsInDesignMode)
            {

            }
            else
            {

            }

            CheckIn = new RelayCommand(__checkIn);
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

        public ICommand CheckIn { get; set; }

        private void __checkIn()
        {
            var rp = _service().UserCheckTime(UserName, Password);
            if (rp.HasError)
            {
                MessageBox.Show(rp.ToErrorMsg(), "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Chấm công thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            UserName = null;
            Password = null;
        }
    }
}
