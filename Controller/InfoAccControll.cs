using Microsoft.EntityFrameworkCore;
using ProjectPrn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ProjectPrn.Controller
{
    public class InfoAccControll
    {
        public void changePass(string email, string pass)
        {
            var x = Prn212medicineContext.INSTANCE.InfoAccs.FirstOrDefault(c => c.Email == email);
            if (x != null)
            {
                x.Password = pass;
                Prn212medicineContext.INSTANCE.Update(x);
                Prn212medicineContext.INSTANCE.SaveChanges();
            }
        }

        public void addAcc(string username, string pass, string email, string phone, int roleId, decimal wallet)
        {
            var acc = new InfoAcc
            {
                Username = username.Trim(),
                Password = pass.Trim(),
                Email = email.Trim(),
                Phone = phone,
                RoleId = roleId,
                Wallet = wallet
            };

            Prn212medicineContext.INSTANCE.Add(acc);
            Prn212medicineContext.INSTANCE.SaveChanges();
        }

        public bool ValidateAccount(string username, string pass, string email, string phone, decimal wallet)
        {
            // Check if username is not empty
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username cannot be empty.");
                return false;
            }

            // Check if password is at least 8 characters long
            if (pass.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.");
                return false;
            }

            // Check if email has a valid email format
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.");
                return false;
            }

            // check email existed
            var x = Prn212medicineContext.INSTANCE.InfoAccs.FirstOrDefault(x => x.Email == email);
            if (x != null)
            {
                MessageBox.Show("Email already exists");
                return false;
            }

            // Check if phone number is exactly 10 digits
            if (!IsValidPhone(phone))
            {
                MessageBox.Show("Phone number must be exactly 10 digits.");
                return false;
            }
            
            //check phone existed
            var y = Prn212medicineContext.INSTANCE.InfoAccs.FirstOrDefault(x => x.Phone == phone);
            if (y != null)
            {
                MessageBox.Show("Phone already exists");
                return false;
            }

            // Additional validation for wallet if needed
            if (wallet < 0)
            {
                MessageBox.Show("Wallet balance cannot be negative.");
                return false;
            }

            return true;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return phone.All(char.IsDigit) && phone.Length == 10;
        }

        public void UpdateAccount(int id, string username, string password, string newEmail, string newPhone, int roleId, decimal wallet)
        {
            var account = Prn212medicineContext.INSTANCE.InfoAccs.FirstOrDefault(acc => acc.Id == id);
            if (account != null)
            {
                account.Username = username;
                account.Password = password;
                account.Email = newEmail;
                account.Phone = newPhone;
                account.Wallet = wallet;
                account.RoleId = roleId;

                Prn212medicineContext.INSTANCE.InfoAccs.Update(account);
                Prn212medicineContext.INSTANCE.SaveChanges();
            }
        }

        public void SendCodeEmail(string email, string code)
        {
            try
            {
                EmailUtil.SendEmail(email, "Email confirmation code", code);
                changePass(email, code);
                MessageBox.Show("Email sent successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send email. Error: " + ex.Message);
            }
        }

        public string GenerateCode()
        {
            string rs = "";
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                rs += random.Next(0, 9);
            }
            return rs;
        }

    }
}
