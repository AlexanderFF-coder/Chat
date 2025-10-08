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
        private static string id,ids;
        int tam = 0,tamaux;
        public Chat()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            id = InicioSesion.Sesionid.IdUsuario;

            buttonEmoji.Click += btnEmoji_Click;
            this.Controls.Add(buttonEmoji);

            btnEmojiSmile.Click += Emoji_Click;
            btnEmojiHeart.Click += Emoji_Click;
            btnEmojiSad.Click += Emoji_Click;

            this.Controls.Add(panelEmojis);
            panelEmojis.BringToFront();

            //estos son para cerrar el panel de emojis al hacer clic fuera de él
            //si llegan a agregar mas tools pongan: tool.MouseDown += Chat_MouseDown;
            this.MouseDown += Chat_MouseDown;
            listBox1.MouseDown += Chat_MouseDown;
            panel1.MouseDown += Chat_MouseDown;
            textBox1.MouseDown += Chat_MouseDown;
            textBox2.MouseDown += Chat_MouseDown;
            label1.MouseDown += Chat_MouseDown;
            label2.MouseDown += Chat_MouseDown;
        }

        private void btnEmoji_Click(object sender, EventArgs e)
        {
            panelEmojis.Visible = !panelEmojis.Visible; // Mostrar u ocultar el panel de emojis
        }

        private void Emoji_Click(object sender, EventArgs e)
        {
            Button emojiButton = sender as Button;
            if (emojiButton != null)
            {
                textBox2.Text += emojiButton.Text; // Añade emoji al texto del mensaje
            }
        }

        private void Chat_MouseDown(object sender, MouseEventArgs e)
        {
            // Si el panel de emojis está visible y el clic NO fue dentro de él
            if (panelEmojis.Visible && !panelEmojis.Bounds.Contains(e.Location) && !buttonEmoji.Bounds.Contains(e.Location))
            {
                panelEmojis.Visible = false;
            }
        }



        private void Chat_Load(object sender, EventArgs e)
        {

        }
        //Pendiente(Mostrar grupos del usu solamente)
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Buscar chat en listbox y base de datos y selecionar el que mas se parezca y oculta lo demas (no funciona) 
            string chec = textBox1.Text;
            //Si no es nada llena con todos los chats
            listBox1.Items.Clear();
            panel1.Controls.Clear();
            comando.Dispose();
            leer.Close();
            //LLena el listbox con los grupos que alla en la base de datos (probado y funcional)
            comando = new MySqlCommand("SELECT Nombre_grupo FROM  grupos ", conexion);
            leer = comando.ExecuteReader();
            while (leer.Read())
            {
                listBox1.Items.Add(leer["Nombre_grupo"].ToString());
                //Mostrar linea en medio pero no esta activa o puede presionarse en el programa
                listBox1.Items.Add("--------------------------------------------------");
            }
            leer.Close();
            comando.Dispose();
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                //Si esta esconde los elementos que no son
                //Convierte el texto a un string y en el indice checa si son iguales
                // StringComparison.CurrentCultureIgnoreCase se encarg de comporar sin importar si es mayus o minus si es menor no son iguales
                if (listBox1.Items[i].ToString().IndexOf(chec, StringComparison.CurrentCultureIgnoreCase) < 0)
                {
                    listBox1.Items.RemoveAt(i);
                    i--;
                }
            }
            int cont = 0;
            //Eliminamos los grupos que no corresponden a la persona con el id de inicio
            for (int i = listBox1.Items.Count - 1; i >= 0; i--)
            {
                //Checamos el id del grupo
                string nom = listBox1.Items[i].ToString();
                int idob = 0;
                using (comando = new MySqlCommand("Select id from grupos where Nombre_grupo=@nom", conexion))
                {
                    comando.Parameters.AddWithValue("@nom", nom);
                    using (leer = comando.ExecuteReader())
                    {
                        if (leer.Read())
                        {
                            idob = (int)leer["id"];
                        }
                    }
                }
                if (idob == 0)
                {
                    listBox1.Items.RemoveAt(i);
                    //Continue salta al siguiente
                    continue;

                }
                //Ahora checa el id de miembro grupo al actual
                using (MySqlCommand comando1 = new MySqlCommand("Select count(*) from miembros_grupos where id_grupo=@id and id_usuarios=@idus", conexion))
                {
                    comando1.Parameters.AddWithValue("@id", idob);
                    comando1.Parameters.AddWithValue("@idus", id);
                    cont = Convert.ToInt32(comando1.ExecuteScalar());
                    if (cont == 0)
                    {
                        listBox1.Items.RemoveAt(i);
                    }
                }
            }
        }
        //Pendiente
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Crea un chat a partir del grupo seleccionado
            int id2 = -1;
            panel1.Controls.Clear();
            comando.Dispose();
            leer.Close();
            comando = new MySqlCommand("SELECT id FROM grupos WHERE Nombre_grupo=@nom", conexion);
            comando.Parameters.AddWithValue("@nom", listBox1.SelectedItem.ToString());
            leer = comando.ExecuteReader();
            //Obtiene id del grupo
            if (leer.Read())
            {
                id2 = (int)leer["id"];
            }
            leer.Close();
            comando.Dispose();
            if (id2 == -1)
            {
                return;
            }
            //Guarda los elementos en un panel con el contenido del mensje,hora y persona
            comando = new MySqlCommand("SELECT contenido,fecha from mensajes WHERE Id_grupo=@id",conexion);
            comando.Parameters.AddWithValue("@id", id2);
            leer = comando.ExecuteReader();
            tam = 0;
            while (leer.Read())
            {
                //Pone los mensjes en el panel en un cuadro de color
                Panel pan = new Panel();
                pan.Width = panel1.Width - 25;
                pan.Height = 30;
                pan.Top = tam;
                //Crea un lugar para guardar el mensaje
                Label txt = new Label();
                txt.Width = pan.Width - 10;
                txt.Height = pan.Height - 10;
                txt.Font = new Font("Arial", 12);
                //por si el usuario usó texto plano pa representar un emoji
                txt.Text = ConvertirTextoPlanoAEmojis(leer["contenido"].ToString());

                pan.Controls.Add(txt);
                //Agrega  a principal
                panel1.Controls.Add(pan);
                //Pasamos la fecha
                Label lab = new Label();
                lab.Text = leer[leer.GetOrdinal("fecha")].ToString();
                lab.Top = pan.Bottom;
                lab.Left = pan.Left;
                lab.Text = "Fecha:" + lab.Text;
                panel1.Controls.Add(lab);
                tam += pan.Height + lab.Height;
            }
            tamaux = tam;
            leer.Close();
            comando.Dispose();
            //Si  es mi mensaje es beige si no  es azul
            MySqlCommand comando2 = new MySqlCommand("SELECT id_usuarios FROM miembros_grupos where id_grupo=@idg", conexion);
            comando2.Parameters.AddWithValue("@idg", id2);
            //Generamos una lista de usuarios que pertenece al grupo
            List<int> usuarios = new List<int>();
            leer = comando2.ExecuteReader();
            while (leer.Read())
            {
                usuarios.Add((int)leer["id_usuarios"]);
            }
            leer.Close();
            int j = 0;
            //Cambiamos a checar el control (queda pendiente checar una manera
            foreach(Control c in panel1.Controls)
            {
                if(c is Panel)
                {
                    //checamos color
                    int ids = Convert.ToInt32(id);
                    if (j<usuarios.Count&& ids == usuarios[j])
                    {
                        c.BackColor = Color.Beige;
                    }
                    else
                    {
                        c.BackColor = Color.LightBlue;
                    }
                    j++;
                }
               
                    
            }
            comando2.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Crea_grupo crea = new Crea_grupo();
            conexion.Close();
            crea.Show();
            this.Hide();
        }
        //Pendiente(Mostrar grupos del usu)
        private void Chat_VisibleChanged(object sender, EventArgs e)
        {
            int cont = 0;
            listBox1.Items.Clear();
            //Cuando es visible lee chat
            conexion = new MySqlConnection(MYSQL_CONNECTION_STRING);
            conexion.Open();
            comando = new MySqlCommand("SELECT Nombre_grupo FROM  grupos", conexion);
            leer = comando.ExecuteReader();
            while (leer.Read())
            {
                listBox1.Items.Add(leer["Nombre_grupo"].ToString());
                //Mostrar linea en medio pero no esta activa o puede presionarse en el programa
                listBox1.Items.Add("--------------------------------------------------");
            }
            leer.Close();
            //Eliminamos los grupos que no corresponden a la persona con el id de inicio
            for (int i = listBox1.Items.Count - 1; i >= 0; i--)
            {
                //Checamos el id del grupo
                string nom = listBox1.Items[i].ToString();
                int idob = 0;
                using (comando = new MySqlCommand("Select id from grupos where Nombre_grupo=@nom", conexion))
                {
                    comando.Parameters.AddWithValue("@nom", nom);
                    using (leer = comando.ExecuteReader())
                    {
                        if (leer.Read())
                        {
                            idob = (int)leer["id"];
                        }
                    }
                }
                if (idob == 0)
                {
                    listBox1.Items.RemoveAt(i);
                    //Continue salta al siguiente
                    continue;

                }
                //Ahora checa el id de miembro grupo al actual
                using (MySqlCommand comando1 = new MySqlCommand("Select count(*) from miembros_grupos where id_grupo=@id and id_usuarios=@idus", conexion))
                {
                    comando1.Parameters.AddWithValue("@id", idob);
                    comando1.Parameters.AddWithValue("@idus", id);
                    cont = Convert.ToInt32(comando1.ExecuteScalar());
                    if (cont == 0)
                    {
                        listBox1.Items.RemoveAt(i);
                    }
                }
            }
        }
    
        //Pendiente
        private void label2_Click(object sender, EventArgs e)
        {
            int id=0;
            //Tiene que tener un grupo selecionado afuerza
            if(listBox1.SelectedItem==null||textBox2.Text=="")
            {
                return;
            }
            //Pasa los mensajes a la base de datos
            conexion =new MySqlConnection(MYSQL_CONNECTION_STRING);
            conexion.Open();
            //Obtiene el nombre del grupo y la clave del grupo
            string grupo=listBox1.SelectedItem.ToString();
            comando=new MySqlCommand("SELECT id FROM grupos WHERE Nombre_grupo=@nom",conexion);
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
            string textoPlano = ConvertirEmojisATextoPlano(textBox2.Text);

            comando = new MySqlCommand("INSERT INTO mensajes (Id_grupo,contenido) \r\nvalues(@id,@cont) ;", conexion);
            comando.Parameters.AddWithValue("@id",id);
            comando.Parameters.AddWithValue("@cont",textoPlano);
            comando.ExecuteNonQuery();
            comando.Dispose();

            //Pone los mensjes en el panel en un cuadro de color
            Panel pan = new Panel();
            pan.BackColor = Color.Beige;
            pan.Width = panel1.Width - 25;
            pan.Height = 30;
            pan.Top = tamaux;
            //Crea un lugar para guardar el mensaje
            Label txt = new Label();
            txt.Font = new Font("Arial", 12); 
            txt.Width = pan.Width - 10;
            txt.Height = pan.Height - 10;
            txt.Text = textBox2.Text;
            pan.Controls.Add(txt);
            //Agrega  a principal
            panel1.Controls.Add(pan);
            //Pasamos la fecha
            Label lab = new Label();
            lab.Text = DateTime.Now.ToString();
            lab.Top = pan.Bottom;
            lab.Left = pan.Left;
            lab.Text = "Fecha:" + lab.Text;
            tamaux += pan.Height + lab.Height;
            panel1.Controls.Add(lab);
            textBox2.Clear();
        }

        private string ConvertirEmojisATextoPlano(string texto)
        {
            //por si el usuario escribió un emoji real
            texto = texto.Replace("😁", ":smile:");
            texto = texto.Replace("❤️", ":heart:");
            texto = texto.Replace("😔", ":sad:");
            return texto;
        }

        //metodo por si el usuario puso texto plano pa representar un emoji 
        private string ConvertirTextoPlanoAEmojis(string texto)
        {
            texto = texto.Replace(":smile:", "😁");
            texto = texto.Replace(":heart:", "❤️");
            texto = texto.Replace(":sad:", "😔");
            return texto;
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            comando.Dispose();
            leer.Close();
            conexion.Close();
            Application.Exit();
        }

        public static class Sesionid1
        {
            public static string IdUsuario1=id;
        }
    }
}
