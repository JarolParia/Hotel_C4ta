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
using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;


namespace Hotel_C4ta.View.ReceptionistViews.Sections
{
    /// <summary>
    /// Lógica de interacción para RegisterClientsContent.xaml
    /// </summary>
    public partial class RegisterClientsContent : UserControl
    {

        private DatabaseConnection _db = new DatabaseConnection();
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

            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("DNI, Nombre y Email son obligatorios.");
                return;
            }

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string sql = "INSERT INTO Client (Dni, Names, Email, Phone) VALUES (@dni, @name, @mail, @phone)";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@dni", dni);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@mail", email);
                        cmd.Parameters.AddWithValue("@phone", (object)phone ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente registrado correctamente.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar cliente: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }

            TxtDni.Clear();
            TxtName.Clear();
            TxtEmail.Clear();
            TxtPhone.Clear();
        }

    }
}