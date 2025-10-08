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
            this.btnEmojiSad = new System.Windows.Forms.Button();
            this.btnEmojiHeart = new System.Windows.Forms.Button();
            this.btnEmojiSmile = new System.Windows.Forms.Button();
            this.panelEmojis.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(171, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(356, 424);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(404, 20);
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
            this.listBox1.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 29;
            this.listBox1.Location = new System.Drawing.Point(1, 27);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(267, 410);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(189, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "+";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(766, 422);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "✓";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(274, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 414);
            this.panel1.TabIndex = 6;
            // 
            // buttonEmoji
            // 
            this.buttonEmoji.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEmoji.Location = new System.Drawing.Point(314, 419);
            this.buttonEmoji.Margin = new System.Windows.Forms.Padding(2);
            this.buttonEmoji.Name = "buttonEmoji";
            this.buttonEmoji.Size = new System.Drawing.Size(23, 25);
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
            this.panelEmojis.Location = new System.Drawing.Point(273, 358);
            this.panelEmojis.Margin = new System.Windows.Forms.Padding(2);
            this.panelEmojis.Name = "panelEmojis";
            this.panelEmojis.Size = new System.Drawing.Size(158, 57);
            this.panelEmojis.TabIndex = 0;
            this.panelEmojis.Visible = false;
            // 
            // btnEmojiSad
            // 
            this.btnEmojiSad.BackColor = System.Drawing.Color.LightBlue;
            this.btnEmojiSad.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiSad.Location = new System.Drawing.Point(109, 8);
            this.btnEmojiSad.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmojiSad.Name = "btnEmojiSad";
            this.btnEmojiSad.Size = new System.Drawing.Size(38, 41);
            this.btnEmojiSad.TabIndex = 2;
            this.btnEmojiSad.Text = "😔";
            this.btnEmojiSad.UseVisualStyleBackColor = false;
            // 
            // btnEmojiHeart
            // 
            this.btnEmojiHeart.BackColor = System.Drawing.Color.Pink;
            this.btnEmojiHeart.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiHeart.Location = new System.Drawing.Point(60, 8);
            this.btnEmojiHeart.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmojiHeart.Name = "btnEmojiHeart";
            this.btnEmojiHeart.Size = new System.Drawing.Size(38, 41);
            this.btnEmojiHeart.TabIndex = 1;
            this.btnEmojiHeart.Text = "❤️";
            this.btnEmojiHeart.UseVisualStyleBackColor = false;
            // 
            // btnEmojiSmile
            // 
            this.btnEmojiSmile.BackColor = System.Drawing.Color.LightYellow;
            this.btnEmojiSmile.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiSmile.Location = new System.Drawing.Point(11, 8);
            this.btnEmojiSmile.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmojiSmile.Name = "btnEmojiSmile";
            this.btnEmojiSmile.Size = new System.Drawing.Size(38, 41);
            this.btnEmojiSmile.TabIndex = 0;
            this.btnEmojiSmile.Text = "😁";
            this.btnEmojiSmile.UseVisualStyleBackColor = false;
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelEmojis);
            this.Controls.Add(this.buttonEmoji);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "Chat";
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chat_FormClosing);
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