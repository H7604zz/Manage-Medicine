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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProjectPrn.View.Common
{
    /// <summary>
    /// Interaction logic for RegisterAcc.xaml
    /// </summary>
    public partial class RegisterAcc : Window
    {
        InfoAccControll controlAcc;
        private DateTime codeGenerationTime;
        public RegisterAcc()
        {
            InitializeComponent();
            controlAcc = new InfoAccControll();  
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;

            var x = Prn212medicineContext.INSTANCE.InfoAccs.FirstOrDefault(x => x.Email == email);
            if (x != null)
            {
                MessageBox.Show("Email is already exist");
                return;
            }
            string pass = txtPassword.Password;
            string repass = txtRePassword.Password;
            
            if (!pass.Equals(repass))
            {
                MessageBox.Show("Password and RePassword not match. Try again!");
                return;
            }

            string code = controlAcc.GenerateCode();
            codeGenerationTime = DateTime.Now;
            controlAcc.SendCodeEmail(email, code);
            ConfirmCode dialog = new ConfirmCode(code, codeGenerationTime);
            if (dialog.ShowDialog() == true)
            {
                string username = txtFullname.Text;
                string phone = txtPhone.Text;

                controlAcc.addAcc(username, pass, email, phone, 2, 0);
                MessageBox.Show("Register sucessfull");

            }
        }

        
    }
}
