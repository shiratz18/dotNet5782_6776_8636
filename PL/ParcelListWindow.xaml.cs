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
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        private IBL myBL;

        public ParcelListWindow(IBL bl)
        {
            myBL = bl;
            InitializeComponent();
            try
            {
                ParcelsListView.ItemsSource = myBL.GetParcelList();
            }
            catch (BO.EmptyListException) { }
        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(myBL).ShowDialog();
        }

        
    }
}