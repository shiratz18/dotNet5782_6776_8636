using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL myBL;
        private Drone drone;

        #region Add drone grid

        #region Constructor
        /// <summary>
        /// Constructor for add grid
        /// </summary>
        /// <param name="bl"></param>
        public DroneWindow(IBL bl)
        {
            myBL = bl;

            InitializeComponent();

            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid
            this.Title = "New drone"; //change the title

            droneMaxWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            droneStation.ItemsSource = myBL.GetStationNameList();
        }
        #endregion

        #region ID
        /// <summary>
        /// Text changed event for droenId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(droneId.Text, out int num);
            if (flag && num < 1000) //if the id the user entered is less than 4 digits the border is red
            {
                droneId.BorderBrush = Brushes.Red;
                RedMes1.Content = "Incorrect entry, please try again";
            }
            else
            {
                droneId.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes1 != null)
                    RedMes1.Content = "";
            }
            setOkButton();
        }

        /// <summary>
        /// Sets droneId text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numbersOnly(object sender, TextCompositionEventArgs e)
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
        #endregion

        #region Model
        /// <summary>
        /// Text changed event for droneModel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            string m = droneModel.Text;
            if (!String.IsNullOrEmpty(m) && m.Length < 5)//if the model the user entered is less than 5 characters the border is red
            {
                droneModel.BorderBrush = Brushes.Red;
                RedMes2.Content = "Incorrect entry, please try again";
            }
            else
            {
                droneModel.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes2 != null)
                    RedMes2.Content = "";
            }
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
            tb.GotFocus -= modelTbGotFocus;
        }
        #endregion

        #region Weight
        /// <summary>
        /// droneMaxWeight combobox selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneMaxWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (droneMaxWeight.SelectedItem != null)
                maxWeightLbl.Content = "";
            setOkButton();
        }
        #endregion

        #region Station
        /// <summary>
        /// droneStation combobox selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (droneStation.SelectedItem != null)
                stationLbl.Content = "";
            setOkButton();
        }
        #endregion

        #region OK button
        /// <summary>
        /// Enables OK button only when all fields are filled
        /// </summary>
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

                if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
                {
                    Close();
                }
            }
            catch (WrongFormatException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
                {
                    Close();
                }
            }
            catch (NoAvailableChargeSlotsException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
                {
                    Close();
                }
            }
            catch (DoubleIDException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
                {
                    Close();
                }
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
        #endregion

        #region Close window
        /// <summary>
        /// Closes this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var mb = MessageBox.Show("Are you sure you want to cancel?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (mb == MessageBoxResult.Yes)
            {
                Close();
            }
        }
        #endregion

        #endregion

        #region Action drone

        #region Constructor
        /// <summary>
        /// Constructor for action grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="d"></param>
        public DroneWindow(IBL bl, Drone d)
        {
            myBL = bl;

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible
            this.Title = "Update drone"; //change the title
            drone = d;
            DataContext = drone;

            display();
        }

        //<summary>
        //Display the chosen drone and update button options according to drone status
        // </summary>
        private void display()
        {
            RedMes3.Content = " ";
            if (drone.Status == DroneStatuses.Shipping)
            {
                parcelExpander.IsExpanded = true;
                parcelExpander.IsEnabled = true;
            }
            else
            {
                parcelExpander.IsEnabled = false;
                parcelExpander.IsExpanded = false;
            }

            if (drone.Status != DroneStatuses.Available) //if the drone is not available do not shoe charge button
            {
                btnCharge.Visibility = Visibility.Hidden;
                btnDroneToDelivery.Visibility = Visibility.Hidden;
            }

            if (drone.Status != DroneStatuses.Maintenance)
            {
                btnReleaseCharge.Visibility = Visibility.Hidden; //if the drone is not in maintenace do not show release charge button
            }

            if (drone.Status != DroneStatuses.Shipping)
            {
                btnDronePickUp.Visibility = Visibility.Hidden;
                btnDroneDeliver.Visibility = Visibility.Hidden;
            }

            if (drone.InShipping.IsPickedUp)
                btnDronePickUp.Visibility = Visibility.Hidden;

            if (!drone.InShipping.IsPickedUp)
                btnDroneDeliver.Visibility = Visibility.Hidden;

        }
        #endregion

        #region Close window
        /// <summary>
        /// Close this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Update
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
            catch (WrongFormatException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NoIDException ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Model
        private void modelToPrint_TextChanged(object sender, TextChangedEventArgs e)
        {
            string m = modelToPrint.Text;
            if (!String.IsNullOrEmpty(m) && m.Length < 5)//if the model the user entered is less than 5 characters the border is red
            {
                modelToPrint.SelectionBrush = Brushes.Red;
                if (RedMes3 != null)
                    RedMes3.Content = "Incorrect entry, please try again";
            }
            else
            {
                modelToPrint.SelectionBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes3 != null)
                    RedMes3.Content = "";
            }
        }
        #endregion

        #region Release charge
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
        #endregion

        #region Charge
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
        #endregion

        #region Parcel delivery
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

                parcelExpander.IsExpanded = true;
                parcelExpander.IsEnabled = true;
                parcelId.Content = drone.InShipping.Id;
                isPickedUp.Content = drone.InShipping.IsPickedUp;
                priority.Content = drone.InShipping.Priority;
                weight.Content = drone.InShipping.Weight;
                senderName.Content = drone.InShipping.Sender.Name;
                targetName.Content = drone.InShipping.Target.Name;
                pickUpLocation.Content = drone.InShipping.PickUpLocation;
                deliveryLocation.Content = drone.InShipping.DeliveryLocation;
                deliveryDistance.Content = drone.InShipping.DeliveryDistance;

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

                isPickedUp.Content = drone.InShipping.IsPickedUp;
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

                parcelExpander.IsEnabled = false;
                parcelExpander.IsExpanded = false;

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
        #endregion

        
        private void parcelExpander_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Parcel p = myBL.GetParcel(drone.InShipping.Id);
            new ParcelWindow(myBL, p).ShowDialog();
        }

        private void autoMode_Checked(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        //private void autoMode_Checked(object sender, RoutedEventArgs e)
        //{
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.WorkerReportsProgress = true;
        //    worker.DoWork += autoMode_DoWork;
        //    worker.ProgressChanged += autoMode_ProgressChanged;

        //    worker.RunWorkerAsync();
        //}



        //private void autoMode_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    myBL.ActivateSimulator(drone.Id,)
        //}

        //private void autoMode_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    drone = e.
        //}
    }
}

