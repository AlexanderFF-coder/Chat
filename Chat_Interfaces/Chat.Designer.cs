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
            this.textBox2 = new System.Windows.Forms.RichTextBox();
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
            this.button1 = new System.Windows.Forms.Button();
            this.panelEmojis.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(1, 5);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(227, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(475, 522);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(537, 24);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            this.textBox2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyUp_1);
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
            this.listBox1.ItemHeight = 35;
            this.listBox1.Location = new System.Drawing.Point(1, 33);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(355, 459);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(237, 4);
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
            this.label2.Location = new System.Drawing.Point(1021, 519);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 29);
            this.label2.TabIndex = 5;
            this.label2.Text = "✓";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(368, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 513);
            this.panel1.TabIndex = 6;
            // 
            // buttonEmoji
            // 
            this.buttonEmoji.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEmoji.Location = new System.Drawing.Point(419, 516);
            this.buttonEmoji.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonEmoji.Name = "buttonEmoji";
            this.buttonEmoji.Size = new System.Drawing.Size(31, 31);
            this.buttonEmoji.TabIndex = 7;
            this.buttonEmoji.UseVisualStyleBackColor = true;
            this.buttonEmoji.Click += new System.EventHandler(this.buttonEmoji_Click);
            // 
            // panelEmojis
            // 
            this.panelEmojis.BackColor = System.Drawing.Color.LightGray;
            this.panelEmojis.Controls.Add(this.btnEmojiSad);
            this.panelEmojis.Controls.Add(this.btnEmojiHeart);
            this.panelEmojis.Controls.Add(this.btnEmojiSmile);
            this.panelEmojis.Location = new System.Drawing.Point(364, 441);
            this.panelEmojis.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelEmojis.Name = "panelEmojis";
            this.panelEmojis.Size = new System.Drawing.Size(211, 70);
            this.panelEmojis.TabIndex = 0;
            this.panelEmojis.Visible = false;
            // 
            // btnEmojiSad
            // 
            this.btnEmojiSad.BackColor = System.Drawing.Color.LightBlue;
            this.btnEmojiSad.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiSad.Location = new System.Drawing.Point(145, 10);
            this.btnEmojiSad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEmojiSad.Name = "btnEmojiSad";
            this.btnEmojiSad.Size = new System.Drawing.Size(51, 50);
            this.btnEmojiSad.TabIndex = 2;
            this.btnEmojiSad.UseVisualStyleBackColor = false;
            // 
            // btnEmojiHeart
            // 
            this.btnEmojiHeart.BackColor = System.Drawing.Color.Pink;
            this.btnEmojiHeart.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiHeart.Location = new System.Drawing.Point(80, 10);
            this.btnEmojiHeart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEmojiHeart.Name = "btnEmojiHeart";
            this.btnEmojiHeart.Size = new System.Drawing.Size(51, 50);
            this.btnEmojiHeart.TabIndex = 1;
            this.btnEmojiHeart.UseVisualStyleBackColor = false;
            // 
            // btnEmojiSmile
            // 
            this.btnEmojiSmile.BackColor = System.Drawing.Color.LightYellow;
            this.btnEmojiSmile.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmojiSmile.Location = new System.Drawing.Point(15, 10);
            this.btnEmojiSmile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEmojiSmile.Name = "btnEmojiSmile";
            this.btnEmojiSmile.Size = new System.Drawing.Size(51, 50);
            this.btnEmojiSmile.TabIndex = 0;
            this.btnEmojiSmile.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(275, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 28);
            this.button1.TabIndex = 8;
            this.button1.Text = "+ Personas";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.button1);
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
            this.Activated += new System.EventHandler(this.Chat_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chat_FormClosing);
            this.Load += new System.EventHandler(this.Chat_Load);
            this.VisibleChanged += new System.EventHandler(this.Chat_VisibleChanged);
            this.panelEmojis.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox textBox2;
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
        private System.Windows.Forms.Button button1;
    }
}