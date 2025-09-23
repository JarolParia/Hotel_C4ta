using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using Hotel_C4ta.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Infrastructure.Repositories
{
    internal class ClientRepository : IClientRepository
    {
        private readonly DBContext _dbContext;

        public ClientRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Client> GetAllClients()
        {
            var clients = new List<Client>();
            using var conn = _dbContext.OpenConnection();
            string sql = "SELECT DNI, FullName, Email, Phone FROM Client";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clients.Add(new Client
                {
                    DNI = reader.GetString(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Phone = reader.GetString(3)
                });
            }
            return clients;
        }

        public Client GetClient(string dni)
        {
            using var conn = _dbContext.OpenConnection();
            string sql = "SELECT DNI, FullName, Email, Phone FROM Client WHERE DNI=@dni";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dni", dni);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Client
                {
                    DNI = reader.GetString(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Phone = reader.GetString(3)
                };
            }
            return null;
        }

        public void RegisterClient(Client client)
        {
            using var conn = _dbContext.OpenConnection();
            string sql = "INSERT INTO Client (DNI, FullName, Email, Phone) VALUES (@dni, @fullname, @email, @phone)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dni", client.DNI);
            cmd.Parameters.AddWithValue("@fullname", client.FullName);
            cmd.Parameters.AddWithValue("@email", client.Email);
            cmd.Parameters.AddWithValue("@phone", client.Phone);
            cmd.ExecuteNonQuery();
        }

        public void UpdateClient(Client client)
        {
            using var conn = _dbContext.OpenConnection();
            string sql = "UPDATE Client SET FullName=@fullname, Email=@email, Phone=@phone WHERE DNI=@dni";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dni", client.DNI);
            cmd.Parameters.AddWithValue("@fullname", client.FullName);
            cmd.Parameters.AddWithValue("@email", client.Email);
            cmd.Parameters.AddWithValue("@phone", client.Phone);
            cmd.ExecuteNonQuery();
        }

        public void DeleteClient(string dni)
        {
            using var conn = _dbContext.OpenConnection();
            string sql = "DELETE FROM Client WHERE DNI=@dni";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dni", dni);
            cmd.ExecuteNonQuery();
        }
    }
}
