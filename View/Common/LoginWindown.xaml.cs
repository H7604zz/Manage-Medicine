using ProjectPrn.Models;
using ProjectPrn.View.Admin;
using ProjectPrn.View.Customer;
using ProjectPrn.View.Sale;
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

namespace ProjectPrn.View.Common
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindown : Window
    {
        private List<InfoAcc> accList;
        public LoginWindown()
        {
            InitializeComponent();
            accList = Prn212medicineContext.INSTANCE.InfoAccs.ToList();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string pass = chkShowPassword.IsChecked == true ? txtPasswordVisible.Text : txtPassword.Password;

            using (var context = new Prn212medicineContext())
            {
                var user = context.InfoAccs.FirstOrDefault(x => x.Email == email && x.Password == pass);
                if(user == null)
                {
                    MessageBox.Show("Email and Password incorrect!");
                    return;
                }
                else
                {
                    int id = user.Id;
                    //admin
                    if(user.RoleId == 1)
                    {
                        //admin page
                        var dialog = new AdminPage(id);
                        dialog.Show();
                        this.Close();
                    }
                    //customer
                    if (user.RoleId == 2)
                    {
                        //customer page
                        var dialog = new CustomerPage(id);
                        dialog.Show();
                        this.Close();
                    }
                    //seller
                    if (user.RoleId == 3)
                    {
                        //customer page
                        var dialog = new SallerPage();
                        dialog.Show();
                        this.Close();
                    }
                }
            }
        }

        private void chkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            txtPasswordVisible.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Hidden;
            txtPasswordVisible.Visibility = Visibility.Visible;
        }

        private void chkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = txtPasswordVisible.Text;
            txtPassword.Visibility = Visibility.Visible;
            txtPasswordVisible.Visibility = Visibility.Hidden;
        }

        private void ForgetPw_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword dialog = new ForgotPassword();
            dialog.ShowDialog();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterAcc dialog = new RegisterAcc();
            dialog.ShowDialog();
        }
    }
}
