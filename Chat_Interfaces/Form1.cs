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

namespace Chat_Interfaces
{
    public partial class AgregarMiembros : Form
    {
        private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";

        // Variables para almacenar los IDs del grupo y del creador
        private int _idGrupo;
        private int _idCreador;

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
        public AgregarMiembros(int idGrupo, int idCreador)
        {
            InitializeComponent();
            _idGrupo = idGrupo;
            _idCreador = idCreador;

            this.Text = "Agregar Miembros al Grupo ID: " + idGrupo; // Título de la ventana

            // Configurar el CheckedListBox para que muestre el nombre y guarde el ID
            checkedListBoxUsuarios.DisplayMember = "NombreCompleto";
            // Nota: No se necesita ValueMember si usamos la clase UsuarioItem

            CargarUsuarios();
        }

        private void AgregarMiembros_Load(object sender, EventArgs e)
        {
            // Puedes usar el evento Load para inicializar si es necesario, 
            // pero CargarUsuarios ya se llama en el constructor.
        }

        private void CargarUsuarios()
        {
            checkedListBoxUsuarios.Items.Clear();

            using (MySqlConnection conexion = new MySqlConnection(MYSQL_CONNECTION_STRING))
            {
                try
                {
                    conexion.Open();
                    // Query: Selecciona todos los usuarios cuyo ID NO sea el ID del creador
                    // y que NO sean ya miembros de este grupo.
                    string query = "SELECT id, nombre, email FROM usuarios WHERE id != @idCreador AND id NOT IN (SELECT id_usuarios FROM miembros_grupos WHERE id_grupo = @idGrupo)";

                    using (MySqlCommand comando = new MySqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@idCreador", _idCreador);
                        comando.Parameters.AddWithValue("@idGrupo", _idGrupo);

                        using (MySqlDataReader leer = comando.ExecuteReader())
                        {
                            while (leer.Read())
                            {
                                // Crea un objeto UsuarioItem para cada usuario encontrado
                                checkedListBoxUsuarios.Items.Add(new UsuarioItem
                                {
                                    Id = leer.GetInt32("id"),
                                    NombreCompleto = leer.GetString("nombre") + " (" + leer.GetString("email") + ")"
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Este método maneja el botón "Finalizar" o "Agregar"
        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            List<int> idsSeleccionados = new List<int>();

            // 1. Recopilar IDs de los usuarios seleccionados
            foreach (UsuarioItem item in checkedListBoxUsuarios.CheckedItems)
            {
                idsSeleccionados.Add(item.Id);
            }

            if (idsSeleccionados.Count == 0)
            {
                MessageBox.Show("No se seleccionó ningún miembro. Volviendo al chat.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                VolverAFormularioChat();
                return;
            }

            // 2. Insertar múltiples miembros en la base de datos
            using (MySqlConnection conexion = new MySqlConnection(MYSQL_CONNECTION_STRING))
            {
                try
                {
                    conexion.Open();
                    string query = "INSERT INTO miembros_grupos (id_usuarios, id_grupo) VALUES (@idu, @idg)";

                    using (MySqlCommand comando = new MySqlCommand(query, conexion))
                    {
                        // Preparar los parámetros fijos (solo el ID del grupo)
                        comando.Parameters.Add("@idg", MySqlDbType.Int32).Value = _idGrupo;
                        comando.Parameters.Add("@idu", MySqlDbType.Int32); // Este se actualizará en el loop

                        int miembrosAgregados = 0;
                        foreach (int idUsuario in idsSeleccionados)
                        {
                            // Actualizar el valor del parámetro del usuario
                            comando.Parameters["@idu"].Value = idUsuario;
                            comando.ExecuteNonQuery();
                            miembrosAgregados++;
                        }

                        MessageBox.Show($"Se agregaron {miembrosAgregados} miembros al grupo.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar miembros: " + ex.Message, "Error DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            VolverAFormularioChat();
        }

        // --- MÉTODOS DE RETORNO Y OBTENCIÓN DE DATOS ---

        // Método para obtener los datos necesarios del usuario logeado
        private (string email, string nombre) ObtenerDatosUsuarioLogeado()
        {
            string email = string.Empty;
            string nombre = string.Empty;

            using (MySqlConnection conexion = new MySqlConnection(MYSQL_CONNECTION_STRING))
            {
                try
                {
                    conexion.Open();
                    string query = "SELECT email, nombre FROM usuarios WHERE id = @idUsuario";

                    using (MySqlCommand comando = new MySqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@idUsuario", _idCreador);
                        using (MySqlDataReader leer = comando.ExecuteReader())
                        {
                            if (leer.Read())
                            {
                                email = leer.GetString("email");
                                nombre = leer.GetString("nombre");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al recuperar datos del usuario: " + ex.Message, "Error DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return (email, nombre);
        }

        // Método para volver a la forma principal (Chat)
        private void VolverAFormularioChat()
        {
            var datos = ObtenerDatosUsuarioLogeado();

            // Verificamos que se hayan recuperado los datos
            if (string.IsNullOrEmpty(datos.email))
            {
                MessageBox.Show("Error: No se pudo identificar al usuario logeado. Cerrando aplicación.", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // Llamada CORRECTA al constructor de Chat, usando los 3 parámetros necesarios.
            Chat chatW = new Chat(datos.email, _idCreador.ToString(), datos.nombre);
            chatW.Show();
            this.Close();
        }
    }
}
