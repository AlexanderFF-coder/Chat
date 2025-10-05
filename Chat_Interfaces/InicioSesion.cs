using System;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chat_Interfaces;

namespace Chat_Interfaces
{
    public partial class InicioSesion : Form
    {
        private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=test;Uid=Alex;Pwd=12345";
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";

        private MySqlConnection conexion;
        private MySqlCommand comando;
        private MySqlDataReader leer;

        public InicioSesion()
        {
            InitializeComponent();

            CenterControlsInPanel();
            panelLogin.Resize += (s, e) => CenterControlsInPanel();
            textBoxEmail.KeyDown += TextBoxEmail_KeyDown;
            textBoxPassword.UseSystemPasswordChar = true;

            conexion = new MySqlConnection(MYSQL_CONNECTION_STRING);
        }

        //este metodo centra los controles dentro del panel
        private void CenterControlsInPanel()
        {
            foreach (Control control in panelLogin.Controls)
            {
                control.Left = (panelLogin.ClientSize.Width - control.Width) / 2;
            }
        }

        //al dar enter en el textBoxEmail, el foco se mueve al textBoxPassword
        private void TextBoxEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                textBoxPassword.Focus();
            }
        }

        private void lblRegistro_Click(object sender, EventArgs e)
        {
            Registrarse nuevaVentana = new Registrarse();

            nuevaVentana.Show();

            this.Hide();
        }

        private void InicioSesion_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text.Trim();
            string password = textBoxPassword.Text;
            string hashedPassword = string.Empty;

            // Validar que los campos no esten vacios
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, ingrese email y contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bloque de Código para la consulta de la contraseña en la base de datos
            try
            {
                conexion.Open();

                string query = "SELECT password FROM usuarios WHERE email = @email";

                comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@email", email);

                leer = comando.ExecuteReader();

                if (leer.Read())
                {
                    //Si encuentra el usuario, obtiene la contraseña hasheada
                    hashedPassword = leer["password"].ToString();
                }

                leer.Close(); //Cerrat lector
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
            }


            if (string.IsNullOrEmpty(hashedPassword))
            {
                // Si no se encuentra el usuario
                MessageBox.Show("Usuario no encontrado.", "Error de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isPasswordValid = PasswordHelper.VerifyPassword(password, hashedPassword);

            if (isPasswordValid)
            {
                MessageBox.Show("¡Inicio de sesión exitoso!", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Gurardamos el id
                comando = new MySqlCommand("Select id from usuarios where email=@mail", conexion);
                comando.Parameters.AddWithValue("@mail", email);
                conexion.Open();
                leer = comando.ExecuteReader();
                while (leer.Read())
                {
                    InicioSesion.Sesionid.IdUsuario= leer["id"].ToString();
                }
                // Si el login es exitoso, abrir la ventana de chat
                Chat chatW = new Chat();
                chatW.Show();
                this.Hide();
            }
            else
            {
                //Contraseña incorrecta
                MessageBox.Show("Contraseña incorrecta. Inténtalo de nuevo.", "Error de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public static class Sesionid
        {
            public static string IdUsuario;
        }


    }
}

public static class PasswordHelper
{
    // Metodo para hashear la contraseña usando SHA256
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        string enteredHash = HashPassword(enteredPassword);
        return string.Equals(enteredHash, storedHash, StringComparison.OrdinalIgnoreCase);
    }

}