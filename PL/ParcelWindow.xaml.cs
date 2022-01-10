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
            this.Title = "New parcel"; //change the title

            parcelWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            parcelPriority.ItemsSource = Enum.GetValues(typeof(Priorities));
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

        }


        #region Sender ID
        /// <summary>
        /// Text changed event for SenderId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelSenderId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(parcelSenderId.Text, out int num);
            if (flag && num < 1000000000) //if the id the user entered is less than 4 digits the border is red
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
            if (flag && num < 1000000000) //if the id the user entered is less than 4 digits the border is red
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
        #endregion

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
            setOkButton();
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
            parcel.Sender = new CustomerInParcel() { Id = id };
            bool flag2 = int.TryParse(parcelTargetId.Text, out  id);
            parcel.Target = new CustomerInParcel() { Id = id };

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
            MessageBox.Show("parcel successfully added", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
        
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

        private void SenderInformation_Click(object sender, RoutedEventArgs e)
        {
            var customer = myBL.GetCustomer(parcel.Target.Id);
            new CustomerWindow(myBL, customer).ShowDialog();
        }

        private void TargetInformation_Click(object sender, RoutedEventArgs e)
        {
            var customer = myBL.GetCustomer(parcel.Sender.Id);
            new CustomerWindow(myBL, customer).ShowDialog();
        }

        private void DroneInformation_Click(object sender, RoutedEventArgs e)
        {
            if (parcel.AssignedDrone.Id != 0)
            {
                var drone = myBL.GetDrone(parcel.AssignedDrone.Id);
                new DroneWindow(myBL, drone).ShowDialog();
            }

        }
    }
}
