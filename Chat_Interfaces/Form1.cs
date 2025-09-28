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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            CenterControlsInPanel();
            panelLogin.Resize += (s, e) => CenterControlsInPanel();
            textBoxEmail.KeyDown += TextBoxEmail_KeyDown;
            textBoxPassword.UseSystemPasswordChar = true;
        }

        //este metodo centra los controles dentro del panel
        private void CenterControlsInPanel()
        {
            foreach (Control control in panelLogin.Controls)
            {
                control.Left = (panelLogin.ClientSize.Width - control.Width) / 2;
            }
        }

        //al dar enter en el textBoxEmail, el foco se mueve al textBoxPassword
        private void TextBoxEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                textBoxPassword.Focus();
            }
        }

        private void lblRegistro_Click(object sender, EventArgs e)
        {
            //redirigir al formulario de registro
        }
    }
}