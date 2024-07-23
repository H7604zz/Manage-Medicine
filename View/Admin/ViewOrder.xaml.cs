using Microsoft.EntityFrameworkCore;
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

namespace ProjectPrn.View.Admin
{
    /// <summary>
    /// Interaction logic for ViewOrder.xaml
    /// </summary>
    public partial class ViewOrder : Page
    {
        public ViewOrder()
        {
            InitializeComponent();
            LoadOrder();
            LoadOrderStatus();
        }

        private void cbxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectStatus = cbxStatus.SelectedItem as StatusOrder;
            if (selectStatus != null)
            {
                DbSet<OrderHistory> orderHistories = Prn212medicineContext.INSTANCE.OrderHistories;
                DbSet<InfoAcc> infoAccs = Prn212medicineContext.INSTANCE.InfoAccs;
                DbSet<PaymentMethod> paymentMethods = Prn212medicineContext.INSTANCE.PaymentMethods;
                DbSet<StatusOrder> statusOrders = Prn212medicineContext.INSTANCE.StatusOrders;
                var data = from or in orderHistories
                           join ia in infoAccs on or.AccountId equals ia.Id
                           join pm in paymentMethods on or.PaymentMethod equals pm.Id
                           join so in statusOrders on or.Status equals so.Id
                           select new
                           {
                               or.Id,
                               ia.Email,
                               or.Amount,
                               or.OrderDate,
                               pm.PaymentMethodName,
                               so.StatusName
                           };
                dgvDisplay.ItemsSource = data.Where(x => x.StatusName.Equals(selectStatus.StatusName)).ToList();
            }

            
        }

        private void LoadOrderStatus()
        {
            DbSet<StatusOrder> statusOrders = Prn212medicineContext.INSTANCE.StatusOrders;
            cbxStatus.ItemsSource = statusOrders.ToList();
            cbxStatus.DisplayMemberPath = "StatusName";
        }

        private void LoadOrder()
        {
            DbSet<OrderHistory> orderHistories = Prn212medicineContext.INSTANCE.OrderHistories;
            DbSet<InfoAcc> infoAccs = Prn212medicineContext.INSTANCE.InfoAccs;
            DbSet<PaymentMethod> paymentMethods = Prn212medicineContext.INSTANCE.PaymentMethods;
            DbSet<StatusOrder> statusOrders = Prn212medicineContext.INSTANCE.StatusOrders;
            var data = from or in orderHistories
                       join ia in infoAccs on or.AccountId equals ia.Id
                       join pm in paymentMethods on or.PaymentMethod equals pm.Id
                       join so in statusOrders on or.Status equals so.Id
                       select new
                       {
                           or.Id,
                           ia.Email,
                           or.Amount,
                           or.OrderDate,
                           pm.PaymentMethodName,
                           so.StatusName
                       };
            dgvDisplay.ItemsSource = data.ToList();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            Report dialog = new Report();
            dialog.Show();
        }
    }
}
