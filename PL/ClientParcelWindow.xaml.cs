using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ClientParcelWindow.xaml
    /// </summary>
    public partial class ClientParcelWindow : Window
    {
        IBL myBL;
        Customer client;
        ClientWindow clientWindow;

        #region Constructor
        public ClientParcelWindow(Customer c, ClientWindow w)
        {
            myBL = BlFactory.GetBl();
            client = c;
            clientWindow = w;
            InitializeComponent();

            parcelWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            parcelPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
        }
        #endregion

        #region Numbers only
        /// <summary>
        /// Sets text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); //allow only numbers in the text box
        }
        #endregion

        #region Close
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mb = MessageBox.Show("Are you sure you want to cancel this order?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mb == MessageBoxResult.Yes)
                Close();
        }
        #endregion

        #region Target Text box
        private void parcelTargetId_LostFocus(object sender, RoutedEventArgs e)
        {
            bool flag = int.TryParse(parcelTargetId.Text, out int num);
            if (flag && num < 100000000)
                redMes1.Content = "Incorrect entry, please try again";
            else
            {
                if (redMes1 != null)
                    redMes1.Content = "";
            }
        }
        private void parcelTargetId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(parcelTargetId.Text, out int num);
            if (flag && num >= 100000000)
                if (redMes1 != null)
                    redMes1.Content = "";

        }
        #endregion

        #region Add parcel
        /// <summary>
        /// Add a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Parcel p = new Parcel()
                {
                    Sender = new CustomerInParcel()
                    {
                        Id = client.Id,
                        Name = client.Name
                    },
                    Target = new CustomerInParcel()
                    {
                        Id = int.Parse(parcelTargetId.Text)
                    },
                    Priority = (Priorities)parcelPriority.SelectedItem,
                    Weight = (WeightCategories)parcelWeight.SelectedItem
                };

                myBL.AddParcel(p);
                clientWindow.client = myBL.GetCustomer(client.Id);
                clientWindow.lstParcelsFrom.ItemsSource = clientWindow.client.FromCustomer;
                clientWindow.lstParcelsTo.ItemsSource = clientWindow.client.ToCustomer;
                Close();
            }
            catch (InvalidNumberException ex)
            {
                MessageBoxResult mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (mb == MessageBoxResult.No)
                    Close();
            }
        }
        #endregion

        #region Combox selection
        private void parcelWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightLbl.Visibility = Visibility.Hidden;
        }

        private void parcelPriority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriorityLbl.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
