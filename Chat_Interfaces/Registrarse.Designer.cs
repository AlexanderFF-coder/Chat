namespace Chat_Interfaces
{
    partial class Registrarse
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelRegister = new System.Windows.Forms.Panel();
            this.buttonRegister = new System.Windows.Forms.Button();
            this.dateTimeFechaNac = new System.Windows.Forms.DateTimePicker();
            this.textBoxConfirmPassw = new System.Windows.Forms.TextBox();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.textBoxPassw = new System.Windows.Forms.TextBox();
            this.lblFechaNac = new System.Windows.Forms.Label();
            this.lblPasswConfirm = new System.Windows.Forms.Label();
            this.lblPassw = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelNombre = new System.Windows.Forms.Label();
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.panelRegister.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRegister
            // 
            this.panelRegister.Controls.Add(this.textBoxNombre);
            this.panelRegister.Controls.Add(this.labelNombre);
            this.panelRegister.Controls.Add(this.buttonRegister);
            this.panelRegister.Controls.Add(this.dateTimeFechaNac);
            this.panelRegister.Controls.Add(this.textBoxConfirmPassw);
            this.panelRegister.Controls.Add(this.textBoxEmail);
            this.panelRegister.Controls.Add(this.textBoxPassw);
            this.panelRegister.Controls.Add(this.lblFechaNac);
            this.panelRegister.Controls.Add(this.lblPasswConfirm);
            this.panelRegister.Controls.Add(this.lblPassw);
            this.panelRegister.Controls.Add(this.lblEmail);
            this.panelRegister.Location = new System.Drawing.Point(61, 99);
            this.panelRegister.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelRegister.Name = "panelRegister";
            this.panelRegister.Size = new System.Drawing.Size(395, 579);
            this.panelRegister.TabIndex = 0;
            // 
            // buttonRegister
            // 
            this.buttonRegister.Location = new System.Drawing.Point(131, 534);
            this.buttonRegister.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(118, 31);
            this.buttonRegister.TabIndex = 8;
            this.buttonRegister.Text = "Registrarse";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // dateTimeFechaNac
            // 
            this.dateTimeFechaNac.Location = new System.Drawing.Point(93, 478);
            this.dateTimeFechaNac.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTimeFechaNac.MaxDate = new System.DateTime(2025, 12, 25, 23, 59, 59, 0);
            this.dateTimeFechaNac.MinDate = new System.DateTime(1900, 12, 31, 0, 0, 0, 0);
            this.dateTimeFechaNac.Name = "dateTimeFechaNac";
            this.dateTimeFechaNac.Size = new System.Drawing.Size(201, 26);
            this.dateTimeFechaNac.TabIndex = 7;
            // 
            // textBoxConfirmPassw
            // 
            this.textBoxConfirmPassw.Location = new System.Drawing.Point(93, 371);
            this.textBoxConfirmPassw.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxConfirmPassw.Name = "textBoxConfirmPassw";
            this.textBoxConfirmPassw.PasswordChar = '•';
            this.textBoxConfirmPassw.Size = new System.Drawing.Size(201, 26);
            this.textBoxConfirmPassw.TabIndex = 6;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(93, 143);
            this.textBoxEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(201, 26);
            this.textBoxEmail.TabIndex = 4;
            this.textBoxEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxEmail_KeyDown);
            // 
            // textBoxPassw
            // 
            this.textBoxPassw.Location = new System.Drawing.Point(93, 255);
            this.textBoxPassw.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxPassw.Name = "textBoxPassw";
            this.textBoxPassw.PasswordChar = '•';
            this.textBoxPassw.Size = new System.Drawing.Size(201, 26);
            this.textBoxPassw.TabIndex = 5;
            this.textBoxPassw.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPassw_KeyDown);
            // 
            // lblFechaNac
            // 
            this.lblFechaNac.AutoSize = true;
            this.lblFechaNac.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaNac.Location = new System.Drawing.Point(51, 427);
            this.lblFechaNac.Name = "lblFechaNac";
            this.lblFechaNac.Size = new System.Drawing.Size(312, 26);
            this.lblFechaNac.TabIndex = 3;
            this.lblFechaNac.Text = "Ingrese fecha de nacimiento";
            // 
            // lblPasswConfirm
            // 
            this.lblPasswConfirm.AutoSize = true;
            this.lblPasswConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswConfirm.Location = new System.Drawing.Point(75, 320);
            this.lblPasswConfirm.Name = "lblPasswConfirm";
            this.lblPasswConfirm.Size = new System.Drawing.Size(246, 26);
            this.lblPasswConfirm.TabIndex = 2;
            this.lblPasswConfirm.Text = "Confirmar Contraseña";
            // 
            // lblPassw
            // 
            this.lblPassw.AutoSize = true;
            this.lblPassw.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassw.Location = new System.Drawing.Point(126, 208);
            this.lblPassw.Name = "lblPassw";
            this.lblPassw.Size = new System.Drawing.Size(134, 26);
            this.lblPassw.TabIndex = 1;
            this.lblPassw.Text = "Contraseña";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(98, 99);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(207, 26);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Correo electrónico";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(111, 42);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(313, 32);
            this.label3.TabIndex = 6;
            this.label3.Text = "Formulario de registro";
            // 
            // labelNombre
            // 
            this.labelNombre.AutoSize = true;
            this.labelNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNombre.Location = new System.Drawing.Point(98, 11);
            this.labelNombre.Name = "labelNombre";
            this.labelNombre.Size = new System.Drawing.Size(200, 26);
            this.labelNombre.TabIndex = 9;
            this.labelNombre.Text = "Nombre completo";
            // 
            // textBoxNombre
            // 
            this.textBoxNombre.Location = new System.Drawing.Point(103, 49);
            this.textBoxNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxNombre.Name = "textBoxNombre";
            this.textBoxNombre.Size = new System.Drawing.Size(201, 26);
            this.textBoxNombre.TabIndex = 10;
            this.textBoxNombre.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxNombre_KeyDown);
            // 
            // Registrarse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(541, 691);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panelRegister);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Registrarse";
            this.Text = "Registrarse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Registrarse_FormClosing);
            this.Load += new System.EventHandler(this.Registrarse_Load);
            this.panelRegister.ResumeLayout(false);
            this.panelRegister.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelRegister;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblFechaNac;
        private System.Windows.Forms.Label lblPasswConfirm;
        private System.Windows.Forms.Label lblPassw;
        private System.Windows.Forms.DateTimePicker dateTimeFechaNac;
        private System.Windows.Forms.TextBox textBoxConfirmPassw;
        private System.Windows.Forms.TextBox textBoxPassw;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Button buttonRegister;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelNombre;
        private System.Windows.Forms.TextBox textBoxNombre;
    }
}