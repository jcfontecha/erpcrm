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
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Window
    {
        private ERPContext context = new ERPContext();
        private System.Windows.Data.CollectionViewSource productViewSource;

        public Products()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data
            productViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ProductViewSource")));
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
            context.Products.Load();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            productViewSource.Source = context.Products.Local;
            SetupActiveUserInterface();
        }

        private void SetupInactiveUserInterface()
        {
            txtStatusDerecha.Text = "Loading...";
            btnAddProduct.IsEnabled = false;
            btnSave.IsEnabled = false;
            btnSearch.IsEnabled = false;
        }

        private void SetupActiveUserInterface()
        {
            txtStatusDerecha.Text = "Ready";
            btnAddProduct.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnSearch.IsEnabled = true;
        }

        #endregion

        #region Search functions

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (btnSearch.Content.ToString() == "Clear")
            {
                productDataGrid.Items.Filter = null;
                btnSearch.Content = "Search";
                txtSearch.Text = "";
            }

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                productViewSource.Filter += new FilterEventHandler(SearchRows);
                btnSearch.Content = "Clear";
            }
        }

        private void SearchRows(object sender, FilterEventArgs e)
        {
            var product = e.Item as Product;
            if (product != null)
            {
                // Filter out products
                var term = txtSearch.Text;

                var isInDesc = product.description.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
                var isInKey = product.key.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;

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

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            context.Products.Local.Add(new Product());
            productDataGrid.SelectedIndex = productDataGrid.Items.Count - 1;
            txtKey.Focus();
        }

        private void btnRemoveComponent_Click(object sender, RoutedEventArgs e)
        {
            context.Products.Local[productDataGrid.SelectedIndex].Components.RemoveAt(componentsListBox.SelectedIndex);
        }

        private void btnAddComponent_Click(object sender, RoutedEventArgs e)
        {
            var window = new SelectItem();
            window.DataLoader = () => {
                context.Components.Load();
                return context.Components.Local;
            };
            window.CompletionHandler = (components) => {
                foreach (var component in components)
                {
                    var product = context.Products.Local[productDataGrid.SelectedIndex] as Product;
                    product.Components.Add(component as Component); 
                }
            };
            window.Show();
        }
    }
}
