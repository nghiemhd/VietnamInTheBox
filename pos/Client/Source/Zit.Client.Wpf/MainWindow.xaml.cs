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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1 || e.Key == Key.F2 || e.Key == Key.F9 || e.Key == Key.F10)
            {
                var model = DataContext as MainViewModel;
                model.CallFunction.Execute(e.Key.ToString());
            }


            if (e.Key == Key.Escape)
            {
                if (!txtCommand.IsFocused)
                {
                    FocusManager.SetFocusedElement(this, txtCommand);
                }
            }
            else
            {
                var element = FocusManager.GetFocusedElement(this);

                if (element is TextBox && ((TextBox)element).Name == "txtAddUser") return;

                if (e.Key == Key.Enter || e.Key == Key.Escape)
                {
                    if (element is TextBox)
                    {
                        if (!((TextBox)element).AcceptsReturn)
                        {
                            FocusManager.SetFocusedElement(this, txtCommand);
                        }
                    }
                }
                else
                {
                    if (!(element is TextBox || element is PasswordBox))
                        FocusManager.SetFocusedElement(this, txtCommand);
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var model = DataContext as MainViewModel;
                var txt = sender as TextBox;
                model.AddUser(txt.Text);
            }
        }
    }
}
