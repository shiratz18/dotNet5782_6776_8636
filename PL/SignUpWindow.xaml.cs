using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        private IBL myBL;

        public SignUpWindow(IBL bl)
        {
            InitializeComponent();
            myBL = bl;
        }

        #region Numbers only for TextBox
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

        #region Password
        /// <summary>
        /// Confirms the password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string p1 = passwordConfirm.Password;
            string p2 = password.Password;

            //if both passwords have been entered and they are the same, change icon to be green to indicate that the password was confirmed
            if (!String.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2) && p1 == p2)
            {
                confirmed.Foreground = Brushes.LightGreen;
                answer.IsEnabled = true;
            }
        }

        private void answer_PasswordChanged(object sender, RoutedEventArgs e)
        {
            chkBoxAgree.IsEnabled = true;
        }
        #endregion

        #region Add customer
        /// <summary>
        /// Add the customer to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //proceed only if all fields are full
            if (String.IsNullOrEmpty(txtBoxId.Text) || String.IsNullOrEmpty(txtBoxName.Text) ||
                String.IsNullOrEmpty(txtBoxPhone.Text) || String.IsNullOrEmpty(password.Password) ||
                String.IsNullOrEmpty(passwordConfirm.Password) || String.IsNullOrEmpty(answer.Password))
            {
                MessageBox.Show("Please fill in all the fields.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Customer c = new Customer()
            {
                Id = int.Parse(txtBoxId.Text),
                Name = txtBoxName.Text,
                Phone = txtBoxPhone.Text,
                FromCustomer = new List<ParcelAtCustomer>(),
                ToCustomer = new List<ParcelAtCustomer>(),
                Location = new Location()
                {
                    Latitude = sliderLatitude.Value,
                    Longitude = sliderLongitude.Value
                },
                Password = password.Password,
                Answer = answer.Password,
                Active = true
            };

            try
            {
                myBL.AddCustomer(c);
                new ClientWindow(myBL, c).ShowDialog();
                Close();
            }
            catch (InvalidNumberException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (WrongFormatException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (DoubleIDException)
            {
                MessageBox.Show("ID number already exists within the system.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #endregion

        #region Close window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion       
    }
}
