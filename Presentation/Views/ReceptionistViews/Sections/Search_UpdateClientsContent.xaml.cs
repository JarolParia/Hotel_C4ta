using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using System.Windows;
using System.Windows.Controls;

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
                MessageBox.Show("Please select a client.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TxtDni.Text != _selectedDni)
            {
                MessageBox.Show("You are not allowed to update the DNI.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            // 🔔 Success alert
            MessageBox.Show("Client updated successfully.", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedDni))
            {
                MessageBox.Show("Please select a client.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this client?",
                                          "Confirm",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                _clientService.DeleteClient(_selectedDni);
                LoadClients();

                // ✅ Success alert
                MessageBox.Show("Client deleted successfully.", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // 🔍 Verificamos si es un error de restricción de clave foránea
                if (ex.InnerException != null && ex.InnerException.Message.Contains("FOREIGN KEY"))
                {
                    MessageBox.Show("This client cannot be deleted because it is related to other records.",
                                    "Delete Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
                else
                {
                    // ⚠️ Para otros errores genéricos
                    MessageBox.Show($"An error occurred while deleting the client:\n{ex.Message}",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }
    }
}
