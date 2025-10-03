using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Chat_Interfaces
{
    public partial class Chat : Form
    {
        //Variables para manejar base de datos
        MySqlConnection conexion;
        MySqlCommand comando;
        MySqlDataReader leer;
        public Chat()
        {
            InitializeComponent();
            listBox1.Items.Clear();          
        }

        private void Chat_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Buscar chat en listbox y base de datos y selecionar el que mas se parezca y oculta lo demas (no funciona) 
            string chec=textBox1.Text;
            //Si no es nada llena con todos los chats
            listBox1.Items.Clear();

                //LLena el listbox con los grupos que alla en la base de datos (probado y funcional)
                comando=new MySqlCommand("SELECT Nombre_grupo FROM  grupo",conexion);
                leer=comando.ExecuteReader();
                while(leer.Read())
                {
                    listBox1.Items.Add(leer["Nombre_grupo"].ToString());
                    //Mostrar linea en medio pero no esta activa o puede presionarse en el programa
                    listBox1.Items.Add("--------------------------------------------------");
                }
            leer.Close();
            comando.Dispose();
            for(int i=0;i<listBox1.Items.Count;i++)
            {
                //Si esta esconde los elementos que no son
                //Convierte el texto a un string y en el indice checa si son iguales
                // StringComparison.CurrentCultureIgnoreCase se encarg de comporar sin importar si es mayus o minus si es menor no son iguales
                if(listBox1.Items[i].ToString().IndexOf(chec,StringComparison.CurrentCultureIgnoreCase)<0)
                {
                    listBox1.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Crea_grupo crea=new Crea_grupo();
            conexion.Close();
            crea.Show();
            this.Hide();
        }

        private void Chat_VisibleChanged(object sender, EventArgs e)
        {
            //Cuando es visible lee chat
            conexion=new MySqlConnection("Server=localhost;Port=3306;Database=test;Uid=Alex;Pwd=12345");
            conexion.Open();
            comando=new MySqlCommand("SELECT Nombre_grupo FROM  grupo", conexion);
            leer=comando.ExecuteReader();
            while(leer.Read())
            {
                listBox1.Items.Add(leer["Nombre_grupo"].ToString());
                //Mostrar linea en medio pero no esta activa o puede presionarse en el programa
                listBox1.Items.Add("--------------------------------------------------");
            }
            leer.Close();
            comando.Dispose();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            int id=0;
            //Pasa los mensajes a la base de datos
            conexion=new MySqlConnection("Server=localhost;Port=3306;Database=test;Uid=Alex;Pwd=12345");
            conexion.Open();
            //Obtiene el nombre del grupo y la clave del grupo
            string grupo=listBox1.SelectedItem.ToString();
            comando=new MySqlCommand("SELECT id FROM grupo WHERE Nombre_grupo=@nom",conexion);
            comando.Parameters.AddWithValue("@nom",grupo);
            leer=comando.ExecuteReader();
            while (leer.Read())
            {
                id=(int)leer["id"];
            }
            leer.Close();
            comando.Dispose();
            if(id==0)
            {
                return;
            }
            //Inserta el mensaje en la base de datos
            comando = new MySqlCommand("INSERT INTO mensajes (Id_grupo,contenido) \r\nvalues(@id,@cont) ;", conexion);
            comando.Parameters.AddWithValue("@id",id);
            comando.Parameters.AddWithValue("@cont",textBox2.Text);
            comando.ExecuteNonQuery();
            comando.Dispose();
            textBox2.Clear();
            conexion.Close();
        }
    }
}
