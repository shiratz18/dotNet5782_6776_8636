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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL myBL;
        private bool grouped;

        #region Constructor
        /// <summary>
        /// Window constructor
        /// </summary>
        /// <param name="bl">Business logic paramater</param>
        public DroneListWindow(IBL bl)
        {
            myBL = bl;
            InitializeComponent();

            try
            {
                DronesListView.ItemsSource = myBL.GetDroneList();
            }
            catch (BO.EmptyListException) { }
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        #endregion

        #region Close window
        /// <summary>
        /// Closes this window
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
        /// Refreshes the list of drones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = myBL.GetDroneList();

            checkFilters();
        }
        #endregion

        #region Add drone
        /// <summary>
        /// Opens new DroneWindow in order to add a drone to the list, and updates DronesListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(myBL).ShowDialog(); //open add drone window

            checkFilters();
        }
        #endregion

        #region Edit drone
        /// <summary>
        /// Opens a new DroneWindow in order to update the selcted drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ListDrone ld = b.CommandParameter as ListDrone;
            Drone d = myBL.GetDrone(ld.Id);

            new DroneWindow(myBL, d).Show();

            checkFilters();
        }
        #endregion

        #region Remove drone
        /// <summary>
        /// Removes the selected drone from the list of drones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult ans = MessageBox.Show("Are you sure you want to delete this drone?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ans == MessageBoxResult.Yes)
            {
                Button b = sender as Button;
                ListDrone ld = b.CommandParameter as ListDrone;
                myBL.RemoveDrone(ld.Id);

         //       DronesListView.ItemsSource = myBL.GetDroneList();

                checkFilters();
            }
        }
        #endregion

        #region Filters and grouping

        #region Status filter
        /// <summary>
        /// Filters list of drones according to status selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null)
            {
                statusLabel.Content = "";

                checkFilters();
            }
        }

        /// <summary>
        /// clears status selction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearStatusSelection_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            statusLabel.Content = "- All Statuses -";

            checkFilters();
        }
        #endregion

        #region Weight filter
        /// <summary>
        /// Filters list of drones according to selected status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedItem != null)
            {
                weightLabel.Content = "";

                checkFilters();
            }
        }

        /// <summary>
        /// Clears the weight selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearWeightSelection_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;

            weightLabel.Content = "- All Weights -";

            checkFilters();
        }
        #endregion

        #region Group
        private void btnGroupByStatus_Click(object sender, RoutedEventArgs e)
        {
            grouped = true;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
        }
        #endregion

        /// <summary>
        /// Checks if any filters are applied and updates list accordingly
        /// </summary>
        private void checkFilters()
        {
            if (WeightSelector.SelectedItem != null)
            {
                if (StatusSelector.SelectedItem != null)
                    DronesListView.ItemsSource = myBL.GetDroneList((WeightCategories)WeightSelector.SelectedItem, (DroneStatuses)StatusSelector.SelectedItem);
                else
                    DronesListView.ItemsSource = myBL.GetDroneList((WeightCategories)WeightSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = myBL.GetDroneList(null, (DroneStatuses)StatusSelector.SelectedItem);
            else
                DronesListView.ItemsSource = myBL.GetDroneList();

            if(grouped)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        #endregion

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
