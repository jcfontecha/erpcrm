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
    /// Interaction logic for SelectItem.xaml
    /// </summary>
    public partial class SelectItem : Window
    {
        private CollectionViewSource collectionViewSource;
        private IEnumerable<Object> items;
        public Func<IEnumerable<Object>> DataLoader;
        public Action<Object[]> CompletionHandler;

        public SelectItem()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            collectionViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("itemsViewSource")));
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
            items = DataLoader();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            collectionViewSource.Source = items;
            SetupActiveUserInterface();
        }

        private void SetupInactiveUserInterface()
        {
            btnChoose.IsEnabled = false;
        }

        private void SetupActiveUserInterface()
        {
            btnChoose.IsEnabled = true;
        }

        #endregion

        #region Search functions

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (btnSearch.Content.ToString() == "Clear")
            {
                listBox.Items.Filter = null;
                btnSearch.Content = "Search";
                txtSearch.Text = "";
            }

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                collectionViewSource.Filter += new FilterEventHandler(SearchRows);
                btnSearch.Content = "Clear";
            }
        }

        private void SearchRows(object sender, FilterEventArgs e)
        {
            // Filter out products
            var term = txtSearch.Text;

            var shouldAppear = e.Item.ToString().IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;

            if (!string.IsNullOrEmpty(term))
            {
                if (shouldAppear)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            var components = listBox.SelectedItems.OfType<Object>().ToArray();
            CompletionHandler(components);
            this.Close();
        }
    }
}
