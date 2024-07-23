using Microsoft.EntityFrameworkCore;
using ProjectPrn.Controller;
using ProjectPrn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjectPrn.View.Customer
{
    public partial class ViewOrder : Window, INotifyPropertyChanged
    {
        private int accountId;
        private decimal totalPrice;
        private CartControll controllCart;
        private OrderControll controllOrder;
        public ViewOrder(int id)
        {
            InitializeComponent();
            accountId = id;
            LoadListCart();
            controllCart = new CartControll();
            controllOrder = new OrderControll();
            LoadPaymentMethod();
        }

        private void LoadListCart()
        {
            List<CartItemDisplayModel> cartItems = GetCartItems(accountId);
            foreach (var cartItem in cartItems)
            {
                cartItem.PropertyChangedCallback = () => UpdateTotalPrice(cartItems);
            }
            dgvCartItems.ItemsSource = cartItems;
            UpdateTotalPrice(cartItems);
        }

        private List<CartItemDisplayModel> GetCartItems(int accountId)
        {
            using (var context = new Prn212medicineContext())
            {
                var cartItems = (from cart in context.Carts
                                 join medicine in context.Medecines on cart.MedicineId equals medicine.Id
                                 where cart.AccountId == accountId
                                 select new CartItemDisplayModel
                                 {
                                     CartId = cart.Id,
                                     MedicineId = medicine.Id,
                                     MedicineName = medicine.Name,
                                     Price = medicine.Price,
                                     Quantity = cart.Quantity
                                 }).ToList();

                return cartItems;
            }
        }

        private void UpdateTotalPrice(IEnumerable<CartItemDisplayModel> cartItems)
        {
            GrandTotal = cartItems.Sum(item => item.Quantity * item.Price);
        }

        public decimal GrandTotal
        {
            get => totalPrice;
            set
            {
                if (totalPrice != value)
                {
                    totalPrice = value;
                    OnPropertyChanged(nameof(GrandTotal));
                    txtGrandTotal.Text = totalPrice.ToString("C2");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnDeleteCart_Click(object sender, RoutedEventArgs e)
        {
            var cartItem = dgvCartItems.SelectedItem as CartItemDisplayModel;

            if (cartItem != null)
            {
                int cartId = cartItem.CartId;
                controllCart.DeleteCart(cartId);
                LoadListCart();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbxPaymentMethod.SelectedValue != null)
            {
                int paymentMethodId = (int)cbxPaymentMethod.SelectedValue;
                var result = MessageBox.Show("Order confirmation?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (PlaceOrder(accountId, paymentMethodId))
                    {
                        DialogResult = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a payment method.");
            }
        }

        private void LoadPaymentMethod()
        {
            var methodList = Prn212medicineContext.INSTANCE.PaymentMethods.ToList();
            cbxPaymentMethod.ItemsSource = methodList;
            cbxPaymentMethod.DisplayMemberPath = "PaymentMethodName";
        }

        private bool PlaceOrder(int accountId, int paymentMethodId)
        {
            using (var context = new Prn212medicineContext())
            {
                // Get the current cart items
                var cartItems = GetCartItems(accountId);

                var account = context.InfoAccs.FirstOrDefault(a => a.Id == accountId);
                //only check wallet with payment method bank (=2)
                if (paymentMethodId == 2)
                {
                    //check wallet
                    if (account.Wallet < totalPrice)
                    {
                        MessageBox.Show("Insufficient balance to pay for this order.");
                        return false;
                    }
                }

                // Check if medicine quantities are sufficient
                foreach (var item in cartItems)
                {
                    var medicine = context.Medecines.SingleOrDefault(m => m.Id == item.MedicineId);
                    if (medicine.Quantity < item.Quantity)
                    {
                        MessageBox.Show($"Insufficient quantity for {item.MedicineName}.");
                        return false;
                    }
                }

                // Determine payment date based on payment method
                DateOnly? paymentDate = paymentMethodId == 2 ? DateOnly.FromDateTime(DateTime.Now) : (DateOnly?)null;

                // Create a new order
                var newOrder = new OrderHistory
                {
                    AccountId = accountId,
                    Amount = cartItems.Sum(item => item.Price * item.Quantity),
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    PaymentDate = paymentDate,
                    PaymentMethod = paymentMethodId,
                    Status = 1
                };
                context.OrderHistories.Add(newOrder);
                context.SaveChanges();

                // Add order details and update medicine quantities
                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderHistoryDetail
                    {
                        OrderId = newOrder.Id,
                        MedicineId = item.MedicineId,
                        PurchasePrice = item.Price,
                        Quantity = item.Quantity
                    };
                    context.OrderHistoryDetails.Add(orderDetail);

                    // Update medicine quantity
                    var medicine = context.Medecines.SingleOrDefault(m => m.Id == item.MedicineId);
                    if (medicine != null)
                    {
                        medicine.Quantity -= item.Quantity;
                    }
                }
                context.SaveChanges();

                // Deduct amount from wallet
                account.Wallet -= totalPrice;
                context.SaveChanges();

                // Clear the cart
                var cartsToRemove = context.Carts.Where(c => c.AccountId == accountId);
                context.Carts.RemoveRange(cartsToRemove);
                context.SaveChanges();

                // Reload cart items (which should now be empty)
                LoadListCart();
                return true;
            }
        }

    }

    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        private int quantity;
        public int CartId { get; init; }
        public int MedicineId { get; init; }
        public string MedicineName { get; set; }
        public decimal Price { get; set; }

        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    PropertyChangedCallback?.Invoke();
                }
            }
        }

        public Action PropertyChangedCallback { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
