using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Projekt_zaliczeniowy_.NET;

namespace Projekt_zaliczeniowy_.NET
{
    public partial class Form6 : Form
    {

        public Chat CurrentChat { get; set; }
        public CurrentUser cuser { get; set; }

        public Form6()
        {
            InitializeComponent();
            this.Load += Form6_Load;
        }

        private void Form6_Load(object sender, EventArgs e)
        {

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
            }
            else
            {
                MessageBox.Show("Brak danych czatu.");
                this.Close();
            }
        }
    }
}
