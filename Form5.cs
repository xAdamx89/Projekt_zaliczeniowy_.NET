using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;

namespace Projekt_zaliczeniowy_.NET
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            txtPassword.UseSystemPasswordChar = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form5 = Application.OpenForms["Form5"];
            if (form5 != null)
            {
                form5.Close();
            }

            Form2 form2 = new Form2();
            form2.StartPosition = FormStartPosition.CenterScreen;
            form2.Show();
            this.Close();
        }

        public async Task<string> RegisterUserAsync(RegisterRequest request)
        {
            var responseContent = "";

            using var client = new HttpClient();
            try
            {
                var response = await client.PostAsJsonAsync("http://localhost:8001/register", request);

                responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parsowanie pola "message" z JSON-a
                    var jsonDoc = JsonDocument.Parse(responseContent);
                    var msg = jsonDoc.RootElement.GetProperty("message").GetString();
                    return $"Sukces: {msg}";
                }
                else
                {
                    // Parsowanie pola "detail" z JSON-a w przypadku błędu
                    var jsonDoc = JsonDocument.Parse(responseContent);
                    var errorMsg = jsonDoc.RootElement.GetProperty("detail").GetString();
                    return $"Błąd: {errorMsg}";
                }
            }
            catch (HttpRequestException ex)
            {
                return $"Błąd sieci: {ex.Message}";
            }
            catch (JsonException)
            {
                // Jeśli odpowiedź nie jest w formacie JSON lub brakuje oczekiwanych pól
                return $"Nieoczekiwana odpowiedź serwera: {responseContent}";
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie pola.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new RegisterRequest
            {
                fullname = txtName.Text,
                login = txtLogin.Text,
                passwd = txtPassword.Text,
                email = txtEmail.Text
            };

            var result = await RegisterUserAsync(request);
            MessageBox.Show(result);

            if (result.StartsWith("Sukces:"))
            {
                Form4 form4 = new Form4();
                form4.Show();
                this.Close();
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }

    public class RegisterRequest
    {
        public string fullname { get; set; }
        public string login { get; set; }
        public string passwd { get; set; }
        public string email { get; set; }
    }
}
