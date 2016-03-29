using System;
using System.Collections.Generic;
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
using System.ComponentModel;

namespace ERPCRM
{
    /// <summary>
    /// Interaction logic for SelectComponent.xaml
    /// </summary>
    public partial class SelectComponent : Window
    {
        private ERPContext context = new ERPContext();
        private System.Windows.Data.CollectionViewSource componentViewSource;
        public IEnumerable<Component> components;

        public Action<Component[]> CompletionHandler;

        public SelectComponent()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             componentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("componentViewSource")));
             componentViewSource.Source = components;
        }

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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            var components = componentDataGrid.SelectedItems.OfType<Component>().ToArray();
            CompletionHandler(components);
            this.Close();
        }
    }
}
