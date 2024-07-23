using Microsoft.Identity.Client.NativeInterop;
using ProjectPrn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectPrn.Controller
{

    public class OrderControll
    {
        public void ChangeStatusOrder(int orderId, int status)
        {
            var order = Prn212medicineContext.INSTANCE.OrderHistories.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                // Check if refund is needed
                if (status == 4 && order.PaymentMethod == 2)
                {
                    // Find the account associated with the order
                    var account = Prn212medicineContext.INSTANCE.InfoAccs.FirstOrDefault(a => a.Id == order.AccountId);
                    if (account != null)
                    {
                        // Refund the amount to the account
                        account.Wallet += order.Amount;
                        Prn212medicineContext.INSTANCE.InfoAccs.Update(account);
                    }
                }

                // Update the status
                order.Status = status;
                Prn212medicineContext.INSTANCE.OrderHistories.Update(order);
                Prn212medicineContext.INSTANCE.SaveChanges();
            }
        }
    }

}
