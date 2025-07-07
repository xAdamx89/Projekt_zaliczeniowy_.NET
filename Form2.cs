using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AuthService;
using static AuthService.AuthService;

namespace Projekt_zaliczeniowy_.NET
{
    public partial class Form2 : Form
    {

        LoginResponse userToken = new LoginResponse();
        public Form2()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            label1.Visible = true;
            label2.Visible = true;
            label4.Visible = false;
            label5.Visible = false;
        }

        private async void button2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text, haslo = textBox2.Text;

            if (login == "" || haslo == "")
            {
                label4.Visible = true;
                return;
            }

            bool success = await AuthService.AuthService.LoginAsync(login, haslo);

            if (success)
            {
                label5.Visible = true;

                bool hasPublicKey = await AuthService.AuthService.HasPublicKeyAsync(AuthService.AuthService.AccessToken);

                userToken = await GetTokenByLoginAsync(login);

                await Task.Delay(1000);
                Form3 form3 = new Form3(userToken);
                form3.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Niepoprawny login lub hasło.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Close();
        }
    }

    public static class YourCryptoHelper
    {
        public static (string publicKey, string privateKey) GenerateKeyPair()
        {
            using var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(2048);
            try
            {
                string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                return (publicKey, privateKey);
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
    }
}
