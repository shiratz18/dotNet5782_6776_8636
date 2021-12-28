using BlApi;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL myBL;

        public MainWindow()
        {
            myBL = BlFactory.GetBl();
            InitializeComponent();
        }

        private void btnWorker_Click(object sender, RoutedEventArgs e)
        {
            new AdministratorPasswordWindow(myBL).Show();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            new SignUpWindow(myBL).Show();
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            new ClientPasswordWindow(myBL).Show();
        }
    }
}
