using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        private IBL myBL;
        private bool senderGrouped, targetGrouped;

        private static ParcelListWindow instance = null;
        public static ParcelListWindow Instance
        {
            get
            {
                if (instance == null)
                    instance = new ParcelListWindow();
                return instance;
            }
        }

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bl"></param>
        private ParcelListWindow()
        {
            myBL = BlFactory.GetBl();
            senderGrouped = false;
            targetGrouped = false;

            InitializeComponent();

            try
            {
                ParcelsListView.ItemsSource = myBL.GetParcelList();
            }
            catch (BO.EmptyListException) { }

            StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelState));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
        }
        #endregion

        #region Close
        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Refresh
        /// <summary>
        /// Refresh the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            CheckFilters();
        }
        #endregion

        #region Add parcel
        /// <summary>
        /// Opens parcel window to add a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow().ShowDialog();
            CheckFilters();
        }
        #endregion

        #region Edit parcel
        /// <summary>
        /// Opens parcel window to edit a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            ListParcel ld = b.CommandParameter as ListParcel;
            Parcel p = myBL.GetParcel(ld.Id);

            new ParcelWindow(p).ShowDialog();
            CheckFilters();
        }
        #endregion

        #region Remove parcel
        /// <summary>
        /// Removes a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult ans = MessageBox.Show("Are you sure you want to delete this parcel?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ans == MessageBoxResult.Yes)
            {
                Button b = sender as Button;
                ListParcel lp = b.CommandParameter as ListParcel;
                try
                {
                    myBL.RemoveParcel(lp.Id);

                    //       DronesListView.ItemsSource = myBL.GetDroneList();

                    CheckFilters();
                }
                catch (CannotDeleteException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region Filters and grouping
        #region Status filter
        /// <summary>
        /// Filters list of parcels according to status selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem != null)
            {
                statusLabel.Content = "";

                CheckFilters();
            }
        }

        /// <summary>
        /// clears status selction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearStatusSelection_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            statusLabel.Content = "- All Statuses -";

            CheckFilters();
        }
        #endregion

        #region Weight filter
        /// <summary>
        /// Filters list of drones according to selected status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedItem != null)
            {
                weightLabel.Content = "";

                CheckFilters();
            }
        }

        /// <summary>
        /// Clears the weight selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearWeightSelection_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedItem = null;

            weightLabel.Content = "- All Weights -";

            CheckFilters();
        }
        #endregion

        #region Priority filter
        /// <summary>
        /// Filters list of parcels according to priority selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PrioritySelector.SelectedItem != null)
            {
                priorityLabel.Content = "";

                CheckFilters();
            }
        }

        /// <summary>
        /// clears priority selction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearPrioritySelection_Click(object sender, RoutedEventArgs e)
        {
            PrioritySelector.SelectedItem = null;
            priorityLabel.Content = "- All Priorities -";

            CheckFilters();
        }
        #endregion

        #region Grouping
        /// <summary>
        /// Groups list by sender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupBySender_Click(object sender, RoutedEventArgs e)
        {
            //marking that the list is grouped by sender
            senderGrouped = true;
            targetGrouped = false;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("SenderName");
            view.GroupDescriptions.Insert(0, groupDescription); //add to the beginning of the groupdescription list
        }

        /// <summary>
        /// Groups list by target
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupByReciever_Click(object sender, RoutedEventArgs e)
        {
            //marking that the list is grouped by target
            targetGrouped = true;
            senderGrouped = false;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("TargetName");
            view.GroupDescriptions.Insert(0, groupDescription);
        }
        #endregion

        /// <summary>
        /// Checks if any filters are applied and updates list accordingly
        /// </summary>
        internal void CheckFilters()
        {
            if (WeightSelector.SelectedItem != null) //if weight is not null
            {
                if (StatusSelector.SelectedItem != null) //if also status is not null
                    if (PrioritySelector.SelectedItem != null) //if also priority is not null
                        //filter by all 3
                        ParcelsListView.ItemsSource = myBL.GetParcelList((WeightCategories)WeightSelector.SelectedItem, (ParcelState)StatusSelector.SelectedItem, (Priorities)PrioritySelector.SelectedItem);
                    else
                        //filter by weight and status
                        ParcelsListView.ItemsSource = myBL.GetParcelList((WeightCategories)WeightSelector.SelectedItem, (ParcelState)StatusSelector.SelectedItem);

                else if (PrioritySelector.SelectedItem != null) //if priority is not null
                    //filter by wight and priority
                    ParcelsListView.ItemsSource = myBL.GetParcelList((WeightCategories)WeightSelector.SelectedItem, null, (Priorities)PrioritySelector.SelectedItem);

                else
                    //filter by weight
                    ParcelsListView.ItemsSource = myBL.GetParcelList((WeightCategories)WeightSelector.SelectedItem);
            }

            else if (StatusSelector.SelectedItem != null)
            {
                if (PrioritySelector.SelectedItem != null)
                    ParcelsListView.ItemsSource = myBL.GetParcelList(null, (ParcelState)StatusSelector.SelectedItem, (Priorities)PrioritySelector.SelectedItem);
                else
                    ParcelsListView.ItemsSource = myBL.GetParcelList(null, (ParcelState)StatusSelector.SelectedItem);
            }

            else if (PrioritySelector.SelectedItem != null)
                ParcelsListView.ItemsSource = myBL.GetParcelList(null, null, (Priorities)PrioritySelector.SelectedItem);

            else
                ParcelsListView.ItemsSource = myBL.GetParcelList();

            if (senderGrouped)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("SenderName");
                view.GroupDescriptions.Add(groupDescription);
            }
            else if (targetGrouped)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("TargetName");
                view.GroupDescriptions.Add(groupDescription);
            }
        }
        #endregion
    }
}
