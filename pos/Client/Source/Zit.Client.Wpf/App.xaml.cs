using log4net;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Zit.Client.Proxy.ZitServices;
using Zit.Client.Wpf.Infractstructure;
using Zit.Client.Wpf.ViewModel;

namespace Zit.Client.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(App));

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            //Init Culture
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN");
            //Unity
            IServiceLocator locator = new UnityServiceLocator(UnityReg.Reg());
            ServiceLocator.SetLocatorProvider(() => locator);
            //Mapper
            Map.Boot();
            //Scanner
            var scanner = ServiceLocator.Current.GetInstance<IScanner>();
            if (!scanner.TryOpen())
            {
                MessageBox.Show("Không thể kết nối đến máy scan barcode");
            }

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _log.Error(e.Exception);
            if (e.Exception.InnerException is SecurityAccessDeniedException
                || (e.Exception.InnerException != null && e.Exception.InnerException.InnerException is SecurityAccessDeniedException)
                )
            {
                MessageBox.Show("Bạn không có quyền thực hiện chức năng này, hoặc tài khoản đã timeout cần login lại.","Thông báo",MessageBoxButton.OK,MessageBoxImage.Error);

                UnityContainer container = (UnityContainer)ServiceLocator.Current.GetInstance<IUnityContainer>();
                container.Teardown(ServiceLocator.Current.GetInstance<IZitServices>());

                var mainvm = ServiceLocator.Current.GetInstance<MainViewModel>();
                mainvm.IsAuthenticate = false;

                ServiceLocator.Current.GetInstance<MainViewModel>().CurrentView = ViewLocator.Login;
            }
            else
            {
                MessageBox.Show(e.Exception.Message);
            }
            e.Handled = true;
        }        

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //Try close Scanner
            var scanner = ServiceLocator.Current.GetInstance<IScanner>();
            scanner.Dispose();
        }
    }
}
