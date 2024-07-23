using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        InfoAccControll controllAcc;
        public CreateAccount()
        {
            InitializeComponent();
            controllAcc = new InfoAccControll();
            LoadRole();
        }

        private void LoadRole()
        {
            List<Role> listRole = Prn212medicineContext.INSTANCE.Roles.ToList();
            cbxRole.ItemsSource = listRole;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtFullname.Text;
            string pass = txtPassword.Text;
            string rePass = txtRePassword.Text;
            if (!pass.Equals(rePass))
            {
                MessageBox.Show("Password and repassword not match!");
                return;
            }
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            if (cbxRole.SelectedItem == null)
            {
                MessageBox.Show("Please select role");
                return;
            }
            Role role = cbxRole.SelectedItem as Role;
            int roleId = role.RoleId;

            string walletRaw = txtWallet.Text;
            if (walletRaw.IsNullOrEmpty())
            {
                MessageBox.Show("Enter the amount in the wallet");
                return;
            }
            decimal wallet = decimal.Parse(walletRaw);

            bool checkAcc = controllAcc.ValidateAccount(username, pass, email, phone, wallet);
            if (checkAcc)
            {
                controllAcc.addAcc(username, pass, email, phone, roleId, wallet);
                MessageBox.Show("Add account succesfully");
                DialogResult = true;
                Close();
            }

        }
    }
}
