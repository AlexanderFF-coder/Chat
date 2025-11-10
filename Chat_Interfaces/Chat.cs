using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
//Se usa para manejar emojis
using System.Text.RegularExpressions;
namespace Chat_Interfaces
{
    public partial class Chat : Form
    {
        //Variables para server 
        TcpClient cliente;
        public NetworkStream flujo;
        Thread hilo;
        bool ejecutando = true;

        //Variables de sesión
        private string _usuarioEmail;
        private string _idUsuario;
        private string _nombreUsuario;

        //Menciones
        private Panel panelMenciones;
        private ListBox listBoxUsuarios;
        private List<string> listaUsuarios = new List<string>();

      
        int tam = 0, tamaux;
        string respaldo = "";
        bool borrando = false;

        //Diccionario de emojis 
        private Dictionary<string, Image> emojis = new Dictionary<string, Image>();

        //Constructor 
        public Chat(string email, string idUsuario, string nombreUsuario)
        {
            InitializeComponent();

            _usuarioEmail = email;
            _idUsuario = idUsuario;
            _nombreUsuario = nombreUsuario;

            cargarEmojis();

            //Configurar botón emoji 
            buttonEmoji.Click += btnEmoji_Click;
            if (emojis.TryGetValue(":smile:", out Image btnImg))
            {
                var small = new Bitmap(btnImg, new Size(14, 14));
                buttonEmoji.Image = small;
                buttonEmoji.ImageAlign = ContentAlignment.MiddleCenter;
            }
            btnEmojiSmile.Click += btnSmile_Click;
            btnEmojiHeart.Click += btnHeart_Click;
            btnEmojiSad.Click += btnSad_Click;

            btnEmojiSmile.Tag = ":smile:";
            if (emojis.ContainsKey(":smile:")) btnEmojiSmile.Image = new Bitmap(emojis[":smile:"], new Size(20, 20));
            btnEmojiHeart.Tag = ":heart:";
            if (emojis.ContainsKey(":heart:")) btnEmojiHeart.Image = new Bitmap(emojis[":heart:"], new Size(20, 20));
            btnEmojiSad.Tag = ":sad:";
            if (emojis.ContainsKey(":sad:")) btnEmojiSad.Image = new Bitmap(emojis[":sad:"], new Size(20, 20));

            this.Controls.Add(panelEmojis);
            panelEmojis.BringToFront();

            this.MouseDown += Chat_MouseDown;
            listBox1.MouseDown += Chat_MouseDown;
            panel1.MouseDown += Chat_MouseDown;
            textBox1.MouseDown += Chat_MouseDown;
            textBox2.MouseDown += Chat_MouseDown;
            label1.MouseDown += Chat_MouseDown;
            label2.MouseDown += Chat_MouseDown;

            //Crear panel de menciones
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

            //Crear listbox dentro del panel de menciones
            listBoxUsuarios = new ListBox
            {
                Dock = DockStyle.Fill
            };
            panelMenciones.Controls.Add(listBoxUsuarios);

            //Iniciar hilo escucha si se proporcionó flujo
            if (flujo != null)
            {
                escuchahilo();
            }

            textBox2.Font = new Font("Segoe UI Emoji", 9f);

            //Mostrar nombre de sesión en el título
            this.Text = "Chat-Sesión:"+ _nombreUsuario;

            listBox1.Items.Clear();
        }


        //Espera una respuesta del server
        private string respuesta(string mensaje, string host = "192.168.1.83", int port = 8080)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    client.Connect(host, port);
                    using (var s = client.GetStream())
                    {
                        byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                        s.Write(datos, 0, datos.Length);

                        byte[] buffer = new byte[4096];
                        int bytesLeidos = s.Read(buffer, 0, buffer.Length);
                        return Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
                return string.Empty;
            }
        }


        private void escuchahilo()
        {
            if (hilo == null || !hilo.IsAlive)
            {
                ejecutando = true;
                hilo = new Thread(escuchaservidor);
                if (hilo.IsBackground == false)
                {
                    hilo.IsBackground = true;
                }
                hilo.Start();
            }
        }


        private void cargarEmojis()
        {
            try
            {
                string basePath = Application.StartupPath;
                string pSm = Path.Combine(basePath, @"..\..\Resources\smile.png");
                string pHe = Path.Combine(basePath, @"..\..\Resources\heart.png");
                string pSa = Path.Combine(basePath, @"..\..\Resources\sad.png");

                if (File.Exists(pSm)) emojis[":smile:"] = Image.FromFile(pSm);
                if (File.Exists(pHe)) emojis[":heart:"] = Image.FromFile(pHe);
                if (File.Exists(pSa)) emojis[":sad:"] = Image.FromFile(pSa);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error cargarEmojis: " + ex.Message);
            }
        }

        private void Chat_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = this.PointToClient(Control.MousePosition);

            if (panelEmojis.Visible)
            {
                if (!panelEmojis.Bounds.Contains(pt) && !buttonEmoji.Bounds.Contains(pt))
                {
                    panelEmojis.Visible = false;
                }
            }
        }

        private void btnEmoji_Click(object sender, EventArgs e)
        {
            panelEmojis.Visible = !panelEmojis.Visible;
        }

        private async void Chat_Load(object sender, EventArgs e)
        {
            try
            {
                cliente = new TcpClient();
                await cliente.ConnectAsync("192.168.1.83", 8080);
                flujo = cliente.GetStream();
                hilo = new Thread(escuchaservidor);
                hilo.IsBackground = true;
                hilo.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string chec = textBox1.Text;
            listBox1.Items.Clear();
            panel1.Controls.Clear();
            //Si no hay texto, cargar todos los grupos
            if (string.IsNullOrEmpty(chec))
            {
                string grupos1 = "Mostrargrupo|";
                string res1 = respuesta(grupos1 + _idUsuario);
                if (string.IsNullOrEmpty(res1)) return;
                string[] grupos2 = res1.Split(';');
                foreach (string grupo in grupos2)
                {
                    if (!string.IsNullOrWhiteSpace(grupo))
                    {
                        listBox1.Items.Add(grupo);
                        listBox1.Items.Add("--------------------------------------");
                    }
                }
                return;
            }
            string mensaje = "buscar_grupo|" + chec + "|" + _idUsuario;
            string res = respuesta(mensaje);
            if (string.IsNullOrEmpty(res))
            {
                return;
            }
            string[] grupos = res.Split('|');
            foreach (string grupo in grupos)
            {
                if (!string.IsNullOrWhiteSpace(grupo)&&!grupo.Contains("0"))
                {
                    listBox1.Items.Add(grupo);
                    listBox1.Items.Add("--------------------------------------");
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return; 
            }

            string nombreg = listBox1.SelectedItem.ToString();
            panel1.Controls.Clear();

            string mensaje = "4|"+nombreg;

            try
            {
                if (flujo != null && cliente != null && cliente.Connected)
                {
                    byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                    flujo.Write(datos, 0, datos.Length);
                }
                else
                {
                    respuesta(mensaje);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : " + ex.Message);
            }
        }
        //Crear grupo
        private void label1_Click(object sender, EventArgs e)
        {
            Crea_grupo crea = new Crea_grupo(_idUsuario, this);
            crea.Show();
            this.Enabled = false;
        }

        private void Chat_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void InsertarImagenEnRichTextBox(RichTextBox richtb, Image img, int ancho = 16, int alto = 16)
        {
            if (img == null || richtb == null)
            {
                return;
            }
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
                MessageBox.Show("error al poner imagen: " + ex.Message);
            }
            finally
            {
                bmp.Dispose();
            }
        }

        private void InsertEmoji(RichTextBox rtb, Image emojiImage, string emojiText)
        {
            if (emojiImage == null || rtb == null) return;

            try
            {
                int selectionStart = rtb.SelectionStart;

                respaldo += emojiText;

                Clipboard.SetImage(new Bitmap(emojiImage));
                rtb.SelectionStart = selectionStart;
                rtb.Paste();

                rtb.SelectionStart = rtb.TextLength;
                rtb.Focus();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error InsertEmoji: " + ex.Message);
            }
        }

        private void btnSmile_Click(object sender, EventArgs e)
        {
            if (emojis.TryGetValue(":smile:", out Image img))
            {
                InsertEmoji(textBox2, img, ":smile:");
            }
        }

        private void btnHeart_Click(object sender, EventArgs e)
        {
            if (emojis.TryGetValue(":heart:", out Image img))
            {
                InsertEmoji(textBox2, img, ":heart:");
            }
        }

        private void btnSad_Click(object sender, EventArgs e)
        {
            if (emojis.TryGetValue(":sad:", out Image img))
            {
                InsertEmoji(textBox2, img, ":sad:");
            }
        }

        private void MostrarTextoConEmojis(RichTextBox richtb, string texto)
        {
            richtb.Clear();
            //Encontramos los emojis en el texto y se remplaza por imagenes
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
            if (listBox1.SelectedItem == null || listBox1.SelectedItem.ToString().Contains("---"))
            {
                MessageBox.Show("No es grupo valido");
                return;
            }

            string nombreGrupo = listBox1.SelectedItem.ToString();
            string mensaje = "ObtenerIDGrupo|" + nombreGrupo;

            string res = respuesta(mensaje);
            if (string.IsNullOrEmpty(res))
            {
                MessageBox.Show("error al obtener ID del grupo");
                return;
            }

            string[] partes = res.Split('|');
            if (partes.Length < 2)
            {
                MessageBox.Show("Respuesta inválida ");
                return;
            }

            int idg = 0;
            //Se usa out para indicar que es una parametro que se pasara
            if (!int.TryParse(partes[1],out idg))
            {
                MessageBox.Show("id de grupo inválido.");
                return;
            }
            //Abrir formulario para agregar miembros
            AgregarMiembros ag = new AgregarMiembros(idg, Convert.ToInt32(_idUsuario), this);
            ag.Show();
            this.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (borrando)
            {
                return;
            }
            if (textBox2.Text.Length == 0)
            {
                respaldo = "";
                return;
            }
            try
            {
                if (respaldo != "")
                {
                    respaldo += textBox2.Text.Substring(textBox2.Text.Length - 1, 1);
                }
                else
                {
                    respaldo = textBox2.Text;
                }
            }
            catch
            {
                respaldo = textBox2.Text;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBox2_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (textBox2.SelectionStart == 0 || textBox2.TextLength == 0)
                    return;

                borrando = true;

                int val = textBox2.SelectionStart;

                if (respaldo.Length == 0)
                {
                    borrando = false;
                    return;
                }

                if (textBox2.Text[val - 1] == '\uFFFC')
                {
                    int cont = 0;
                    while (cont < 2 && respaldo.Length > 0)
                    {
                        if (respaldo[respaldo.Length - 1] == ':')
                            cont++;

                        respaldo = respaldo.Remove(respaldo.Length - 1, 1);
                    }

                    //eliminar marcador visual del emoji
                    textBox2.Select(val - 1, 1);
                    textBox2.SelectedText = "";
                }
                else
                {
                    //eliminar último carácter
                    respaldo = respaldo.Remove(respaldo.Length - 1, 1);
                }

                textBox2.SelectionStart = Math.Max(0, textBox2.TextLength);
                borrando = false;
            }
        }

        private void Chat_Activated(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (!this.Visible)
            {
                return;
            }
            string grupos = "Mostrargrupo|";
            string res = respuesta(grupos + _idUsuario);
            if (string.IsNullOrEmpty(res)) return;

            string[] grupos1 = res.Split(';');
            foreach (string grupo in grupos1)
            {
                if (!string.IsNullOrWhiteSpace(grupo))
                {
                    listBox1.Items.Add(grupo);
                    listBox1.Items.Add("--------------------------------------");
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {
            ejecutando = false;
            flujo.Close(); 
            if (cliente != null && cliente.Connected)
            {
                cliente.Close();
            }
            if (hilo != null && hilo.IsAlive)
            {
               hilo.Join(500);
            }
            foreach (var kv in emojis)
            {
                kv.Value.Dispose();
            }
            emojis.Clear();

        }

        private void escuchaservidor()
        {
            byte[] buffer = new byte[4096];
            int bytesLeidos;

            try
            {
                while (ejecutando && (bytesLeidos = flujo.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                    string[] partes = mensaje.Split('|');

                    //Nuevo mensaje
                    if (partes[0] == "nuevo_mensaje")
                    {
                        string idGrupo = partes[1];
                        string usuario = partes[2];
                        string contenido = partes[3];

                        this.Invoke((Action)(() =>
                        {
                            mostrarmensajep(new List<(string, string, DateTime)>{ (usuario, contenido, DateTime.Now)});
                        }));
                    }

                    //Confirmación de guardado de mensaje
                    else if (partes[0] == "5" && partes.Length > 1 && partes[1] == "OK")
                    {
                        Console.WriteLine("Mensaje guardado correctamente en el servidor.");
                    }

                    //Lista de grupos
                    else if (partes[0] == "1")
                    {
                        string nombreGrupo = partes[1];
                        listBox1.Invoke((Action)(() =>
                        {
                            listBox1.Items.Add(nombreGrupo);
                            listBox1.Items.Add("--------------------------------------");
                        }));
                    }
                    else
                    {
                        Console.WriteLine("Mensaje no reconocido del servidor: " + mensaje);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Conexión perdida: " + ex.Message);
                if (ejecutando)
                {
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show("Se perdió la conexión con el servidor.");
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error en hilo de escucha: " + ex.Message);
            }
        }
        //Se manda un mensaje par mostrar todos los mensajes en el panel
        private void mostrarmensajep(List<(string usuario, string contenido, DateTime fecha)> mensajes)
        {
            panel1.Invoke((Action)(() =>
            {
                panel1.Controls.Clear();
                int alturaAcumulada = 0;

                foreach (var m in mensajes)
                {
                    bool esTuyo = m.usuario.Equals(_nombreUsuario, StringComparison.OrdinalIgnoreCase);
                    string nombreus = "";
                    if (esTuyo)
                    {
                        nombreus = "Tú";
                    }
                    else
                    {
                        nombreus = m.usuario;
                    }
                    Color fondo = esTuyo ? Color.LightBlue : Color.Beige;

                    //Panel principal del mensaje
                    Panel pan = new Panel();
                    pan.Width = panel1.Width - 25;
                    pan.BackColor = fondo;
                    pan.Top = alturaAcumulada;

                    //Label del nombre
                    Label lblNombre = new Label();
                    lblNombre.AutoSize = true;
                    lblNombre.Font = new Font("Arial", 8, FontStyle.Bold);
                    lblNombre.Text = nombreus + ":";
                    lblNombre.Top = 5;
                    lblNombre.Left = 5;
                    pan.Controls.Add(lblNombre);

                    //Texto del mensaje 
                    RichTextBox txt = new RichTextBox();
                    txt.BorderStyle = BorderStyle.None;
                    txt.BackColor = fondo;
                    txt.ReadOnly = true;
                    txt.Font = new Font("Segoe UI Emoji", 12f);
                    txt.Width = pan.Width - 10;
                    txt.Top = lblNombre.Bottom + 2;
                    txt.Left = 5;
                    txt.ScrollBars = RichTextBoxScrollBars.None;
                    txt.Multiline = true;

                    MostrarTextoConEmojis(txt, m.contenido);

                    pan.Controls.Add(txt);

                    txt.Height = txt.GetPreferredSize(new Size(txt.Width, 0)).Height;
                    pan.Height = lblNombre.Height + txt.Height + 10;

                    panel1.Controls.Add(pan);

                    //Fecha 
                    Label lab = new Label();
                    lab.AutoSize = true;
                    lab.Font = new Font("Arial", 6);
                    lab.ForeColor = Color.Gray;
                    lab.Text = "Fecha: " + m.fecha.ToString("g");
                    lab.Top = pan.Bottom;
                    lab.Left = pan.Left;
                    panel1.Controls.Add(lab);

                    alturaAcumulada += pan.Height + lab.Height + 5;
                }

                if (panel1.Controls.Count > 0)
                {
                    panel1.ScrollControlIntoView(panel1.Controls[panel1.Controls.Count - 1]);
                }    
            }));
        }

        //Envia el mensaje al servidor y carga los mensajes en el panel
        private void label2_Click(object sender, EventArgs e)
        {
            string contenido = respaldo.Trim();
            if (string.IsNullOrEmpty(contenido))
            {
                MessageBox.Show("No puedes enviar un mensaje vacío.");
                return;
            }

            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un grupo antes de enviar un mensaje.");
                return;
            }

            string nombreGrupo = listBox1.SelectedItem.ToString();

            //Obtener id del grupo desde el servidor
            string mensajeIdGrupo = "ObtenerIDGrupo|" + nombreGrupo;
            string res = respuesta(mensajeIdGrupo);
            if (string.IsNullOrEmpty(res))
            {
                MessageBox.Show("error al obtener id");
                return;
            }

            string[] partes = res.Split('|');
            //Mismo caso de antes checamos los argumentos y si se puede convertir a int
            if (partes.Length < 2 || !int.TryParse(partes[1], out int idg))
            {
                MessageBox.Show("id de grupo inválido.");
                return;
            }
            int idGrupo = idg;
            //mensaje
            string mensaje = "guardar_mensaje|"+_idUsuario+"|"+idg+"|"+contenido;

            try
            {
                if (cliente != null && cliente.Connected && flujo != null)
                {
                    byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                    flujo.Write(datos, 0, datos.Length);
                }
                else
                {
                    MessageBox.Show("No hay conexión activa con el servidor.");
                    return;
                }
                //Mostrar mensaje en el panel
                mostrarmensajep(new List<(string, string, DateTime)> { (_nombreUsuario, contenido, DateTime.Now) });
                //Limpiar campo de texto
                respaldo = "";
                textBox2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error al enviar mensaje: " + ex.Message);
            }
        }
        //Checa emojis
        private void buttonEmoji_Click(object sender, EventArgs e)
        {
            panelEmojis.Visible = !panelEmojis.Visible;
        }
    }
}
