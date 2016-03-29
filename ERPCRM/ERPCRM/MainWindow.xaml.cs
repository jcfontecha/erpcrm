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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERPCRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            var window = new Products();
            window.Show();
        }

        private void btnClientes_Click(object sender, RoutedEventArgs e)
        {
            var window = new Clientes();
            window.Show();
        }

        private void menuItemEditCountries_Click(object sender, RoutedEventArgs e)
        {
            var window = new Countries();
            window.Show();
        }

        private void btnComponentes_Click(object sender, RoutedEventArgs e)
        {
            var window = new Components();
            window.Show();
        }

        private void btnEmployees_Click(object sender, RoutedEventArgs e)
        {
            var window = new Employees();
            window.Show();
        }

        private void btnExpenses_Click(object sender, RoutedEventArgs e)
        {
            var window = new Expenses();
            window.Show();
        }
    }
}
