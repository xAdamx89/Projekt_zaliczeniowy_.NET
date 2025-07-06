using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AuthService;

namespace Projekt_zaliczeniowy_.NET
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += Form3_Load;

        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            List<User> users = await GetUsersAsync(AuthService.AuthService.AccessToken);

            var displayList = users.Select(u => new
            {
                ImieNazwisko = u.Fullname,
                Email = u.Email
            }).ToList();

            dataGridView1.DataSource = displayList;

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.Refresh();
        }

        public async Task<List<User>> GetUsersAsync(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                // Dodaj nagłówek autoryzacji
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://localhost:8001/getusers");

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        var options = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        List<User> users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(json, options);
                        return users ?? new List<User>();
                    }
                    else
                    {
                        MessageBox.Show($"Błąd serwera: {response.StatusCode}");
                        return new List<User>();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia: " + ex.Message);
                    return new List<User>();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
