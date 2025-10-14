using System;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Interfaces
{
    public partial class Registrarse : Form
    {
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=test;Uid=Alex;Pwd=12345";
        private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";

        private MySqlConnection conexion;
        private MySqlCommand comando;
        private MySqlDataReader leer;

        // Bandera para evitar que FormClosing abra la ventana de nuveo después de un resgitro exitoso
        private bool registroExitoso = false;
        public Registrarse()
        {
            InitializeComponent();

            CenterControlsInPanel();
            panelRegister.Resize += (s, e) => CenterControlsInPanel();
            textBoxPassw.UseSystemPasswordChar = true;
            textBoxConfirmPassw.UseSystemPasswordChar = true;

            conexion = new MySqlConnection(MYSQL_CONNECTION_STRING);
        }

        private void CenterControlsInPanel()
        {
            foreach (Control control in panelRegister.Controls)
            {
                control.Left = (panelRegister.ClientSize.Width - control.Width) / 2;
            }
        }

        private void textBoxNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                textBoxEmail.Focus();
            }
        }
        private void textBoxEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                textBoxPassw.Focus();
            }
        }

        private void textBoxPassw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                textBoxConfirmPassw.Focus();
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string nombre = textBoxNombre.Text;
            string email = textBoxEmail.Text;
            string pass = textBoxPassw.Text;
            string confirmPass = textBoxConfirmPassw.Text;

            // VALIDACIONES BÁSICAS
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Todos los campos son obligatorios",
                        "Error de Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                textBoxNombre.Focus();
                return;
            }

            if (pass != confirmPass)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor, inténtalo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassw.Clear();
                textBoxConfirmPassw.Clear();
                textBoxPassw.Focus();
                return;
            }

            // HASH DE LA CONTRASEÑA
            string hashedPass = PasswordHelper.HashPassword(pass);

            // GUARDAR EN BASE DE DATOS
            try
            {
                if (conexion.State != ConnectionState.Open)
                {
                    conexion.Open();
                }
                // Verificar si el email ya existe
                if (EmailExiste(email))
                {
                    MessageBox.Show("El correo electrónico ya está registrado. Por favor, utiliza otro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxEmail.Clear();
                    textBoxEmail.Focus();
                    return;
                }
                // Insertar nuevo usuario
                string query = "INSERT INTO usuarios (nombre, email, password,fecha) VALUES (@nombre, @correo, @password,@fecha)";

                using (comando = new MySqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@correo", email);
                    comando.Parameters.AddWithValue("@password", hashedPass);
                    //Obtengo la fecha que indico en el formato año mes y dia
                    string fecha = dateTimeFechaNac.Value.ToString("yyyy-MM-dd");
                    comando.Parameters.AddWithValue("@fecha", fecha);
                }
                int filasAfectadas = comando.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show("¡Registro exitoso! Ya puedes iniciar sesión.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Establecer bandera como exito
                    registroExitoso = true;
                    // Redireccionar al formulario de inicio de sesión
                    InicioSesion ventanaSes = new InicioSesion();
                    ventanaSes.Show();
                    //Cerrar esta forma
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al registrar el usuario. Por favor, inténtalo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open && conexion != null)
                    conexion.Close();
            }
        }

        private bool EmailExiste(string email)
        {
            string query = "SELECT COUNT(id) FROM usuarios WHERE email = @email";
            using (MySqlCommand comando = new MySqlCommand(query, this.conexion))
            {
                comando.Parameters.AddWithValue("@email", email);
                int count = Convert.ToInt32(comando.ExecuteScalar());
                return count > 0;
            }
        }

        private void Registrarse_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!registroExitoso)
            {
                InicioSesion ventanaSes = new InicioSesion();
                ventanaSes.Show();
            }
        }

        private void Registrarse_Load(object sender, EventArgs e)
        {

        }
    }
}
