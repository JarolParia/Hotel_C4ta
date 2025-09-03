using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hotel_C4ta.Model;
using Hotel_C4ta.Controller;
using Hotel_C4ta.View.AdminViews;
using Hotel_C4ta.View.ReceptionistViews;
using System.Windows.Media.Animation;

namespace Hotel_C4ta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AuthController _authController = new AuthController();
        public MainWindow()
        {
            InitializeComponent();

        }
  

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            string? UserType = _authController.HandleLogin(username, password);

            if (UserType == "Admin")
            {

                AdminPanel admin = new AdminPanel();
                admin.Show();
                this.Close();
            }
            else if (UserType == "Recepcionist")
            {
                ReceptionistPanel recep = new ReceptionistPanel();
                recep.Show();
                this.Close();
            }
            else { 
            
                MessageBox.Show("Invalid credentials, please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

         
        }

      
       

    }
}