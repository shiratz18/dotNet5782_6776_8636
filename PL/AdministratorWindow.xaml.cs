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

        public AdministratorWindow(IBL bl, MainWindow w)
        {
            myBL = bl;
            main = w;
            InitializeComponent();
        }

        private void btnDrones_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(myBL).Show();
        }

        private void btnStations_Click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(myBL).Show();
        }

        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(myBL).Show();
        }

        private void btnParcels_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(myBL).Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            main.WindowState = WindowState.Normal;
        }
    }
}
