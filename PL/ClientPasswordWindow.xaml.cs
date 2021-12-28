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
            new ClientWindow(myBL).ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
