using Hotel_C4ta.Model;
using Hotel_C4ta.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hotel_C4ta.Controller
{
    public class UserController
    {
        private readonly AdministratorController _adminCtrl = new AdministratorController();
        private readonly ReceptionistController _recepCtrl = new ReceptionistController();

        public UserModel _UserModel;
        // Load Users

        public List<UserModel> GetAllUsers()
        {
            using var conn = DBContext.OpenConnection();
            var users = new List<UserModel>();

            try
            {
                string sql = @"
                            SELECT ID, FullName, Email, PasswordHashed, Rol FROM Administrator
                            UNION
                            SELECT ID, FullName, Email, PasswordHashed, Rol FROM Receptionist";

                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        string rol = reader.GetString(4);
                        UserModel user;

                        if (rol == "Admin")
                        {
                            user = new AdministratorModel();
                        }
                        else if (rol == "Recep")
                        {
                            user = new ReceptionistModel();
                        }
                        else
                        {
                            throw new Exception("Unknow rol" + rol);
                        }

                        user.ID = reader.GetInt32(0);
                        user.FullName = reader.GetString(1);
                        user.Email = reader.GetString(2);
                        user.PasswordHashed = reader.GetString(3);
                        user.Rol = rol;

                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);

            }

            return users;

        }

        // Create Users
        public void CreateUser(UserModel user)
        {
            if (user is AdministratorModel admin)
                _adminCtrl.CreateAdministrator(admin);
            else if (user is ReceptionistModel recep)
                _recepCtrl.CreateReceptionist(recep);
            else
                throw new Exception("Modelo no válido");
        }


        // Update Users
        public void UpdateUser(UserModel user)
        {
            if (user.Rol == "Admin")
                _adminCtrl.UpdateAdministrator((AdministratorModel)user);
            else if (user.Rol == "Recep")
                _recepCtrl.UpdateReceptionist((ReceptionistModel)user);
            else
                throw new Exception("Unknow Rol");
        }

        // Delete Users 
        public void DeleteUser(UserModel user)
        {
            if (user.Rol == "Admin")
                _adminCtrl.DeleteAdministrator(user.ID);
            else if (user.Rol == "Recep")
                _recepCtrl.DeleteReceptionist(user.ID);
            else
                throw new Exception("Unknow Rol");
        }

    }
}