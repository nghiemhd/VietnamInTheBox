using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Controls;
using System;
using Zit.Client.Wpf.Messages;
using Microsoft.Practices.ServiceLocation;
using System.Windows;
using Zit.Client.Proxy.ZitServices;
using Zit.Client.Wpf.Infractstructure;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sendo.Web.Mvc4.Infractstructure;
using System.Linq;

namespace Zit.Client.Wpf.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Func<IZitServices> _service = () => {
            return ServiceLocator.Current.GetInstance<IZitServices>();
        };
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Title = "Hệ thống bán lẻ Aero87";

            CurrentView = ViewLocator.Login;

            if (IsInDesignMode)
            {

            }
            else
            {
                Users = new ObservableCollection<string>();
                //Init Command
                CallFunction = new RelayCommand<string>(__callFunction);
                //InputCommand
                InputCommand = new RelayCommand(__inputCommand);

                ClearUser = new RelayCommand(__clearUser);

                var scanner = ServiceLocator.Current.GetInstance<IScanner>();

                if (scanner.Status)
                {
                    scanner.ScanEvent += (data) =>
                    {
                        App.Current.Dispatcher.Invoke(() => {
                            if (!App.Current.MainWindow.IsActive)
                            {
                                App.Current.MainWindow.Activate();
                                App.Current.MainWindow.WindowState = WindowState.Maximized;
                            }
                        });
                        BarcodeDataMsg msg = new BarcodeDataMsg();
                        msg.Data = data;
                        msg.CmdData = KeyboardCommand;
                        KeyboardCommand = null;
                        if (CurrentViewModel != null)
                        {
                            __processBeforeSendMsg(msg.Data);
                            MessengerInstance.Send<BarcodeDataMsg>(msg, CurrentViewModel.ViewId);
                        }
                    };
                }
            }   
        }

        private void __processBeforeSendMsg(string data)
        {
            if (data != null && data.Length > 2)
                switch (data.Substring(0, 2))
                {
                    case "IT":
                        if (CurrentView != ViewLocator.InventoryTransfer)
                        {
                            CurrentView = ViewLocator.InventoryTransfer;
                        }
                        break;
                    case "PS":
                        if (CurrentView != ViewLocator.Saleorder)
                        {
                            CurrentView = ViewLocator.Saleorder;
                        }
                        break;
                }
        }

        #region Command

        public ICommand CallFunction
        {
            get;
            private set;
        }

        public ICommand ClearUser
        {
            get;
            private set;
        }

        public ICommand InputCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Call Function
        /// </summary>
        /// <param name="fcode"></param>
        private void __callFunction(string fcode)
        {
            switch (fcode)
            {
                case "F1":
                    CurrentView = ViewLocator.Saleorder;
                    break;
                case "F2":
                    CurrentView = ViewLocator.InventoryTransfer;
                    break;
                case "F9":
                    CurrentView = ViewLocator.CheckIn;
                    break;
                case "F10":
                    CurrentView = ViewLocator.UserEdit;
                    break;
                default:
                    MessageBox.Show("Chức năng chưa có trong hệ thống");
                    break;
            }
        }

        private void __inputCommand()
        {
            var cmd = KeyboardCommand;
            if (cmd != null)
            {
                cmd = cmd.ToUpper();
            }

            CommandMsg msg = new CommandMsg();
            msg.Data = cmd;
            __processBeforeSendMsg(msg.Data);
            MessengerInstance.Send<CommandMsg>(msg, CurrentViewModel.ViewId);
            
            KeyboardCommand = null;
        }

        private void __clearUser()
        {
            Users = new ObservableCollection<string>();
            Users.Add(WorkContext.UserContext.UserName);
        }

        #endregion

        private string _title;
        public string Title
        {
            get {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private object _currentView;
        public object CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                if (_currentView != null && _currentView is UserControl)
                    CurrentViewModel = ((UserControl)_currentView).DataContext as FormViewModelBase;
                RaisePropertyChanged("CurrentView");
            }
        }

        private FormViewModelBase _currentViewModel;
        public FormViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;

            }
            set
            {
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        private string _keyboardCommand;

        public string KeyboardCommand
        {
            get { return _keyboardCommand; }
            set { _keyboardCommand = value;
                    RaisePropertyChanged("KeyboardCommand");
            }
        }

        private ObservableCollection<string> _users;

        public ObservableCollection<string> Users
        {
            get { return _users; }
            set { _users = value; RaisePropertyChanged("Users");
                _users.CollectionChanged += _users_CollectionChanged;
            }
        }

        void _users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var salevm = ServiceLocator.Current.GetInstance<SaleOrderViewModel>();
            if (salevm.Model.IsNew && (!salevm.Model.IsReturn))
                salevm.Model.Users = _users.Aggregate((a, b) => { return a + "," + b; });
        }

        private bool isAuthenticate;

        public bool IsAuthenticate
        {
            get { return isAuthenticate; }
            set { isAuthenticate = value; RaisePropertyChanged("IsAuthenticate");}
        }
        

        public void AddUser(string userName)
        {
            if (Users.Contains(userName)) return;
            var rp = _service().GetUserByName(userName);
            if (rp.Data != null)
            {
                Users.Add(rp.Data);
            }
            else
            {
                MessageBox.Show("Không tìm thấy tên nhân viên", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}