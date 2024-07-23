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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectPrn.View.Admin
{
    /// <summary>
    /// Interaction logic for ManageAccount.xaml
    /// </summary>
    public partial class ManageAccount : Page
    {
        
        public ManageAccount()
        {
            InitializeComponent();
            LoadAccount();
        }

        private void LoadAccount()
        {
            DbSet<Role> roles = Prn212medicineContext.INSTANCE.Roles;
            DbSet<InfoAcc> infoAccs = Prn212medicineContext.INSTANCE.InfoAccs;

            var joinData = from acc in infoAccs
                           join ro in roles
                           on acc.RoleId equals ro.RoleId
                           select new
                           {
                               acc.Id,
                               acc.Username,
                               acc.Password,
                               acc.Email,
                               acc.Wallet,
                               acc.Phone,
                               acc.RoleId,
                               ro.RoleName
                           };
            dgvDisplay.ItemsSource = joinData.ToList();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text;
            var context = new Prn212medicineContext();
            var listSearch = context.InfoAccs.Where(c => c.Username.Contains(searchText) ||
                            c.Email.Contains(searchText) || c.Phone.Contains(searchText)
                            ).ToList();

            dgvDisplay.ItemsSource = listSearch;
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount dialog = new CreateAccount();
            if (dialog.ShowDialog() == true)
            {
                LoadAccount();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgvDisplay.SelectedItem as dynamic;
            if (selectedItem != null)
            {
                InfoAcc acc = new InfoAcc
                {
                    Id = selectedItem.Id,
                    Username = selectedItem.Username,
                    Password = selectedItem.Password,
                    Email = selectedItem.Email,
                    Phone = selectedItem.Phone,
                    Wallet = selectedItem.Wallet,
                    RoleId = selectedItem.RoleId,
                };
                UpdateAccount dialog = new UpdateAccount(acc);
                {
                    if(dialog.ShowDialog() == true)
                    {
                        LoadAccount();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select account to update");
                return;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgvDisplay.SelectedItem as dynamic;
            if (selectedItem != null)
            {
                InfoAcc acc = new InfoAcc
                {
                    Id = selectedItem.Id,
                    Username = selectedItem.Username,
                    Password = selectedItem.Password,
                    Email = selectedItem.Email,
                    Phone = selectedItem.Phone,
                    Wallet = selectedItem.Wallet,
                    RoleId = selectedItem.RoleId,
                };
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {acc.Email}?",
                                        "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Prn212medicineContext.INSTANCE.InfoAccs.Remove(acc);
                    Prn212medicineContext.INSTANCE.SaveChanges();
                    LoadAccount();
                }
            }
            else
            {
                MessageBox.Show("Please select account to delete");
                return;
            }
            
        }
    }
}
