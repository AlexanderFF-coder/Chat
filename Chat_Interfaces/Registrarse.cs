using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Interfaces
{
    public partial class Registrarse : Form
    {
        public Registrarse()
        {
            InitializeComponent();

            CenterControlsInPanel();
            panelRegister.Resize += (s, e) => CenterControlsInPanel();
            textBoxPassw.UseSystemPasswordChar = true;
            textBoxConfirmPassw.UseSystemPasswordChar = true;
        }

        private void CenterControlsInPanel()
        {
            foreach (Control control in panelRegister.Controls)
            {
                control.Left = (panelRegister.ClientSize.Width - control.Width) / 2;
            }
        }

        private void textBoxEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                textBoxPassw.Focus();
            }
        }

        private void textBoxPassw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                textBoxConfirmPassw.Focus();
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string pass = textBoxPassw.Text;
            string confirmPass = textBoxConfirmPassw.Text;
            string email = textBoxEmail.Text;

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("El campo de email no puede estar vacío.",
                        "Error de Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                textBoxEmail.Focus();
                return;
            }

            if (pass != confirmPass)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor, inténtalo de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassw.Clear();
                textBoxConfirmPassw.Clear();
                textBoxPassw.Focus();
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("El campo de contraseña no puede estar vacío.",
                        "Error de Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                textBoxPassw.Focus();
                return;
            }

            //Aqui se debe agregar la funcion para alamacenar los datos en la base de datos

            MessageBox.Show("¡Registro exitoso!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            InicioSesion ventanaSesion = new InicioSesion();
            ventanaSesion.Show();
            this.Close();
        }

        private void Registrarse_FormClosing(object sender, FormClosingEventArgs e)
        {
            InicioSesion ventanaSesion = new InicioSesion();
            if (ventanaSesion != null)
            {
                ventanaSesion.Show();
            }
        }

        private void Registrarse_Load(object sender, EventArgs e)
        {

        }
    }


}
