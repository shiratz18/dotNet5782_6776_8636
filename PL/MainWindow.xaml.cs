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
            new AdministratorPasswordWindow(this).Show();
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            new SignUpWindow().Show();
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            new ClientPasswordWindow().Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
