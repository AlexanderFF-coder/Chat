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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net.Sockets;
using System.Threading;
namespace Chat_Interfaces
{
    public partial class AgregarMiembros : Form
    {
        //Variables para  server
        TcpClient cliente;
        NetworkStream flujo;
        Thread hilo;
        bool ejecutando = true;
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=test;Uid=Alex;Pwd=12345";
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";

        // Variables para almacenar los IDs del grupo y del creador
        private int _idGrupo;
        private int _idCreador;
        Chat ch;
        // Clase simple para guardar el ID y el Nombre del usuario
        private class UsuarioItem
        {
            public int Id { get; set; }
            public string NombreCompleto { get; set; }

            public override string ToString()
            {
                // Esto es lo que se mostrará en el CheckedListBox
                return NombreCompleto;
            }
        }

        // El constructor ahora recibe el ID del grupo y el ID del usuario creador
        public AgregarMiembros(int idGrupo, int idCreador,Chat ch)
        {
            InitializeComponent();
            _idGrupo = idGrupo;
            _idCreador = idCreador;
            this.Text = "Agregar Miembros al Grupo ID: " + idGrupo; // Título de la ventana
            this.ch = ch;
            // Configurar el CheckedListBox para que muestre el nombre y guarde el ID
            checkedListBoxUsuarios.DisplayMember = "NombreCompleto";
            checkedListBoxUsuarios.ValueMember = "Id";

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            checkedListBoxUsuarios.Items.Clear();

            //Solicita los usuarios disponibles que no estan en el grupo
            string mensaje = "lista_miembros|" + _idGrupo+"|" +_idCreador;
            //flujo = ch.flujo; queda pendiente de ponerlo en chat
            byte[] datos = Encoding.UTF8.GetBytes(mensaje);
            flujo.Write(datos, 0, datos.Length);
            //Lee la respuesta del servidor
            byte[] buffer = new byte[4096];
            int bytesLeidos = flujo.Read(buffer, 0, buffer.Length);
            string respuesta = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
            string[] usuarios = respuesta.Split(';');
            foreach (string usuario in usuarios)
            {
                if (!string.IsNullOrWhiteSpace(usuario))
                {
                    string[] partes = usuario.Split('|');
                    if (partes.Length == 2)
                    {
                        int idUsuario = int.Parse(partes[0]);
                        string nombreCompleto = partes[1];
                        UsuarioItem item = new UsuarioItem
                        {
                            Id = idUsuario,
                            NombreCompleto = nombreCompleto
                        };
                        checkedListBoxUsuarios.Items.Add(item);
                    }
                }
            }
        }

        // Este método maneja el botón "Finalizar" o "Agregar"
        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            
        }


        private void AgregarMiembros_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Activa el form chat y cierra esteform
            ch.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> idsSeleccionados = new List<int>();

            // 1. Recopilar IDs de los usuarios seleccionados
            foreach (UsuarioItem item in checkedListBoxUsuarios.CheckedItems)
            {
                idsSeleccionados.Add(item.Id);
            }

            // No es necesario verificar si Count == 0, porque el creador ya está en el grupo.
            // Si hay seleccionados, los insertamos.

            //Manda la lista de ids al servidor para agregarlos al grupo
            string mensaje = "agregar_miembros|" + idsSeleccionados.Count+"|" + _idGrupo + " | ";
            mensaje += string.Join(",", idsSeleccionados);
            byte[] datos = Encoding.UTF8.GetBytes(mensaje);
            flujo.Write(datos, 0, datos.Length);
            //Lee la respuesta del servidor
            byte[] buffer = new byte[4096];
            int bytesLeidos = flujo.Read(buffer, 0, buffer.Length);
            string respuesta = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
            MessageBox.Show(respuesta, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);


            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
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
                        if (partes[0] == "0")
                        {
                            MessageBox.Show("Grupo creado", "dime los miembros que quieres agregar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            int idGrupo = int.Parse(partes[1]);
                            int idusuario = _idCreador;
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
