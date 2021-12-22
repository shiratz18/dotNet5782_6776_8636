using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        private IBL myBL;

        public StationListWindow(IBL bl)
        {
            myBL = bl;
            InitializeComponent();
            try
            {
                StationsListView.ItemsSource = myBL.GetStationList();
            }
            catch (BO.EmptyListException) { }
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(myBL).ShowDialog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult ans = MessageBox.Show("Are you sure you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ans == MessageBoxResult.Yes)
                Close();
        }
    }
}
