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
using System.Text.Json.Serialization;
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
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("conversation_id")]
            public int ConversationId { get; set; }

            [JsonPropertyName("sender_id")]
            public int SenderId { get; set; }

            [JsonPropertyName("content")]
            public string Content { get; set; }

            [JsonPropertyName("sent_at")]
            public DateTime SentAt { get; set; }

            [JsonPropertyName("content_sender_encrypted")]
            public string ContentSenderEncrypted { get; set; }
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
            public string content_sender_encrypted { get; set; }
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


                //richTextBox1.AppendText($"{cuser.Fullname}  {CurrentChat.FullnameTo}");
                var userNames = new Dictionary<int, string>
                {
                    { user1_id, cuser.Fullname },  // np. 1 -> "Adam Mazurek"
                    { user2_id, CurrentChat.FullnameTo }         // np. 2 -> "Kuba Mazurek"
                };

                foreach (var msg in messages)
                {
                    string decryptedMessage = "";

                    try
                    {
                        //richTextBox1.AppendText($"DEBUG: SenderId={msg.SenderId}, SentAt={msg.SentAt}, Content={msg.Content}\n");
                        //richTextBox1.AppendText($"[DEBUG] Content: {msg.Content}\n");
                        // Odszyfrowanie wiadomości (content to zaszyfrowana baza64)
                        decryptedMessage = DecryptMessage(msg.Content, privKey);

                    }
                    catch (Exception ex)
                    {
                        decryptedMessage = $"[Błąd odszyfrowania]: {ex.Message}";
                    }

                    string senderName = userNames.ContainsKey(msg.SenderId) ? userNames[msg.SenderId] : "Nieznany nadawca";
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

        private async void button1_Click(object sender, EventArgs e)
        {
            string token = ctoken.token;


            SendEncryptedMessageAsync(CurrentChat.UserIdFrom, CurrentChat.UserIdTo, token);

            textBox1.Clear();

            await LoadAndDisplayMessagesAsync();
        }

        public static string DecryptMessage(string encryptedBase64, string privateKeyPem)
        {
            // Usuń nagłówki PEM
            string pemHeader = "-----BEGIN RSA PRIVATE KEY-----";
            string pemFooter = "-----END RSA PRIVATE KEY-----";

            string base64Key = privateKeyPem
                .Replace(pemHeader, "")
                .Replace(pemFooter, "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            byte[] privateKeyBytes = Convert.FromBase64String(base64Key);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public async Task SendEncryptedMessageAsync(int senderId, int recipientId, string token)
        {
            string messageText = textBox1.Text;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Funkcja pomocnicza do pobierania klucza publicznego
            async Task<string> GetPublicKeyAsync(int userId)
            {
                var publicKeyRequest = new PublicKeyRequest { user_id = userId };
                string requestJson = JsonSerializer.Serialize(publicKeyRequest);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("http://localhost:8001/get_public_key", content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Błąd pobierania klucza publicznego użytkownika {userId}: {response.StatusCode}");
                }

                string responseString = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseString);
                return jsonDoc.RootElement.GetProperty("public_key").GetString();
            }

            try
            {
                // 1. Pobierz klucz publiczny odbiorcy
                string RPK = await GetPublicKeyAsync(recipientId);
                string recipientPublicKey = ExtractBase64FromPEM(RPK);

                // 2. Pobierz klucz publiczny nadawcy
                string SPK = await GetPublicKeyAsync(senderId);
                string senderPublicKey = ExtractBase64FromPEM(SPK);

                // 3. Zaszyfruj wiadomość dla odbiorcy
                string encryptedForRecipient = EncryptMessageWithPublicKey(messageText, recipientPublicKey);

                // 4. Zaszyfruj wiadomość dla nadawcy
                string encryptedForSender = EncryptMessageWithPublicKey(messageText, senderPublicKey);

                // 5. Przygotuj i wyślij wiadomość
                var sendRequest = new SendMessageRequest
                {
                    id_od = senderId,
                    id_do = recipientId,
                    content = encryptedForRecipient,
                    content_sender_encrypted = encryptedForSender
                };

                string sendRequestJson = JsonSerializer.Serialize(sendRequest);
                var sendContent = new StringContent(sendRequestJson, Encoding.UTF8, "application/json");

                HttpResponseMessage sendResponse = await httpClient.PostAsync("http://localhost:8001/send_message", sendContent);
                if (sendResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("Wiadomość wysłana pomyślnie.");
                    richTextBox1.Clear();
                    LoadAndDisplayMessagesAsync();
                }
                else
                {
                    MessageBox.Show($"Błąd wysyłania wiadomości: {sendResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
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

        public static RSA LoadPublicKeyFromPEM(string pem)
        {
            string header = "-----BEGIN PUBLIC KEY-----";
            string footer = "-----END PUBLIC KEY-----";

            // Usuń nagłówki, końcówki i nowe linie
            string base64 = pem
                .Replace(header, "")
                .Replace(footer, "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            byte[] keyBytes = Convert.FromBase64String(base64);

            RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(new ReadOnlySpan<byte>(keyBytes), out _);
            return rsa;
        }

        public static string ExtractBase64FromPEM(string pem)
        {
            string header = "-----BEGIN PUBLIC KEY-----";
            string footer = "-----END PUBLIC KEY-----";

            string base64 = pem
                .Replace(header, "")
                .Replace(footer, "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            return base64;
        }

    }
}
