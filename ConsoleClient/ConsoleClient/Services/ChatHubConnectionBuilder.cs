using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient.Services
{
    public static class ChatHubConnectionBuilder
    {
        private const string ServerUrl = @"http://localhost:51878/chathub";

        public static HubConnection Build()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(ServerUrl)
                .Build();

            return connection;
        }
    }
}