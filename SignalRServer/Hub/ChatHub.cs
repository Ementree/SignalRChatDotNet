using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

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
                answer = $"Welcome, {userName}!";

                _authorizations.Add(connectionId);
                await Clients.Caller.SendAsync("Authorize", answer, true);
            }
        }

        public async Task SendMessage(string connectionId, string userName, string message)
        {
            if (!_authorizations.Contains(connectionId))
            {
                await DenySendingMessage(userName);
                return;
            }

            await Clients.All.SendAsync("SendMessage", connectionId, userName, message);
        }

        private async Task DenySendingMessage(string userName)
        {
            await Clients.Caller.SendAsync("DenySendingMessage", $"Hey {userName}, you are not authorized!");
        }
    }
}