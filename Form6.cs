using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Projekt_zaliczeniowy_.NET;

namespace Projekt_zaliczeniowy_.NET
{
    public partial class Form6 : Form
    {

        public Chat CurrentChat { get; set; }
        public CurrentUser cuser { get; set; }
        public cToken ctoken { get; set; }

        public class Message
        {
            public int Id { get; set; }
            public int ConversationId { get; set; }
            public int SenderId { get; set; }
            public string Content { get; set; }
            public DateTime SentAt { get; set; }
        }

        public class PublicKeyRequest
        {
            public int user_id { get; set; }
        }

        public class SendMessageRequest
        {
            public int id_od { get; set; }
            public int id_do { get; set; }
            public string content { get; set; }
        }

        public Form6()
        {
            InitializeComponent();
            this.Load += Form6_Load;
        }

        private async void Form6_Load(object sender, EventArgs e)
        {

            textBox1.Multiline = true;
            textBox1.AcceptsReturn = true;

            if (UserSession.Current == null)
            {
                MessageBox.Show("UserSession.Current is NULL");
                this.Close();
                return;
            }

            if (CurrentChat != null)
            {
                string imie = UserSession.Current.Fullname;
                int id = UserSession.Current.Id;

                label1.Text = $"Od: {imie} (ID: {id})";
                label2.Text = $"Do: {CurrentChat.FullnameTo} (ID: {CurrentChat.UserIdTo})";

                string path = $@"C:\Projekt_zaliczeniowy_.NET_AdamMazurek\Projekt_zaliczeniowy_.NET\KluczePrywatne\{imie}.txt";
                string privKey = string.Empty;

                await LoadAndDisplayMessagesAsync();

                try
                {
                    privKey = System.IO.File.ReadAllText(path);
                }
                catch (Exception ex)
                {
                    // Obsłuż wyjątek (np. plik nie istnieje lub brak uprawnień)
                    Console.WriteLine($"Błąd podczas odczytu pliku: {ex.Message}");
                }

            }
            else
            {
                MessageBox.Show("Brak danych czatu.");
                this.Close();
            }
        }

        private async Task LoadAndDisplayMessagesAsync()
        {
            // Sprawdzenie sesji i danych czatu
            if (UserSession.Current == null || CurrentChat == null)
            {
                MessageBox.Show("Brak danych użytkownika lub czatu.");
                this.Close();
                return;
            }

            int user1_id = UserSession.Current.Id;
            int user2_id = CurrentChat.UserIdTo;

            string imie = UserSession.Current.Fullname;
            string path = $@"C:\Projekt_zaliczeniowy_.NET_AdamMazurek\Projekt_zaliczeniowy_.NET\KluczePrywatne\{imie}.txt";
            string privKey = string.Empty;

            try
            {
                privKey = System.IO.File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas odczytu klucza prywatnego: {ex.Message}");
                return;
            }

            // URL endpointa - dostosuj adres serwera i port!
            string url = $"http://localhost:8001/get_messages/{user1_id}/{user2_id}";

            try
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ctoken.token);
                // Dodaj token autoryzacji, jeśli potrzebny (przykład)
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserSession.Current.Token);

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                // Deserializacja listy wiadomości (dostosuj model MessageResponse do JSON)
                var messages = System.Text.Json.JsonSerializer.Deserialize<List<Message>>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (messages == null || messages.Count == 0)
                {
                    richTextBox1.Text = "Czat jest pusty!";
                    return;
                }

                richTextBox1.Clear();

                foreach (var msg in messages)
                {
                    string decryptedMessage = "";

                    try
                    {
                        // Odszyfrowanie wiadomości (content to zaszyfrowana baza64)
                        decryptedMessage = DecryptMessage(msg.Content, privKey);
                    }
                    catch (Exception ex)
                    {
                        decryptedMessage = $"[Błąd odszyfrowania]: {ex.Message}";
                    }

                    string senderName = msg.SenderId == user1_id ? UserSession.Current.Fullname : CurrentChat.FullnameTo;
                    string displayText = $"{senderName} ({msg.SentAt.ToLocalTime()}): {decryptedMessage}\n";

                    richTextBox1.AppendText(displayText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd pobierania wiadomości: {ex.Message}");
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            //Funkcja wysyłania wiadomości
        }

        public static string DecryptMessage(string encryptedBase64, string privateKeyBase64)
        {
            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyBase64);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public async Task SendEncryptedMessageAsync(int senderId, int recipientId, string token)
        {
            string messageText = textBox1.Text;

            // 1. Pobierz publiczny klucz odbiorcy
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var publicKeyRequest = new PublicKeyRequest { user_id = recipientId };
            string publicKeyRequestJson = JsonSerializer.Serialize(publicKeyRequest);
            var publicKeyContent = new StringContent(publicKeyRequestJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("http://localhost:8001/get_public_key", publicKeyContent);
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Błąd pobierania klucza publicznego: {response.StatusCode}");
                return;
            }

            string responseString = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseString);
            string publicKeyBase64 = jsonDoc.RootElement.GetProperty("public_key").GetString();

            // 2. Szyfruj wiadomość kluczem publicznym
            string encryptedMessage = EncryptMessageWithPublicKey(messageText, publicKeyBase64);

            // 3. Wyślij zaszyfrowaną wiadomość
            var sendRequest = new SendMessageRequest
            {
                id_od = senderId,
                id_do = recipientId,
                content = encryptedMessage
            };

            string sendRequestJson = JsonSerializer.Serialize(sendRequest);
            var sendContent = new StringContent(sendRequestJson, Encoding.UTF8, "application/json");

            HttpResponseMessage sendResponse = await httpClient.PostAsync("http://localhost:8001/send_message", sendContent);
            if (sendResponse.IsSuccessStatusCode)
            {
                MessageBox.Show("Wiadomość wysłana pomyślnie.");
            }
            else
            {
                MessageBox.Show($"Błąd wysyłania wiadomości: {sendResponse.StatusCode}");
            }
        }

        private string EncryptMessageWithPublicKey(string message, string publicKeyBase64)
        {
            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyBase64);
            using RSA rsa = RSA.Create();

            // Import klucza publicznego w formacie PKCS#8 (SubjectPublicKeyInfo)
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] encryptedBytes = rsa.Encrypt(messageBytes, RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(encryptedBytes);
        }

    }
}
