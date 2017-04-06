﻿using System;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class CheckIn : UserControl
    {
        public CheckIn()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = DataContext as CheckInViewModel;
            model.Password = txtPassword.Password;
            model.CheckIn.Execute(null);

            txtPassword.Password = null;
            txtUserName.Text = null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = null;
            txtUserName.Text = null;

            FocusManager.SetFocusedElement(this, txtUserName);
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            var model = DataContext as CheckInViewModel;
            model.Password = txtPassword.Password;
            if (e.Key == Key.Enter)
            {
                FocusManager.SetFocusedElement(this, btnCheckIn);
                model.CheckIn.Execute(null);

                txtPassword.Password = null;
                txtUserName.Text = null;
            }
        }
    }
}