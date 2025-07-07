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
using static System.Windows.Forms.DataFormats;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.VisualBasic.Logging;
using AuthService;

namespace Projekt_zaliczeniowy_.NET
{
    public partial class Form3 : Form
    {
        private AuthService.AuthService.LoginResponse _userToken;
        private CurrentUser _userData;
        public Form3(AuthService.AuthService.LoginResponse userToken)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _userToken = userToken;

            //if (_userToken == null)
            //{
            //    MessageBox.Show("_userToken jest nullem ;(");
            //}
            //else
            //{
            //    MessageBox.Show($"Token: {_userToken.access_token}");
            //}



            _userData = JwtHelper.ParseUserFromToken(_userToken.access_token);

            label4.Visible = true;
            label5.Visible = true;
            label5.Text = _userData.Fullname;

            //MessageBox.Show(
            //    $"Token: {_userToken.access_token}\n" +
            //    $"ID: {_userData.Id}\n" +
            //    $"Imię i nazwisko: {_userData.Fullname}\n" +
            //    $"Login: {_userData.Login}\n" +
            //    $"Hasło: {_userData.Password}\n" +
            //    $"Email: {_userData.Email}",
            //    "Dane użytkownika"
            //);

            Form3_Load(this, EventArgs.Empty);
        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            List<User> users = await GetUsersAsync(AuthService.AuthService.AccessToken);

            var displayList = users.Select(u => new
            {
                ImieNazwisko = u.Fullname,
                Id = u.Id,
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pobierz dane klikniętego użytkownika z DataGridView
            string selectedFullname = dataGridView1.Rows[e.RowIndex].Cells["ImieNazwisko"].Value?.ToString();
            int selectedUserId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Id"].Value);

            // Pobierz token (zakładam, że masz go gdzieś w AuthService)
            string token = AuthService.AuthService.AccessToken;

            // Parsuj dane zalogowanego użytkownika z tokena
            var currentUser = JwtHelper.ParseUserFromToken(token);

            // Utwórz nową instancję Chat z danymi nadawcy (z tokena) i odbiorcy (z DataGridView)
            // Pobierz dane z tokena (np. AuthService.AuthService.ParseUserFromToken(token))

            // Utwórz obiekt czatu
            Chat currentChat = new Chat()
            {
                FullnameFrom = currentUser.Fullname,
                UserIdFrom = currentUser.Id,
                FullnameTo = selectedFullname,
                UserIdTo = selectedUserId
            };

            CurrentUser cuser = new CurrentUser();
            cuser = JwtHelper.ParseUserFromToken(token);

            // Przekaż do Form6
            UserSession.Current = JwtHelper.ParseUserFromToken(token);
            Form6 form6 = new Form6();
            form6.CurrentChat = currentChat;
            form6.cuser = cuser;
            form6.Show();
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

    public class CurrentUser
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        // Statyczna instancja użytkownika
        public static CurrentUser Instance { get; set; }
    }

    public static class UserSession
    {
        public static CurrentUser Current { get; set; }
    }

    public class Chat
    {
        public string FullnameFrom { get; set; }
        public int UserIdFrom { get; set; }
        public string FullnameTo { get; set; }
        public int UserIdTo { get; set; }
    }

    public class JwtHelper
    {
        public static CurrentUser ParseUserFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var user = new CurrentUser();

            foreach (var claim in jwtToken.Claims)
            {
                switch (claim.Type)
                {
                    case "id":
                        user.Id = int.Parse(claim.Value);
                        break;
                    case "fullname":
                        user.Fullname = claim.Value;
                        break;
                    case "login":
                        user.Login = claim.Value;
                        break;
                    case "password":
                        user.Password = claim.Value;
                        break;
                    case "email":
                        user.Email = claim.Value;
                        break;
                }
            }

            return user;
        }
    }
}
