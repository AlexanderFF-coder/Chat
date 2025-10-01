namespace Chat_Interfaces
{
    partial class InicioSesion
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblEmail = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblRegistro = new System.Windows.Forms.Label();
            this.panelLogin = new System.Windows.Forms.Panel();
            this.labelPregunta = new System.Windows.Forms.Label();
            this.panelLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(91, 29);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(93, 13);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Correo electrónico";
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(44, 55);
            this.textBoxEmail.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(189, 20);
            this.textBoxEmail.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(108, 110);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(61, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Contraseña";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(53, 142);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(166, 20);
            this.textBoxPassword.TabIndex = 3;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(93, 203);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(85, 19);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Iniciar sesión";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblRegistro
            // 
            this.lblRegistro.AutoSize = true;
            this.lblRegistro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRegistro.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegistro.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblRegistro.Location = new System.Drawing.Point(108, 281);
            this.lblRegistro.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRegistro.Name = "lblRegistro";
            this.lblRegistro.Size = new System.Drawing.Size(60, 13);
            this.lblRegistro.TabIndex = 5;
            this.lblRegistro.Text = "Registrarse";
            this.lblRegistro.Click += new System.EventHandler(this.lblRegistro_Click);
            // 
            // panelLogin
            // 
            this.panelLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelLogin.Controls.Add(this.labelPregunta);
            this.panelLogin.Controls.Add(this.lblEmail);
            this.panelLogin.Controls.Add(this.lblRegistro);
            this.panelLogin.Controls.Add(this.textBoxEmail);
            this.panelLogin.Controls.Add(this.btnLogin);
            this.panelLogin.Controls.Add(this.lblPassword);
            this.panelLogin.Controls.Add(this.textBoxPassword);
            this.panelLogin.Location = new System.Drawing.Point(56, 78);
            this.panelLogin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(263, 363);
            this.panelLogin.TabIndex = 6;
            // 
            // labelPregunta
            // 
            this.labelPregunta.AutoSize = true;
            this.labelPregunta.Location = new System.Drawing.Point(78, 268);
            this.labelPregunta.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPregunta.Name = "labelPregunta";
            this.labelPregunta.Size = new System.Drawing.Size(121, 13);
            this.labelPregunta.TabIndex = 6;
            this.labelPregunta.Text = "¿No tienes cuenta aún?";
            // 
            // InicioSesion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 449);
            this.Controls.Add(this.panelLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InicioSesion";
            this.Text = "Inicio de sesión";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InicioSesion_FormClosing);
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblRegistro;
        private System.Windows.Forms.Panel panelLogin;
        private System.Windows.Forms.Label labelPregunta;
    }
}

