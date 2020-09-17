using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRServer.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private const string DefaultUser = "default";
        private const string DefaultMessage = "hello";
        
        public async Task SendMessage(string user = DefaultMessage, string message = DefaultMessage)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}