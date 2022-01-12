using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        private IBL myBL;
        private bool isGrouped;

        #region Constructor
        public StationListWindow(IBL bl)
        {
            myBL = bl;
            InitializeComponent();
            try
            {
                StationsListView.ItemsSource = myBL.GetStationList();
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

        #region Minimize window
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region Refresh
        /// <summary>
        /// Refreshes the list to the updates list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            StationsListView.ItemsSource = myBL.GetStationList();

            if (isGrouped)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableChargeSlots");
                view.GroupDescriptions.Add(groupDescription);
            }
        }
        #endregion

        #region Add station
        /// <summary>
        /// Adds a station to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(myBL).ShowDialog();

            StationsListView.ItemsSource = myBL.GetStationList();

            if (isGrouped)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableChargeSlots");
                view.GroupDescriptions.Add(groupDescription);
            }
        }
        #endregion

        #region Edit station
        /// <summary>
        /// Opens a station windows to update the selected station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ListStation ls = b.CommandParameter as ListStation;
            Station s = myBL.GetStation(ls.Id);
            new StationWindow(myBL, s).ShowDialog();

            StationsListView.ItemsSource = myBL.GetStationList();

            if (isGrouped)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableChargeSlots");
                view.GroupDescriptions.Add(groupDescription);
            }
        }
        #endregion

        #region Remove station
        /// <summary>
        /// Removes a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult ans = MessageBox.Show("Are you sure you want to delete this station?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ans == MessageBoxResult.Yes)
            {
                Button b = sender as Button;
                ListStation ls = b.CommandParameter as ListStation;
                try
                {
                    myBL.RemoveStation(ls.Id);

                    StationsListView.ItemsSource = myBL.GetStationList();

                    if (isGrouped)
                    {
                        CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
                        PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableChargeSlots");
                        view.GroupDescriptions.Add(groupDescription);
                    }
                }
                catch (CannotDeleteException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region Group
        /// <summary>
        /// Groups the list by number of available chargers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupByNumber_Click(object sender, RoutedEventArgs e)
        {
            isGrouped = true;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableChargeSlots");
            view.GroupDescriptions.Add(groupDescription);
        }
        #endregion    
    }
}
