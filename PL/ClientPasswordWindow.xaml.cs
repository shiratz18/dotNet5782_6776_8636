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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int.TryParse(usernameTxtBox.Text, out int id);
                Customer c = myBL.GetCustomer(id);
                new ClientWindow(myBL, c).ShowDialog();
            }
            catch (NoIDException)
            { }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }

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

        private void usernameTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
