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
    /// Interaction logic for Employees.xaml
    /// </summary>
    public partial class Employees : Window
    {
        private ERPContext context = new ERPContext();
        private System.Windows.Data.CollectionViewSource employeeViewSource;
        private System.Windows.Data.CollectionViewSource countryViewSource;

        public Employees()
        {
            InitializeComponent();
        }

        #region Load data

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data
            employeeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("employeeViewSource")));
            countryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("CountryViewSource")));
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
            context.Employees.Load();
            context.Countries.Load();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            employeeViewSource.Source = context.Employees.Local;
            countryViewSource.Source = context.Countries.Local;

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
                clientDataGrid.Items.Filter = null;
                btnSearch.Content = "Search";
                txtSearch.Text = "";
            }

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                employeeViewSource.Filter += new FilterEventHandler(SearchRows);
                btnSearch.Content = "Clear";
            }
        }

        private void SearchRows(object sender, FilterEventArgs e)
        {
            var employee = e.Item as Employee;
            if (employee != null)
            {
                // Filter out products
                var term = txtSearch.Text;

                var isInName = employee.name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
                var isInLastName = employee.lastName.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
                var isInEmail = employee.email.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
                var isInUsername = employee.username.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;

                if (!string.IsNullOrEmpty(term))
                {
                    if (isInName || isInLastName || isInEmail || isInUsername)
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
            var client = context.Clients.Local.Last();
            context.SaveChanges();
            this.Close();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var employee = new Employee();
            employee.Address = new Address();
            context.Employees.Local.Add(employee);
            clientDataGrid.SelectedIndex = clientDataGrid.Items.Count - 1;
            txtName.Focus();
        }
    }
}
