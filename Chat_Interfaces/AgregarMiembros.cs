using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Interfaces
{
    public partial class AgregarMiembros : Form
    {
        private int _idGrupo;
        private int _idCreador;
        private Chat ch;

        private class UsuarioItem
        {
            public int Id { get; set; }
            public string NombreCompleto { get; set; }
            public override string ToString() => NombreCompleto;
        }

        public AgregarMiembros(int idGrupo, int idCreador, Chat ch)
        {
            InitializeComponent();
            _idGrupo = idGrupo;
            _idCreador = idCreador;
            this.ch = ch;
            this.Text = "Agregar Miembros al Grupo ID: " + idGrupo;

            checkedListBoxUsuarios.DisplayMember = "NombreCompleto";
            checkedListBoxUsuarios.ValueMember = "Id";

            //Cargar usuarios  usando un metodo asincrono que no bloquee la interfaz https://we-school.es/como-manejar-operaciones-asincronas-en-c/
            _ = CargarUsuariosAsync();
        }
        //Cargamos usuarios
        private async Task CargarUsuariosAsync()
        {
            checkedListBoxUsuarios.Items.Clear();

            string mensaje = "lista_miembros|"+_idGrupo+"|"+_idCreador;
            try
            {
                using (TcpClient cliente = new TcpClient())
                {
                    await cliente.ConnectAsync("192.168.1.83", 8080);

                    using (NetworkStream flujo = cliente.GetStream())
                    {
                        byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                        await flujo.WriteAsync(datos, 0, datos.Length);
                        await flujo.FlushAsync(); 

                        //Leer respuesta completa
                        byte[] buffer = new byte[8192];
                        int bytesLeidos = await flujo.ReadAsync(buffer, 0, buffer.Length);
                        string respuesta = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);

                        //Procesar lista
                        string[] usuarios = respuesta.Split(';');

                        this.Invoke((Action)(() =>
                        {
                            foreach (string usuario in usuarios)
                            {
                                string[] partes = usuario.Split('|');
                                if (partes.Length >= 4 && partes[0] == "usuario_lista")
                                {
                                    int idUsuario = int.Parse(partes[1]);
                                    string nombre = partes[2].Trim();
                                    string email = partes[3].Trim();
                                    //Agregar a la lista
                                    checkedListBoxUsuarios.Items.Add(new UsuarioItem { Id = idUsuario, NombreCompleto = $"{nombre} ({email})" });
                                }
                            }

                            if (checkedListBoxUsuarios.Items.Count == 0)
                            {
                                MessageBox.Show("No hay usuarios disponibles para agregar al grupo.");
                            }
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }
        //Agregar a miembros seleccionados
        private async void button1_Click(object sender, EventArgs e)
        {
            List<int> idsSeleccionados = new List<int>();
            foreach (UsuarioItem item in checkedListBoxUsuarios.CheckedItems)
            {
                idsSeleccionados.Add(item.Id);
            }

            if (idsSeleccionados.Count == 0)
            {
                //Manda un mensaje de agregar un usuario
                MessageBox.Show("Selecciona al menos un usuario ", "mensaje",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string mensaje = "agregar_miembros|"+idsSeleccionados.Count+"|"+_idGrupo+"|"+string.Join(",", idsSeleccionados);
            try
            {
                using (TcpClient cliente = new TcpClient())
                {
                    await cliente.ConnectAsync("192.168.1.83", 8080);
                    using (NetworkStream flujo = cliente.GetStream())
                    {
                        byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                        await flujo.WriteAsync(datos, 0, datos.Length);

                        byte[] buffer = new byte[4096];
                        int bytesLeidos = await flujo.ReadAsync(buffer, 0, buffer.Length);
                        string respuesta = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);

                        MessageBox.Show(respuesta, "Usuarios agregados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                this.Close();
                ch.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar usuarios: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            ch.Enabled = true;
        }

        private void AgregarMiembros_FormClosing(object sender, FormClosingEventArgs e)
        {
            ch.Enabled = true;
        }
    }
}