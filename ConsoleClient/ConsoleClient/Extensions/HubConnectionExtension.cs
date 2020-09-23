using System;
using ConsoleClient.Clients;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient.Extensions
{
    public static class HubConnectionExtension
    {
        public static HubConnection AddOnReceiveMethods(this HubConnection hubConnection)
        {
            AddOnSendMessage(hubConnection);
            AddOnAuthorize(hubConnection);

            return hubConnection;
        }

        private static void AddOnSendMessage(HubConnection hubConnection)
        {
            hubConnection.On<string, string, string>("SendMessage", (connectionId, name, message) =>
            {
                Console.Write(name + ": ");
                Console.WriteLine(message);
            });
        }

        private static void AddOnAuthorize(HubConnection hubConnection)
        {
            hubConnection.On<string, bool>("Authorize",
                TerminalChatClient.OnAuthorize);
        }
    }
}