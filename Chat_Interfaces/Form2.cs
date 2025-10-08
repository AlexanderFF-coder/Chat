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
            checkedListBoxUsuarios.ValueMember = "Id";

            CargarUsuarios();
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

            // No es necesario verificar si Count == 0, porque el creador ya está en el grupo.
            // Si hay seleccionados, los insertamos.

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

                        // Solo mostramos un mensaje de éxito si realmente se agregaron miembros
                        if (miembrosAgregados > 0)
                        {
                            MessageBox.Show($"Se agregaron {miembrosAgregados} miembros al grupo.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Si falla el insert, igual intentamos regresar al chat
                    MessageBox.Show("Error al insertar miembros: " + ex.Message, "Error DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // 3. Volver al formulario Chat con los datos del creador.
            VolverAFormularioChat();
        }

        // Método para volver a la forma principal
        private void VolverAFormularioChat()
        {
            string email = string.Empty;
            string nombre = string.Empty;

            // Paso 1: Obtener el email y el nombre del usuario creador (que está activo)
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
                    // Mostramos el error, pero permitimos que el flujo continúe
                    MessageBox.Show("Error al obtener datos del usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Paso 2: Crear el nuevo formulario Chat con los 3 parámetros requeridos
            if (!string.IsNullOrEmpty(email))
            {
                // La navegación será exitosa
                Chat chatW = new Chat(email, _idCreador.ToString(), nombre);
                chatW.Show();
                this.Close();
            }
            else
            {
                // La navegación falló (probablemente por el error capturado arriba), cerramos este formulario
                MessageBox.Show("No se pudo recuperar la información del usuario para volver al Chat.", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
