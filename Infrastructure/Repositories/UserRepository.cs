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
    /// Responsible for CRUD operations for Users (Administrators and Receptionists)
    internal class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext; /// Database context used to open connections

        public UserRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// Retrieves a user by email and password (login authentication)
        /// Looks into both Administrator and Receptionist tables
        public User? GetByCredentials(string email, string password)
        {
            using var conn = _dbContext.OpenConnection();
            /// SQL query checks both tables with UNION
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

            return null; /// No user found
        }

        /// Retrieves all users from both Administrator and Receptionist tables
        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            using var conn = _dbContext.OpenConnection();

            /// SQL query retrieves all users (Admin + Receptionist)
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

        /// Adds a new user into the correct table depending on their role
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

        /// Updates an existing user in the database
        public void Update(User user)
        {
            using var conn = _dbContext.OpenConnection();
            /// Choose the correct table based on the role
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

        /// Deletes a user from the database
        public void Delete(User user)
        {
            using var conn = _dbContext.OpenConnection();

            /// Choose the table depending on the role
            string table = user.Rol == "Admin" ? "Administrator" : "Receptionist";

            string sql = $"DELETE FROM {table} WHERE ID=@Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", user.ID);
            cmd.ExecuteNonQuery(); /// Execute the delete
        }


    }
}
