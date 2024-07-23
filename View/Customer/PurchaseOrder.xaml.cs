using ProjectPrn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProjectPrn.View.Customer
{
    public partial class PurchaseOrder : Window
    {
        private int accountId;

        public PurchaseOrder(int id)
        {
            InitializeComponent();
            accountId = id;
            LoadOrderHistory();
        }

        private void LoadOrderHistory()
        {
            using (var context = new Prn212medicineContext())
            {
                var orderHistory = (from or in context.OrderHistories
                                    join pm in context.PaymentMethods on or.PaymentMethod equals pm.Id
                                    where or.AccountId == accountId
                                    select new 
                                    {
                                        Id = or.Id,
                                        AccountId = or.AccountId,
                                        Amount = or.Amount,
                                        OrderDate = or.OrderDate,
                                        PaymentDate = or.PaymentDate,
                                        PaymentMethod = pm.Id == 1 ? "COD" : "Direct Payment",
                                        Status = or.Status == 1 ? "Processing" : or.Status == 2 ? "Delivering" : or.Status ==3 ? "Complete" : "Denied"
                                    }).ToList();

                dgvOrderHistory.ItemsSource = orderHistory;
            }
        }
    }

}
