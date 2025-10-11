namespace Chat_Interfaces
{
    partial class AgregarMiembros
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
            this.checkedListBoxUsuarios = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBoxUsuarios
            // 
            this.checkedListBoxUsuarios.FormattingEnabled = true;
            this.checkedListBoxUsuarios.Location = new System.Drawing.Point(65, 65);
            this.checkedListBoxUsuarios.Margin = new System.Windows.Forms.Padding(2);
            this.checkedListBoxUsuarios.Name = "checkedListBoxUsuarios";
            this.checkedListBoxUsuarios.Size = new System.Drawing.Size(250, 214);
            this.checkedListBoxUsuarios.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(155, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 42);
            this.label1.TabIndex = 2;
            this.label1.Text = "Agregar usuarios";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(364, 129);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 52);
            this.button1.TabIndex = 3;
            this.button1.Text = "Agregar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AgregarMiembros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(533, 292);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBoxUsuarios);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AgregarMiembros";
            this.Text = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AgregarMiembros_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxUsuarios;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}