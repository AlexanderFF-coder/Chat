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
        public Crea_grupo()
        {
            InitializeComponent();
            conexion = new MySqlConnection("Server=localhost;Port=3306;Database=test;Uid=Alex;Pwd=12345");
            conexion.Open();
        }

        private void Crea_grupo_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Obtenemos los datos de textbox y checamos que no esten vacios
            string nombre =textBox1.Text;
            string clave=textBox2.Text;
            if (nombre=="")
            {
                MessageBox.Show("No puedes tener nombre de grupo vacio");
                return;
            }
            if(clave=="")
            {
                MessageBox.Show("No puedes tener clave vacia vacio");
            }
            comando=new MySqlCommand("INSERT INTO grupo (clave_grupo,Nombre_grupo) \r\nvalues(@clav,@nom) ;", conexion);
            comando.Parameters.AddWithValue("@clav", clave);
            comando.Parameters.AddWithValue("@nom", nombre);
            comando.ExecuteNonQuery();
            this.Hide();
            Chat chat = new Chat();
            chat.Show();
        }
    }
}
