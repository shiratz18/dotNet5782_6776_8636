using System.Windows;
using System.Windows.Controls;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        private IBL myBL;

        private static CustomerListWindow instance = null;
        public static CustomerListWindow Instance
        {
            get
            {
                if (instance == null)
                    instance = new CustomerListWindow();
                return instance;
            }
        }

        #region Constructor
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="bl"></param>
        private CustomerListWindow()
        {
            myBL = BlFactory.GetBl();
            InitializeComponent();
            try
            {
                CustomersListView.ItemsSource = myBL.GetCustomerList();
            }
            catch (BO.EmptyListException) { }
        }
        #endregion

        #region Close window
        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Refresh
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            CustomersListView.ItemsSource = myBL.GetCustomerList();
        }
        #endregion

        #region Add customer
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow().ShowDialog();

            CustomersListView.ItemsSource = myBL.GetCustomerList();
        }
        #endregion

        #region Edit button
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ListCustomer lc = b.CommandParameter as ListCustomer;
            Customer c = myBL.GetCustomer(lc.Id);

            new CustomerWindow(c).ShowDialog();
            CustomersListView.ItemsSource = myBL.GetCustomerList();
        }
        #endregion

        #region Remove customer
        /// <summary>
        /// Removes a customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult ans = MessageBox.Show("Are you sure you want to delete this customer?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ans == MessageBoxResult.Yes)
            {
                Button b = sender as Button;
                ListCustomer lp = b.CommandParameter as ListCustomer;
                try
                {
                    myBL.RemoveCustomer(lp.Id);

                    CustomersListView.ItemsSource = myBL.GetCustomerList();
                }
                catch (CannotDeleteException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion
    }
}
