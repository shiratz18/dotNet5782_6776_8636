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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private IBL myBL;
        private Parcel parcel;

        /// <summary>
        /// Constructor for add grid
        /// </summary>
        /// <param name="bl"></param>
        public ParcelWindow(IBL bl)
        {
            myBL = bl;

            InitializeComponent();

            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid
            this.Title = "New drone"; //change the title

            parcelWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            parcelPriority.ItemsSource = Enum.GetValues(typeof(Priorities));

            //droneStation.ItemsSource = myBL.GetStationNameList();
        }
        /// <summary>
        /// Constructor for action grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="d"></param>
        public ParcelWindow(IBL bl, Parcel p)
        {
            myBL = bl;

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible
            this.Title = "Update parcel"; //change the title
            parcel = p;
            DataContext = parcel;

            display();
        }



        /// <summary>
        /// Text changed event for SenderId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelSenderId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(parcelSenderId.Text, out int num);
            if (flag && num < 1000) //if the id the user entered is less than 4 digits the border is red
            {
                parcelSenderId.BorderBrush = Brushes.Red;
                RedMes1.Content = "Incorrect entry, please try again";
            }
            else
            {
                parcelSenderId.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes1 != null)
                    RedMes1.Content = "";
            }
            setOkButton();
        }

        /// <summary>
        /// Removes the current text from parcelSenderId text box, occurs only once and then removed parcel events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void idSenderTbGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = ""; //changing the text to be empty
            tb.GotFocus -= idSenderTbGotFocus;
        }

        /// <summary>
        /// Text changed event for TargetId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelTargetId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(parcelTargetId.Text, out int num);
            if (flag && num < 1000) //if the id the user entered is less than 4 digits the border is red
            {
                parcelTargetId.BorderBrush = Brushes.Red;
                RedMes2.Content = "Incorrect entry, please try again";
            }
            else
            {
                parcelTargetId.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes2 != null)
                    RedMes2.Content = "";
            }
            setOkButton();
        }

        /// <summary>
        /// Removes the current text from parcelTargetId text box, occurs only once and then removed parcel events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void idTargetTbGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = ""; //changing the text to be empty
            tb.GotFocus -= idTargetTbGotFocus;
        }

        /// <summary>
        /// Sets parcelSenderId and parcelTargetId text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); //allow only numbers in the text box
        }

        /// <summary>
        /// parcelWeight combobox selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (parcelWeight.SelectedItem != null)
                WeightLbl.Content = "";
            setOkButton();
        }
        
        /// <summary>
        /// parcelPriority combobox selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelPriority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (parcelPriority.SelectedItem != null)
                parcelPriorityLbl.Content = "";
            setOkButton();
        }

        /// <summary>
        /// Enables OK button only when all fields are filled
        /// </summary>
        private void setOkButton()
        {
            //enable OK button only if all fields were filled
            if (btnOK != null)
                btnOK.IsEnabled = (!String.IsNullOrEmpty(parcelSenderId.Text) && parcelSenderId.Text != "Enter sender ID here") &&
                    (!String.IsNullOrEmpty(parcelTargetId.Text) && parcelTargetId.Text != "Enter target ID here") &&
                    (parcelWeight.SelectedItem != null) &&
                    (parcelPriority.SelectedItem != null);
        }

        /// <summary>
        /// Adds parcel with user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //get the data the user entered
            parcel = new Parcel();
            bool flag1 = int.TryParse(parcelSenderId.Text, out int id);
            parcel.Sender = id;
            bool flag2 = int.TryParse(parcelSenderId.Text, out int id);
            parcel.Target = id;

            parcel.Weight = (WeightCategories)parcelWeight.SelectedItem;
            parcel.Priority= (Priorities)parcelPriority.SelectedItem;

            MessageBoxResult mb = default;
            try
            {
                myBL.AddParcel(parcel);
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
            MessageBox.Show("parcel successfully added", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        
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
        /// Display the chosen drone and update button options according to drone status
        /// </summary>
        private void display()
        {
            RedMes3.Content = " ";
            //idToPrint.Content = drone.Id;
            //modelToPrint.Text = drone.Model;
            //maxWeightToPrint.Content = drone.MaxWeight;
            //batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery);
            //statusToPrint.Content = drone.Status;
            //locationToPrint.Content = drone.CurrentLocation;
            if (drone.Status == DroneStatuses.Shipping)
            {
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











        /// <summary>
        /// Allows to drag the window (because there is no title to drag from)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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

    }
}
