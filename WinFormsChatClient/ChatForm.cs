using ConsoleClient.Services;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsChatClient
{
    public partial class ChatForm : Form
    {
        private HubConnection HubConnection;

        //private static readonly List<string> MessageHistory;
        private static readonly ObservableCollection<string> MessageHistory;

        static ChatForm()
        {
            MessageHistory = new ObservableCollection<string>();
        }

        public ChatForm()
        {
            HubConnection = ChatHubConnectionBuilder.Build();

            var task = HubConnection.StartAsync();

            Task.WhenAll(task);

            HubConnection.On<UserMessage>("ReceiveMessage", UpdateMessageHistory);

            InitializeComponent();
        }

        private void UpdateMessageHistory(UserMessage userMessage)
        {
            MessageHistory.Add($"{userMessage.UserName} : {userMessage.Text}");
            MessageHistory.CollectionChanged += (sender, args) =>
            {
                ShowMessages();
            };
        }

        private void ShowMessages()
        {
            ChatBox.Clear();

            StringBuilder messages = new StringBuilder();

            foreach (var message in MessageHistory)
            {
                messages.Append(message + "\n");
            }

            ChatBox.Text = messages.ToString();
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_ClickAsync(object sender, EventArgs e)
        {
            var name = NameBox.Text;
            var message = MessageBox.Text;
            MessageBox.Text = string.Empty;

            var task = HubConnection.InvokeAsync("SendMessage", new UserMessage {UserName = name, Text = message});

            Task.WhenAll(task);
        }

        private void ChatBox_TextChanged(object sender, EventArgs e)
        {
        }
    }
}