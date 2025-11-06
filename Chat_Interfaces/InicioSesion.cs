using Chat_Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
namespace Chat_Interfaces
{
    public partial class InicioSesion : Form
    {
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=test;Uid=Alex;Pwd=12345";
        private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";

        private MySqlConnection conexion;
        private MySqlCommand comando;
        private MySqlDataReader leer;
        // Variables para  server
        TcpClient cliente;
        NetworkStream flujo;
        Thread hilo;
        bool ejecutando = true;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ejecutando = false;
            if (flujo != null)
            {
                flujo.Close();
            }
            if (cliente != null && cliente.Connected)
            {
                cliente.Close();
            }
            if (hilo != null && hilo.IsAlive)
            {
                hilo.Join(500);
            }
            Application.Exit(); 
        }
        public InicioSesion()
        {
            InitializeComponent();

            CenterControlsInPanel();
            panelLogin.Resize += (s, e) => CenterControlsInPanel();
            textBoxEmail.KeyDown += TextBoxEmail_KeyDown;
            textBoxPassword.UseSystemPasswordChar = true;
            //Conectar al servidor en el puerto  8080
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
        //Codigo modificado para que se conecte al servidor y verifique el usuario
        private void btnLogin_Click(object sender, EventArgs e)
        {
            cliente = new TcpClient("192.168.1.83", 8080);
            flujo = cliente.GetStream();
            //Manamos el email y la contraseña al servidor para verificar si el usuario existe separados por un |
            // Validar que los campos de correo y contraseña no estén vacíos
            if (string.IsNullOrEmpty(textBoxEmail.Text) || string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Por favor, ingrese email y contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string mensaje ="1|"+textBoxEmail.Text+"|"+textBoxPassword.Text;
            byte[] datos = Encoding.UTF8.GetBytes(mensaje);
            flujo.Write(datos, 0, datos.Length);
            //Iniciamos el hilo para escuchar al servidor
            if (hilo == null || !hilo.IsAlive)
            {
                hilo = new Thread(new ThreadStart(escuchaservidor));
                hilo.IsBackground = true;
                hilo.Start();
            }

        }

        private void InicioSesion_Load(object sender, EventArgs e)
        {
           
        }

        private void escuchaservidor()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesLeidos;

                while (ejecutando && (bytesLeidos = flujo.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                    string[] partes = mensaje.Split('|');
                    if (partes[0] == "0")
                    {
                        MessageBox.Show("¡Inicio de sesión exitoso!", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Chat chatW = new Chat(partes[2], partes[1], partes[3]);
                        chatW.Show();
                        this.Hide();
                    }
                    else
                    {
                        if (partes[0] == "1")
                        {
                            MessageBox.Show("El usuario ya ha iniciado sesión en otro dispositivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        else
                        {
                            if (partes[0] == "2")
                            {
                                MessageBox.Show("El usuario no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }
                            else
                            {
                                if (partes[0] == "3")
                                {
                                    MessageBox.Show("contrasena incorrecta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ejecutando)
                {
                    Console.WriteLine("Error en hilo: " + ex.Message);
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

