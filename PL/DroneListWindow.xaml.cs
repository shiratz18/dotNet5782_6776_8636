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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL.IBL myBL;

        /// <summary>
        /// Window constructor
        /// </summary>
        /// <param name="bl">Business logic paramater</param>
        public DroneListWindow(IBL.IBL bl)
        {
            myBL = bl;
            InitializeComponent();

            DronesListView.ItemsSource = myBL.GetDroneList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /// <summary>
        /// Filters list of drones according to status selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null)
            {
                DroneStatuses ds = (DroneStatuses)StatusSelector.SelectedItem;

                statusLabel.Content = "";

                //get all the drones with the selected status
                //if there are no drones, catch the exception and do nothing
                try
                {
                    IEnumerable<ListDrone> tmpList = myBL.GetDroneList(); //get all the drones

                    if (WeightSelector.SelectedItem != null) //if there is a selectes weight, get all the drones with that weight
                        tmpList = tmpList.Where(d => d.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);

                    DronesListView.ItemsSource = tmpList.Where(d => d.Status == ds); //get all the drones with the selected status
                }
                catch (IBL.BO.EmptyListException)
                { }
            }
        }

        /// <summary>
        /// Filters list of drones according to selected status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedItem != null)
            {
                WeightCategories wc = (WeightCategories)WeightSelector.SelectedItem;

                weightLabel.Content = "";

                //get all the drones with the selected weight category
                //if there are no drones, catch the exception and do nothing
                try
                {
                    IEnumerable<ListDrone> tmpList = myBL.GetDroneList(); //get all the drones

                    if (StatusSelector.SelectedItem != null) //if there is a selected status get all the drones with that status
                        tmpList = tmpList.Where(d => d.Status == (DroneStatuses)StatusSelector.SelectedItem);

                    DronesListView.ItemsSource = tmpList.Where(d => d.MaxWeight == wc); //get all the drones with the selected weight
                }
                catch (IBL.BO.EmptyListException)
                { }
            }
        }

        /// <summary>
        /// Opens new DroneWindow in order to add a drone to the list, and updates DronesListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(myBL).ShowDialog(); //open add drone window

            //update the list
            IEnumerable<ListDrone> tmpList = myBL.GetDroneList();
            if (WeightSelector.SelectedItem != null)
                tmpList = tmpList.Where(d => d.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);
            if (StatusSelector.SelectedItem != null)
                tmpList = tmpList.Where(d => d.Status == (DroneStatuses)StatusSelector.SelectedItem);
            DronesListView.ItemsSource = tmpList;
        }

        /// <summary>
        /// Opens a new DroneWindow in order to update the selcted drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IBL.BO.ListDrone tmp = (IBL.BO.ListDrone)DronesListView.SelectedItem;

            IBL.BO.Drone d = myBL.GetDrone(tmp.Id); //get the drone information from the selected drone

            new DroneWindow(myBL, d).ShowDialog(); //open action window

            IEnumerable<ListDrone> tmpList = myBL.GetDroneList();
            if (WeightSelector.SelectedItem != null)
                tmpList = tmpList.Where(d => d.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);
            if (StatusSelector.SelectedItem != null)
                tmpList = tmpList.Where(d => d.Status == (DroneStatuses)StatusSelector.SelectedItem);
            DronesListView.ItemsSource = tmpList;
        }

        /// <summary>
        /// Closes this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Drags the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// Clears the weight selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearWeightSelection_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;

            IEnumerable<ListDrone> tmpList = myBL.GetDroneList();
            if (StatusSelector.SelectedItem != null)
                tmpList = tmpList.Where(d => d.Status == (DroneStatuses)StatusSelector.SelectedItem);
            DronesListView.ItemsSource = tmpList;

            weightLabel.Content = "Weight";
        }

        /// <summary>
        /// clears status selction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearStatusSelection_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;

            IEnumerable<ListDrone> tmpList = myBL.GetDroneList();
            if (WeightSelector.SelectedItem != null)
                tmpList = tmpList.Where(d => d.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);
            DronesListView.ItemsSource = tmpList;

            statusLabel.Content = "Status";
        }

        /// <summary>
        /// clears all selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAllSelections_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;
            StatusSelector.SelectedItem = null;

            DronesListView.ItemsSource = myBL.GetDroneList();

            weightLabel.Content = "Weight";
            statusLabel.Content = "Status";
        }
    }
}
