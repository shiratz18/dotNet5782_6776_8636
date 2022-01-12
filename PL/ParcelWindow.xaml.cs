using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        #region Add

        #region Constructor
        /// <summary>
        /// Constructor for add grid
        /// </summary>
        /// <param name="bl"></param>
        public ParcelWindow(IBL bl)
        {
            myBL = bl;

            InitializeComponent();

            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid
            this.Title = "New parcel"; //change the title

            parcelWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            parcelPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
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

        #region Sender ID
        /// <summary>
        /// Text changed event for SenderId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelSenderId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(parcelSenderId.Text, out int num);
            if (flag && num < 100000000) //if the id the user entered is less than 4 digits the border is red
            {
                RedMes1.Content = "Incorrect entry, please try again";
            }
            else
            {
                if (RedMes1 != null)
                    RedMes1.Content = "";
            }
        }

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
        #endregion

        #region Target ID
        /// <summary>
        /// Text changed event for TargetId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelTargetId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(parcelTargetId.Text, out int num);
            if (flag && num < 100000000) //if the id the user entered is less than 4 digits the border is red
            {
                RedMes2.Content = "Incorrect entry, please try again";
            }
            else
            {
                if (RedMes2 != null)
                    RedMes2.Content = "";
            }
        }
        #endregion

        #region Weight
        /// <summary>
        /// parcelWeight combobox selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (parcelWeight.SelectedItem != null)
                WeightLbl.Content = "";
        }
        #endregion

        #region Priority
        /// <summary>
        /// parcelPriority combobox selection event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelPriority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (parcelPriority.SelectedItem != null)
                PriorityLbl.Content = "";
        }
        #endregion

        #region OK button
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
            parcel.Sender = new CustomerInParcel() { Id = id };
            bool flag2 = int.TryParse(parcelTargetId.Text, out id);
            parcel.Target = new CustomerInParcel() { Id = id };

            parcel.Weight = (WeightCategories)parcelWeight.SelectedItem;
            parcel.Priority = (Priorities)parcelPriority.SelectedItem;

            MessageBoxResult mb = default;
            try
            {
                myBL.AddParcel(parcel);
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
            catch (NoIDException ex)
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
            MessageBox.Show("parcel successfully added", "Message", MessageBoxButton.OK, MessageBoxImage.Information);

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
        public ParcelWindow(IBL bl, Parcel p)
        {
            myBL = bl;

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible
            this.Title = "Update parcel"; //change the title
            parcel = p;
            DataContext = parcel;
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

        #region Open windows
        private void SenderInformation_Click(object sender, RoutedEventArgs e)
        {
            var customer = myBL.GetCustomer(parcel.Target.Id);
            if (customer.Active)
                new CustomerWindow(myBL, customer).Show();
            else
                MessageBox.Show("Customer no longer active.", "", MessageBoxButton.OK);
        }

        private void TargetInformation_Click(object sender, RoutedEventArgs e)
        {
            var customer = myBL.GetCustomer(parcel.Sender.Id);
            if (customer.Active)
                new CustomerWindow(myBL, customer).Show();
            else
                MessageBox.Show("Customer no longer active.", "", MessageBoxButton.OK);
        }

        private void DroneInformation_Click(object sender, RoutedEventArgs e)
        {
            if (parcel.Scheduled != null)
            {
                if (parcel.AssignedDrone.Active)
                {
                    var drone = myBL.GetDrone(parcel.AssignedDrone.Id);
                    new DroneWindow(myBL, drone).Show();
                }
                else
                    MessageBox.Show("Drone no longer active.", "", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Drone not yet assigned.", "", MessageBoxButton.OK);
        }
        #endregion

        #endregion
    }
}
