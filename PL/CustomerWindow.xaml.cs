using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private IBL myBL;
        private Customer customer;

        #region Add

        #region Constructor
        /// <summary>
        /// Constructor for add grid
        /// </summary>
        /// <param name="bl"></param>
        public CustomerWindow(IBL bl)
        {
            myBL = bl;

            InitializeComponent();
            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid
            this.Title = "New customer"; //change the title

        }
        #endregion

        #region Add button
        /// <summary>
        /// Adds customer with user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //get the data the user entered
            customer = new Customer()
            {
                Id = int.Parse(customerId.Text),
                Name = customerName.Text,
                Phone = customerPhone.Text,
                Location = new Location()
                {
                    Longitude = SliderLongitude.Value,
                    Latitude = SliderLatitude.Value
                }
            };

            MessageBoxResult mb = default;
            try
            {
                myBL.AddCustomer(customer);
            }
            catch (InvalidNumberException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
                {
                    Close();
                }
            }
            catch (WrongFormatException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
                {
                    Close();
                }
            }
            catch (NoAvailableChargeSlotsException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
                {
                    Close();
                }
            }
            catch (DoubleIDException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
                {
                    Close();
                }
            }


            if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
            {
                Close();
            }
            if (mb == MessageBoxResult.OK) //if the user clicked ok in message box, try again to add the drone
            {
                return;
            }


            Close();
            MessageBox.Show("Customer successfully added", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region ID
        /// <summary>
        /// Text changed event for customerId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(customerId.Text, out int num);
            if (flag && num < 100000000) //if the id the user entered is less than 4 digits the border is red
            {
                RedMes1.Content = "Incorrect entry, please try again";
            }
            else
            {
                if (RedMes1 != null)
                    RedMes1.Content = "";
            }
        }
        #endregion

        #region Phone
        /// <summary>
        /// Text changed event for customerPhone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            string m = customerPhone.Text;
            if (!String.IsNullOrEmpty(m) && m.Length < 10)//if the phone the user entered is less than 10 characters the border is red
            {
                customerPhone.BorderBrush = Brushes.Red;
                RedMes3.Content = "Incorrect entry, please try again";
            }
            else
            {
                customerPhone.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes3 != null)
                    RedMes3.Content = "";
            }
        }
        #endregion

        #region Cancel
        /// <summary>
        /// Closes this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var mb = MessageBox.Show("Are you sure you want to cancel?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (mb == MessageBoxResult.Yes)
            {
                Close();
            }
        }
        #endregion

        #endregion

        #region Update

        #region Constructor
        /// <summary>
        /// Constructor for action grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="d"></param>
        public CustomerWindow(IBL bl, Customer c)
        {
            myBL = bl;

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible
            this.Title = "Update customer"; //change the title
            customer = c;
            DataContext = customer;
            lstParcelsFrom.ItemsSource = customer.FromCustomer;
            lstParcelsTo.ItemsSource = customer.ToCustomer;
        }
        #endregion

        #region Close
        /// <summary>
        /// Close this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Minimize window
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region Open parcel in customer
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ParcelAtCustomer p = b.CommandParameter as ParcelAtCustomer;

            Parcel parcel = myBL.GetParcel(p.Id);
            new ParcelWindow(myBL, parcel).Show();
        }
        #endregion

        #region Edit name
        /// <summary>
        /// Update the customer name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnUpdateNAME_Click(object sender, RoutedEventArgs e)
        {

            string newName = nameToPrint.Text;

            try
            {
                myBL.UpdateCustomerName(customer.Id, newName); //update the customer name
                customer.Name = newName; //update also the current customer

                MessageBox.Show("Customer name updated successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (WrongFormatException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NoIDException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Edit phone
        /// <summary>
        /// Update the customer phone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdatePHON_Click(object sender, RoutedEventArgs e)
        {
            string newPhone = phoneToPrint.Text;

            try
            {
                myBL.UpdateCustomerPhone(customer.Id, newPhone); //update the customer phone
                customer.Name = newPhone; //update also the current customer

                MessageBox.Show("Customer phone updated successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (WrongFormatException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NoIDException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #endregion

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

    }
}
