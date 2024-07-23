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
    /// Interaction logic for ConfirmCode.xaml
    /// </summary>
    public partial class ConfirmCode : Window
    {
        private string confirmCode;
        private DateTime codeGenerationTime;
        public ConfirmCode(string code, DateTime time)
        {
            InitializeComponent();
            confirmCode = code;
            codeGenerationTime = time;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DateTime.Now.Subtract(codeGenerationTime).TotalMinutes > 2)
            {
                MessageBox.Show("Code expired.");
                this.DialogResult = false;
                this.Close();
                return;
            }

            string inputCode = txtCode.Text;
            if (inputCode.Equals(confirmCode))
            {
                MessageBox.Show("Check code successfully");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Code incorrect");
            }
        }
    }
}
