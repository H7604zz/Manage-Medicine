using Microsoft.EntityFrameworkCore;
using ProjectPrn.Controller;
using ProjectPrn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjectPrn.View.Admin
{
    /// <summary>
    /// Interaction logic for ManageMedicine.xaml
    /// </summary>
    public partial class ManageMedicine : Page
    {
        private int currentPage = 1;
        private int itemsPerPage = 10;
        private List<MedicineDisplayModel> allMedicines;
        private MedicineControll controlMedicine;
        public ManageMedicine()
        {
            InitializeComponent();
            LoadTypeMedicine();
            LoadMedicine();
            controlMedicine = new MedicineControll();
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
            cbxType.ItemsSource = typesList;
            cbxType.DisplayMemberPath = "Type";
            cbxTypeMedicine.ItemsSource = typesList;
            cbxTypeMedicine.DisplayMemberPath = "Type";
        }

        private void cbxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxType.SelectedItem != null)
            {
                var selectedType = cbxType.SelectedItem as MedicineType;
                allMedicines = GetJoinedMedicineData().Where(x => x.Type == selectedType.Type).ToList();
                currentPage = 1;
                DisplayPage(currentPage);
            }
        }

        private void DisplayPage(int pageNumber)
        {
            var paginatedData = allMedicines.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
            dgvDisplay.ItemsSource = paginatedData;

            int totalPages = (int)Math.Ceiling((double)allMedicines.Count / itemsPerPage);
            lstPageNumbers.ItemsSource = Enumerable.Range(1, totalPages).ToList(); // Ensure it's a List
            lstPageNumbers.SelectedItem = pageNumber; // Set the selected item to the current page number
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

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to add medicine?",
                "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                string name = txtName.Text;
                if (string.IsNullOrEmpty(name)) 
                {
                    MessageBox.Show("Name medicine cannot empty");
                    return;
                }
                string description = txtDecription.Text;
                if (string.IsNullOrEmpty(description))
                {
                    MessageBox.Show("Name medicine cannot empty");
                    return;
                }
                int minAge = int.Parse(txtAge.Text);
                if(minAge < 0)
                {
                    MessageBox.Show("Min age must be a positive integer");
                    return;
                }
                if(cbxTypeMedicine.SelectedItem == null)
                {
                    MessageBox.Show("Please select a status");
                    return;
                }
                int typeId = ((MedicineType)cbxTypeMedicine.SelectedItem).Id;
                int quantity = int.Parse(txtQuantity.Text);
                if(quantity < 0)
                {
                    MessageBox.Show("Quantity must be greater than 0");
                    return;
                }
                decimal price = decimal.Parse(txtPrice.Text);
                if(price < 0)
                {
                    MessageBox.Show("Price must be greater than 0");
                    return; 
                }
                if(dpkDate.SelectedDate == null)
                {
                    MessageBox.Show("Please select date");
                    return;
                }
                DateOnly date = DateOnly.Parse(dpkDate.Text);
                if(date < DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("Expired date must be greater than today.");
                    return;
                }
                if (cbxStatus.SelectedValue == null)
                {
                    MessageBox.Show("Please select a status");
                    return;
                }
                int status = int.Parse(cbxStatus.SelectedValue.ToString());

                controlMedicine.AddMedicine(name, description, minAge, typeId, quantity, price, date, status);
                MessageBox.Show($"Add {name} succesfully");
                LoadMedicine();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedMedicine = dgvDisplay.SelectedItem as MedicineDisplayModel;
            if (selectedMedicine != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to update medicine {selectedMedicine.Id}?",
                    "Update Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    string name = txtName.Text;
                    string description = txtDecription.Text;
                    int  minAge = int.Parse(txtAge.Text);
                    int typeId = ((MedicineType)cbxTypeMedicine.SelectedItem).Id;
                    int quantity = int.Parse(txtQuantity.Text);
                    decimal price = decimal.Parse(txtPrice.Text);
                    DateOnly expiredDate = DateOnly.Parse(dpkDate.Text);
                    int status = int.Parse(cbxStatus.SelectedValue.ToString());
                    controlMedicine.UpdateMedicine(selectedMedicine.Id, name, description, minAge, typeId, quantity, price, expiredDate, status);
                    MessageBox.Show($"Update {selectedMedicine.Id} succesfully");
                    LoadMedicine();
                }
            }
            else 
            {
                MessageBox.Show("Please select medicine to update");
                return;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedMedicine = dgvDisplay.SelectedItem as MedicineDisplayModel;
            if (selectedMedicine != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete medicine {selectedMedicine.Name}?", 
                    "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    controlMedicine.DeleteMedicine(selectedMedicine.Id);
                    MessageBox.Show($"Delete {selectedMedicine.Name} succesfully");
                    LoadMedicine();
                }
            }
            else
            {
                MessageBox.Show("Please choose medicine to delete");
                return;
            }
        }

        private void btnNewType_Click(object sender, RoutedEventArgs e)
        {
            NewTypeMedicine dialog = new NewTypeMedicine();
            if (dialog.ShowDialog() == true)
            {
                LoadTypeMedicine();
                MessageBox.Show("Add new type medicine successfull");
                dialog.Close();
            }

            
        }
    }

    public class MedicineDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Decription { get; set; }
        public int MinAge { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateOnly ExpiredDate { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
    }
}
