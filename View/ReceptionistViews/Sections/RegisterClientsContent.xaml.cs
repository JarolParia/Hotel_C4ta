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

    public partial class RegisterClientsContent : UserControl
    {
        private readonly ClientController _clientController = new ClientController();

        public RegisterClientsContent()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string dni = TxtDni.Text.Trim();
            string name = TxtName.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string phone = TxtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(dni) 
               || string.IsNullOrWhiteSpace(name) 
               || string.IsNullOrWhiteSpace(email) 
               || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            _clientController.RegisterClient(dni, name, email, phone);

            TxtDni.Clear();
            TxtName.Clear();
            TxtEmail.Clear();
            TxtPhone.Clear();
        }
    }
}