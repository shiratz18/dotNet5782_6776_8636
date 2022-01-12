using BlApi;
using System.Windows;

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
            new AdministratorPasswordWindow(myBL, this).Show();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            new SignUpWindow(myBL).Show();
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            new ClientPasswordWindow(myBL).Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
