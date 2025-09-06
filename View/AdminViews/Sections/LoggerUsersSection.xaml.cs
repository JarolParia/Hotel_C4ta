using Hotel_C4ta.Controller;
using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.View.AdminViews.Sections
{
    public partial class LoggerUsersSection : UserControl
    {
        private int? userId = null; // 👈 null = modo crear, número = modo editar REVIEWREVIEWREVIEWREVIEWREVIEWREVIEWREVIEW
        public event Action UsuarioGuardado;

        UserController _userController = new UserController();

        public LoggerUsersSection()
        {
            InitializeComponent();
        }

        // 👇 Constructor para edición
        public LoggerUsersSection(int id) : this()
        {
            userId = id;

            var user = _userController.CargarUsuario(id);

            TxtNombre.Text = user.FullName;
            TxtRolCodigo.Text = user.RoleCode;
            Pwd.Password = user.PasswordHashed;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            var nombre = TxtNombre.Text.Trim();
            var rolCodigo = TxtRolCodigo.Text.Trim();
            var password = Pwd.Password.Trim();

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(rolCodigo) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor llena todos los campos.");
                return;
            }

            var user = _userController.GuardarUsuario(userId, nombre, rolCodigo, password);

            MessageBox.Show(user == true ? "Usuario creado exitosamente." : "Usuario actualizado exitosamente.");

            UsuarioGuardado?.Invoke(); // 👈 Notificar a la tabla

            var parent = this.Parent as ContentControl;

            if (parent != null)
            {
                parent.Content = null;
            }

        }
    }
}
