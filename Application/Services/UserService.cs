using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    /// Service layer for managing user-related operations.
    /// This class interacts with the repository layer to handle
    /// authentication and CRUD operations for users.
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// Handles user login by checking email and password credentials.
        public User Login(string Email, string Password)
        {
            var user = _userRepository.GetByCredentials(Email, Password);
            if (user == null)
                throw new Exception("User not found or Invalid credentials"); /// Throws an exception if the credentials are invalid or user is not found.
            return user;
        }
        /// Retrieves all users
        public IEnumerable<User> GetAllUsers() => _userRepository.GetAll();

        /// Adds a new user
        public void CreateUser(User user) => _userRepository.Add(user);

        /// Updates an existing user
        public void UpdateUser(User user) => _userRepository.Update(user);

        /// Deletes a user 
        public void DeleteUser(User user) => _userRepository.Delete(user);
    }
}
