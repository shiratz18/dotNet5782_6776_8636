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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.IBL myBL;
        IBL.BO.Drone drone;
        bool closingFlag;

        /// <summary>
        /// Constructor for add grid
        /// </summary>
        /// <param name="bl"></param>
        public DroneWindow(IBL.IBL bl)
        {
            myBL = bl;
            closingFlag = false;

            InitializeComponent();
            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid

            droneMaxWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /// <summary>
        /// Constructoe for action grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="d"></param>
        public DroneWindow(IBL.IBL bl, IBL.BO.Drone d)
        {
            myBL = bl;
            closingFlag = false;

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible

            drone = d;
            display();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!closingFlag)
            {
                MessageBoxResult mbResult = MessageBox.Show("Close the program?", "Close",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Cancel);

                if (mbResult == MessageBoxResult.No)
                    e.Cancel = true;
                else
                {
                    e.Cancel = false;
                    this.Close();
                }
            }
        }

        private void droneId_TextChanged(object sender, TextChangedEventArgs e) { }

        private void droneModel_TextChanged(object sender, TextChangedEventArgs e) { }

        private void droneMaxWeight_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void stationId_TextChanged(object sender, TextChangedEventArgs e) { }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //get the data the user entered
            drone = new Drone();
            int.TryParse(droneId.Text, out int id);
            drone.Id = id;
            string model = droneModel.Text;
            drone.Model = model;
            drone.MaxWeight = (WeightCategories)droneMaxWeight.SelectedItem;
            int.TryParse(stationId.Text, out id);

            MessageBoxResult mb = default;
            try
            {
                myBL.AddDrone(drone, id);
            }
            catch (InvalidNumberException ex)
            {
                mb = MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (NoAvailableChargeSlotsException ex)
            {
                mb = MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (DoubleIDException ex)
            {
                mb = MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }

            if (mb == MessageBoxResult.OK) //if the user clicked of on message box, try again to add the drone
            {
                this.btnOK_Click(sender, e);
            }

            closingFlag = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            closingFlag = true;
            Close();
        }

        private void droneId_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
                tb.Text = "";
        }

        private void display()
        {
            idToPrint.Content = drone.Id;
            modelToPrint.Text = drone.Model;
            maxWeightToPrint.Content = drone.MaxWeight;
            batteryToPrint.Content = String.Format("{0:0.00}", drone.Battery);
            statusToPrint.Content = drone.Status;
            locationToPrint.Content = drone.CurrentLocation;

            if (drone.Status != DroneStatuses.Shipping) //if the drone is not shipping print no parcel
                parcelToPrint.Content = "(no parcel)";
            else
                parcelToPrint.Content = drone.InShipping.Id; //if the drone is shipping print the parcel id

            if (drone.Status != DroneStatuses.Available) //if the drone is not available do not shoe charge button
            {
                btnCharge.Visibility = Visibility.Hidden;
                btnDroneToDelivery.Visibility = Visibility.Hidden;
            }

            if (drone.Status != DroneStatuses.Maintenance)
            {
                btnReleaseCharge.Visibility = Visibility.Hidden; //if the drone is not in maintenace do not show release charge button
            }

            if (drone.Status != DroneStatuses.Shipping || drone.InShipping.IsPickedUp == true) //if the drone has picked up the package or it is not in shipping, hide the button of pick up
                btnDronePickUp.Visibility = Visibility.Hidden;

            if (drone.Status != DroneStatuses.Shipping || drone.InShipping.IsPickedUp == false) //if the drone has not picked up the package or it is not in shipping, hide the button of pick up
                btnDroneDeliver.Visibility = Visibility.Hidden;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            closingFlag = true;
            Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string newModel = modelToPrint.Text;
            try
            {
                myBL.UpdateDroneName(drone.Id, newModel);
            }
            catch (IBL.BO.NoIDException ex)
            {

            }
        }

        private void btnReleaseCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.ReleaseDroneCharge(drone.Id);
                drone = myBL.GetDrone(drone.Id);
                batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery);
                statusToPrint.Content = drone.Status;
                btnReleaseCharge.Visibility = Visibility.Hidden;
                btnCharge.Visibility = Visibility.Visible;
                btnDroneToDelivery.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCharge_Click(object sender, RoutedEventArgs e)
        {
            myBL.ChargeDrone(drone.Id);
            drone = myBL.GetDrone(drone.Id);
            statusToPrint.Content = drone.Status;
            btnCharge.Visibility = Visibility.Hidden;
            btnReleaseCharge.Visibility = Visibility.Visible;
            btnDroneToDelivery.Visibility = Visibility.Hidden;
        }

        private void btnDroneToDelivery_Click(object sender, RoutedEventArgs e)
        {
            myBL.AssignDroneToParcel(drone.Id);
            drone = myBL.GetDrone(drone.Id);
            parcelToPrint.Content = drone.InShipping.Id;
        }

        private void btnDronePickUp_Click(object sender, RoutedEventArgs e)
        {
            myBL.DronePickUp(drone.Id);
            drone = myBL.GetDrone(drone.Id);
            batteryToPrint.Content = drone.Battery;
            btnDronePickUp.Visibility = Visibility.Hidden;
            btnDroneDeliver.Visibility = Visibility.Visible;
        }

        private void btnDroneDeliver_Click(object sender, RoutedEventArgs e)
        {
            myBL.DroneDeliver(drone.Id);
            drone = myBL.GetDrone(drone.Id);
            btnDroneDeliver.Visibility = Visibility.Hidden;
            statusToPrint.Content = drone.Status;
            btnDroneToDelivery.Visibility = Visibility.Visible;
            btnCharge.Visibility = Visibility.Visible;
        }
    }
}

