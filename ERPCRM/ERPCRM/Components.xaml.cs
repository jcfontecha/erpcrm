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
    /// Interaction logic for Components.xaml
    /// </summary>
    public partial class Components : Window
    {
        private ERPContext context = new ERPContext();
        private System.Windows.Data.CollectionViewSource componentViewSource;

        public Components()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data
            componentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ComponentViewSource")));
            LoadData();
        }

        #region Loading data in BW

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
            context.Components.Load();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            componentViewSource.Source = context.Components.Local;
            SetupActiveUserInterface();
        }

        private void SetupInactiveUserInterface()
        {
            txtStatusDerecha.Text = "Loading...";
            btnNew.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSearch.IsEnabled = false;
        }

        private void SetupActiveUserInterface()
        {
            txtStatusDerecha.Text = "Ready";
            btnNew.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnSearch.IsEnabled = true;
        }

        #endregion

        #region Search functions

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (btnSearch.Content.ToString() == "Clear")
            {
                componentDataGrid.Items.Filter = null;
                btnSearch.Content = "Search";
                txtSearch.Text = "";
            }

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                componentViewSource.Filter += new FilterEventHandler(SearchRows);
                btnSearch.Content = "Clear";
            }
        }

        private void SearchRows(object sender, FilterEventArgs e)
        {
            var component = e.Item as Component;
            if (component != null)
            {
                // Filter out products
                var term = txtSearch.Text;

                var isInDesc = component.description.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
                var isInKey = component.key.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;

                if (!string.IsNullOrEmpty(term))
                {
                    if (isInDesc || isInKey)
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSearch.Content = "Search";
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearch_Click(this, null);
            }
        }

        #endregion

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
            this.Close();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            context.Components.Local.Add(new Component());
            componentDataGrid.SelectedIndex = componentDataGrid.Items.Count - 1;
            txtKey.Focus();
        }

    }
}
