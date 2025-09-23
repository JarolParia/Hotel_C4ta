using Hotel_C4ta.Application.Services;
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

namespace Hotel_C4ta.Presentation.Views
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServiceManager _services;
        public MainWindow(ServiceManager services)
        {
            InitializeComponent();
            _services = services;
        }


        private void Join_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var email = txtEmail.Text;
                var password = txtPassword.Password;

                var user = _services.UserService.Login(email, password);

                if (user != null)
                {
                    if (user.Rol == "Admin")
                    {
                        MessageBox.Show("Admin panel is under construction.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        // new AdminPanel(_services, user).Show();
                        //this.Close();
                    }
                    else if (user.Rol == "Recep")
                    {
                        MessageBox.Show("Receptionist panel is under construction.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        //    new ReceptionistPanel(_services, user).Show();
                        //  this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect Credentials.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
