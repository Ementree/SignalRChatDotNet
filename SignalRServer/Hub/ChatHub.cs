using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using SignalRServer.Models;

namespace SignalRServer.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        // Тока не ржите...

        #region Authorization
        private static string _passwordHash = "pswd";
        private static HashSet<string> _authorizations;
        #endregion

        public ChatHub()
        {
            _authorizations = new HashSet<string>();
        }

        public async Task Authorize(string connectionId, string userName, string passwordHash)
        {
            string answer;
            if (_passwordHash != passwordHash)
            {
                answer = "Wrong password!";

                await Clients.Caller.SendAsync("Authorize", answer, false);
            }
            else
            {
                answer = $"Welcome {userName}!";

                _authorizations.Add(connectionId);
                await Clients.Caller.SendAsync("Authorize", answer, true);
            }
        }

        public async Task SendMessage(UserMessage message)
        {
            if (message.Text.Length > 100 || message.Text.Length < 1 || 
                message.UserName.Length > 20 || message.UserName.Length < 1)
                await Clients.Caller.SendAsync("ReceiveMessage",
                    new UserMessage {Text = "Чел ты", UserName = "System: "});
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task DenySendingMessage(string userName)
        {
            await Clients.Caller.SendAsync("DenySendingMessage", $"Hey {userName}, you are not authorized!");
        }
    }
}