using System;
using ConsoleClient.Clients;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRServer.Models;

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
            hubConnection.On<UserMessage>("ReceiveMessage", TerminalChatClient.OnReceiveMessage);
        }

        private static void AddOnAuthorize(HubConnection hubConnection)
        {
            hubConnection.On<string, bool>("Authorize",
                TerminalChatClient.OnAuthorize);
        }
    }
}