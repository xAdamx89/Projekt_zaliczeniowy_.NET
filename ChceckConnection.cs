using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace std
{
    internal static class ConnectionChecker
    {
        public static async Task<bool> IsServerOnlineAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://localhost:8001/");
                    if (!response.IsSuccessStatusCode)
                        return false;

                
                 string content = await response.Content.ReadAsStringAsync();
                 return content.Trim() == "{\"message\":\"Hello World!\"}";

                }
                catch
                {
                    return false;
                }
            }
        }
    }
}