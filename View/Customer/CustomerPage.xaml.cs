using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using ProjectPrn.Controller;
using ProjectPrn.Models;
using ProjectPrn.View.Admin;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ProjectPrn.View.Customer
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Window
    {
        private int currentPage = 1;
        private int itemsPerPage = 10;
        private List<MedicineDisplayModel> allMedicines;
        private MedicineControll controlMedicine;
        private int accountId;
        private CartControll controlCart;
        public CustomerPage(int id)
        {
            InitializeComponent();
            accountId = id;
            controlMedicine = new MedicineControll();
            LoadTypeMedicine();
            LoadMedicine();
            controlCart = new CartControll();
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

        private void btnViewProfile_Click(object sender, RoutedEventArgs e)
        {
            ViewProfile dialog = new ViewProfile(accountId);
            dialog.Show();
        }

        private IQueryable<MedicineDisplayModel> GetJoinedMedicineData()
        {
            DbSet<Medecine> medecines = Prn212medicineContext.INSTANCE.Medecines;
            DbSet<MedicineType> medicineTypes = Prn212medicineContext.INSTANCE.MedicineTypes;

            var joinData = from me in medecines
                           join ty in medicineTypes on me.TypeId equals ty.Id
                           select new MedicineDisplayModel
                           {
                               Id = me.Id,
                               Name = me.Name,
                               Decription = me.Decription,
                               MinAge = me.MinAge,
                               Quantity = me.Quantity,
                               Price = me.Price,
                               ExpiredDate = me.ExpiredDate,
                               Status = me.Status,
                               Type = ty.Type
                           };
            return joinData;
        }

        private void LoadMedicine()
        {
            allMedicines = GetJoinedMedicineData().ToList();
            DisplayPage(currentPage);
        }

        private void LoadTypeMedicine()
        {
            DbSet<MedicineType> medicineTypes = Prn212medicineContext.INSTANCE.MedicineTypes;
            var typesList = medicineTypes.ToList();
            typesList.Insert(0, new MedicineType { Id = 0, Type = "All Types" });
            cbxType.ItemsSource = typesList;
            cbxType.DisplayMemberPath = "Type";
        }

        private void cbxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxType.SelectedItem != null)
            {
                var selectedType = cbxType.SelectedItem as MedicineType;
                if (selectedType.Type == "All Types")
                {
                    allMedicines = GetJoinedMedicineData().ToList();
                }
                else
                {
                    allMedicines = GetJoinedMedicineData().Where(x => x.Type == selectedType.Type).ToList();
                }
                currentPage = 1;
                DisplayPage(currentPage);
            }
        }

        private void DisplayPage(int pageNumber)
        {
            var paginatedData = allMedicines.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            dgvDisplay.ItemsSource = paginatedData;

            int totalPages = (int)Math.Ceiling((double)allMedicines.Count / itemsPerPage);
            lstPageNumbers.ItemsSource = Enumerable.Range(1, totalPages).ToList();
            lstPageNumbers.SelectedItem = pageNumber;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            allMedicines = GetJoinedMedicineData().Where(m => m.Name.ToLower().Contains(searchText)).ToList();
            currentPage = 1;
            DisplayPage(currentPage);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayPage(currentPage);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)allMedicines.Count / itemsPerPage);
            if (currentPage < totalPages)
            {
                currentPage++;
                DisplayPage(currentPage);
            }
        }

        private void lstPageNumbers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPageNumbers.SelectedItem != null)
            {
                currentPage = (int)lstPageNumbers.SelectedItem;
                DisplayPage(currentPage);
            }
        }

        private void btnMyOrder_Click(object sender, RoutedEventArgs e)
        {
            ViewOrder viewOrder = new ViewOrder(accountId);
            if (viewOrder.ShowDialog() == true)
            {
                LoadMedicine();
            }
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            MedicineDisplayModel selectedMedicine = dgvDisplay.SelectedItem as MedicineDisplayModel;
            if (selectedMedicine != null)
            {
                int medicineId = selectedMedicine.Id;
                int availableQuantity = selectedMedicine.Quantity;
                InputQuantityMedicine dialog = new InputQuantityMedicine(availableQuantity);

                if (dialog.ShowDialog() == true)
                {
                    var existedMedicine = Prn212medicineContext.INSTANCE.Carts
                        .FirstOrDefault(x => x.MedicineId == medicineId && x.AccountId == accountId);
                    int quantity = dialog.Quantity;
                    if (existedMedicine != null)
                    {
                        controlCart.NewQuantity(accountId, medicineId, quantity);
                    }
                    else
                    {
                        controlCart.AddCart(accountId, medicineId, quantity);
                    }

                    MessageBox.Show("Add to cart successful");
                }
            }
            else
            {
                MessageBox.Show("Please select medicine to add to cart");
                return;
            }
        }

        private void btnPurchasedOrder_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrder dialog = new PurchaseOrder(accountId);
            dialog.Show();
        }
    }


}