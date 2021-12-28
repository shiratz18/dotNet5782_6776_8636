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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private IBL myBL;
        private Customer customer;

        public CustomerWindow(IBL bl)
        {
            myBL = bl;

            InitializeComponent();
            ActionGrid.Visibility = Visibility.Hidden; //hide the action grid
            this.Title = "New customer"; //change the title

        }
        /// <summary>
        /// Constructor for action grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="d"></param>
        public CustomerWindow(IBL bl, Customer c)
        {
            myBL = bl;

            InitializeComponent();

            AddGrid.Visibility = Visibility.Hidden; //add grid will be invisible
            this.Title = "Update customer"; //change the title
            customer = c;
            DataContext = customer;

           // display();
        }


        /// <summary>
        /// Text changed event for customerId textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerId_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool flag = int.TryParse(customerId.Text, out int num);
            if (flag && num < 1000) //if the id the user entered is less than 4 digits the border is red
            {
                customerId.BorderBrush = Brushes.Red;
                RedMes1.Content = "Incorrect entry, please try again";
            }
            else
            {
                customerId.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes1 != null)
                    RedMes1.Content = "";
            }
            setOkButton();
        }

        /// <summary>
        /// Sets customerId text box to accept only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numbersOnly(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text); //allow only numbers in the text box
        }

        /// <summary>
        /// Removes the current text from customerId text box, occurs only once and then removed drom events
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
        /// Enables OK button only when all fields are filled
        /// </summary>
        private void setOkButton()
        {
            //enable OK button only if all fields were filled
            if (btnOK != null)
                btnOK.IsEnabled = (!String.IsNullOrEmpty(customerId.Text) && customerId.Text != "Enter ID here") &&
                    (!String.IsNullOrEmpty(customerName.Text) && customerName.Text != "Enter model here") &&
                    (!String.IsNullOrEmpty(customerPhone.Text) && customerPhone.Text != "Enter ID here") ;
        }

        /// <summary>
        /// Text changed event for customerName
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string m = customerName.Text;
            if (!String.IsNullOrEmpty(m) && m.Length < 10)//if the model the user entered is less than 5 characters the border is red
            {
                customerName.BorderBrush = Brushes.Red;
                RedMes2.Content = "Incorrect entry, please try again";
            }
            else
            {
                customerName.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes2 != null)
                    RedMes2.Content = "";
            }
            setOkButton();
        }

        /// <summary>
        ///  Removes the current text from customerName text box, occurs only once and then removed drom events
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
        /// Text changed event for customerPhone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            string m = customerPhone.Text;
            if (!String.IsNullOrEmpty(m) && m.Length < 10)//if the phone the user entered is less than 10 characters the border is red
            {
                customerPhone.BorderBrush = Brushes.Red;
                RedMes3.Content = "Incorrect entry, please try again";
            }
            else
            {
                customerPhone.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179)); //otherwise the border is gray (original)
                if (RedMes3 != null)
                    RedMes3.Content = "";
            }
            setOkButton();
        }

        /// <summary>
        ///  Removes the current text from customerPhone text box, occurs only once and then removed drom events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void phoneTbGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.Text = ""; //changing the text to be empty
            tb.GotFocus -= phoneTbGotFocus;
        }



        /// <summary>
        /// Adds customer with user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //get the data the user entered
            customer = new Customer();
            bool flag1 = int.TryParse(customerId.Text, out int id);
            customer.Id = id;
            string name = customerName.Text;
            customer.Name = name;
            string phone = customerPhone.Text;
            customer.Phone = name;

          // double flag2 = double.Parse(customerLongitude.Text,out double longitude);
          //customer.Location.Longitude = longitude;
          //  bool flag3 = double.Parse(customerLatitude.Text, out double latitude);
          //  customer.Location.Latitude = latitude;



            MessageBoxResult mb = default;
            try
            {
                myBL.AddCustomer(customer);
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
            MessageBox.Show("Customer successfully added", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Display the chosen customer and update button options according to drone status
        /// </summary>
        //private void display()
        //{
        //    RedMes6.Content = " ";
        //    //idToPrint.Content = drone.Id;
        //    //modelToPrint.Text = drone.Model;
        //    //maxWeightToPrint.Content = drone.MaxWeight;
        //    //batteryToPrint.Content = String.Format("{0:0.0}", drone.Battery);
        //    //statusToPrint.Content = drone.Status;
        //    //locationToPrint.Content = drone.CurrentLocation;
        //    if (drone.Status == DroneStatuses.Shipping)
        //    {
        //        parcelExpander.IsExpanded = true;
        //        parcelExpander.IsEnabled = true;
        //        parcelId.Content = drone.InShipping.Id;
        //        isPickedUp.Content = drone.InShipping.IsPickedUp;
        //        priority.Content = drone.InShipping.Priority;
        //        weight.Content = drone.InShipping.Weight;
        //        senderName.Content = drone.InShipping.Sender.Name;
        //        targetName.Content = drone.InShipping.Target.Name;
        //        pickUpLocation.Content = drone.InShipping.PickUpLocation;
        //        deliveryLocation.Content = drone.InShipping.DeliveryLocation;
        //        deliveryDistance.Content = drone.InShipping.DeliveryDistance;
        //    }
        //    else
        //    {
        //        parcelExpander.IsEnabled = false;
        //        parcelExpander.IsExpanded = false;
        //    }

        //    if (drone.Status != DroneStatuses.Available) //if the drone is not available do not shoe charge button
        //    {
        //        btnCharge.Visibility = Visibility.Hidden;
        //        btnDroneToDelivery.Visibility = Visibility.Hidden;
        //    }

        //    if (drone.Status != DroneStatuses.Maintenance)
        //    {
        //        btnReleaseCharge.Visibility = Visibility.Hidden; //if the drone is not in maintenace do not show release charge button
        //    }

        //    if (drone.Status != DroneStatuses.Shipping)
        //    {
        //        btnDronePickUp.Visibility = Visibility.Hidden;
        //        btnDroneDeliver.Visibility = Visibility.Hidden;
        //    }

        //    if (drone.InShipping.IsPickedUp)
        //        btnDronePickUp.Visibility = Visibility.Hidden;

        //    if (!drone.InShipping.IsPickedUp)
        //        btnDroneDeliver.Visibility = Visibility.Hidden;

        //}

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
        /// Allows to drag the window (because there is no title to drag from)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        
        /// <summary>
        /// Update the customer name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnUpdateNAME_Click(object sender, RoutedEventArgs e)
        {

            string newName = nameToPrint.Text;

            try
            {
                myBL.UpdateCustomerName(customer.Id, newName); //update the customer name
                customer.Name = newName; //update also the current customer

                MessageBox.Show("Customer name updated successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
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
        
        /// <summary>
        /// Update the customer phone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdatePHON_Click(object sender, RoutedEventArgs e)
        {
            string newPhone = phoneToPrint.Text;

            try
            {
                myBL.UpdateCustomerPhone(customer.Id, newPhone); //update the customer phone
                customer.Name = newPhone; //update also the current customer

                MessageBox.Show("Customer phone updated successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
    

}
