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

        private static bool _isAuthorized = false;
        private static string _userName;
        
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


            while (!_isAuthorized)
                await ConsoleAuthorize();
            
            

            while (true)
            {
                await ConsoleSendMessage();
            }
        }

        private static async Task ConsoleAuthorize()
        {
            var userCredentials = TerminalConfigurator.GetUserCredentials();
            _userName = userCredentials.UserName;
            await ServerAuthorize(userCredentials.UserName, userCredentials.Password);
        }

        private static async Task ServerAuthorize(string userName, string password)
        {
            await HubConnection.InvokeAsync("Authorize", HubConnection.ConnectionId, userName, password);
        }

        private static async Task ServerSendMessage(string userName, string message)
        {
            await HubConnection.InvokeAsync("SendMessage", HubConnection.ConnectionId, userName, message);
        }

        private static async Task ConsoleSendMessage()
        {
            var userMessage = TerminalConfigurator.GetUserMessage();

            await ServerSendMessage(_userName, userMessage);
        }

        public static void OnAuthorize(string answer, bool isSuccess)
        {
            _isAuthorized = isSuccess;
            Console.Write(answer);
        }

        public static void OnSendMessage(string connectionId, string userName, string message)
        {
            Console.WriteLine("----------");
            
            TerminalConfigurator.AddMessageToStorage(connectionId,userName, message);
        }
    }
}