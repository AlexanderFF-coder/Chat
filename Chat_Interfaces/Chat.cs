using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
//Se usa para manejar emojis
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Mysqlx.Crud.Order.Types;

namespace Chat_Interfaces
{
    public partial class Chat : Form
    {
        //Variables para server 
        TcpClient cliente;
        public NetworkStream flujo;
        bool ejecutando = true;
        private TaskCompletionSource<string> respuestapen = null;
        private bool conectado = false;
        Direcionip direcionip;
        //Variables de sesión
        private string _usuarioEmail;
        private string _idUsuario;
        private string _nombreUsuario;
        private System.Windows.Forms.Timer timerRefresco;
        //Menciones
        private Panel panelMenciones;
        private ListBox listBoxUsuarios;
        private List<string> listaUsuarios = new List<string>();

        //Variables de opcion extra

        private System.Windows.Forms.Timer timerBusqueda;
        private SemaphoreSlim _semaforo = new SemaphoreSlim(1, 1);

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
            //Se usa la variable timer para definir la parte de las consultas del buscador para hacer una consulta
            timerBusqueda = new System.Windows.Forms.Timer();
            timerBusqueda.Interval = 500; 
            timerBusqueda.Tick += busqueda; 
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

            textBox2.Font = new Font("Segoe UI Emoji", 9f);

            //Mostrar nombre de sesión en el título
            this.Text = "Chat-Sesión:" + _nombreUsuario;

            listBox1.Items.Clear();

            cliente = new TcpClient();

            timerRefresco = new System.Windows.Forms.Timer();
            timerRefresco.Interval = 500; // 2000 ms = 2 segundos (ajusta si quieres más rápido o lento)
            timerRefresco.Tick += timerRefresco_Tick; // Vinculamos el evento
            timerRefresco.Start(); // Arrancamos el timer
        }


        //Espera una respuesta del server
        private async Task<string> respuesta(string mensaje)
        {
            if (!conectado || flujo == null || cliente == null || !cliente.Connected)
                return null;

            //esperamos turno
            await _semaforo.WaitAsync();
            try
            {
                respuestapen = new TaskCompletionSource<string>();

                await Enviar(mensaje);

                //Esperamos respuesta
                var tareares = respuestapen.Task;
                var tareatiempo = Task.Delay(5000);

                var completada = await Task.WhenAny(tareares, tareatiempo);

                if (completada == tareatiempo)
                {
                    Console.WriteLine("Timeout esperando: " + mensaje);
                    return null;
                }

                return await tareares;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                //libera el semaforo
                respuestapen = null;
                _semaforo.Release();
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
            listBox1.Items.Clear();
            try
            {
                Direcionip direcionip = new Direcionip();
                await cliente.ConnectAsync(direcionip.direcion, 8080);
                conectado = true;
                flujo = cliente.GetStream();
                _ = escuchaservidor();
                await CargarGrupos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
        }


        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Ajustamos la busqueda en la barra de busqueda para no sobrecargar con varias letras a la vez si no que  analiza por periodo de tiempo
            timerBusqueda.Stop();
            timerBusqueda.Start();
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }

            string nombreg = listBox1.SelectedItem.ToString();
            panel1.Controls.Clear();

            string mensaje = "4|" + nombreg;

            try
            {
                if (flujo != null && cliente != null && cliente.Connected)
                {
                    await Enviar(mensaje);
                }
                else
                {
                    await respuesta(mensaje);
                }
                //La notacion indica como un await en un metodo async
                _ = mostrartodosmensajes(nombreg);
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

        private async void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null || listBox1.SelectedItem.ToString().Contains("---"))
            {
                MessageBox.Show("No es grupo valido");
                return;
            }

            string nombreGrupo = listBox1.SelectedItem.ToString();
            string mensaje = "Obtenerclave|" + nombreGrupo;

            string res = await respuesta(mensaje);
            if (string.IsNullOrEmpty(res))
            {
                MessageBox.Show("error al obtener clave del grupo");
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
            if (!int.TryParse(partes[1], out idg))
            {
                MessageBox.Show("clave de grupo incorrecta");
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
                {
                    return;
                }

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
                        {
                            cont++;
                        }
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
        //Carg grupos
        private async void Chat_Activated(object sender, EventArgs e)
        {
            if (this.Visible && conectado)
            {
                await CargarGrupos();
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
            foreach (var kv in emojis)
            {
                kv.Value.Dispose();
            }
            emojis.Clear();

        }

        private async Task escuchaservidor()
        {
            byte[] buffer = new byte[4096];
            int bytesLeidos;
            StringBuilder juntar = new StringBuilder();

            try
            {
                while (ejecutando)
                {
                    bytesLeidos = await flujo.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesLeidos == 0) break;

                    string recibido = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                    juntar.Append(recibido);

                    string contenido = juntar.ToString();
                    int index;

                    //checa que termine con \n
                    while ((index = contenido.IndexOf('\n')) >= 0)
                    {
                        string mensaje = contenido.Substring(0, index).Trim();
                        contenido = contenido.Substring(index + 1);

                        if (!string.IsNullOrEmpty(mensaje))
                            await procesarmensaje(mensaje);
                    }

                    juntar.Clear();
                    juntar.Append(contenido);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en escucha: " + ex.Message);
            }
        }
        //Funcion de checa mensje posible
        private async Task procesarmensaje(string mensaje)
        {
            mensaje = mensaje.Trim();
            if (string.IsNullOrEmpty(mensaje)) return;

            string[] partes = mensaje.Split('|');
            string tok = partes[0];
            //Cambiamos la logica para que sea los mensajes que se usan para recibir o mandar al servidor dependiendo de las opciones que mandemos de acuerdo a la notacion 

            switch (tok)
            {
                case "Mostrargrupo":
                case "buscar_grupo":
                case "Obtenerclave":
                case "cargar_mensajes":
                case "mensajes_grupo":
                case "agregar_grupos1":
                    respuestapen?.SetResult(mensaje);
                    respuestapen = null;
                    return;
            }

            switch (tok)
            {
                case "mensaje_nuevo":
                    if (partes.Length >= 4)
                    {
                        string usuario = partes[2];
                        string contenido = partes[3];
                        string fecha = "";

                        if (partes.Length >= 5)
                        {
                            fecha = partes[4];
                        }
                        else
                        {
                            fecha = DateTime.Now.ToString("g");
                        }
                        //Espera un momento para mostrar el mensaje
                        await Task.Delay(100);
                        await this.checasync(async () =>
                        {
                            // muestra el mensaje en el panel
                            _ = mostrarmensajeunico(usuario, contenido, fecha);
                            await Task.CompletedTask;
                        });

                    }
                    break;
                case "5":
                    if (partes.Length > 1 && partes[1] == "OK")
                    {
                        Console.WriteLine("Mensaje guardado en el servidor.");
                    }
                    break;

                case "1":
                    if (partes.Length >= 2)
                    {
                        string nombreGrupo = partes[1];
                        await this.checasync(async () =>
                        {
                            listBox1.Items.Add(nombreGrupo);
                            listBox1.Items.Add("--------------------------------------");
                            await Task.CompletedTask;
                        });
                    }
                    break;
                case "agregar_miembros":
                    string grupos = "Mostrargrupo|";
                    //string res =await respuesta(grupos + _idUsuario);
                    if (partes.Length >= 2)
                    {
                        string nombreGrupo = partes[1];
                        await this.checasync(async () =>
                        {
                            listBox1.Items.Add(nombreGrupo);
                            listBox1.Items.Add("--------------------------------------");
                            await Task.CompletedTask;
                        });
                    }
                    break;
                ///////////////////////////////////////////////////////////////
                case "agregar_grupos1":
                    if (partes.Length >= 2)
                    {
                        string[] grupo = partes[1].Split(';');

                        await this.checasync(async () =>
                        {
                            foreach (string car in grupo)
                            {
                                if (!string.IsNullOrWhiteSpace(car) && !listBox1.Items.Contains(car))
                                {
                                    listBox1.Items.Add(car);
                                    listBox1.Items.Add("--------------------------------------");
                                }
                            }
                            await Task.CompletedTask;
                        });
                    }
                    break;

                default:
                    Console.WriteLine("Mensaje no reconocido: " + mensaje);
                    break;
            }
        }
        //Checa si es async
        //Se cambia la accion por functask parqa que pueda ser async
        private Task checasync(Func<Task> action)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return Task.CompletedTask;
            }

            var tarea = new TaskCompletionSource<object>();
            this.BeginInvoke(new MethodInvoker(async () =>
            {
                try
                {
                    await action();
                    tarea.SetResult(null);
                }
                catch (Exception ex)
                {
                    tarea.SetException(ex);
                }
            }));
            //Devuelve la tarea
            return tarea.Task;
        }


        private async Task mostrarmensajep(List<(string usuario, string contenido, string fecha)> mensajes)
        {
            await checasync(async () =>
            {
                // 1. Contamos cuántos mensajes ya están dibujados en el panel
                // (Dividimos entre 2 porque cada mensaje usa 1 Panel + 1 Label de fecha)
                int mensajesActuales = panel1.Controls.Count / 2;

                // 2. Si la lista que llegó del server no tiene nada nuevo, no hacemos nada
                if (mensajes.Count <= mensajesActuales)
                {
                    await Task.CompletedTask;
                    return;
                }

                int alturaAcumulada = 0;

                // 3. Calculamos la altura donde nos quedamos (debajo del último mensaje)
                var ultimoControl = panel1.Controls.OfType<Control>().OrderByDescending(c => c.Bottom).FirstOrDefault();
                if (ultimoControl != null)
                {
                    alturaAcumulada = ultimoControl.Bottom + 5;
                }
                else
                {
                    alturaAcumulada = 0;
                }

                // 4. AQUÍ ESTÁ LA MAGIA: Empezamos el ciclo DESDE donde nos quedamos
                // Usamos un 'for' en lugar de 'foreach' para saltarnos los viejos
                for (int i = mensajesActuales; i < mensajes.Count; i++)
                {
                    var m = mensajes[i]; // Obtenemos solo el mensaje nuevo

                    // --- DE AQUÍ PARA ABAJO ES TU MISMO CÓDIGO DE DISEÑO ---
                    Color fondo;
                    bool esTuyo = m.usuario.Equals(_nombreUsuario, StringComparison.OrdinalIgnoreCase);
                    string nombreus = "";
                    if (esTuyo)
                    {
                        nombreus = "Tú";
                        fondo = Color.LightBlue;
                    }
                    else
                    {
                        nombreus = m.usuario;
                        fondo = Color.Beige;
                    }

                    Panel pan = new Panel();
                    pan.Width = panel1.Width - 25;
                    pan.BackColor = fondo;
                    pan.Top = alturaAcumulada;

                    Label lblNombre = new Label();
                    lblNombre.AutoSize = true;
                    lblNombre.Font = new Font("Arial", 8, FontStyle.Bold);
                    lblNombre.Text = nombreus + ":";
                    lblNombre.Top = 5;
                    lblNombre.Left = 5;
                    pan.Controls.Add(lblNombre);

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

                    Label lab = new Label();
                    lab.AutoSize = true;
                    lab.Font = new Font("Arial", 6);
                    lab.ForeColor = Color.Gray;
                    lab.Text = "Fecha: " + m.fecha;
                    lab.Top = pan.Bottom;
                    lab.Left = pan.Left;
                    panel1.Controls.Add(lab);

                    alturaAcumulada += pan.Height + lab.Height + 5;
                }

                // 5. Solo hacemos scroll si se agregaron mensajes nuevos
                if (panel1.Controls.Count > 0)
                {
                    panel1.ScrollControlIntoView(panel1.Controls[panel1.Controls.Count - 1]);
                }
                await Task.CompletedTask;
            });
        }

        //Envia el mensaje al servidor y carga los mensajes en el panel
        private async void label2_Click(object sender, EventArgs e)
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

            //Obtener clave del grupo desde el servidor
            string mensajeIdGrupo = "Obtenerclave|" + nombreGrupo+"\n";
            //Esperamos respuesta
            string res = await respuesta(mensajeIdGrupo);
            if (string.IsNullOrEmpty(res))
            {
                MessageBox.Show("error al obtener clave");
                return;
            }

            string[] partes = res.Split('|');
            //Mismo caso de antes checamos los argumentos y si se puede convertir a int
            if (partes.Length < 2 || !int.TryParse(partes[1], out int idg))
            {
                MessageBox.Show("clave inválido.");
                return;
            }
            int idGrupo = idg;
            //mensaje
            string mensaje = "guardar_mensaje|" + _idUsuario + "|" + idg + "|" + contenido+"\n";

            try
            {
                if (cliente != null && cliente.Connected && flujo != null)
                {
                    await Enviar(mensaje);
                }
                else
                {
                    MessageBox.Show("No hay conexión con el servidor.");
                    return;
                }

                respaldo = "";
                textBox2.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("error al enviar mensaje: " + ex.Message);
            }
        }

        private void buttonEmoji_Click(object sender, EventArgs e)
        {
            panelEmojis.Visible = panelEmojis.Visible;
        }

        //Muestra todos los mensajes del grupo selecionado
        private async Task mostrartodosmensajes(string nombreg)
        {
            if (listBox1.SelectedItem == null || listBox1.SelectedItem.ToString().Contains("---"))
            {
                MessageBox.Show("No es grupo valido");
                return;
            }
            string res = "cargar_mensajes|" + nombreg;
            string mensajesrecibidos = await respuesta(res);
            
            if (mensajesrecibidos.StartsWith("mensajes_grupo|"))
                mensajesrecibidos = mensajesrecibidos.Substring("mensajes_grupo|".Length);

            if (string.IsNullOrEmpty(mensajesrecibidos))
            {
                MessageBox.Show("No se pudieron cargar los mensajes del grupo.");
                return;
            }
            else
            {
                //Iniciamos lista con los mensajes del grupo
                List<(string usuario, string contenido, string fecha)> mensajes = new List<(string, string, string)>();
                string[] mensajesgrupo = mensajesrecibidos.Split('°');
                foreach (string mensaje in mensajesgrupo)
                {
                    if (!string.IsNullOrWhiteSpace(mensaje))
                    {
                        string[] partes = mensaje.Split('|');
                        if (partes.Length >= 3)
                        {
                            string usuario = partes[0];
                            string contenido = partes[1];

                            mensajes.Add((usuario, contenido, partes[2]));
                        }
                    }
                }
                //panel1.Controls.Clear();
                _ = mostrarmensajep(mensajes);
            }
        }

        //Envio un solo mensaje al panel
        private async Task mostrarmensajeunico(string usuario, string contenido, string fecha)
        {
            //Obtenemos los mensajes del chat y agregamos el nuevo
            await mostrarmensajep(new List<(string usuario, string contenido, string fecha)> { (usuario, contenido, fecha) });
            //Desplazamos el scroll al final
            if (panel1.Controls.Count > 0)
            {
                panel1.ScrollControlIntoView(panel1.Controls[panel1.Controls.Count - 1]);
            }
        }
        //Envia mensaje
        private async Task Enviar(string mensaje)
        {
            byte[] datos = Encoding.UTF8.GetBytes(mensaje + "\n");
            await flujo.WriteAsync(datos, 0, datos.Length);
        }

        //Carg grupos
        private async Task CargarGrupos()
        {
            if (!conectado) return; 

            listBox1.Items.Clear(); 
            string grupos1 = "Mostrargrupo|";

            //Pedimos al servidor
            string res = await respuesta(grupos1 + _idUsuario);

            if (string.IsNullOrEmpty(res)) return;

            string[] grupos = res.Split(';');
            string[] sep = grupos[0].Split('|');
            grupos[0]=sep[1];
            foreach (string grupo in grupos)
            {
                if (!string.IsNullOrWhiteSpace(grupo))
                {
                    listBox1.Items.Add(grupo);
                    listBox1.Items.Add("--------------------------------------");
                }
            }
        }
        //Funcion para la barra de busqueda
        private async void busqueda(object sender, EventArgs e)
        {

            timerBusqueda.Stop();


            if (!conectado || cliente == null || !cliente.Connected) return;

            string textoBusqueda = textBox1.Text;

            try
            {
                if (string.IsNullOrEmpty(textoBusqueda))
                {
                    await CargarGrupos();
                }
                else
                {
                    string mensaje = "buscar_grupo|" + textoBusqueda + "|" + _idUsuario;

                    string res = await respuesta(mensaje);
                    if (string.IsNullOrEmpty(res)) return;

                    string[] partes = res.Split('|');
                    if (partes.Length < 2) return;

                    string gruposCadena = partes[1];
                    string[] grupos = gruposCadena.Split(';');

                    listBox1.Items.Clear();
                    foreach (string g in grupos)
                    {
                        if (!string.IsNullOrWhiteSpace(g))
                        {
                            listBox1.Items.Add(g);
                            listBox1.Items.Add("--------------------------------------");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en el buscador: " + ex.Message);
            }
        }
        private void timerRefresco_Tick(object sender, EventArgs e)
        {
            // Solo actualizamos si hay conexión y si el usuario seleccionó un grupo válido
            if (conectado && cliente != null && cliente.Connected &&
                listBox1.SelectedItem != null &&
                !listBox1.SelectedItem.ToString().Contains("---"))
            {
                // Obtenemos el nombre del grupo actual
                string nombreGrupo = listBox1.SelectedItem.ToString();

                // Llamamos a tu función existente que descarga y pinta los mensajes
                // Usamos _ = para descartar la tarea async y que no de warning
                _ = mostrartodosmensajes(nombreGrupo);
            }
        }
    }

}

