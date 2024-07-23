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

namespace ProjectPrn.View.Customer
{
    /// <summary>
    /// Interaction logic for ViewProfile.xaml
    /// </summary>
    public partial class ViewProfile : Window
    {
        private int accountId;
        private InfoAccControll controllAcc;
        public ViewProfile(int id)
        {
            InitializeComponent();
            this.accountId = id;
            DataContext = Prn212medicineContext.INSTANCE.InfoAccs.FirstOrDefault(x => x.Id == accountId); 
            controllAcc = new InfoAccControll();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)this.accountId;
            string username = txtFullname.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;
            string phone = txtPhone.Text;
            decimal wallet = decimal.Parse(txtWallet.Text);
            controllAcc.UpdateAccount(id, username, password, email, phone, 2, wallet);
            MessageBox.Show("Save succesfully");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
