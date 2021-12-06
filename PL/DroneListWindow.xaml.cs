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
        IBL.IBL myBL;

        /// <summary>
        /// Window constructor
        /// </summary>
        /// <param name="bl">Business logic paramater</param>
        public DroneListWindow(IBL.IBL bl)
        {
            myBL = bl;
            InitializeComponent();
            DronesListView.ItemsSource = bl.GetDroneList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatuses ds = (DroneStatuses)StatusSelector.SelectedItem;

            //get all the drones with the selected status
            //if there are no drones, catch the exception and do nothing
            try
            {
                this.DronesListView.ItemsSource = myBL.GetDroneList().Where(d => d.Status == ds);
            }
            catch (IBL.BO.EmptyListException)
            {

            }
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories wc = (WeightCategories)WeightSelector.SelectedItem;

            //get all the drones with the selected weight category
            //if there are no drones, catch the exception and do nothing
            try
            {
                this.DronesListView.ItemsSource = myBL.GetDroneList().Where(d => d.MaxWeight == wc);
            }
            catch (IBL.BO.EmptyListException)
            {

            }
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(myBL).ShowDialog(); //open add drone window
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IBL.BO.ListDrone tmp = (IBL.BO.ListDrone)DronesListView.SelectedItem;

            IBL.BO.Drone d = myBL.GetDrone(tmp.Id); //get the drone information from the selected drone
            
            new DroneWindow(myBL, d).ShowDialog(); //open action window
        }
    }
}
