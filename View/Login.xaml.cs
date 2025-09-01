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

namespace Hotel_C4ta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Join_Click(object sender, RoutedEventArgs e)
        {
            // Lógica de autenticación aquí
            // Ejemplo: MessageBox.Show("Botón JOIN pulsado");
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdatePasswordPlaceholder();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            PlaceholderPassword.Visibility = Visibility.Hidden;
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdatePasswordPlaceholder();
        }

        private void UpdatePasswordPlaceholder()
        {
            if (string.IsNullOrEmpty(txtPassword.Password) && !txtPassword.IsFocused)
                PlaceholderPassword.Visibility = Visibility.Visible;
            else
                PlaceholderPassword.Visibility = Visibility.Hidden;
        }

    }
}