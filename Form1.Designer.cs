namespace Projekt_zaliczeniowy_.NET
{
    partial class OknoPowitalne
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            Label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Label1.AutoSize = true;
            Label1.Font = new Font("Segoe UI", 12F);
            Label1.Location = new Point(115, 39);
            Label1.Name = "Label1";
            Label1.Size = new Size(283, 28);
            Label1.TabIndex = 0;
            Label1.Text = "Komunikator ProSec - Witamy!";
            Label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(76, 111);
            label2.Name = "label2";
            label2.Size = new Size(366, 20);
            label2.TabIndex = 1;
            label2.Text = "Proszę o chwile cierpliwości. Łączymy się z serwerem...";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(128, 131);
            label3.Name = "label3";
            label3.Size = new Size(270, 20);
            label3.TabIndex = 2;
            label3.Text = "Coś poszło nie tak, ponawiwam próbe...";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(149, 91);
            label4.Name = "label4";
            label4.Size = new Size(216, 20);
            label4.TabIndex = 3;
            label4.Text = "Udało połączyć się z serwerem!";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(149, 151);
            label5.Name = "label5";
            label5.Size = new Size(241, 20);
            label5.TabIndex = 4;
            label5.Text = "Nie udało połączyć się z serwerem!";
            // 
            // OknoPowitalne
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(512, 220);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(Label1);
            Name = "OknoPowitalne";
            Text = "Witaj!";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label Label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
    }
}
