using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Chat_Interfaces
{
    public partial class Chat : Form
    {
        private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=test;Uid=Alex;Pwd=12345";
        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";
        //Variables para manejar base de datos
        MySqlConnection conexion;
        MySqlCommand comando;
        MySqlDataReader leer;
        string id=InicioSesion.Sesionid.IdUsuario;
        public Chat()
        {
            InitializeComponent();
            listBox1.Items.Clear();          
            Sesionid1.IdUsuario1 = id;
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
                comando=new MySqlCommand("SELECT Nombre_grupo FROM  grupo ",conexion);
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
            //Crea un chat a partir del grupo seleccionado
            int id=0;
            comando=new MySqlCommand("SELECT id FROM grupo WHERE Nombre_grupo=@nom",conexion);
            comando.Parameters.AddWithValue("@nom",listBox1.SelectedItem.ToString());
            leer=comando.ExecuteReader();
            //Obtiene id del grupo
            while(leer.Read())
            {
                id=(int)leer["id"];
            }
            leer.Close();
            comando.Dispose();
            if(id==0)
            {
                return;
            }
            //Guarda los elementos en un panel con el contenido del mensje,hora y persona
            comando=new MySqlCommand("SELECT contenido,fecha WHERE Id_grupo=@id");
            comando.Parameters.AddWithValue("@id",id);
            leer=comando.ExecuteReader();
            while (leer.Read())
            {
                //Pone los mensjes en el panel en un cuadro de color
                Panel pan=new Panel();
                pan.BackColor=Color.Beige;
                pan.Width=panel1.Width-25;
                pan.Height=60;
                //Crea un lugar para guardar el mensaje
                TextBox txt=new TextBox();
                txt.Multiline=true;
                txt.Width=pan.Width-10;
                txt.Height=pan.Height-10;
                txt.Text=leer[leer.GetOrdinal("contenido")].ToString();
                pan.Controls.Add(txt);
                //Agrega  a principal
                panel1.Controls.Add(pan);
                //Pasamos la fecha
                Label lab=new Label();
                lab.Text=leer[leer.GetOrdinal("fecha")].ToString();
                lab.Top=pan.Bottom;
                lab.Left=pan.Left;
                panel1.Controls.Add(lab);



            }

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
            conexion=new MySqlConnection(MYSQL_CONNECTION_STRING);
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
            //Eliminamos los grupos que no corresponden a la persona con el id de inicio
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                comando = new MySqlCommand("Select id_usuarios from miembros_grupos where id_usuarios=@id", conexion);
                comando.Parameters.AddWithValue("@id", id);
                leer = comando.ExecuteReader();
                int checa = 0;
                while (leer.Read())
                {
                    checa = (int)leer["id_usuarios"];
                }
                leer.Close();
                comando.Dispose();
                if (checa == 0)
                {
                    listBox1.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            int id=0;
            //Pasa los mensajes a la base de datos
            conexion=new MySqlConnection(MYSQL_CONNECTION_STRING);
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
    public static class Sesionid1
    {
        public static string IdUsuario1;
    }
}
