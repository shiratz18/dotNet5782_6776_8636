using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for TrackParcelWindow.xaml
    /// </summary>
    public partial class TrackParcelWindow : Window
    {
        private IBL myBL;
        private Parcel parcel;
        private bool isSender;
        BackgroundWorker worker = new BackgroundWorker();

        public TrackParcelWindow(IBL bl, Parcel p, bool b)
        {
            InitializeComponent();
            myBL = bl;
            parcel = p;
            isSender = b;
            DataContext = p;

            if (isSender)
                btnCancel.Visibility = Visibility.Visible;

            if (parcel.Delivered != null)
            {
                btnCancel.IsEnabled = false;
                lblSch.Visibility = Visibility.Visible;
                lblPck.Visibility = Visibility.Visible;
                lblDlv.Visibility = Visibility.Visible;
            }
            else if (parcel.PickedUp != null)
            {
                btnCancel.IsEnabled = false;
                lblSch.Visibility = Visibility.Visible;
                lblPck.Visibility = Visibility.Visible;
            }
            else if (parcel.Scheduled != null)
            {
                btnCancel.IsEnabled = false;
                lblSch.Visibility = Visibility.Visible;
            }
            else
                btnCancel.IsEnabled = true;
        }

        #region Close window
        private void Close_Click(object sender, RoutedEventArgs e)
        { 
            worker.CancelAsync();
            Close();
        }
        #endregion

        #region Refresh
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            //get the parcel again from bl 
            parcel = myBL.GetParcel(parcel.Id);
        }
        #endregion

        #region Progress bar backround worker

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += progress_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

        private void progress_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (parcel.Delivered != null)
                    (sender as BackgroundWorker).ReportProgress(3);
                else if (parcel.PickedUp != null)
                    (sender as BackgroundWorker).ReportProgress(2);
                else if (parcel.Scheduled != null)
                    (sender as BackgroundWorker).ReportProgress(1);
                else
                    (sender as BackgroundWorker).ReportProgress(0);
               
                parcel = myBL.GetParcel(parcel.Id);
                DataContext = parcel;
                Thread.Sleep(1500);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prclProgress.Value = e.ProgressPercentage;

            switch (prclProgress.Value)
            {
                case 1:
                    if (isSender)
                        btnCancel.IsEnabled = false;
                    lblSch.Visibility = Visibility.Visible;
                    break;
                case 2:
                    lblPck.Visibility = Visibility.Visible;
                    break;
                case 3:
                    lblDlv.Visibility = Visibility.Visible;
                    break;
            }
        }
        #endregion

        #region Delete parcel
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var mb = MessageBox.Show("Are you sure you want to cancel this delivery?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (mb == MessageBoxResult.Yes)
            {
                myBL.RemoveParcel(parcel.Id);
                Close();
                MessageBox.Show("Parcel successfully canceled.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
    }
}
