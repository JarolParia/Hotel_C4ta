using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_C4ta.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public User Login(string Email, string Password)
        {
            var user = _userRepository.GetByCredentials(Email, Password);
            if (user == null)
                throw new Exception("User not found or Invalid credentials");
            return user;
        }

        public IEnumerable<User> GetAllUsers() => _userRepository.GetAll();
        public void CreateUser(User user) => _userRepository.Add(user);
        public void UpdateUser(User user) => _userRepository.Update(user);
        public void DeleteUser(User user) => _userRepository.Delete(user);
    }
}
