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
using System.Windows.Shapes;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        private IBL myBL;

        public CustomerListWindow(IBL bl)
        {
            myBL = bl;
            InitializeComponent();
            try
            {
                CustomersListView.ItemsSource = myBL.GetCustomerList();
            }
            catch (BO.EmptyListException) { }
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}