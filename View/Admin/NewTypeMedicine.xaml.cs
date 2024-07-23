using ProjectPrn.Controller;
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

namespace ProjectPrn.View.Admin
{
    /// <summary>
    /// Interaction logic for NewTypeMedicine.xaml
    /// </summary>
    public partial class NewTypeMedicine : Window
    {
        private MedicineControll controllMedicine;
        public NewTypeMedicine()
        {
            InitializeComponent();
            controllMedicine = new MedicineControll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string newType = txtNewType.Text;
            controllMedicine.CreateNewTypeMedicine(newType);
            DialogResult = true;
            Close();
        }
    }
}
