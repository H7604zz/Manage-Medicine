using ProjectPrn.Controller;
using ProjectPrn.Models;
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

namespace ProjectPrn.View.Sale
{
    /// <summary>
    /// Interaction logic for ApproveOrders.xaml
    /// </summary>
    public partial class ApproveOrders : Window
    {
        private int orderId;
        private OrderControll controllOrder;
        private int selectedStatusId;
        public ApproveOrders(int id)
        {
            InitializeComponent();
            orderId = id;
            controllOrder = new OrderControll();
            LoadStatusOrder();
        }

        private void LoadStatusOrder()
        {
            spnStatus.Children.Clear();
            foreach (StatusOrder s in Prn212medicineContext.INSTANCE.StatusOrders)
            {
                RadioButton radioButton = new RadioButton()
                {
                    Content = s.StatusName,
                    Tag = s.Id,
                    IsChecked = (s.Id == 1)
                };
                radioButton.Checked += Rd_Checked;
                spnStatus.Children.Add(radioButton);
            }
        }

        private void Rd_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                selectedStatusId = (int)radioButton.Tag;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string selectedStatusName = Prn212medicineContext.INSTANCE.StatusOrders
                                     .FirstOrDefault(s => s.Id == selectedStatusId).StatusName;

            var result = MessageBox.Show($"Confirm order status, change status to {selectedStatusName}?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                controllOrder.ChangeStatusOrder(orderId, selectedStatusId);
                MessageBox.Show("Confirm order status successfully");
                this.Close();  
            }
                
        }
    }
}
