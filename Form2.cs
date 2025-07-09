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
        public static (string publicKeyPEM, string privateKeyPEM) GenerateKeyPair()
        {
            using var rsa = new System.Security.Cryptography.RSACryptoServiceProvider(2048);
            try
            {
                // Export klucza publicznego w formacie SubjectPublicKeyInfo (X.509) — to jest standard dla "BEGIN PUBLIC KEY"
                var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
                string publicKeyBase64 = Convert.ToBase64String(publicKeyBytes);
                string publicKeyPEM = PemFormat(publicKeyBase64, "PUBLIC KEY");

                // Export klucza prywatnego w formacie PKCS#1 — odpowiada nagłówkowi "BEGIN RSA PRIVATE KEY"
                var privateKeyBytes = rsa.ExportRSAPrivateKey();
                string privateKeyBase64 = Convert.ToBase64String(privateKeyBytes);
                string privateKeyPEM = PemFormat(privateKeyBase64, "RSA PRIVATE KEY");

                return (publicKeyPEM, privateKeyPEM);
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }

        private static string PemFormat(string base64Data, string label)
        {
            // Dodaje nagłówki PEM i łamie co 64 znaki
            var builder = new StringBuilder();
            builder.AppendLine($"-----BEGIN {label}-----");

            int lineLength = 64;
            for (int i = 0; i < base64Data.Length; i += lineLength)
            {
                int len = Math.Min(lineLength, base64Data.Length - i);
                builder.AppendLine(base64Data.Substring(i, len));
            }

            builder.AppendLine($"-----END {label}-----");
            return builder.ToString();
        }
    }
}
