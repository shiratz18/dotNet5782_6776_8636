using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        internal Customer client;

        #region Constructor
        public ClientWindow(IBL bl, Customer c)
        {
            myBL = bl;
            client = c;
            InitializeComponent();
            DataContext = client;

            lstParcelsFrom.ItemsSource = client.FromCustomer;
            lstParcelsTo.ItemsSource = client.ToCustomer;
        }
        #endregion

        #region Close button
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Add parcel
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            new ClientParcelWindow(myBL, client, this).ShowDialog();
            client = myBL.GetCustomer(client.Id);
            lstParcelsFrom.ItemsSource = client.FromCustomer;
            lstParcelsTo.ItemsSource = client.ToCustomer; DataContext = client;
        }
        #endregion

        #region Track parcel
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ParcelAtCustomer p = b.CommandParameter as ParcelAtCustomer;

            Parcel parcel = myBL.GetParcel(p.Id);
            bool isSender = false;
            if (parcel.Sender.Id == client.Id)
                isSender = true;
            new TrackParcelWindow(myBL, parcel, isSender).ShowDialog();
            client = myBL.GetCustomer(client.Id);
            lstParcelsFrom.ItemsSource = client.FromCustomer;
            lstParcelsTo.ItemsSource = client.ToCustomer;
        }
        #endregion

        #region Menu
        #region Edit name
        private void nameTxtBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (nameTxtBox.Text != client.Name)
            {
                var mb = MessageBox.Show($"Change your name to {nameTxtBox.Text}?", "Confrimation", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.Yes)
                {
                    if (!String.IsNullOrEmpty(nameTxtBox.Text))
                    {
                        client.Name = nameTxtBox.Text;
                        myBL.UpdateCustomer(client);
                    }
                }
            }
        }
        #endregion

        #region Edit phone number
        private void numbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); //allow only numbers in the text box
        }

        private void phoneTxtBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (phoneTxtBox.Text != client.Phone)
            {
                if (!String.IsNullOrEmpty(phoneTxtBox.Text) && phoneTxtBox.Text.Length == 10)
                {
                    var mb = MessageBox.Show($"Change your phone number to {phoneTxtBox.Text}?", "Confrimation", MessageBoxButton.YesNo);
                    if (mb == MessageBoxResult.Yes)
                    {
                        client.Phone = phoneTxtBox.Text;
                        myBL.UpdateCustomer(client);
                    }
                }
                else
                    phoneTxtBox.Text = client.Phone;
            }
        }
        #endregion

        #region Delete
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var mb = MessageBox.Show("Are you sure you want to delete your account?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (mb == MessageBoxResult.Yes)
            {
                myBL.RemoveCustomer(client.Id);
                MessageBox.Show("Account successfully deleted.", "", MessageBoxButton.OK);
            }
        }
        #endregion

        #endregion

        #region Refresh
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            client = myBL.GetCustomer(client.Id);
            lstParcelsFrom.ItemsSource = client.FromCustomer;
            lstParcelsTo.ItemsSource = client.ToCustomer;
        }
        #endregion
    }
}
