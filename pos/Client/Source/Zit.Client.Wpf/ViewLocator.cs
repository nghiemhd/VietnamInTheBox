using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Zit.Client.Wpf
{
    public static class ViewLocator
    {
        public static SaleOrder Saleorder
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SaleOrder>();
            }
        }

        public static InventoryTransfer InventoryTransfer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InventoryTransfer>();
            }
        }

        public static Login Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Login>();
            }
        }

        public static CheckIn CheckIn
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CheckIn>();
            }
        }

        public static UserEdit UserEdit
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserEdit>();
            }
        }
    }
}
