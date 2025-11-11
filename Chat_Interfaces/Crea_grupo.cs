using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
namespace Chat_Interfaces
{
    public partial class Crea_grupo : Form
    {
        //Variables para  server
        TcpClient cliente;
        NetworkStream flujo;
        Thread hilo;
        bool ejecutando = true;
        private string _idUsuario;
        public Chat ch;
        public Crea_grupo(string idUsuario, Chat ch)
        {
            InitializeComponent();
            this.ch = ch;
            _idUsuario = idUsuario;
        }

        private void Crea_grupo_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombre = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("No puedes tener nombre de grupo vacío");
                return;
            }

            try
            {
                //Conexión al servidor
                Direcionip direcionip = new Direcionip();
                string direcionp = direcionip.direcion;
                cliente = new TcpClient(direcionp, 8080);
                flujo = cliente.GetStream();

                //Generar número aleatorio para la clave del grupo 
                Random r = new Random();
                int rand = (int)(DateTime.Now.Ticks % 10000);

                //Enviar mensaje de que se creo el grupo con los datos
                string mensaje = "3|"+rand+"|"+nombre+"|"+ _idUsuario;
                byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                flujo.Write(datos, 0, datos.Length);

                //Espera respuesta
                hilo = new Thread(new ThreadStart(escuchaservidor));
                hilo.IsBackground = true;
                hilo.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con el servidor: " + ex.Message);
            }

        }

        private void Crea_grupo_FormClosing(object sender, FormClosingEventArgs e)
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
            ch.Enabled = true;
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

                    if (partes.Length > 0)
                    {
                        if (partes[0] == "7")
                        {
                            this.Invoke((Action)(() =>
                            {
                                int idGrupo = int.Parse(partes[1]);
                                if (int.TryParse(_idUsuario, out int idusuario))
                                {
                                    AgregarMiembros am = new AgregarMiembros(idGrupo, idusuario, ch);
                                    am.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("id de usuario inválido. No se puede crear el grupo.");
                                }
                            }));
                        }
                        else if (partes[0] == "8")
                        {
                            this.Invoke((Action)(() =>
                            {
                                MessageBox.Show("Error al crear el grupo", "Intenta de nuevo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }));
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
            finally
            {
                try
                {
                    flujo.Close();
                    cliente.Close();
                }
                catch 
                { 
                }
            }
        }
    }
}



       
