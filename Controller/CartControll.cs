using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectPrn.Models;

namespace ProjectPrn.Controller
{
    public class CartControll
    {
        public void AddCart(int accountId, int medicineId, int quantity)
        {
            using (var context = new Prn212medicineContext())
            {
                var cart = new Cart
                {
                    AccountId = accountId,
                    MedicineId = medicineId,
                    Quantity = quantity
                };
                context.Carts.Add(cart);
                context.SaveChanges();
            }
        }

        public void NewQuantity(int accountId, int medicineId, int quantity)
        {
            int oldQuantity = Prn212medicineContext.INSTANCE.Carts
                                                    .Where(c => c.AccountId == accountId && c.MedicineId == medicineId)
                                                    .Select(c => c.Quantity).FirstOrDefault();
            var cart = Prn212medicineContext.INSTANCE.Carts.FirstOrDefault(c => c.AccountId == accountId &&  c.MedicineId == medicineId);
            if (cart != null)
            {
                cart.Quantity = quantity + oldQuantity;
                Prn212medicineContext.INSTANCE.Update(cart);
                Prn212medicineContext.INSTANCE.SaveChanges();
            }
            
        }

        public void DeleteCart(int cartItemId)
        {
            var cart = Prn212medicineContext.INSTANCE.Carts.FirstOrDefault(c => c.Id == cartItemId);
            if (cart != null)
            {
                Prn212medicineContext.INSTANCE.Remove(cart);
                Prn212medicineContext.INSTANCE.SaveChanges();
            }
        }
    }
}
