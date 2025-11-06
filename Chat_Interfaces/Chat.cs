using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
namespace Chat_Interfaces
{
    public partial class Chat : Form
    {
        //Variables para  server
        TcpClient cliente;
        NetworkStream flujo;
        Thread hilo;
        bool ejecutando = true;

        //private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=test;Uid=Alex;Pwd=12345";
        private const string MYSQL_CONNECTION_STRING = "Server = localhost; Port=3306;Database=chat;Uid=root;Pwd=Alex";

        // Variables de sesión ahora como campos de instancia
        private string _usuarioEmail;
        private string _idUsuario;
        private string _nombreUsuario;
        // Servidor
        TcpListener servidor;
        //variables para la mencion (@)
        private Panel panelMenciones;
        private ListBox listBoxUsuarios;
        private List<string> listaUsuarios = new List<string>();


        //Variables para manejar base de datos
        MySqlConnection conexion;
        MySqlCommand comando;
        MySqlDataReader leer;

        // Se eliminan los campos estáticos problemáticos (id, ids) y se usa _idUsuario
        int tam = 0, tamaux;
        string respaldo= "";
        bool borrando = false;

        //diccionario de emojis
        private Dictionary<string, Image> emojis = new Dictionary<string, Image>();

        // CONSTRUCTOR ACTUALIZADO para recibir los 3 parámetros
        public Chat(string email, string idUsuario, string nombreUsuario)
        {
            InitializeComponent();

            textBox2.Font = new Font("Segoe Ui Emoji", 9f);

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
            buttonEmoji.Image = Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\smile.png"));
            buttonEmoji.Image = new Bitmap(buttonEmoji.Image, new Size(14, 14));
            buttonEmoji.ImageAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(buttonEmoji);

            btnEmojiSmile.Click += btnSmile_Click;
            btnEmojiHeart.Click += btnHeart_Click;
            btnEmojiSad.Click += btnSad_Click;

            btnEmojiSmile.Tag = ":smile:";
            btnEmojiSmile.Image = Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\smile.png"));

            btnEmojiHeart.Tag = ":heart:";
            btnEmojiHeart.Image = Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\heart.png"));

            btnEmojiSad.Tag = ":sad:";
            btnEmojiSad.Image = Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\sad.png"));

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

            // Crear panel de menciones
            panelMenciones = new Panel
            {
                Visible = false,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Width = 200,
                Height = 100
            };
            this.Controls.Add(panelMenciones);
            panelMenciones.BringToFront();

            // Crear listbox dentro del panel
            listBoxUsuarios = new ListBox
            {
                Dock = DockStyle.Fill
            };
            panelMenciones.Controls.Add(listBoxUsuarios);

            // Evento click en lista
            listBoxUsuarios.Click += ListBoxUsuarios_Click;

            textBox2.KeyUp += TextBox2_KeyUp;

            cargarEmojis();
        }

        // Cargar usuarios del grupo desde la base de datos para las menciones
        private void CargarUsuariosDelGrupo(int idGrupo)
        {
            listaUsuarios.Clear();

            bool cerrarConexion = false;

            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
                cerrarConexion = true;
            }

            string sql = @"
                        SELECT u.nombre 
                        FROM usuarios u
                        JOIN miembros_grupos mg ON u.id = mg.id_usuario
                        WHERE mg.id_grupo = @idGrupo";

            using (MySqlCommand cmd = new MySqlCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@idGrupo", idGrupo);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaUsuarios.Add(reader["nombre"].ToString());
                    }
                    reader.Close();
                }
            }

            if (cerrarConexion)
                conexion.Close();
        }


        // Evento click en la lista de usuarios para menciones
        private void TextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            int atIndex = textBox2.Text.LastIndexOf('@');
            if (atIndex >= 0)
            {
                string palabra = textBox2.Text.Substring(atIndex + 1);

                var coincidencias = listaUsuarios
                    .Where(u => u.StartsWith(palabra, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                if (coincidencias.Count > 0)
                {
                    listBoxUsuarios.Items.Clear();
                    listBoxUsuarios.Items.AddRange(coincidencias.ToArray());

                    panelMenciones.Visible = true;
                    panelMenciones.Left = textBox2.Left + 5;
                    panelMenciones.Top = textBox2.Top - panelMenciones.Height - 5;
                }
                else
                {
                    panelMenciones.Visible = false;
                }
            }
            else
            {
                panelMenciones.Visible = false;
            }
        }

        //seleccionar usuario
        private void ListBoxUsuarios_Click(object sender, EventArgs e)
        {
            if (listBoxUsuarios.SelectedItem == null) return;

            string usuarioSeleccionado = listBoxUsuarios.SelectedItem.ToString();
            int atIndex = textBox2.Text.LastIndexOf("@");
            if (atIndex >= 0)
            {
                string seleccion = "@" + usuarioSeleccionado + " ";
                
                textBox2.Text = textBox2.Text.Substring(0, atIndex) + seleccion;

                respaldo = textBox2.Text;

                //colorea la mencion actual
                textBox2.Select(atIndex, seleccion.Length);
                textBox2.SelectionColor = Color.Blue;

                // Restaurar cursor al final y color negro
                textBox2.Select(textBox2.Text.Length, 0);
                textBox2.SelectionColor = Color.Black;
            }

            panelMenciones.Visible = false;
        }
        private void btnEmoji_Click(object sender, EventArgs e)
        {
            panelEmojis.Visible = !panelEmojis.Visible; // Mostrar u ocultar el panel de emojis
        }

        private void Emoji_Click(object sender, EventArgs e)
        {
            Button emojiButton = sender as Button;
            if (emojiButton!=null && emojiButton.Tag!= null)
            {
                // inserta el texto plano en vez de la imagen
                string codigoEmoji = emojiButton.Tag.ToString();

                // inserta el código en la posición del cursor
                int posicion = textBox2.SelectionStart;
                textBox2.Text = textBox2.Text.Insert(posicion, codigoEmoji + " ");
                textBox2.SelectionStart = posicion + codigoEmoji.Length + 1;
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
                    //Si esta esconde los elementos que no son
                    if (listBox1.Items[i].ToString().IndexOf(chec, StringComparison.CurrentCultureIgnoreCase) < 0)
                    {
                        listBox1.Items.RemoveAt(i);
                        i--;
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
                            //id2 = (int)readerId["id"];
                            id2 = Convert.ToInt32(readerId["id"]);
                        }
                        readerId.Close();
                    }
                }

                if (id2 != -1)
                {
                    CargarUsuariosDelGrupo(id2);
                }

                //Guarda los elementos en un panel con el contenido del mensje,hora y persona
                //string sql = "SELECT contenido, fecha, Id_usuario FROM mensajes WHERE Id_grupo=@id ORDER BY fecha ASC";
                string sql = @"SELECT m.contenido, m.fecha, m.Id_usuario, u.nombre AS nombre_usuario
                            FROM mensajes m
                            JOIN usuarios u ON m.Id_usuario = u.id
                            WHERE m.Id_grupo=@id
                            ORDER BY m.fecha ASC";

                using (MySqlCommand cmdMensajes = new MySqlCommand(sql, conexion))
                {
                    cmdMensajes.Parameters.AddWithValue("@id", id2);
                    using (MySqlDataReader readerMensajes = cmdMensajes.ExecuteReader())
                    {
                        tam = 0;
                        while (readerMensajes.Read())
                        {
                            // Obtener ID del emisor
                            //int idMensajeUsuario = readerMensajes.GetInt32("Id_usuario");
                            string nombreUsuarioMensaje;

                            if (readerMensajes["Id_usuario"].ToString() == _idUsuario)
                                nombreUsuarioMensaje = "Tú";
                            else
                                nombreUsuarioMensaje = readerMensajes["nombre_usuario"].ToString();

                            //Pone los mensjes en el panel en un cuadro de color
                            Panel pan = new Panel();
                            pan.Width = panel1.Width - 25;
                            pan.Height = 30;
                            pan.Top = tam;

                            // Asignar color segun el emisor
                            if (readerMensajes["Id_usuario"].ToString() == _idUsuario)
                                pan.BackColor = Color.LightBlue; // Mi mensaje
                            else
                                pan.BackColor = Color.Beige; // Mensaje de otro usuario

                            //label del nombre del usuario
                            Label lblNombre = new Label();
                            lblNombre.AutoSize = true;
                            lblNombre.Font = new Font("Arial", 8, FontStyle.Bold); // Fuente pequeña y en negrita
                            lblNombre.Text = nombreUsuarioMensaje + ":";
                            lblNombre.Top = 5;
                            lblNombre.Left = 5;

                            pan.Controls.Add(lblNombre);

                            //Crea un lugar para guardar el mensaje
                            RichTextBox txt = new RichTextBox();
                            txt.BorderStyle = BorderStyle.None;
                            txt.BackColor = pan.BackColor;
                            txt.ReadOnly = true;
                            txt.Font = new Font("Segoe UI Emoji", 12f);
                            txt.Width = pan.Width - 10;
                            txt.Top = lblNombre.Bottom + 2;
                            txt.Left = 5;
                            txt.ScrollBars = RichTextBoxScrollBars.None;
                            txt.Multiline = true;


                            string contenido = readerMensajes["contenido"].ToString();
                            MostrarTextoConEmojis(txt, contenido);

                            // Buscar menciones y colorearlas
                            ColorearMencionesEnHistorial(txt);

                            pan.Controls.Add(txt);

                            // Ajustar altura del panel según contenido
                            txt.Height = txt.GetPreferredSize(new Size(txt.Width, 0)).Height;
                            pan.Height = lblNombre.Height + txt.Height + 10;

                            //Agrega  a principal
                            panel1.Controls.Add(pan);

                            //label de fecha
                            Label lab = new Label();
                            lab.AutoSize = true; // Hace que el label se ajuste al texto
                            lab.Font = new Font("Arial", 6); // Tamaño más pequeño para la fecha
                            lab.ForeColor = Color.Gray; // Opcional: color distinto para la fecha
                            lab.Text = "Fecha: " + readerMensajes.GetDateTime("fecha").ToString("g"); // Formato corto
                            lab.Top = pan.Bottom; // Un pequeño margen debajo del panel
                            lab.Left = pan.Left; // Un margen lateral

                            panel1.Controls.Add(lab);

                            tam += pan.Height + lab.Height  + 5;
                        }
                    }
                    // Desplaza el panel hacia el último mensaje
                    if (panel1.Controls.Count > 0)
                    {
                        panel1.ScrollControlIntoView(panel1.Controls[panel1.Controls.Count - 1]);
                    }

                }

                tamaux = tam;
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
            Crea_grupo crea = new Crea_grupo(_idUsuario,this);
            if (conexion.State == ConnectionState.Open) conexion.Close();
            crea.Show();
            this.Enabled = false;
        }
        //Referencia para habilitar el chat desde otro formulario
        public Chat()
        {
            this.Enabled = true;
        }
        private void Chat_VisibleChanged(object sender, EventArgs e)
        { 
        }

        private void ColorearMencionesEnHistorial(RichTextBox txt)
        {
            // Guardar posición inicial del cursor
            int originalSelectionStart = txt.SelectionStart;
            int originalSelectionLength = txt.SelectionLength;

            txt.SuspendLayout();

            // Colorear menciones
            foreach (string usuario in listaUsuarios)
            {
                string pattern = "@" + Regex.Escape(usuario);
                foreach (Match match in Regex.Matches(txt.Text, pattern))
                {
                    txt.Select(match.Index, match.Length);
                    txt.SelectionColor = Color.Blue;
                }
            }

            // Verificar si el usuario movió el cursor durante la ejecución
            bool cursorNoCambiado = txt.SelectionStart == originalSelectionStart && txt.SelectionLength == originalSelectionLength;

            if (cursorNoCambiado)
            {
                txt.Select(originalSelectionStart, originalSelectionLength);
                txt.SelectionColor = Color.Black;
            }

            txt.ResumeLayout();
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
                    //Convierte los emojis a texto plano
                    //string textoPlano = ConvertirEmojisATextoPlano(textBox2.Text);
                    //textBox2.Text = textoPlano;
                    //Si no hay nada no hace nada

                    //Insertar el mensaje en la base de datos
                    //string textoPlano = textBox2.Text;

                    string textoPlano = (textBox2.Tag as StringBuilder)?.ToString() ?? textBox2.Text;

                    using (MySqlCommand cmdInsert = new MySqlCommand("INSERT INTO mensajes (Id_grupo, ID_usuario, contenido) VALUES(@idg, @idu, @cont)", conn))
                    {
                        cmdInsert.Parameters.AddWithValue("@idg", id);
                        cmdInsert.Parameters.AddWithValue("@idu", Convert.ToInt32(_idUsuario)); // Clave del usuario que manda el mensaje
                        cmdInsert.Parameters.AddWithValue("@cont", respaldo);
                        cmdInsert.ExecuteNonQuery();
                    }
                    respaldo = "";
                    textBox2.Clear();
                    textBox2.Tag = null;
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

                //Mueve el scroll al último mensaje
                if (panel1.Controls.Count > 0)
                {
                    panel1.ScrollControlIntoView(panel1.Controls[panel1.Controls.Count - 1]);
                }
            }
        }

        private string ConvertirEmojisATextoPlano(string texto)
        {
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

        //por si se ocupa
        private void cargarEmojis()
        {
            emojis[":smile:"] = Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\smile.png"));
            emojis[":heart:"] = Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\heart.png"));
            emojis[":sad:"] = Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\sad.png"));
        }

        //inserta la imagen del emoji en el richtextbox del historial del chat
        private void InsertarImagenEnRichTextBox(RichTextBox richtb, Image img, int ancho = 16, int alto = 16)
        {
            if (img == null) return;

            Bitmap bmp = new Bitmap(img, new Size(ancho, alto));

            try
            {
                Clipboard.Clear();
                Clipboard.SetImage(bmp);

                richtb.ReadOnly = false;
                richtb.Paste();
                richtb.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el chat: " + ex.Message);
            }
        }

        //inserta la imagen del emoji en el richtextbox del mensaje (textBox2)
        private void InsertEmoji(RichTextBox rtb, Image emojiImage, string emojiText)
        {
            if (emojiImage == null || rtb == null)
                return;

            int selectionStart = rtb.SelectionStart;

            //Agrega a la cadena auxiliar donde tiene las letras 
            respaldo= respaldo + emojiText;

            // vuelve a poner la imagen (visualmente)
            Clipboard.SetImage(emojiImage);
            rtb.SelectionStart = selectionStart;
            rtb.Paste();

            rtb.SelectionStart = rtb.TextLength;
            rtb.Focus();
        }

        private void btnSmile_Click(object sender, EventArgs e)
        {
            InsertEmoji(textBox2, Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\smile.png")), ":smile:");
        }

        private void btnHeart_Click(object sender, EventArgs e)
        {
            InsertEmoji(textBox2, Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\heart.png")), ":heart:");
        }

        private void btnSad_Click(object sender, EventArgs e)
        {
            InsertEmoji(textBox2, Image.FromFile(Path.Combine(Application.StartupPath, @"..\..\Resources\sad.png")), ":sad:");
        }

        private void MostrarTextoConEmojis(RichTextBox richtb, string texto)
        {
            richtb.Clear();

            var matches = Regex.Matches(texto, @"(:smile:|:heart:|:sad:)|([^\s:]+)|(\s+)");

            foreach (Match match in matches)
            {
                if (string.IsNullOrEmpty(match.Value)) continue;

                if (emojis.ContainsKey(match.Value))
                {
                    InsertarImagenEnRichTextBox(richtb, emojis[match.Value], 16, 16);
                }
                else
                {
                    richtb.AppendText(match.Value);
                }
            }
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
                        AgregarMiembros ag=new AgregarMiembros(idg, Convert.ToInt32(_idUsuario),this);
                        ag.Show();
                        this.Enabled =false;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener el id del grupo seleccionado");
                    }
                }
            }
            conexion.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (borrando) return;

            if(textBox2.Text.Length==0)
            {
                respaldo = "";
                return;
            }
            //Agrega el texto que se esta creando a la cadena respaldo sin perder el texto pasado usando respaldo
            if (respaldo != "")
                respaldo += textBox2.Text.Substring(textBox2.Text.Length - 1, 1);
            else
                respaldo = textBox2.Text;

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            //textBox2.SelectionStart = textBox2.Text.Length;
        }

        private void textBox2_KeyUp_1(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Back)
            {
                if (textBox2.SelectionStart == 0 || textBox2.TextLength == 0)
                    return;

                borrando = true;

                //Obtenemos el inicio del cursor donde esta
                int val = textBox2.SelectionStart;

                //si el respaldo esta vacio no hay nada que borrar
                if (respaldo.Length == 0)
                {
                    borrando = false;
                    return;
                }

                //Si es una imagen elimina eso y en la cadena de respaldo elimina el texto plano

                if (textBox2.Text[val - 1] == '\uFFFC')
                {

                    int cont = 0;
                    while (cont < 2 && respaldo.Length > 0)
                    {
                        if (respaldo[respaldo.Length - 1] == ':')
                            cont++;

                        respaldo = respaldo.Remove(respaldo.Length - 1, 1);
                    }

                    // eliminar el marcador visual del emoji
                    textBox2.Select(val - 1, 1);
                    textBox2.SelectedText = "";
                }
                else
                    respaldo = respaldo.Remove(respaldo.Length - 1, 1);

                textBox2.SelectionStart = Math.Max(0, textBox2.TextLength);
                borrando = false;
            }
        }

        private void buttonEmoji_Click(object sender, EventArgs e)
        {

        }

        private void Chat_Activated(object sender, EventArgs e)
        {
            // Esta lógica es compleja. Para simplificar, si quieres que los chats se recarguen al ser visible,
            // puedes llamar a la función de carga/filtrado. 
            // Para el propósito de este ejercicio, dejaremos solo la conexión.
            listBox1.Items.Clear();
            string chec = textBox1.Text;

            if (!this.Visible)
                return;

            try
            {
                if (conexion.State != ConnectionState.Open)
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
                        comando1.Parameters.AddWithValue("@idus", _idUsuario); 
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
                MessageBox.Show("Error al abrir conexión al cargar chats: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conexion.State == ConnectionState.Open) conexion.Close();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
            Application.Exit();
        }
    }
}
