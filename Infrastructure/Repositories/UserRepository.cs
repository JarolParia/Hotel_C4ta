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
    internal class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;

        public UserRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User? GetByCredentials(string email, string password)
        {
            using var conn = _dbContext.OpenConnection();
            string query = @"
                SELECT ID, FullName, Email, PasswordHashed, Rol 
                FROM Administrator 
                WHERE Email = @Email AND PasswordHashed = @Password
                UNION
                SELECT ID, FullName, Email, PasswordHashed, Rol 
                FROM Receptionist 
                WHERE Email = @Email AND PasswordHashed = @Password";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    ID = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHashed = reader.GetString(3),
                    Rol = reader.GetString(4)
                };
            }

            return null;
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            using var conn = _dbContext.OpenConnection();

            string sql = @"
                SELECT ID, FullName, Email, PasswordHashed, Rol FROM Administrator
                UNION
                SELECT ID, FullName, Email, PasswordHashed, Rol FROM Receptionist";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    ID = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHashed = reader.GetString(3),
                    Rol = reader.GetString(4)
                });
            }

            return users;
        }

        public void Add(User user)
        {
            using var conn = _dbContext.OpenConnection();
            string table = user.Rol == "Admin" ? "Administrator" : "Receptionist";

            string sql = $"INSERT INTO {table} (FullName, Email, PasswordHashed, Rol) VALUES (@Name, @Email, @Pass, @Rol)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Pass", user.PasswordHashed);
            cmd.Parameters.AddWithValue("@Rol", user.Rol);
            cmd.ExecuteNonQuery();
        }
        public void Update(User user)
        {
            using var conn = _dbContext.OpenConnection();
            string table = user.Rol == "Admin" ? "Administrator" : "Receptionist";

            string sql = $"UPDATE {table} SET FullName=@Name, Email=@Email, PasswordHashed=@Pass, Rol=@Rol WHERE ID=@Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", user.ID);
            cmd.Parameters.AddWithValue("@Name", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Pass", user.PasswordHashed);
            cmd.Parameters.AddWithValue("@Rol", user.Rol);
            cmd.ExecuteNonQuery();
        }

        public void Delete(User user)
        {
            using var conn = _dbContext.OpenConnection();
            string table = user.Rol == "Admin" ? "Administrator" : "Receptionist";

            string sql = $"DELETE FROM {table} WHERE ID=@Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", user.ID);
            cmd.ExecuteNonQuery();
        }


    }
}
