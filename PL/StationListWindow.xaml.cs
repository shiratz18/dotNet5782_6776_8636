using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        #region Edit drone
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
            // new StationWindow(myBL, b).ShowDialog();

            StationsListView.ItemsSource = myBL.GetStationList();

            if (isGrouped)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableChargeSlots");
                view.GroupDescriptions.Add(groupDescription);
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
