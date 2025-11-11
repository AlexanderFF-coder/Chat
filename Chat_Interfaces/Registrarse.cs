using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Chat_Interfaces
{
    public partial class Registrarse : Form
    {
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

            //Checa todos los escenarios
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Todos los campos son obligatorios", "Error de Validación",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                textBoxNombre.Focus();
                return;
            }

            if (pass != confirmPass)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassw.Clear();
                textBoxConfirmPassw.Clear();
                textBoxPassw.Focus();
                return;
            }

            //Mandamos informacion al servidor
            Direcionip direcionip = new Direcionip();
            string direcionp = direcionip.direcion;
            cliente = new TcpClient(direcionp, 8080);
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
                            registroExitoso = true;
                            InicioSesion ventanaSes = new InicioSesion();
                            ventanaSes.Show();
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
            //Se agrego un catch por si se desconecta el servidor
            catch (IOException)
            {
                if (ejecutando)
                {
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show("Se perdió la conexión con el servidor.", "Desconectado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
