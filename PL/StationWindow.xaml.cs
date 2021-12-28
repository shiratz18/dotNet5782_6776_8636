using System;
using System.Collections.Generic;
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
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        private IBL myBL;
        private Station station;

        /// <summary>
        /// Constructor for add grid
        /// </summary>
        /// <param name="bl"></param>
        public StationWindow(IBL bl)
        {
            myBL = bl;
            InitializeComponent();
            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid
            this.Title = "New station"; //change the title

        }
       
        /// <summary>
        /// Constructor for action grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="d"></param>
        public StationWindow(IBL bl, Station s)
        {
            myBL = bl;

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible
            this.Title = "Update station"; //change the title
            station = s;
            DataContext = station;

            //display();
        }
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
                stationId.BorderBrush = Brushes.Red;
                RedMes1.Content = "Incorrect entry, please try again";
            }
            else
            {
                stationId.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes1 != null)
                    RedMes1.Content = "";
            }
            setOkButton();
        }

        /// <summary>
        /// Sets stationId text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); //allow only numbers in the text box
        }

        /// <summary>
        /// Removes the current text from stationId text box, occurs only once and then removed drom events
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
        /// Text changed event for stationName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stationName_TextChanged(object sender, TextChangedEventArgs e)
        {
            setOkButton();
        }

        /// <summary>
        ///  Removes the current text from stationName text box, occurs only once and then removed drom events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameTbGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = ""; //changing the text to be empty
            tb.GotFocus -= nameTbGotFocus;
        }
        /// <summary>
        /// Text changed event for ChargeSlotsi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeSlots_TextChanged(object sender, TextChangedEventArgs e)
        {
            setOkButton();
        }

        /// <summary>
        /// Removes the current text from ChargeSlots text box, occurs only once and then removed drom events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeSlotsTbGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = ""; //changing the text to be empty
            tb.GotFocus -= ChargeSlotsTbGotFocus;
        }

        /// <summary>
        /// Enables OK button only when all fields are filled
        /// </summary>
        private void setOkButton()
        {
            //enable OK button only if all fields were filled
            if (btnOK != null)
                btnOK.IsEnabled = (!String.IsNullOrEmpty(stationId.Text) && stationId.Text != "Enter ID here") &&
                             (!String.IsNullOrEmpty(stationName.Text) && stationName.Text != "Enter name here") &&
                            (!String.IsNullOrEmpty(ChargeSlots.Text) && ChargeSlots.Text != "Enter no. of chargers");


        }

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
            }
            catch (InvalidNumberException ex)
            {
                mb = MessageBox.Show($"{ex.Message} Retry?", "ERROR", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            catch (WrongFormatException ex)
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
            MessageBox.Show("Station successfully added", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }

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
        /// Update the station name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string newName = nameToPrint.Text;
            try
            {
                myBL.UpdateStationName(station.Id, newName); //update the drone model
                station.Name = newName; //update also the current drone

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

        private void chargeSlotsToPrint_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void nameToPrint_TextChanged(object sender, TextChangedEventArgs e)
        {

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
