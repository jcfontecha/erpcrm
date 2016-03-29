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
    /// Interaction logic for Expenses.xaml
    /// </summary>
    public partial class Expenses : Window
    {
        private ERPContext context = new ERPContext();
        private CollectionViewSource expenseViewSource;
        private CollectionViewSource employeeViewSource;

        private bool addingNew = false;

        public Expenses()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            expenseViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("expenseViewSource")));
            employeeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("employeeViewSource")));
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
            context.Expenses.Load();
            context.Employees.Load();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            expenseViewSource.Source = context.Expenses.Local;
            employeeViewSource.Source = context.Employees.Local;
            SetupActiveUserInterface();
        }

        private void SetupInactiveUserInterface()
        {
            txtStatusDerecha.Text = "Loading...";
            btnSave.IsEnabled = false;
            btnSearch.IsEnabled = false;
            btnNew.IsEnabled = false;
            btnEditPurchase.IsEnabled = false;
        }

        private void SetupActiveUserInterface()
        {
            txtStatusDerecha.Text = "Ready";
            btnSave.IsEnabled = true;
            btnSearch.IsEnabled = true;
            btnNew.IsEnabled = true;
            btnEditPurchase.IsEnabled = true;
        }

        #endregion

        #region Search functions

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (btnSearch.Content.ToString() == "Clear")
            {
                expenseDataGrid.Items.Filter = null;
                btnSearch.Content = "Search";
                txtSearch.Text = "";
            }

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                expenseViewSource.Filter += new FilterEventHandler(SearchRows);
                btnSearch.Content = "Clear";
            }
        }

        private void SearchRows(object sender, FilterEventArgs e)
        {
            var expense = e.Item as Expense;
            if (expense != null)
            {
                // Filter out products
                var term = txtSearch.Text;

                var isInConcept = expense.concept.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;

                if (!string.IsNullOrEmpty(term))
                {
                    if (isInConcept)
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


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
            addingNew = false;
            MessageBox.Show("Your changes were saved.");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (!addingNew)
            {
                var expense = new Expense();
                context.Expenses.Local.Add(expense);
                expenseDataGrid.SelectedIndex = expenseDataGrid.Items.Count - 1;
                txtAmount.Text = "";
                txtAmount.Focus();
                addingNew = true;
            }
            else
            {
                MessageBox.Show("Click save to finish adding an expense before adding a new one.");
            }
        }

        private void btnEditPurchase_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditPurchase();
            window.Show();
        }
    }
}
