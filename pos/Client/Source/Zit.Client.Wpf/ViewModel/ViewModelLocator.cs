/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Zit.Client.Wpf"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Zit.Client.Wpf.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public SaleOrderViewModel SaleOrder
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SaleOrderViewModel>();
            }
        }

        public InventoryTransferViewModel InventoryTransfer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InventoryTransferViewModel>();
            }
        }

        public LoginViewModel Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }

        public CheckInViewModel CheckIn
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CheckInViewModel>();
            }
        }

        public UserEditViewModel UserEdit
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserEditViewModel>();
            }
        }

        public CommonStaticModel Common
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CommonStaticModel>();
            }
        }
            
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}