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
using System.Net.Sockets;
using System.Threading;
namespace Chat_Interfaces
{
    public partial class InicioSesion : Form
    {
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=test;Uid=Alex;Pwd=12345";
        private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";

        private MySqlConnection conexion;
        private MySqlCommand comando;
        private MySqlDataReader leer;
        bool servidoract = true;
        TcpListener servidor;
        Thread hiloServidor;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            servidoract = false;
            servidor.Stop();
            Application.Exit(); 
        }
        public InicioSesion()
        {
            InitializeComponent();

            CenterControlsInPanel();
            panelLogin.Resize += (s, e) => CenterControlsInPanel();
            textBoxEmail.KeyDown += TextBoxEmail_KeyDown;
            textBoxPassword.UseSystemPasswordChar = true;

            conexion = new MySqlConnection(MYSQL_CONNECTION_STRING);
            servidor= new TcpListener(System.Net.IPAddress.Any, 8080);
            servidor.Start();
            hiloServidor = new Thread(escuchcliente);
            hiloServidor.Start();
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
            // Variables de alamcenamiento de datos
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;

            string hashedPassword = string.Empty;
            string idUsuario = string.Empty;
            string nombreUsuario = string.Empty;


            // Validar que los campos de correo y contraseña no estén vacíos
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, ingrese email y contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Bloque de Código para la consulta de la contraseña en la base de datos
            try
            {
                conexion.Open();
                // Consulta de la contraseña para el email proporcionado
                string query = "SELECT id, password, nombre FROM usuarios WHERE email = @email";

                comando = new MySqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@email", email);

                leer = comando.ExecuteReader();

                if (leer.Read())
                {
                    //Si encuentra el usuario, obtiene los datos necesarios
                    hashedPassword = leer["password"].ToString();
                    // Capturamos el id y nombre del usuario
                    idUsuario = leer["id"].ToString();
                    nombreUsuario = leer["nombre"].ToString();
                }
                leer.Close(); //Cerrar lector
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
                // Contraseña correcta
                MessageBox.Show("¡Inicio de sesión exitoso!", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Chat chatW = new Chat(email, idUsuario, nombreUsuario);
                chatW.Show();
                this.Hide();
            }
            else
            {
                //Contraseña incorrecta
                MessageBox.Show("Contraseña incorrecta. Inténtalo de nuevo.", "Error de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void escuchcliente()
        {
            while (servidoract)
            {
                try
                {
                    TcpClient cliente = servidor.AcceptTcpClient();
                    NetworkStream stream = cliente.GetStream();

                    byte[] buffer = Encoding.UTF8.GetBytes("Conexión exitosa con el servidor");
                    stream.Write(buffer, 0, buffer.Length);

                    cliente.Close();
                }
                catch (SocketException)
                {
                    if (!servidoract)
                    {
                        break;
                    }    
                }
            }
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


