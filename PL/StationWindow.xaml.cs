using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        private IBL myBL;
        private Station station;

        #region Add

        #region Constructor
        /// <summary>
        /// Constructor for add grid
        /// </summary>
        /// <param name="bl"></param>
        public StationWindow()
        {
            myBL = BlFactory.GetBl();
            InitializeComponent();
            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid
            this.Title = "New station"; //change the title

        }
        #endregion

        #region Close
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

        #region ID
        /// <summary>
        /// Text changed event for stationId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stationId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(stationId.Text, out int num);
            if (flag && num < 1000) //if the id the user entered is less than 4 digits the border is red
            {
                RedMes1.Content = "Incorrect entry, please try again";
            }
            else
            {
                if (RedMes1 != null)
                    RedMes1.Content = "";
            }
        }
        #endregion

        #region Ok button
        /// <summary>
        /// Adds station with user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //get the data the user entered
            station = new Station();
            bool flag1 = int.TryParse(stationId.Text, out int id);
            station.Id = id;

            string name = stationName.Text;
            station.Name = name;
            station.Location = new Location()
            {
                Longitude = SliderLongitude.Value,
                Latitude = SliderLatitude.Value
            };

            bool flag2 = int.TryParse(ChargeSlots.Text, out int charges);
            station.AvailableChargeSlots = charges;

            station.Location.Longitude = SliderLongitude.Value;
            station.Location.Latitude = SliderLatitude.Value;

            MessageBoxResult mb = default;
            try
            {
                myBL.AddStation(station);
                Close();
                MessageBox.Show("Station successfully added", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidNumberException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (mb == MessageBoxResult.Cancel)
                    Close();
            }
            catch (WrongFormatException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (mb == MessageBoxResult.Cancel)
                    Close();
            }
            catch (NoAvailableChargeSlotsException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (mb == MessageBoxResult.Cancel)
                    Close();
            }
            catch (DoubleIDException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (mb == MessageBoxResult.Cancel)
                    Close();
            }

            if (mb == MessageBoxResult.Cancel) //if there was an error and the user clicked cancel, close the window
            {
                Close();
            }
            if (mb == MessageBoxResult.OK) //if the user clicked ok in message box, try again to add the drone
            {
                return;
            }
        }
        #endregion

        #endregion

        #region Update
        #region Constructor
        /// <summary>
        /// Constructor for action grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="d"></param>
        public StationWindow(Station s)
        {
            myBL = BlFactory.GetBl();

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible
            this.Title = "Update station"; //change the title
            station = s;
            DataContext = station;
            ChargingDronesListView.ItemsSource = station.ChargingDrones;
            DataContext = station;
        }
        #endregion

        #region Close
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

        #region Minimize window
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        #region Update name
        /// <summary>
        /// Update the station name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateNAME_Click(object sender, RoutedEventArgs e)
        {
            string newName = nameToPrint.Text;
            try
            {
                myBL.UpdateStationName(station.Id, newName); //update the drone model
                station.Name = newName; //update also the current station

                MessageBox.Show("Station name updated successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
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

        #region Update charge slots
        /// <summary>
        /// Update the number of charge slots in a station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateCHARGESLOTS_Click(object sender, RoutedEventArgs e)
        {

            int newCHARGE = int.Parse(chargeSlotsToPrint.Text);
            try
            {
                myBL.UpdateStationChargingSlots(station.Id, newCHARGE); //update the charge slots

                MessageBox.Show("Station charge slots updated successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
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

        #region Open drone
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ChargingDrone ld = b.CommandParameter as ChargingDrone;

            Drone d = myBL.GetDrone(ld.Id);
            new DroneWindow(d).Show();
        }
        #endregion
        #endregion

        /// <summary>
        /// Sets text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); //allow only numbers in the text box
        }
    }
}
