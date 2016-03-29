using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;

namespace ERPCRM
{
    /// <summary>
    /// Interaction logic for Countries.xaml
    /// </summary>
    public partial class Countries : Window
    {
        private ERPContext context = new ERPContext();
        private System.Windows.Data.CollectionViewSource countryViewSource;

        public Countries()
        {
            InitializeComponent();
        }

        #region Load data

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data
            countryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("countryViewSource")));
            LoadData();
        }

        private void LoadData()
        {
            SetupInactiveUserInterface();
            var worker = new BackgroundWorker();
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            context.Countries.Load();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            countryViewSource.Source = context.Countries.Local;
            SetupActiveUserInterface();
        }

        private void SetupInactiveUserInterface()
        {
            txtStatusDerecha.Text = "Loading...";
            btnSave.IsEnabled = false;
        }

        private void SetupActiveUserInterface()
        {
            txtStatusDerecha.Text = "Ready";
            btnSave.IsEnabled = true;
        }


        #endregion

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
