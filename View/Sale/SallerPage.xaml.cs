using Microsoft.EntityFrameworkCore;
using ProjectPrn.Models;
using ProjectPrn.View.Common;
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
    /// Interaction logic for SallerPage.xaml
    /// </summary>
    public partial class SallerPage : Window
    {
        public SallerPage()
        {
            InitializeComponent();
            LoadPendingOrders();
        }

        private void LoadPendingOrders()
        {
            DbSet<InfoAcc> infoAccs = Prn212medicineContext.INSTANCE.InfoAccs;
            DbSet<OrderHistory> orderHistories = Prn212medicineContext.INSTANCE.OrderHistories;
            DbSet<PaymentMethod> paymentMethods = Prn212medicineContext.INSTANCE.PaymentMethods;
            DbSet<StatusOrder> statusOrders = Prn212medicineContext.INSTANCE.StatusOrders;

            var dataDisplay = from or in orderHistories
                              join acc in infoAccs on or.AccountId equals acc.Id
                              join pm in paymentMethods on or.PaymentMethod equals pm.Id
                              join so in statusOrders on or.Status equals so.Id
                              select new
                              {
                                  or.Id,
                                  acc.Email,
                                  or.Amount,
                                  or.OrderDate,
                                  or.PaymentDate,
                                  pm.PaymentMethodName,
                                  or.Status,
                                  so.StatusName,
                              };
            dgvDisplay.ItemsSource = dataDisplay.Where(c => c.Status == 1).ToList();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the login screen
            var loginWindow = new LoginWindown();
            loginWindow.Show();

            // Close the current window
            var currentWindow = Window.GetWindow(this);
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag != null)
            {
                int orderId = (int)button.Tag;
                OrderDetails detailsWindow = new OrderDetails(orderId);
                detailsWindow.Show();
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = dgvDisplay.SelectedItem as dynamic;
            if (selectedOrder != null)
            {
                var order = new OrderHistory
                {
                    Id = selectedOrder.Id
                };

                ApproveOrders dialog = new ApproveOrders(order.Id);
                if (dialog.ShowDialog() == true)
                {
                    LoadPendingOrders();
                }
            }
            else
            {
                MessageBox.Show("please choose order");
                return;
            }
        }
    }
}
