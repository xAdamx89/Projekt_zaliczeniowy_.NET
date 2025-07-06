namespace Projekt_zaliczeniowy_.NET
{
    partial class Form2
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
            label1 = new Label();
            textBox1 = new TextBox();
            button2 = new Button();
            label2 = new Label();
            textBox2 = new TextBox();
            button3 = new Button();
            label4 = new Label();
            label5 = new Label();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(230, 36);
            label1.Name = "label1";
            label1.Size = new Size(191, 28);
            label1.TabIndex = 0;
            label1.Text = "Komunikator ProSec";
            // 
            // textBox1
            // 
            textBox1.Cursor = Cursors.Hand;
            textBox1.Location = new Point(194, 123);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Login";
            textBox1.Size = new Size(256, 27);
            textBox1.TabIndex = 1;
            // 
            // button2
            // 
            button2.Cursor = Cursors.Hand;
            button2.Location = new Point(271, 205);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 3;
            button2.Text = "Zaloguj";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            button2.MouseClick += button2_MouseClick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(122, 75);
            label2.Name = "label2";
            label2.Size = new Size(433, 20);
            label2.TabIndex = 4;
            label2.Text = "Proszę wprowadzić niezbędne dane, niezbędne do zalogowania";
            // 
            // textBox2
            // 
            textBox2.Cursor = Cursors.Hand;
            textBox2.Location = new Point(194, 156);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Hasło";
            textBox2.Size = new Size(256, 27);
            textBox2.TabIndex = 5;
            textBox2.UseSystemPasswordChar = true;
            // 
            // button3
            // 
            button3.BackgroundImageLayout = ImageLayout.None;
            button3.Cursor = Cursors.Hand;
            button3.Location = new Point(246, 240);
            button3.Name = "button3";
            button3.Size = new Size(148, 29);
            button3.TabIndex = 6;
            button3.Text = "Zarejestruj się";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(246, 95);
            label4.Name = "label4";
            label4.Size = new Size(156, 20);
            label4.TabIndex = 8;
            label4.Text = "Uzupełnij login i hasło";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(371, 209);
            label5.Name = "label5";
            label5.Size = new Size(151, 20);
            label5.TabIndex = 9;
            label5.Text = "Udało się zalogować!";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(467, 158);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(108, 24);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "Pokaż hasło";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(627, 333);
            Controls.Add(checkBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(button3);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "Form2";
            Text = "ProSec";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button button2;
        private Label label2;
        private TextBox textBox2;
        private Button button3;
        private Label label4;
        private Label label5;
        private CheckBox checkBox1;
    }
}