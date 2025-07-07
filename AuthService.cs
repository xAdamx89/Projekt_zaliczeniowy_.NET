using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Projekt_zaliczeniowy_.NET;

namespace AuthService
{
    public static class AuthService
    {
        private static readonly HttpClient client = new HttpClient();

        // Zmienna na token przechowywany w pamięci
        public static string AccessToken { get; private set; }

        public static string CurrentUserFullname { get; set; }

        public static void SetCurrentUserFullname(string fullname)
        {
            CurrentUserFullname = fullname;
        }

        // Model żądania do logowania
        private class LoginRequest
        {
            public string login { get; set; }
            public string password { get; set; }
        }

        // Model odpowiedzi z tokenem
        public class LoginResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
        }

        public class HasPublicKeyResponse
        {
            [JsonPropertyName("has_public_key")]
            public bool HasPublicKey { get; set; }
        }

        public class UploadPublicKeyRequest
        {
            [JsonPropertyName("public_key")]
            public string PublicKey { get; set; }
        }

        public static async Task<bool> LoginAsync(string login, string password)
        {
            CurrentUser cuser = new CurrentUser();

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


                    string token = AuthService.AccessToken;
                    UserSession.Current = JwtHelper.ParseUserFromToken(token);
                    //string imie = UserSession.Current.Fullname;
                    //int id = UserSession.Current.Id;
                    //Tak mogę się odwoływać

                    if (!await HasPublicKeyAsync(token))
                    {
                        MessageBox.Show("Wykryto brak klucza publicznego w bazie danych. Inicjalizuje generowanie klucza...");
                        var (publicKey, privateKey) = YourCryptoHelper.GenerateKeyPair();
                        await UploadPublicKeyAsync(publicKey);
                        MessageBox.Show("Wygenerowano klucz!\n" + publicKey + "\nZakończono próbe wysłania go na serwer.");

                        SavePrivateKey(CurrentUserFullname, privateKey);

                        bool success = await UploadPublicKeyAsync(publicKey);
                        if (success)
                        {
                            MessageBox.Show("Klucz publiczny został wysłany pomyślnie.");
                        }
                        else
                        {
                            MessageBox.Show("Błąd podczas wysyłania klucza publicznego.");
                        }


                    }

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void SavePrivateKey(string fullname, string privateKey)
        {
            if (string.IsNullOrWhiteSpace(fullname))
            {
                MessageBox.Show("fullname jest pusty lub null!");
                return;
            }

            string directoryPath = @"C:\Projekt_zaliczeniowy_.NET_AdamMazurek\Projekt_zaliczeniowy_.NET\KluczePrywatne";

            // Upewnij się, że katalog istnieje
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Stwórz pełną ścieżkę do pliku
            string filePath = Path.Combine(directoryPath, $"{fullname}.txt");



            // Zapisz klucz prywatny do pliku
            File.WriteAllText(filePath, privateKey);
            MessageBox.Show("Utworzono plik z kluczem prywatnym");
        }

        public static async Task<bool> HasPublicKeyAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8001/has_public_key");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<HasPublicKeyResponse>(content);
            return result?.HasPublicKey ?? false;
        }

        public static async Task<bool> UploadPublicKeyAsync(string publicKey)
        {
            var url = "http://localhost:8001/upload_public_key";

            var requestData = new UploadPublicKeyRequest
            {
                PublicKey = publicKey
            };

            string token = AuthService.AccessToken;

            var json = JsonSerializer.Serialize(requestData);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public static async Task<LoginResponse> GetTokenByLoginAsync(string login)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://localhost:8001/get_user_token?login={login}";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    return loginResponse;
                }
                else
                {
                    throw new HttpRequestException($"Błąd pobierania tokenu: {response.StatusCode}");
                }
            }
        }

    }

}