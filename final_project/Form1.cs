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
using Newtonsoft.Json.Linq;
using System.IO;

namespace final_project
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private JArray users = new JArray();

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLoadUsers_Click(object sender, EventArgs e)
        {
            string url = "https://jsonplaceholder.typicode.com/users";
            var response = await client.GetStringAsync(url);
            users = JArray.Parse(response);

            listBoxUsers.Items.Clear();
            foreach (var user in users)
            {
                listBoxUsers.Items.Add(user["name"]);
            }
        }

        private void listBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex == -1 || users == null) return;

            var selectedUser = users[listBoxUsers.SelectedIndex];

            textBoxDetails.Text = $"Name: {selectedUser["name"]}\n" +
                                  $"Username: {selectedUser["username"]}\n" +
                                  $"Email: {selectedUser["email"]}\n" +
                                  $"Phone: {selectedUser["phone"]}";
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            var newUser = new JObject
            {
                { "name", textBoxName.Text },
                { "username", textBoxUsername.Text },
                { "email", textBoxEmail.Text },
                { "phone", textBoxPhone.Text }
            };

            users.Add(newUser);
            listBoxUsers.Items.Add(newUser["name"]);
            ClearInputFields();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex == -1) return;

            users.RemoveAt(listBoxUsers.SelectedIndex);
            listBoxUsers.Items.RemoveAt(listBoxUsers.SelectedIndex);
            textBoxDetails.Clear();
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            var filePath = "users.json";
            File.WriteAllText(filePath, users.ToString());
            MessageBox.Show($"Users saved to {filePath}");
        }

        private void ClearInputFields()
        {
            textBoxName.Clear();
            textBoxUsername.Clear();
            textBoxEmail.Clear();
            textBoxPhone.Clear();
        }
    }
}
