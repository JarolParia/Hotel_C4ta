using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
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

namespace Hotel_C4ta.Presentation.Views.AdminViews.Sections
{
    /// <summary>
    /// Lógica de interacción para UserManagementControl.xaml
    /// </summary>
    public partial class UserManagementControl : UserControl
    {
        private int _selectedId;
        private readonly UserService _userService;
        public UserManagementControl(UserService userService)
        {
            InitializeComponent();
            _userService = userService;

            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                UsersGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void UsersGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                _selectedId = selectedUser.ID;
                TxtID.Text = selectedUser.ID.ToString();
                TxtFullName.Text = selectedUser.FullName;
                TxtEmail.Text = selectedUser.Email;
                TxtPassword.Password = selectedUser.PasswordHashed;

                if (selectedUser.Rol == "Admin")
                    CmbRole.SelectedIndex = 0;
                else if (selectedUser.Rol == "Recep")
                    CmbRole.SelectedIndex = 1;
                else
                    CmbRole.SelectedIndex = -1;

                TxtID.IsEnabled = false;
            }
            else
            {
                TxtID.IsEnabled = true;
                ClearForm();
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            string fullName = TxtFullName.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string password = TxtPassword.Password.Trim();
            string role = (CmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHashed = password,
                Rol = role
            };

            try
            {
                _userService.CreateUser(user);
                MessageBox.Show("User created successfully.");
                LoadUsers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating User: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedId == 0)
            {
                MessageBox.Show("Select a user to update.");
                return;
            }

            string fullName = TxtFullName.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string password = TxtPassword.Password.Trim();
            string role = (CmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            var user = new User
            {
                ID = _selectedId,
                FullName = fullName,
                Email = email,
                PasswordHashed = password,
                Rol = role
            };

            try
            {
                _userService.UpdateUser(user);
                MessageBox.Show("User updated successfully.");
                LoadUsers();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete user {selectedUser.FullName}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _userService.DeleteUser(selectedUser);
                        MessageBox.Show("User deleted successfully.");
                        LoadUsers();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting user: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedId = 0;
            TxtID.Clear();
            TxtFullName.Clear();
            TxtEmail.Clear();
            TxtPassword.Clear();
            CmbRole.SelectedIndex = -1;
            TxtID.IsEnabled = true;
        }
    }
}
