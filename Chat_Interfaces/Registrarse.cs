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
using System.Net.Sockets;
using System.Threading;
namespace Chat_Interfaces
{
    public partial class Registrarse : Form
    {
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=test;Uid=Alex;Pwd=12345";
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";

        private MySqlConnection conexion;
        private MySqlCommand comando;
        private MySqlDataReader leer;
        //Variables para server
        TcpClient cliente;
        NetworkStream flujo;
        Thread hilo;
        bool ejecutando = true;
        // Bandera para evitar que FormClosing abra la ventana de nuveo después de un resgitro exitoso
        private bool registroExitoso = false;
        public Registrarse()
        {
            InitializeComponent();
          
            CenterControlsInPanel();
            panelRegister.Resize += (s, e) => CenterControlsInPanel();
            textBoxPassw.UseSystemPasswordChar = true;
            textBoxConfirmPassw.UseSystemPasswordChar = true;

           // conexion = new MySqlConnection(MYSQL_CONNECTION_STRING);
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
           
            //Mandamos informacion al servidor
            cliente = new TcpClient("192.168.1.83", 8080);
            flujo = cliente.GetStream();
            string mensaje = "2|" + nombre + "|" + email + "|" + pass+"|"+ dateTimeFechaNac.Value.ToString("yyyy-MM-dd");
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
                    //abilitamos el boton de registro se usa invoke porque es un hilo differente al que tenemos en inicio de sesion
                    this.Invoke((Action)(() =>
                    {
                        
                        if (partes[0] == "4")
                        {
                            MessageBox.Show("Usuario registrado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            if (partes[0] == "5")
                            {
                                MessageBox.Show("Error al registrar el usuario. Por favor, inténtalo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                if (partes[0] == "6")
                                {
                                    MessageBox.Show("Email repetido poner nuevo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }));
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
