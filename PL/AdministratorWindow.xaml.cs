using System;
using System.Windows;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        private IBL myBL;
        private MainWindow main;

        public AdministratorWindow(MainWindow w)
        {
            myBL = BlFactory.GetBl();
            main = w;
            InitializeComponent();
        }

        private void btnDrones_Click(object sender, RoutedEventArgs e)
        {
            DroneListWindow.Instance.Show(); //show the window
            DroneListWindow.Instance.Activate(); //bring it to the front
        }

        private void btnStations_Click(object sender, RoutedEventArgs e)
        {
            StationListWindow.Instance.Show(); //show the window
            StationListWindow.Instance.Activate(); //bring it to the front
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            CustomerListWindow.Instance.Show(); //show the window
            CustomerListWindow.Instance.Activate(); //bring it to the front
        }

        private void btnParcels_Click(object sender, RoutedEventArgs e)
        {
            ParcelListWindow.Instance.Show(); //show the window
            ParcelListWindow.Instance.Activate(); //bring it to the front
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //close all the windows
            DroneListWindow.Instance.Close();
            StationListWindow.Instance.Close();
            ParcelListWindow.Instance.Close();
            CustomerListWindow.Instance.Close();
            Close();
            main.WindowState = WindowState.Normal;
        }
    }
}
