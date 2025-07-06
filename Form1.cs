using std;
using System;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

namespace Projekt_zaliczeniowy_.NET
{
    public partial class OknoPowitalne : Form
    {
        public OknoPowitalne()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += OknoPowitalne_Load;
        }

        private async void OknoPowitalne_Load(object sender, EventArgs e)
        {
            label5.Visible = false;
            label4.Visible = false;
            label3.Visible = false;
            label2.Visible = false;

            await Task.Delay(3000);
            label2.Visible = true;
            Cursor.Current = Cursors.WaitCursor;

            for (int i = 0; i < 5; i++)
            {
                bool result = await chceckconnection();

                if (result)
                {
                    label4.Visible = true;
                    await Task.Delay(3000);
                    Form2 form2 = new Form2();
                    form2.Show();
                    this.Hide();
                    return;
                }
                else
                {
                    label3.Visible = true;
                    if (i == 4)
                    {
                        label5.Visible = true;
                        await Task.Delay(3000);
                        this.Close();
                    }
                }
                await Task.Delay(3000);
            }
        }

        public static async Task<bool> chceckconnection()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("http://localhost:8001/");
                    if (!response.IsSuccessStatusCode)
                        return false;

                    string content = await response.Content.ReadAsStringAsync();
                    return content.Trim() == "{\"message\":\"Hello World!\"}";
                }
            }
            catch
            {
                return false;
            }
        }
    }
}