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
    /// Interaction logic for AdministratorPasswordWindow.xaml
    /// </summary>
    public partial class AdministratorPasswordWindow : Window
    {
        private IBL myBL;

        public AdministratorPasswordWindow(IBL bl)
        {
            myBL = bl;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            new AdministratorWindow(myBL).ShowDialog();
            Close();
        }
    }
}
