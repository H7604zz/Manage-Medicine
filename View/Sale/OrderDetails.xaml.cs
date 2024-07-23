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
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : Window
    {
        public OrderDetails(int orderId)
        {
            InitializeComponent(); 
            LoadOrderDetails(orderId);
        }

        private void LoadOrderDetails(int orderId)
        {
            using (var context = new Prn212medicineContext())
            {
                var orderDetails = from ohd in context.OrderHistoryDetails
                                   join med in context.Medecines on ohd.MedicineId equals med.Id
                                   where ohd.OrderId == orderId
                                   select new
                                   {
                                       med.Name,
                                       ohd.Quantity,
                                       ohd.PurchasePrice
                                   };
              
                dgvOrderDetails.ItemsSource = orderDetails.ToList();
            }
        }
    }
}
