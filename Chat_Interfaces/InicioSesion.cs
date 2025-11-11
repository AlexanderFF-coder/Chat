using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Chat_Interfaces
{
    public partial class InicioSesion : Form
    {

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
            try
            {
                Direcionip direcionip = new Direcionip();
                string direcionp = direcionip.direcion;
                cliente = new TcpClient(direcionp, 8080);
                flujo = cliente.GetStream();

                if (string.IsNullOrEmpty(textBoxEmail.Text) || string.IsNullOrEmpty(textBoxPassword.Text))
                {
                    MessageBox.Show("Por favor, ingrese email y contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mensaje = "1|" + textBoxEmail.Text + "|" + textBoxPassword.Text;
                byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                flujo.Write(datos, 0, datos.Length);

                if (hilo == null || !hilo.IsAlive)
                {
                    Task.Run(() => escuchaservidor());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InicioSesion_Load(object sender, EventArgs e)
        {
           
        }

        private async Task escuchaservidor()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesLeidos;

                while (ejecutando && (bytesLeidos = await flujo.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                    string[] partes = mensaje.Split('|');

                    if (partes[0] == "0")
                    {
                        await this.checasyn(() =>
                        {
                            MessageBox.Show("Inicio de sesión ", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Chat chatW = new Chat(partes[2], partes[1], partes[3]);

                            ejecutando = false;
                            flujo.Close();
                            cliente.Close();

                            chatW.Show();
                            this.Hide();
                        });
                    }
                    else if (partes[0] == "1")
                    {
                        await this.checasyn(() =>
                        {
                            MessageBox.Show("El usuario ya ha iniciado sesión en otro dispositivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        });
                    }
                    else if (partes[0] == "2")
                    {
                        await this.checasyn(() =>
                        {
                            MessageBox.Show("El usuario no fue encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        });
                    }
                    else if (partes[0] == "3")
                    {
                        await this.checasyn(() =>
                        {
                            MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        });
                    }
                }
            }
            catch (IOException)
            {
                if (ejecutando)
                {
                    await this.checasyn(() =>
                    {
                        MessageBox.Show("Se perdió la conexión con el servidor.", "Desconectado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    });
                }
            }
            catch (Exception ex)
            {
                if (ejecutando)
                {
                    await this.checasyn(() =>
                    {
                        MessageBox.Show("Error en hilo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
            }
        }

        //Checa si es aync
        private Task checasyn(Action action)
        {
            var tarea = new TaskCompletionSource<object>();
            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    action();
                    tarea.SetResult(null);
                }
                catch (Exception ex)
                {
                    tarea.SetException(ex);
                }
            }));
            return tarea.Task;
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

