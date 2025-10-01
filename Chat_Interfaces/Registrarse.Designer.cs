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
            this.panelRegister.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRegister
            // 
            this.panelRegister.Controls.Add(this.buttonRegister);
            this.panelRegister.Controls.Add(this.dateTimeFechaNac);
            this.panelRegister.Controls.Add(this.textBoxConfirmPassw);
            this.panelRegister.Controls.Add(this.textBoxEmail);
            this.panelRegister.Controls.Add(this.textBoxPassw);
            this.panelRegister.Controls.Add(this.lblFechaNac);
            this.panelRegister.Controls.Add(this.lblPasswConfirm);
            this.panelRegister.Controls.Add(this.lblPassw);
            this.panelRegister.Controls.Add(this.lblEmail);
            this.panelRegister.Location = new System.Drawing.Point(52, 51);
            this.panelRegister.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelRegister.Name = "panelRegister";
            this.panelRegister.Size = new System.Drawing.Size(263, 376);
            this.panelRegister.TabIndex = 0;
            // 
            // buttonRegister
            // 
            this.buttonRegister.Location = new System.Drawing.Point(88, 326);
            this.buttonRegister.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonRegister.Name = "buttonRegister";
            this.buttonRegister.Size = new System.Drawing.Size(79, 20);
            this.buttonRegister.TabIndex = 8;
            this.buttonRegister.Text = "Registrarse";
            this.buttonRegister.UseVisualStyleBackColor = true;
            this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
            // 
            // dateTimeFechaNac
            // 
            this.dateTimeFechaNac.Location = new System.Drawing.Point(63, 286);
            this.dateTimeFechaNac.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dateTimeFechaNac.MaxDate = new System.DateTime(2025, 12, 25, 23, 59, 59, 0);
            this.dateTimeFechaNac.MinDate = new System.DateTime(1900, 12, 31, 0, 0, 0, 0);
            this.dateTimeFechaNac.Name = "dateTimeFechaNac";
            this.dateTimeFechaNac.Size = new System.Drawing.Size(135, 20);
            this.dateTimeFechaNac.TabIndex = 7;
            // 
            // textBoxConfirmPassw
            // 
            this.textBoxConfirmPassw.Location = new System.Drawing.Point(63, 208);
            this.textBoxConfirmPassw.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxConfirmPassw.Name = "textBoxConfirmPassw";
            this.textBoxConfirmPassw.PasswordChar = '•';
            this.textBoxConfirmPassw.Size = new System.Drawing.Size(135, 20);
            this.textBoxConfirmPassw.TabIndex = 6;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(63, 52);
            this.textBoxEmail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(135, 20);
            this.textBoxEmail.TabIndex = 4;
            this.textBoxEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxEmail_KeyDown);
            // 
            // textBoxPassw
            // 
            this.textBoxPassw.Location = new System.Drawing.Point(63, 133);
            this.textBoxPassw.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxPassw.Name = "textBoxPassw";
            this.textBoxPassw.PasswordChar = '•';
            this.textBoxPassw.Size = new System.Drawing.Size(135, 20);
            this.textBoxPassw.TabIndex = 5;
            this.textBoxPassw.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPassw_KeyDown);
            // 
            // lblFechaNac
            // 
            this.lblFechaNac.AutoSize = true;
            this.lblFechaNac.Location = new System.Drawing.Point(61, 260);
            this.lblFechaNac.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFechaNac.Name = "lblFechaNac";
            this.lblFechaNac.Size = new System.Drawing.Size(141, 13);
            this.lblFechaNac.TabIndex = 3;
            this.lblFechaNac.Text = "Ingrese fecha de nacimiento";
            // 
            // lblPasswConfirm
            // 
            this.lblPasswConfirm.AutoSize = true;
            this.lblPasswConfirm.Location = new System.Drawing.Point(73, 182);
            this.lblPasswConfirm.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPasswConfirm.Name = "lblPasswConfirm";
            this.lblPasswConfirm.Size = new System.Drawing.Size(108, 13);
            this.lblPasswConfirm.TabIndex = 2;
            this.lblPasswConfirm.Text = "Confirmar Contraseña";
            // 
            // lblPassw
            // 
            this.lblPassw.AutoSize = true;
            this.lblPassw.Location = new System.Drawing.Point(96, 104);
            this.lblPassw.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPassw.Name = "lblPassw";
            this.lblPassw.Size = new System.Drawing.Size(61, 13);
            this.lblPassw.TabIndex = 1;
            this.lblPassw.Text = "Contraseña";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(85, 26);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(93, 13);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Correo electrónico";
            // 
            // Registrarse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 449);
            this.Controls.Add(this.panelRegister);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Registrarse";
            this.Text = "Registrarse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Registrarse_FormClosing);
            this.Load += new System.EventHandler(this.Registrarse_Load);
            this.panelRegister.ResumeLayout(false);
            this.panelRegister.PerformLayout();
            this.ResumeLayout(false);

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
    }
}