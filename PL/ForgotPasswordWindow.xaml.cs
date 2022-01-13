using BlApi;
using BO;
using System;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        IBL myBL;
        Customer customer;

        public ForgotPasswordWindow(Customer c)
        {
            InitializeComponent();
            myBL = BlFactory.GetBl();
            customer = c;
        }

        #region Close window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region OK button
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(answer.Password))
                return;

            if(answer.Password==customer.Answer)
            {
                new ClientWindow(customer).Show();
                Close();
            }
            else
                MessageBox.Show("WRONG ANSWER!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);

        }
        #endregion
    }
}
