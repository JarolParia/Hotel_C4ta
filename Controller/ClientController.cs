using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Hotel_C4ta.Controller
{
    public class ClientController
    {
        public ClientModel _clientModel;

        public List<ClientModel> GetAllClients()
        {
            using var conn = DBContext.OpenConnection();
            var clients = new List<ClientModel>();

            try
            {
                string sql = "SELECT * FROM Client";
                using var cmd = new SqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clients.Add(new ClientModel
                    {
                        DNI = reader.GetString(0),
                        FullName = reader.GetString(1),
                        Email = reader.GetString(2),
                        Phone = reader.GetString(3)
                    });
                }
                return clients;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading clients: " + ex.Message);
            }
            return null;
        }

        public ClientModel GetClient(int dni)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "SELECT * FROM Client WHERE DNI=@dni";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dni", dni);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return new ClientModel
                    {
                        DNI = reader.GetString(0),
                        FullName = reader.GetString(1),
                        Email = reader.GetString(2),
                        Phone = reader.GetString(3),
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading clients: " + ex.Message);
            }
            return null;
        }

        public void RegisterClient(string dni, string fullname, string email, string phone)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "INSERT INTO Client (DNI, FullName, Email, Phone) VALUES (@dni, @fullname, @email, @phone)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dni", dni);
                cmd.Parameters.AddWithValue("@fullname", fullname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Client registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving client: " + ex.Message);
            }
        }

        public void UpdateClient(string dni, string fullname, string email, string phone)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "UPDATE Client SET FullName=@fullname, Email=@email, Phone=@phone WHERE DNI=@dni";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dni", dni);
                cmd.Parameters.AddWithValue("@fullname", fullname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Client updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating client: " + ex.Message);
            }
        }

        public void DeleteClient(string dni)
        {
            using var conn = DBContext.OpenConnection();

            try
            {
                string sql = "DELETE FROM Client WHERE DNI=@dni";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@dni", dni);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Client deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting client: " + ex.Message);
            }
        }
    }
}
//CRUD Complete ON Client