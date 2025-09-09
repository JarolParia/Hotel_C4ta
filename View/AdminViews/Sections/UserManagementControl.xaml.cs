using Hotel_C4ta.Model;
using Hotel_C4ta.Controller;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.View.AdminViews.Sections
{
    public partial class UserManagementControl : UserControl
    {
        private int _selectedId;
        private readonly UserController _UserController = new UserController();
        public UserManagementControl()
        {
            InitializeComponent();
            LoadUsers();
        }


        public void LoadUsers()
        {
            var users = _UserController.GetAllUsers();
            UsersGrid.ItemsSource = users;
        }


        private void UsersGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (UsersGrid.SelectedItem is UserModel selectedUser)
            {

                _selectedId = selectedUser.ID;
                TxtID.Text = selectedUser.ID.ToString();
                TxtFullName.Text = selectedUser.FullName.ToString();
                TxtEmail.Text = selectedUser.Email.ToString();
                TxtPassword.Password = selectedUser.PasswordHashed;

                if (selectedUser.Rol == "Admin")
                {
                    CmbRole.SelectedIndex = 0;
                }
                else if (selectedUser.Rol == "Recep")
                {
                    CmbRole.SelectedIndex = 1;
                }
                else
                {
                    CmbRole.SelectedIndex = -1;
                }

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

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("All fields are required.");
                return;
            }


            UserModel user;

            if (role == "Admin")
            {
                user = new AdministratorModel
                {
                    FullName = fullName,
                    Email = email,
                    PasswordHashed = password,
                    Rol = "Admin"
                };
            }
            else if (role == "Recep")
            {
                user = new ReceptionistModel
                {
                    FullName = fullName,
                    Email = email,
                    PasswordHashed = password,
                    Rol = "Recep"
                };
            }
            else
            {
                MessageBox.Show("Invalid Rold.");
                return;
            }

            try
            {
                _UserController.CreateUser(user);
                MessageBox.Show("User created succesfully.");
                LoadUsers();
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
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("All fields are required.");
                return;
            }
            UserModel user;
            if (role == "Admin")
            {
                user = new AdministratorModel
                {
                    ID = _selectedId,
                    FullName = fullName,
                    Email = email,
                    PasswordHashed = password,
                    Rol = "Admin"
                };
            }
            else if (role == "Recep")
            {
                user = new ReceptionistModel
                {
                    ID = _selectedId,
                    FullName = fullName,
                    Email = email,
                    PasswordHashed = password,
                    Rol = "Recep"
                };
            }
            else
            {
                MessageBox.Show("Invalid Role.");
                return;
            }
            try
            {
                _UserController.UpdateUser(user);
                MessageBox.Show("User updated successfully.");
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message);
            }
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is UserModel selectedUser)
            {
                var result = MessageBox.Show(
                    $"¿Are you sure want to delete user-- {selectedUser.FullName}?",
                    "Confirm Deleting",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _UserController.DeleteUser(selectedUser);
                        MessageBox.Show("User deleted Succesfully.");
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting user: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete");
            }

        }


        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            TxtEmail.Clear();
            TxtFullName.Clear();
            TxtID.Clear();
            TxtPassword.Clear();
            CmbRole.SelectedIndex = -1;
        }


    }
}