using System;
using System.Threading.Tasks;
using ConsoleClient.Extensions;
using ConsoleClient.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient.Clients
{
    public static class TerminalChatClient
    {
        private static readonly HubConnection HubConnection;
        
        static TerminalChatClient()
        {
            HubConnection = ChatHubConnectionBuilder
                .Build()
                .AddOnReceiveMethods();

            TerminalConfigurator.Configure();
        }


        public static async Task RunChat()
        {
            await HubConnection.StartAsync();

            await ConsoleAuthorize();
        }

        private static async Task ConsoleAuthorize()
        {
            var userCredentials = TerminalConfigurator.GetUserCredentials();

            await ServerAuthorize(userCredentials.UserName, userCredentials.Password);
        }

        private static async Task ServerAuthorize(string userName, string password)
        {
            await HubConnection.InvokeAsync("Authorize", HubConnection.ConnectionId, userName, password);
        }


        public static void OnAuthorize(string answer, bool result)
        {
            Console.Write(answer);
        }
    }
}