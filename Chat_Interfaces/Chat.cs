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

        // Variables de sesión ahora como campos de instancia
        private string _usuarioEmail;
        private string _idUsuario;
        private string _nombreUsuario;

        //Variables para manejar base de datos
        MySqlConnection conexion;
        MySqlCommand comando;
        MySqlDataReader leer;

        // Se eliminan los campos estáticos problemáticos (id, ids) y se usa _idUsuario
        int tam = 0, tamaux;

        // CONSTRUCTOR ACTUALIZADO para recibir los 3 parámetros
        public Chat(string email, string idUsuario, string nombreUsuario)
        {
            InitializeComponent();

            // 1. Almacenamos los datos de sesión en las variables de instancia
            _usuarioEmail = email;
            _idUsuario = idUsuario;
            _nombreUsuario = nombreUsuario;

            // Opcional: Mostrar el nombre del usuario en el título de la ventana
            this.Text = $"Chat - Sesión: {_nombreUsuario}";

            listBox1.Items.Clear();

            // Ya NO se necesita esta línea, ahora usamos _idUsuario
            // id = InicioSesion.Sesionid.IdUsuario;

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

            // Inicializamos la conexión aquí para usarla en el resto de la clase
            conexion = new MySqlConnection(MYSQL_CONNECTION_STRING);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Buscar chat en listbox y base de datos y seleccionar el que mas se parezca y oculta lo demas (no funciona) 
            string chec = textBox1.Text;
            //Si no es nada llena con todos los chats
            listBox1.Items.Clear();
            panel1.Controls.Clear();
            // Eliminamos dispose/close fuera de using/finally para evitar errores

            if (conexion.State != ConnectionState.Open) conexion.Open();

            try
            {
                //LLena el listbox con los grupos que alla en la base de datos (probado y funcional)
                using (MySqlCommand cmdGrupos = new MySqlCommand("SELECT Nombre_grupo FROM grupos ", conexion))
                using (MySqlDataReader readerGrupos = cmdGrupos.ExecuteReader())
                {
                    while (readerGrupos.Read())
                    {
                        listBox1.Items.Add(readerGrupos["Nombre_grupo"].ToString());
                        //Mostrar linea en medio pero no esta activa o puede presionarse en el programa
                        listBox1.Items.Add("--------------------------------------------------");
                    }
                }

                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    //Si esta esconde los elementos que no son
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

                    if (nom.Contains("---")) continue; // Saltar separadores

                    int idob = 0;

                    using (MySqlCommand cmdGetId = new MySqlCommand("Select id from grupos where Nombre_grupo=@nom", conexion))
                    {
                        cmdGetId.Parameters.AddWithValue("@nom", nom);
                        using (MySqlDataReader readerId = cmdGetId.ExecuteReader())
                        {
                            if (readerId.Read())
                            {
                                idob = (int)readerId["id"];
                            }
                        }
                    }

                    if (idob == 0)
                    {
                        listBox1.Items.RemoveAt(i);
                        if (i > 0 && listBox1.Items[i - 1].ToString().Contains("---"))
                            listBox1.Items.RemoveAt(i - 1);
                        continue;
                    }

                    //Ahora checa el id de miembro grupo al actual
                    using (MySqlCommand comando1 = new MySqlCommand("Select count(*) from miembros_grupos where id_grupo=@id and id_usuario=@idus", conexion))
                    {
                        comando1.Parameters.AddWithValue("@id", idob);
                        comando1.Parameters.AddWithValue("@idus", _idUsuario); // USAMOS LA VARIABLE DE INSTANCIA
                        cont = Convert.ToInt32(comando1.ExecuteScalar());
                        if (cont == 0)
                        {
                            listBox1.Items.RemoveAt(i);
                            if (i > 0 && listBox1.Items[i - 1].ToString().Contains("---"))
                                listBox1.Items.RemoveAt(i - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar o filtrar grupos: " + ex.Message, "Error DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Crea un chat a partir del grupo seleccionado
            int id2 = -1;
            panel1.Controls.Clear();
            // Eliminamos dispose/close aquí para refactorizar la lógica con using

            if (conexion.State != ConnectionState.Open) conexion.Open();

            try
            {
                //Obtiene id del grupo
                using (MySqlCommand cmdGetId = new MySqlCommand("SELECT id FROM grupos WHERE Nombre_grupo=@nom", conexion))
                {
                    cmdGetId.Parameters.AddWithValue("@nom", listBox1.SelectedItem.ToString());
                    using (MySqlDataReader readerId = cmdGetId.ExecuteReader())
                    {
                        if (readerId.Read())
                        {
                            id2 = (int)readerId["id"];
                        }
                    }
                }

                if (id2 == -1)
                {
                    return;
                }

                //Guarda los elementos en un panel con el contenido del mensje,hora y persona
                string sql = "SELECT contenido, fecha, Id_usuario FROM mensajes WHERE Id_grupo=@id ORDER BY fecha ASC";

                using (MySqlCommand cmdMensajes = new MySqlCommand(sql, conexion))
                {
                    cmdMensajes.Parameters.AddWithValue("@id", id2);
                    using (MySqlDataReader readerMensajes = cmdMensajes.ExecuteReader())
                    {
                        tam = 0;
                        while (readerMensajes.Read())
                        {
                            // Obtener ID del emisor
                            int idMensajeUsuario = readerMensajes.GetInt32("Id_usuario");

                            //Pone los mensjes en el panel en un cuadro de color
                            Panel pan = new Panel();
                            pan.Width = panel1.Width - 25;
                            pan.Height = 30;
                            pan.Top = tam;

                            // Asignar color segun el emisor
                            if (idMensajeUsuario.ToString() == _idUsuario)
                                {
                                pan.BackColor = Color.LightBlue; // Mi mensaje
                            }
                            else
                            {
                                pan.BackColor = Color.Beige; // Mensaje de otro usuario
                            }

                            //Crea un lugar para guardar el mensaje
                            Label txt = new Label();
                            txt.Width = pan.Width - 10;
                            txt.Height = pan.Height - 10;
                            txt.Font = new Font("Arial", 12);
                            //por si el usuario usó texto plano pa representar un emoji
                            txt.Text = ConvertirTextoPlanoAEmojis(readerMensajes["contenido"].ToString());

                            pan.Controls.Add(txt);
                            //Agrega  a principal
                            panel1.Controls.Add(pan);
                            //Pasamos la fecha
                            Label lab = new Label();
                            lab.Text = readerMensajes.GetDateTime("fecha").ToString(); // Usar GetDateTime para precisión
                            lab.Top = pan.Bottom;
                            lab.Left = pan.Left;
                            lab.Text = "Fecha:" + lab.Text;
                            panel1.Controls.Add(lab);
                            tam += pan.Height + lab.Height;
                        }
                    }
                }

                tamaux = tam;
                /* La lógica de colores ya está manejada arriba, no es necesario repetirla
                //Si  es mi mensaje es beige si no  es azul
                using (MySqlCommand comando2 = new MySqlCommand("SELECT id_usuarios FROM miembros_grupos where id_grupo=@idg", conexion))
                {
                    comando2.Parameters.AddWithValue("@idg", id2);
                    //Generamos una lista de usuarios que pertenece al grupo
                    List<int> usuarios = new List<int>();
                    using (MySqlDataReader readerMiembros = comando2.ExecuteReader())
                    {
                        while (readerMiembros.Read())
                        {
                            usuarios.Add((int)readerMiembros["id_usuarios"]);
                        }
                    }
                    if (usuarios.Contains(Convert.ToInt32(_idUsuario))==true)
                    {
                        usuarios.Remove(Convert.ToInt32(_idUsuario));
                    }
                    int j = 0;
                    //Cambiamos a checar el control 
                    foreach (Control c in panel1.Controls)
                    {
                        if (c is Panel)
                        {
                            //checamos color
                            if (usuarios.Contains(Convert.ToInt32(_idUsuario)) == true)
                            {
                                c.BackColor = Color.LightBlue;
                            }
                            else
                            {
                                c.BackColor = Color.Beige;
                            }
                            j++;
                        }
                    }
                }
            */
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar mensajes: " + ex.Message, "Error DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Pasamos el ID del usuario actual al crear el grupo
            Crea_grupo crea = new Crea_grupo(_idUsuario);
            if (conexion.State == ConnectionState.Open) conexion.Close();
            crea.Show();
            this.Hide();
        }

        private void Chat_VisibleChanged(object sender, EventArgs e)
        {
            // Esta lógica es compleja. Para simplificar, si quieres que los chats se recarguen al ser visible,
            // puedes llamar a la función de carga/filtrado. 
            // Para el propósito de este ejercicio, dejaremos solo la conexión.
            string chec = textBox1.Text;
            if (this.Visible)
            {
                conexion.Open();
                //LLena el listbox con los grupos que alla en la base de datos (probado y funcional)
                using (MySqlCommand cmdGrupos = new MySqlCommand("SELECT Nombre_grupo FROM grupos ", conexion))
                using (MySqlDataReader readerGrupos = cmdGrupos.ExecuteReader())
                {
                    while (readerGrupos.Read())
                    {
                        listBox1.Items.Add(readerGrupos["Nombre_grupo"].ToString());
                        //Mostrar linea en medio pero no esta activa o puede presionarse en el programa
                        listBox1.Items.Add("--------------------------------------------------");
                    }
                }

                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    //Si esta esconde los elementos que no son
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

                    if (nom.Contains("---")) continue; // Saltar separadores

                    int idob = 0;

                    using (MySqlCommand cmdGetId = new MySqlCommand("Select id from grupos where Nombre_grupo=@nom", conexion))
                    {
                        cmdGetId.Parameters.AddWithValue("@nom", nom);
                        using (MySqlDataReader readerId = cmdGetId.ExecuteReader())
                        {
                            if (readerId.Read())
                            {
                                idob = (int)readerId["id"];
                            }
                        }
                    }

                    if (idob == 0)
                    {
                        listBox1.Items.RemoveAt(i);
                        if (i > 0 && listBox1.Items[i - 1].ToString().Contains("---"))
                            listBox1.Items.RemoveAt(i - 1);
                        continue;
                    }

                    //Ahora checa el id de miembro grupo al actual
                    using (MySqlCommand comando1 = new MySqlCommand("Select count(*) from miembros_grupos where id_grupo=@id and id_usuario=@idus", conexion))
                    {
                        comando1.Parameters.AddWithValue("@id", idob);
                        comando1.Parameters.AddWithValue("@idus", _idUsuario); // USAMOS LA VARIABLE DE INSTANCIA
                        cont = Convert.ToInt32(comando1.ExecuteScalar());
                        if (cont == 0)
                        {
                            listBox1.Items.RemoveAt(i);
                            if (i > 0 && listBox1.Items[i - 1].ToString().Contains("---"))
                                listBox1.Items.RemoveAt(i - 1);
                        }
                    }
                }
                try
                {
                    if (conexion.State != ConnectionState.Open) conexion.Open();
                    // Aquí se llamaría a una función de carga de grupos si la hubieras extraído.
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir conexión al cargar chats: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conexion.State == ConnectionState.Open) conexion.Close();
                }
            }
        }

        //Pendiente
        private void label2_Click(object sender, EventArgs e)
        {
            int id = 0;
            //Tiene que tener un grupo selecionado afuerza
            if (listBox1.SelectedItem == null || listBox1.SelectedItem.ToString().Contains("---"))
            {
                MessageBox.Show("Mensaje a grupo no valido\n");
                return;
            }
            //Pasa los mensajes a la base de datos
            // Usaremos using para asegurar el manejo de la conexión
            using (MySqlConnection conn = new MySqlConnection(MYSQL_CONNECTION_STRING))
            {
                try
                {
                    conn.Open();
                    //Obtiene el nombre del grupo y la clave del grupo
                    string grupo = listBox1.SelectedItem.ToString();

                    using (MySqlCommand cmdGetId = new MySqlCommand("SELECT id FROM grupos WHERE Nombre_grupo=@nom", conn))
                    {
                        cmdGetId.Parameters.AddWithValue("@nom", grupo);
                        object result = cmdGetId.ExecuteScalar();
                        if (result != null)
                        {
                            id = (int)result;
                        }
                    }

                    if (id == 0)
                    {
                        return;
                    }

                    //Inserta el mensaje en la base de datos
                    string textoPlano = ConvertirEmojisATextoPlano(textBox2.Text);

                    using (MySqlCommand cmdInsert = new MySqlCommand("INSERT INTO mensajes (Id_grupo, ID_usuario, contenido) VALUES(@idg, @idu, @cont)", conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@idg", id);
                        cmdInsert.Parameters.AddWithValue("@idu", Convert.ToInt32(_idUsuario)); // Clave del usuario que manda el mensaje
                        cmdInsert.Parameters.AddWithValue("@cont", textoPlano);
                        cmdInsert.ExecuteNonQuery();
                    }
                    textBox2.Clear();
                    // NOTA: Para una aplicación en tiempo real, deberías recargar los mensajes 
                    // después de la inserción, o al menos añadir el mensaje a la UI con el ID del usuario
                    // que lo envió (que es this._idUsuario)
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al enviar mensaje: " + ex.Message, "Error DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (listBox1.SelectedItem != null && !listBox1.SelectedItem.ToString().Contains("---"))
            {
                // Se llama al manejador de evento para recargar la vista del chat
                listBox1_SelectedIndexChanged(listBox1, EventArgs.Empty);
            }

            /*
            //Pone los mensjes en el panel en un cuadro de color (Lógica de UI)
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
            //Agrega  a principal
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
            */
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem==null || listBox1.SelectedItem.ToString().Contains("---"))
            {
                MessageBox.Show("No es grupo valido");
                return;
            }
            conexion.Open();
            //Muestra la opcion de agregar miembros al grupo y guarda el id del grupo
            using(comando=new MySqlCommand("SELECT id FROM grupos WHERE Nombre_grupo=@nom",conexion))
            {
                string nombre = listBox1.SelectedItem.ToString();   
                comando.Parameters.AddWithValue("@nom",nombre);
                using (leer=comando.ExecuteReader())
                {
                    if (leer.Read())
                    {
                        int idg=(int)leer["id"];
                        AgregarMiembros ag=new AgregarMiembros(idg, Convert.ToInt32(_idUsuario));
                        ag.Show();
                        this.Hide();
 
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener el id del grupo seleccionado");
                    }
                }
            }
            conexion.Close();
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Intentar cerrar la conexión si está abierta
            try
            {
                if (conexion != null && conexion.State == ConnectionState.Open)
                    conexion.Close();
            }
            catch { /* Ignorar errores de cierre */ }
            conexion.Close();
            Application.Exit();
        }
    }
}
