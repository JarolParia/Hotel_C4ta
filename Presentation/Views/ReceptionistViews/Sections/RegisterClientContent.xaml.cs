using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;


namespace Hotel_C4ta.Presentation.ReceptionistViews.Sections
{
    public partial class RegisterClientContent : UserControl
    {
        private readonly ClientService _clientService;

        public RegisterClientContent(ClientService clientService)
        {
            InitializeComponent();
            _clientService = clientService;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string dni = TxtDni.Text.Trim();
            string name = TxtName.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string phone = TxtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(dni) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            try
            {
                var client = new Client
                {
                    DNI = dni,
                    FullName = name,
                    Email = email,
                    Phone = phone
                };

                _clientService.RegisterClient(client);

                MessageBox.Show("Client registered successfully.");

                TxtDni.Clear();
                TxtName.Clear();
                TxtEmail.Clear();
                TxtPhone.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering client: " + ex.Message);
            }
        }
    }
}
