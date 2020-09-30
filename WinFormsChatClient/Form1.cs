using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleClient.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace WinFormsChatClient
{
    public partial class Form1 : Form
    {
        private HubConnection _hubConnection;
        
        public Form1()
        {
            _hubConnection = ChatHubConnectionBuilder.Build();
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var message = this.textBox1.Text;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var message = this.textBox1.Text;
        }
    }
}
