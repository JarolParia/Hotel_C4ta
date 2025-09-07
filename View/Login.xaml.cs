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
        private readonly LoginController _loginController = new LoginController();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            int id;
            string email = txtEmail.Text;
            string password = txtPassword.Password;

            try
            {
                var user = _loginController.HandleLogin(email, password);

                if (user.rol == "Admin")
                {

                    AdminPanel admin = new AdminPanel();
                    admin.Show();
                    this.Close();
                }
                else if (user.rol == "Recep")
                {
                    ReceptionistPanel recep = new ReceptionistPanel(user.id, user.fullname);
                    recep.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid credentials, please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
    }
}