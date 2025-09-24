using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Hotel_C4ta.Presentation.Views.ReceptionistViews.Sections
{
    public partial class Search_UpdateClientsContent : UserControl
    {
        private readonly ClientService _clientService;
        private string _selectedDni = null;

        public Search_UpdateClientsContent(ClientService clientService)
        {
            InitializeComponent();
            _clientService = clientService;
            LoadClients();
        }

        private void LoadClients()
        {
            var clients = _clientService.GetAllClients();
            ClientsGrid.ItemsSource = clients;
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is Client client)
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
            if (string.IsNullOrWhiteSpace(_selectedDni))
            {
                MessageBox.Show("Select a client.");
                return;
            }

            if (TxtDni.Text != _selectedDni)
            {
                MessageBox.Show("You don't have permission to update DNI's.");
                return;
            }

            var updatedClient = new Client
            {
                DNI = TxtDni.Text,
                FullName = TxtName.Text,
                Email = TxtEmail.Text,
                Phone = TxtPhone.Text
            };

            _clientService.UpdateClient(updatedClient);
            LoadClients();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedDni))
            {
                MessageBox.Show("Select a client.");
                return;
            }

            var confirm = MessageBox.Show("Delete this Client?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            _clientService.DeleteClient(_selectedDni);
            LoadClients();
        }
    }
}
