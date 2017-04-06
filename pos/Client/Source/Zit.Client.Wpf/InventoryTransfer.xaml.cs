using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zit.Client.Wpf.ViewModel;

namespace Zit.Client.Wpf
{
    /// <summary>
    /// Interaction logic for SaleOrder.xaml
    /// </summary>
    public partial class InventoryTransfer : UserControl
    {
        public InventoryTransfer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as InventoryTransferViewModel;
            model.FormLoad.Execute(null);
        }
    }
}
