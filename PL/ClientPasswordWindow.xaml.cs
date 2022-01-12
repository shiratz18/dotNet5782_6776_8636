using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ClientPasswordWindow.xaml
    /// </summary>
    public partial class ClientPasswordWindow : Window
    {
        private IBL myBL;

        public ClientPasswordWindow(IBL bl)
        {
            myBL = bl;

            InitializeComponent();
        }

        #region Login button
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(usernameTxtBox.Text) || String.IsNullOrEmpty(userPassword.Password))
                {
                    MessageBox.Show("Please fill all fields.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
                else
                {
                    int.TryParse(usernameTxtBox.Text, out int id);
                    Customer c = myBL.GetCustomer(id);
                    if (c.Password == userPassword.Password)
                    {
                        new ClientWindow(myBL, c).Show();
                        Close();
                    }
                    else
                        MessageBox.Show("WRONG PASSOWRD!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            catch (NoIDException)
            { }
        }
        #endregion

        #region Close window
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Forgot password
        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(usernameTxtBox.Text))
            {
                MessageBox.Show("Please enter your ID", "ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            
            Customer c = myBL.GetCustomer(int.Parse(usernameTxtBox.Text));
            new ForgotPasswordWindow(myBL, c).ShowDialog();
            Close();
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
    }
}
