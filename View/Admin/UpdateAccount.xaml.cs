using ProjectPrn.Controller;
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
    /// Interaction logic for UpdateAccount.xaml
    /// </summary>
    public partial class UpdateAccount : Window
    {
        InfoAcc currentAccount;
        InfoAccControll controlAccount;
        public UpdateAccount(InfoAcc account)
        {
            InitializeComponent();
            LoadRole();
            currentAccount = account;
            DataContext = currentAccount;
            controlAccount = new InfoAccControll();
        }

        private void LoadRole()
        {
            List<Role> listRole = Prn212medicineContext.INSTANCE.Roles.ToList();
            cbxRole.ItemsSource = listRole;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to update this account?",
                    "Update Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string newPhone = txtPhone.Text;
                string newEmail = txtEmail.Text;
                string newFullName = txtFullname.Text;
                string newPassword = txtPassword.Text;
                decimal newWallet = decimal.Parse(txtWallet.Text);

                Role role = cbxRole.SelectedItem as Role;
                controlAccount.UpdateAccount(currentAccount.Id, newFullName, newPassword, newEmail, newPhone, role.RoleId, newWallet);
                MessageBox.Show("Update account successfully");
                DialogResult = true;
                Close();
            }
        }

    }
}
