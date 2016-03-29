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
    /// Interaction logic for EditPurchase.xaml
    /// </summary>
    public partial class EditPurchase : Window
    {

        private ERPContext context = new ERPContext();
        private System.Windows.Data.CollectionViewSource expenseViewSource;
        private System.Windows.Data.CollectionViewSource vendorViewSource;
        private System.Windows.Data.CollectionViewSource componentViewSource;

        public int idExpense;

        public EditPurchase()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            expenseViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("expenseViewSource")));
            vendorViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("vendorViewSource")));
            componentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("componentViewSource")));
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
            context.Vendors.Load();
            context.Components.Load();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            expenseViewSource.Source = context.Expenses.Local;
            vendorViewSource.Source = context.Vendors.Local;
            componentViewSource.Source = context.Components.Local;
            componentComboBox.ItemsSource = context.Components.Local;
            SetupActiveUserInterface();
        }

        private void SetupInactiveUserInterface()
        {
            txtStatusDerecha.Text = "Loading...";
            btnAddComponent.IsEnabled = false;
            btnSave.IsEnabled = false;
        }

        private void SetupActiveUserInterface()
        {
            txtStatusDerecha.Text = "Ready";
            btnAddComponent.IsEnabled = true;
            btnSave.IsEnabled = true;
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

        private void btnAddComponent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int quantity = int.Parse(txtQuantity.Text);

                for (int i = 0; i < quantity; i++)
                {
                    var newStockComponent = new StockComponent();
                    newStockComponent.Component = componentComboBox.SelectedItem as Component;
                    newStockComponent.idComponent = (componentComboBox.SelectedItem as Component).idComponent;
                    newStockComponent.expirationDate = DateTime.Today;
                    var expense = expenseComboBox.SelectedItem as Expense;
                    expense.StockComponents.Add(newStockComponent);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Make sure you're entering a number in the quantity field");
            }
        }

        private void detailComponentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stockComponentsDataGrid.Items.Refresh();
        }
    }
}
