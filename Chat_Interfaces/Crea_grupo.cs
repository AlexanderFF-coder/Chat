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
    public partial class Crea_grupo : Form
    {
        MySqlConnection conexion;
        MySqlCommand comando;
        MySqlDataReader leer;
        MySqlCommand comando1;
        MySqlDataReader leer1;
        // CAMBIO 1: Eliminamos la dependencia estática y la reemplazamos por una variable de instancia
        private string _idUsuario;
        public Chat ch;
        // CAMBIO 2: El constructor ahora recibe el ID del usuario creador
        public Crea_grupo(string idUsuario,Chat ch)
        {
            InitializeComponent();
            this.ch = ch;
            // Asignamos el ID del usuario
            _idUsuario = idUsuario;
            //conexion = new MySqlConnection("Server=localhost;Port=3306;Database=test;Uid=Alex;Pwd=12345");
            conexion = new MySqlConnection("Server=localhost;Port=3306;Database=chat;Uid=root;Pwd=Alex");
            conexion.Open();
        }

        private void Crea_grupo_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Obtenemos los datos de textbox y checamos que no esten vacios
            string nombre = textBox1.Text;
            if (nombre == "")
            {
                MessageBox.Show("No puedes tener nombre de grupo vacio");
                return;
            }
            int rand = 0, id1 = 1, num = 0;
            string val;

            //Genera una clave de grupo aleatoria que no exista
            while (id1 != 0)
            {
                Random r = new Random();
                rand = r.Next(1, 1000000);
                comando1 = new MySqlCommand("SELECT clave_grupo FROM grupos", conexion);
                leer1 = comando1.ExecuteReader();
                id1 = 0;
                while (leer1.Read())
                {
                    val = (string)leer1["clave_grupo"];
                    num = int.Parse(val);
                    if (rand == num)
                    {
                        id1 = 1;
                        break;
                    }
                }
                leer1.Close(); // Aseguramos el cierre del lector
                comando1.Dispose();
            }

            // Insertar grupo
            comando = new MySqlCommand("INSERT INTO grupos (clave_grupo,Nombre_grupo) \r\nvalues(@clav,@nom) ;", conexion);
            comando.Parameters.AddWithValue("@clav", rand);
            comando.Parameters.AddWithValue("@nom", nombre);
            comando.ExecuteNonQuery();
            // this.Hide(); // Mantenemos el hide al final
            comando.Dispose();

            // Obtener id del grupo (OPTIMIZADO: Usamos LAST_INSERT_ID() para mayor fiabilidad)
            comando = new MySqlCommand("SELECT LAST_INSERT_ID() as id", conexion);
            leer = comando.ExecuteReader();
            int idGrupoRecienCreado = -1;
            if (leer.Read())
            {
                idGrupoRecienCreado = leer.GetInt32("id"); // Usamos GetInt32 para el ID
            }
            comando.Dispose();
            leer.Close();

            if (idGrupoRecienCreado == -1)
            {
                MessageBox.Show("Error al obtener el ID del grupo recién creado.", "Error DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insertamos al usuario CREADOR en miembrros grupos
            comando = new MySqlCommand("INSERT into miembros_grupos(id_usuario,id_grupo) \r\nvalues(@idu,@idg) ;", conexion);
            comando.Parameters.AddWithValue("@idu", _idUsuario); // USAMOS EL ID DEL USUARIO CREADOR
            comando.Parameters.AddWithValue("@idg", idGrupoRecienCreado);
            comando.ExecuteNonQuery();
            comando.Dispose();

            // CAMBIO 3: Nueva Lógica de Navegación a AgregarMiembros

            // 1. Convertir el ID de usuario (string) a int para pasarlo a AgregarMiembros
            if (!int.TryParse(_idUsuario, out int idCreadorInt))
            {
                MessageBox.Show("Error al obtener el ID del usuario.", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. Abrir el formulario para agregar miembros, pasando los IDs
            AgregarMiembros agregarMiembros = new AgregarMiembros(idGrupoRecienCreado, idCreadorInt,ch);
            agregarMiembros.Show();
            this.Hide();

        }

        private void Crea_grupo_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Abilitamos el form de chat
            ch.Enabled = true;
        }
    }
}
