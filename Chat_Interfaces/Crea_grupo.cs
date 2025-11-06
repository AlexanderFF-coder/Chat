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
        MySqlConnection conexion;
        MySqlCommand comando;
        MySqlDataReader leer;
        MySqlCommand comando1;
        MySqlDataReader leer1;
        //Variables para  server
        TcpClient cliente;
        NetworkStream flujo;
        Thread hilo;
        bool ejecutando = true;
        // CAMBIO 1: Eliminamos la dependencia estática y la reemplazamos por una variable de instancia
        private string _idUsuario;
        public Chat ch;
        // CAMBIO 2: El constructor ahora recibe el ID del usuario creador
        public Crea_grupo(string idUsuario, Chat ch)
        {
            InitializeComponent();
            this.ch = ch;
            // Asignamos el ID del usuario
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
                // Conexión al servidor
                cliente = new TcpClient("192.168.1.83", 8080);
                flujo = cliente.GetStream();

                // Generar número aleatorio para la clave del grupo (cliente lo genera)
                Random r = new Random();
                int rand = r.Next(1, 1000000);

                // Enviar mensaje de creación de grupo: 3|<clave>|<nombre>|<id_usuario>
                string mensaje = "3|"+rand+"|"+nombre+"|"+ _idUsuario;
                byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                flujo.Write(datos, 0, datos.Length);

                // Iniciar hilo para escuchar respuesta del servidor
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
            //Abilitamos el form de chat
            ejecutando = false;
            flujo.Close();
            cliente.Close();
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
                    this.Invoke((Action)(() =>
                    {
                        if (partes[0] == "7")
                        {
                            MessageBox.Show("Grupo creado", "dime los miembros que quieres agregar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            int idGrupo = int.Parse(partes[1]);
                            int idusuario = int.Parse(_idUsuario);
                            AgregarMiembros am = new AgregarMiembros(idGrupo, idusuario, ch);
                            am.Show();
                            this.Hide();
                        }
                        else
                        {
                            if (partes[0] == "8")
                            {
                                MessageBox.Show("Error al crear el grupo", "Intenta de nuevo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            finally
            {
                //Cerrar el flujo y el cliente al finalizar
                flujo.Close();
                cliente.Close();
            }

        }
    }
}



       
