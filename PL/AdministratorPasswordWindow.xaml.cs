using System.Windows;
using BlApi;

namespace PL
{
    public partial class AdministratorPasswordWindow : Window
    {
        private IBL myBL;
        private MainWindow main;

        public AdministratorPasswordWindow(MainWindow w)
        {
            myBL = BlFactory.GetBl();
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
                new AdministratorWindow(main).Show();
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
