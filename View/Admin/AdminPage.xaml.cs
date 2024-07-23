using ProjectPrn.Controller;
using ProjectPrn.View.Common;
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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        
        public AdminPage(int id)
        {
            InitializeComponent();
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e)
        {
            frAdmin.Content = new ManageAccount();
        }

        private void btnMedicine_Click(object sender, RoutedEventArgs e)
        {
            frAdmin.Content = new ManageMedicine();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the login screen
            var loginWindow = new LoginWindown();
            loginWindow.Show();

            // Close the current window
            var currentWindow = Window.GetWindow(this);
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            ViewOrder dialog = new ViewOrder();
            frAdmin.Content = dialog;
        }
    }
}
