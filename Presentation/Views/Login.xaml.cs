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
using Hotel_C4ta.Presentation.Views.AdminViews;
using Hotel_C4ta.Presentation.Views.ReceptionistViews;

namespace Hotel_C4ta.Presentation.Views
{
    /// Interaction logic for Login.xaml
    public partial class MainWindow : Window
    {
        private readonly ServiceManager _services;
        public MainWindow(ServiceManager services)
        {
            InitializeComponent(); /// Initializes all XAML UI components
            _services = services;
        }

        /// Event handler for the "Join" (Login) button click
        private void Join_Click(object sender, RoutedEventArgs e)
        {
            /// Get values entered by the user
            try
            {
                var email = txtEmail.Text;
                var password = txtPassword.Password;

                /// Try to log in the user using UserService
                var user = _services.UserService.Login(email, password);

                /// If login is successful (user is not null)
                if (user != null)
                {
                    /// If user role is Admin -> open AdminPanel
                    if (user.Rol == "Admin")
                    {
                        new AdminPanel(_services, user).Show(); /// Show Admin panel
                        this.Close(); /// Close login window
                    }
                    else if (user.Rol == "Recep") /// If user role is Receptionist -> open ReceptionistPanel
                    {
                        new ReceptionistPanel(_services, user).Show(); /// If user role is Receptionist -> open ReceptionistPanel
                        this.Close(); /// Close login window
                    }
                }
                else
                {
                    /// If login failed, show error message
                    MessageBox.Show("Incorrect Credentials.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                /// Catch any unexpected error (like DB issues) and show it
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
