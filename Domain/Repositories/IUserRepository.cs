using Hotel_C4ta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Domain.Repositories
{
    /// Defines the contract for retrieving, creating, updating, and deleting users in the system.
    public interface IUserRepository
    {
        User? GetByCredentials(string email, string password); /// Retrieves a user by their login credentials.
        IEnumerable<User> GetAll(); /// Retrieves all users
        void Add(User user); /// Adds (creates) a new user in the system.
        void Update(User user); /// Updates the details of an existing user.
        void Delete(User user); /// Deletes an existing user from the system.
    }
}
