﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private IBL.IBL myBL;
        private IBL.BO.Drone drone;
        private bool enableOkBtn;

        /// <summary>
        /// Constructor for add grid
        /// </summary>
        /// <param name="bl"></param>
        public DroneWindow(IBL.IBL bl)
        {
            myBL = bl;

            InitializeComponent();

            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid
            this.Title = "New drone"; //change the title

            droneMaxWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            droneStation.ItemsSource = myBL.GetStationNameList();
        }

        /// <summary>
        /// Constructor for action grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="d"></param>
        public DroneWindow(IBL.IBL bl, IBL.BO.Drone d)
        {
            myBL = bl;

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible
            this.Title = "Update drone"; //change the title

            drone = d;
            display();
        }

        /// <summary>
        /// Text changed event for droenId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneId_TextChanged(object sender, TextChangedEventArgs e)
        {
            setOkButton();
        }

        /// <summary>
        /// Sets droneId text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); //allow only numbers in the text box
        }

        /// <summary>
        /// Removes the current text from droneId text box, occurs only once and then removed drom events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void idTbGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = ""; //changing the text to be empty
            tb.GotFocus -= idTbGotFocus;
        }

        /// <summary>
        /// Text changed event for droneModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            setOkButton();
        }

        /// <summary>
        ///  Removes the current text from droneModel text box, occurs only once and then removed drom events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelTbGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = ""; //changing the text to be empty
            tb.GotFocus -= idTbGotFocus;
        }

        /// <summary>
        /// droneMaxWeight combobox selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneMaxWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setOkButton();
        }

        /// <summary>
        /// droneStation combobox selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setOkButton();
        }

        private void setOkButton()
        {
            //enable OK button only if all fields were filled
            if (btnOK != null)
                btnOK.IsEnabled = (!String.IsNullOrEmpty(droneId.Text) && droneId.Text != "Enter ID here") &&
                    (!String.IsNullOrEmpty(droneModel.Text) && droneModel.Text != "Enter model here") &&
                    (droneMaxWeight.SelectedItem != null) &&
                    (droneStation.SelectedItem != null);
        }

        /// <summary>
        /// Adds drone with user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //get the data the user entered
            drone = new Drone();
            bool flag1 = int.TryParse(droneId.Text, out int id);
            drone.Id = id;
            string model = droneModel.Text;
            drone.Model = model;
            drone.MaxWeight = (WeightCategories)droneMaxWeight.SelectedItem;
            string name = (string)droneStation.SelectedItem;
            ListStation station = myBL.GetStationList().First(s => s.Name == name);

            MessageBoxResult mb = default;
            try
            {
                myBL.AddDrone(drone, station.Id);
            }
            catch (InvalidNumberException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (NoAvailableChargeSlotsException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (DoubleIDException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }

            if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
            {
                Close();
            }
            if (mb == MessageBoxResult.OK) //if the user clicked ok in message box, try again to add the drone
            {
                return;
            }

            Close();
            MessageBox.Show("Drone successfully added", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Closes this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Display the chosen drone and update button options according to drone status
        /// </summary>
        private void display()
        {
            idToPrint.Content = drone.Id;
            modelToPrint.Text = drone.Model;
            maxWeightToPrint.Content = drone.MaxWeight;
            batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery);
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

        /// <summary>
        /// Close this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Update the model name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string newModel = modelToPrint.Text;
            try
            {
                myBL.UpdateDroneName(drone.Id, newModel); //update the drone model
                drone.Model = newModel; //update also the current drone

                MessageBox.Show("Drone model updated successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (IBL.BO.NoIDException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Release the chosen drone from charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReleaseCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.ReleaseDroneCharge(drone.Id); //release the drone from charging
                drone = myBL.GetDrone(drone.Id); //get the updated drone from the bl

                batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery); //update the battery content according to new battery
                statusToPrint.Content = drone.Status; //update the status content according to status

                btnReleaseCharge.Visibility = Visibility.Hidden; //hide the release charge button
                btnCharge.Visibility = Visibility.Visible; //make the charge button visible, be cause now the drone is available
                btnDroneToDelivery.Visibility = Visibility.Visible; //make the drone to dleivery button visilble, because now th drone is available

                MessageBox.Show("Drone released from charge", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Charge the chosen drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.ChargeDrone(drone.Id); //charge the drone
                drone = myBL.GetDrone(drone.Id); //get the updates drone form bl

                statusToPrint.Content = drone.Status; //change status content according to new status
                batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery); //change battery content according to new battery

                btnCharge.Visibility = Visibility.Hidden; //charge button is hidden because drone id charging
                btnReleaseCharge.Visibility = Visibility.Visible; //release charge button is visible
                btnDroneToDelivery.Visibility = Visibility.Hidden; //drone to delivery button is hidden because drone is unavailable

                MessageBox.Show("Drone sent to charge", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Send drone to delivery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDroneToDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.AssignDroneToParcel(drone.Id); //assign a parcel to the drone
                drone = myBL.GetDrone(drone.Id); //get updated drone

                parcelToPrint.Content = drone.InShipping.Id;
                batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery);

                btnCharge.Visibility = Visibility.Hidden;
                btnDroneToDelivery.Visibility = Visibility.Hidden;
                btnDronePickUp.Visibility = Visibility.Visible;
                MessageBox.Show("Drone assigned to parcel", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Button updates drone to pick up the package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDronePickUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.DronePickUp(drone.Id);
                drone = myBL.GetDrone(drone.Id);

                batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery);

                btnDronePickUp.Visibility = Visibility.Hidden;
                btnDroneDeliver.Visibility = Visibility.Visible;
                MessageBox.Show("Drone picked up parcel", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Button updates drone to deliver the package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDroneDeliver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBL.DroneDeliver(drone.Id);
                drone = myBL.GetDrone(drone.Id);
                statusToPrint.Content = drone.Status;
                batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery);

                btnDroneDeliver.Visibility = Visibility.Hidden;
                btnDroneToDelivery.Visibility = Visibility.Visible;
                btnCharge.Visibility = Visibility.Visible;

                MessageBox.Show("Drone delivered parcel", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Allows to drag the window (because there is no title to drag from)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

