using ProjectPrn.Models;
using System.Windows;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;
using System.Net.Http;
using ProjectPrn.Controller;

namespace ProjectPrn.View.Common
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        Prn212medicineContext context;
        InfoAccControll controlAcc;
        private DateTime codeGenerationTime;
        public ForgotPassword()
        {
            InitializeComponent();
            context = Prn212medicineContext.INSTANCE;
            controlAcc = new InfoAccControll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            var existEmail = context.InfoAccs.FirstOrDefault(x => x.Email == email);
            if (existEmail == null)
            {
                MessageBox.Show("Email not existed");
                return;
            }
            else
            {
                string code = controlAcc.GenerateCode();
                codeGenerationTime = DateTime.Now;
                controlAcc.SendCodeEmail(email, code);
                ConfirmCode dialog = new ConfirmCode(code, codeGenerationTime);
                if (dialog.ShowDialog() == true)
                {
                    RequestEmail(existEmail.Email);
                }
                    
            }

        }

        private void RequestEmail(string recipientEmail)
        {
            try
            {
                string pass = generatePassword();
                EmailUtil.SendEmail(recipientEmail, "Password Recovery", pass);
                controlAcc.changePass(recipientEmail, pass);
                MessageBox.Show("Email sent successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email. Error: " + ex.Message);
            }
        }

        private string generatePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%&*()";

            Random random = new Random();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                int index = random.Next(chars.Length);
                result.Append(chars[index]);
            }

            return result.ToString();
        }
    }
}
