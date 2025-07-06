using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthService
{
    public static class AuthService
    {
        private static readonly HttpClient client = new HttpClient();

        // Zmienna na token przechowywany w pamięci
        public static string AccessToken { get; private set; }

        // Model żądania do logowania
        private class LoginRequest
        {
            public string login { get; set; }
            public string password { get; set; }
        }

        // Model odpowiedzi z tokenem
        private class LoginResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
        }

        public static async Task<bool> LoginAsync(string login, string password)
        {
            var loginRequest = new LoginRequest
            {
                login = login,
                password = password
            };

            var jsonRequest = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:8001/login", content);

                if (!response.IsSuccessStatusCode)
                    return false;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(jsonResponse);

                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.access_token))
                {
                    AccessToken = loginResponse.access_token;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}