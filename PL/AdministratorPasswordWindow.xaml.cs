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
    public partial class AdministratorPasswordWindow : Window
    {
        private IBL myBL;
        private MainWindow main;

        public AdministratorPasswordWindow(IBL bl, MainWindow w)
        {
            myBL = bl;
            main = w;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (password.Password == myBL.GetAccessCode())
            {
                new AdministratorWindow(myBL, main).Show();
                Close();
                main.WindowState = WindowState.Minimized;
            }
            else
            {
                MessageBox.Show("Wrong code.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
