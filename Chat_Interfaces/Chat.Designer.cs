namespace Chat_Interfaces
{
    partial class Chat
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
            this.components = new System.ComponentModel.Container();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonEmoji = new System.Windows.Forms.Button();
            this.panelEmojis = new System.Windows.Forms.Panel();
            this.btnEmojiSmile = new System.Windows.Forms.Button();
            this.btnEmojiHeart = new System.Windows.Forms.Button();
            this.btnEmojiSad = new System.Windows.Forms.Button();
            this.panelEmojis.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(57, 15);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 22);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(455, 521);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(538, 22);
            this.textBox2.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.FloralWhite;
            this.listBox1.Font = new System.Drawing.Font("Niagara Solid", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 47;
            this.listBox1.Location = new System.Drawing.Point(1, 69);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(355, 474);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(252, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 29);
            this.label1.TabIndex = 4;
            this.label1.Text = "+";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1012, 518);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 29);
            this.label2.TabIndex = 5;
            this.label2.Text = "+";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(365, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(703, 510);
            this.panel1.TabIndex = 6;
            // 
            // buttonEmoji
            // 
            this.buttonEmoji.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEmoji.Location = new System.Drawing.Point(402, 518);
            this.buttonEmoji.Name = "buttonEmoji";
            this.buttonEmoji.Size = new System.Drawing.Size(31, 31);
            this.buttonEmoji.TabIndex = 7;
            this.buttonEmoji.Text = "😁";
            this.buttonEmoji.UseVisualStyleBackColor = true;
            // 
            // panelEmojis
            // 
            this.panelEmojis.BackColor = System.Drawing.Color.LightGray;
            this.panelEmojis.Controls.Add(this.btnEmojiSad);
            this.panelEmojis.Controls.Add(this.btnEmojiHeart);
            this.panelEmojis.Controls.Add(this.btnEmojiSmile);
            this.panelEmojis.Location = new System.Drawing.Point(273, 441);
            this.panelEmojis.Name = "panelEmojis";
            this.panelEmojis.Size = new System.Drawing.Size(210, 70);
            this.panelEmojis.TabIndex = 0;
            this.panelEmojis.Visible = false;
            // 
            // btnEmojiSmile
            // 
            this.btnEmojiSmile.BackColor = System.Drawing.Color.LightYellow;
            this.btnEmojiSmile.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiSmile.Location = new System.Drawing.Point(15, 10);
            this.btnEmojiSmile.Name = "btnEmojiSmile";
            this.btnEmojiSmile.Size = new System.Drawing.Size(50, 50);
            this.btnEmojiSmile.TabIndex = 0;
            this.btnEmojiSmile.Text = "😁";
            this.btnEmojiSmile.UseVisualStyleBackColor = false;
            // 
            // btnEmojiHeart
            // 
            this.btnEmojiHeart.BackColor = System.Drawing.Color.Pink;
            this.btnEmojiHeart.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiHeart.Location = new System.Drawing.Point(80, 10);
            this.btnEmojiHeart.Name = "btnEmojiHeart";
            this.btnEmojiHeart.Size = new System.Drawing.Size(50, 50);
            this.btnEmojiHeart.TabIndex = 1;
            this.btnEmojiHeart.Text = "❤️";
            this.btnEmojiHeart.UseVisualStyleBackColor = false;
            // 
            // btnEmojiSad
            // 
            this.btnEmojiSad.BackColor = System.Drawing.Color.LightBlue;
            this.btnEmojiSad.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiSad.Location = new System.Drawing.Point(145, 10);
            this.btnEmojiSad.Name = "btnEmojiSad";
            this.btnEmojiSad.Size = new System.Drawing.Size(50, 50);
            this.btnEmojiSad.TabIndex = 2;
            this.btnEmojiSad.Text = "😔";
            this.btnEmojiSad.UseVisualStyleBackColor = false;
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.panelEmojis);
            this.Controls.Add(this.buttonEmoji);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Chat";
            this.Text = "Chat";
            this.Load += new System.EventHandler(this.Chat_Load);
            this.VisibleChanged += new System.EventHandler(this.Chat_VisibleChanged);
            this.panelEmojis.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonEmoji;
        private System.Windows.Forms.Panel panelEmojis;
        private System.Windows.Forms.Button btnEmojiSmile;
        private System.Windows.Forms.Button btnEmojiHeart;
        private System.Windows.Forms.Button btnEmojiSad;
    }
}