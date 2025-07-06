namespace Projekt_zaliczeniowy_.NET
{
    partial class Form5
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
            txtName = new TextBox();
            txtLogin = new TextBox();
            txtPassword = new TextBox();
            txtEmail = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            checkBox1 = new CheckBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(182, 114);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "np. Adam Mazurek";
            txtName.Size = new Size(168, 27);
            txtName.TabIndex = 0;
            // 
            // txtLogin
            // 
            txtLogin.Location = new Point(182, 147);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new Size(168, 27);
            txtLogin.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(182, 180);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(168, 27);
            txtPassword.TabIndex = 2;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(182, 213);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "przyklad@przyklad.pl";
            txtEmail.Size = new Size(168, 27);
            txtEmail.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label1.Location = new Point(175, 37);
            label1.Name = "label1";
            label1.Size = new Size(211, 54);
            label1.TabIndex = 4;
            label1.Text = "Rejestracja\r\n";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(95, 91);
            label2.Name = "label2";
            label2.Size = new Size(371, 20);
            label2.TabIndex = 5;
            label2.Text = "Podaj niezbędne dane do zarejestrowania użytkownika\r\n";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(356, 117);
            label3.Name = "label3";
            label3.Size = new Size(110, 20);
            label3.TabIndex = 6;
            label3.Text = "Imie i nazwisko\r\n";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(356, 150);
            label4.Name = "label4";
            label4.Size = new Size(46, 20);
            label4.TabIndex = 7;
            label4.Text = "Login";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(356, 183);
            label5.Name = "label5";
            label5.Size = new Size(47, 20);
            label5.TabIndex = 8;
            label5.Text = "Hasło";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(356, 216);
            label6.Name = "label6";
            label6.Size = new Size(52, 20);
            label6.TabIndex = 9;
            label6.Text = "e-mail\r\n";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(409, 182);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(108, 24);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "Pokaż hasło\r\n";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // button1
            // 
            button1.Location = new Point(222, 246);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 11;
            button1.Text = "Wyślij\r\n";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(392, 12);
            button2.Name = "button2";
            button2.Size = new Size(160, 29);
            button2.TabIndex = 12;
            button2.Text = "Powrót do logowania\r\n";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form5
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(564, 337);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(checkBox1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtEmail);
            Controls.Add(txtPassword);
            Controls.Add(txtLogin);
            Controls.Add(txtName);
            Name = "Form5";
            Text = "ProSec";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtName;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private TextBox txtEmail;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private CheckBox checkBox1;
        private Button button1;
        private Button button2;
    }
}