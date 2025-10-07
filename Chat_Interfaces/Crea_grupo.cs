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
        string id = Chat.Sesionid1.IdUsuario1;

        public Crea_grupo()
        {
            InitializeComponent();
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
            string nombre =textBox1.Text;
            if (nombre=="")
            {
                MessageBox.Show("No puedes tener nombre de grupo vacio");
                return;
            }
            int rand=0,id1=1,num=0;
            string val;
            //Genera una clave de grupo aleatoria que no exista
            while(id1!=0)
            {
                Random r = new Random();
                rand = r.Next(1, 1000000);
                comando1 = new MySqlCommand("SELECT clave_grupo FROM grupos", conexion);
                leer1=comando1.ExecuteReader();
                id1=0;
                while(leer1.Read())
                {
                    val=(string)leer1["clave_grupo"];
                    num=int.Parse(val);
                    if (rand==num)
                    {
                        id1=1;
                        break;
                    }
                }
            }
            comando1.Dispose();
            leer1.Close();
            comando =new MySqlCommand("INSERT INTO grupos (clave_grupo,Nombre_grupo) \r\nvalues(@clav,@nom) ;", conexion);
            comando.Parameters.AddWithValue("@clav", rand);
            comando.Parameters.AddWithValue("@nom", nombre);
            comando.ExecuteNonQuery();
            this.Hide();
            comando.Dispose();
            //Obtener id del grupo 
            comando = new MySqlCommand("SELECT id from grupos where clave_grupo=@clav", conexion);
            comando.Parameters.AddWithValue("@clav", rand);
            leer = comando.ExecuteReader();
            int val1 = -1;
            if(leer.Read())
            {
                val1= (int)leer["id"];
            }
            comando.Dispose();
            leer.Close();
            //Insertamos en miembrros grupos
            comando=new MySqlCommand("INSERT into miembros_grupos(id_usuarios,id_grupo) \r\nvalues(@idu,@idg) ;", conexion);
            comando.Parameters.AddWithValue("@idu", id);
            comando.Parameters.AddWithValue("@idg", val1);
            comando.ExecuteNonQuery();
            comando.Dispose();
            Chat chat = new Chat();
            chat.Show();
        }
    }
}
