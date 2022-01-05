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
using System.Windows.Shapes;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        IBL myBL;
        Customer client;

        public ClientWindow(IBL bl, Customer c)
        {
            myBL = bl;
            client = c;
            InitializeComponent();
            DataContext = client;

            lstParcelsFrom.ItemsSource = client.FromCustomer;
            lstParcelsTo.ItemsSource = client.ToCustomer;
        }

        #region Close button
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Add parcel
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            new ClientParcelWindow(myBL, client).ShowDialog();
        }
        #endregion

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ParcelAtCustomer p = b.CommandParameter as ParcelAtCustomer;

            Parcel parcel = myBL.GetParcel(p.Id);
            //new ParcelTrackWindow(myBL, parcel).ShowDialog();
        }
    }
}
