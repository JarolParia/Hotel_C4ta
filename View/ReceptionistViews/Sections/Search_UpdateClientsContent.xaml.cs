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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hotel_C4ta.Controller;
using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;

namespace Hotel_C4ta.View.ReceptionistViews.Sections
{

    public partial class Search_UpdateClientsContent : UserControl
    {
        private string _selectedDni = null;
        private readonly ClientController _clientController = new ClientController();

        public Search_UpdateClientsContent()
        {
            InitializeComponent();
            LoadClients();
        }
        private void LoadClients()
        {
            var clients = _clientController.GetAllClients();
            ClientsGrid.ItemsSource = clients;
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is ClientModel client)
            {
                _selectedDni = client.DNI;
                TxtDni.Text = client.DNI;
                TxtName.Text = client.FullName;
                TxtEmail.Text = client.Email;
                TxtPhone.Text = client.Phone;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_selectedDni))
            {
                var actualDni = _selectedDni;
                var newDni = TxtDni.Text;
                var fullname = TxtName.Text;
                var email = TxtEmail.Text;
                var phone = TxtPhone.Text;

                if(newDni != actualDni)
                {
                    MessageBox.Show("You don't have permission to update DNI's.");
                    return;
                }

                _clientController.UpdateClient(actualDni, fullname, email, phone);
            }
            else
            {
                MessageBox.Show("Select a client.");
                return;
            }
            LoadClients();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_selectedDni))
            {
                var confirm = MessageBox.Show("Delete this Client?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm != MessageBoxResult.Yes) return;

                var dni = _selectedDni;

                _clientController.DeleteClient(dni);
            }
            else
            {
                MessageBox.Show("Select a client.");
                return;
            }
            LoadClients();
        }
    }
}