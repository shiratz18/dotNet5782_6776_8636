using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private IBL myBL;

        /// <summary>
        /// Window constructor
        /// </summary>
        /// <param name="bl">Business logic paramater</param>
        public DroneListWindow(IBL bl)
        {
            myBL = bl;
            InitializeComponent();

            try
            {
                DronesListView.ItemsSource = myBL.GetDroneList();
            }
            catch (BO.EmptyListException) { }
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /// <summary>
        /// Filters list of drones according to status selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null)
            {
                DroneStatuses ds = (DroneStatuses)StatusSelector.SelectedItem;

                statusLabel.Content = "";

                //get all the drones with the selected status
                //if there are no drones, catch the exception and do nothing
                try
                {
                    if (WeightSelector.SelectedItem != null) //if there is a selectes weight, get all the drones with that weight and the status
                        DronesListView.ItemsSource = myBL.GetDroneList((WeightCategories)WeightSelector.SelectedItem, ds);

                    else
                        DronesListView.ItemsSource = myBL.GetDroneList(null, ds); //get all the drones with the selected status
                }
                catch (EmptyListException)
                { }
            }
        }

        /// <summary>
        /// Filters list of drones according to selected status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedItem != null)
            {
                WeightCategories wc = (WeightCategories)WeightSelector.SelectedItem;

                weightLabel.Content = "";

                //get all the drones with the selected weight category
                //if there are no drones, catch the exception and do nothing
                try
                {
                    if (StatusSelector.SelectedItem != null) //if there is a selected status get all the drones with that status
                        DronesListView.ItemsSource = myBL.GetDroneList(wc,(DroneStatuses)StatusSelector.SelectedItem);

                    else
                        DronesListView.ItemsSource = myBL.GetDroneList(wc);   //get all the drones with the selected weight
                }
                catch (EmptyListException)
                { }
            }
        }

        /// <summary>
        /// Opens new DroneWindow in order to add a drone to the list, and updates DronesListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(myBL).ShowDialog(); //open add drone window

            //update the list
            if (WeightSelector.SelectedItem != null)
            {
                if (StatusSelector.SelectedItem != null)
                    DronesListView.ItemsSource = myBL.GetDroneList((WeightCategories)WeightSelector.SelectedItem, (DroneStatuses)StatusSelector.SelectedItem);
                else
                    DronesListView.ItemsSource = myBL.GetDroneList((WeightCategories)WeightSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = myBL.GetDroneList(null, (DroneStatuses)StatusSelector.SelectedItem);
            else
                DronesListView.ItemsSource = myBL.GetDroneList();
        }

        /// <summary>
        /// Opens a new DroneWindow in order to update the selcted drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListDrone tmp = (ListDrone)DronesListView.SelectedItem;

            Drone d = myBL.GetDrone(tmp.Id); //get the drone information from the selected drone

            new DroneWindow(myBL, d).ShowDialog(); //open action window

            if (WeightSelector.SelectedItem != null)
            {
                if (StatusSelector.SelectedItem != null)
                    DronesListView.ItemsSource = myBL.GetDroneList((WeightCategories)WeightSelector.SelectedItem, (DroneStatuses)StatusSelector.SelectedItem);
                else
                    DronesListView.ItemsSource = myBL.GetDroneList((WeightCategories)WeightSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = myBL.GetDroneList(null, (DroneStatuses)StatusSelector.SelectedItem);
            else
                DronesListView.ItemsSource = myBL.GetDroneList();
        }

        /// <summary>
        /// Closes this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            allowClosing = true;
            this.Close();
        }

        /// <summary>
        /// Drags the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// Clears the weight selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearWeightSelection_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;

            if (StatusSelector.SelectedItem != null)
                DronesListView.ItemsSource = myBL.GetDroneList(null, (DroneStatuses)StatusSelector.SelectedItem);
            else
                DronesListView.ItemsSource = myBL.GetDroneList();

            weightLabel.Content = "Weight";
        }

        /// <summary>
        /// clears status selction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearStatusSelection_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;

            if (WeightSelector.SelectedItem != null)
                DronesListView.ItemsSource = myBL.GetDroneList((WeightCategories)WeightSelector.SelectedItem);

            else
                DronesListView.ItemsSource = myBL.GetDroneList();

            statusLabel.Content = "Status";
        }

        /// <summary>
        /// clears all selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAllSelections_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;
            StatusSelector.SelectedItem = null;

            DronesListView.ItemsSource = myBL.GetDroneList();

            weightLabel.Content = "Weight";
            statusLabel.Content = "Status";
        }

        //dont allow closig with x button
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;

            if (hwndSource != null)
            {
                hwndSource.AddHook(HwndSourceHook);
            }

        }

        private bool allowClosing = false;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;

        private const uint SC_CLOSE = 0xF060;

        private const int WM_SHOWWINDOW = 0x00000018;
        private const int WM_CLOSE = 0x10;

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_SHOWWINDOW:
                    {
                        IntPtr hMenu = GetSystemMenu(hwnd, false);
                        if (hMenu != IntPtr.Zero)
                        {
                            EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                        }
                    }
                    break;
                case WM_CLOSE:
                    if (!allowClosing)
                    {
                        handled = true;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
