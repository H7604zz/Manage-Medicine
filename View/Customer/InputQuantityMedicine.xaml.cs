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

namespace ProjectPrn.View.Customer
{
    /// <summary>
    /// Interaction logic for InputQuantityMedicine.xaml
    /// </summary>
    public partial class InputQuantityMedicine : Window
    {
        private int availableQuantity;
        public int Quantity { get; private set; }
        public InputQuantityMedicine(int availableQuantity)
        {
            InitializeComponent();
            this.availableQuantity = availableQuantity;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtQuantity.Text, out int quantity) && quantity > 0 && quantity <= availableQuantity)
            {
                Quantity = quantity;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show($"Please enter a quantity between 1 and {availableQuantity}", "Invalid Quantity", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
