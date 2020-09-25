using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient.Services
{
    public static class ChatHubConnectionBuilder
    {
        private const string localServerUrl = @"http://localhost:51878/chathub";
        private const string herokuServerUrl = @"https://signal-r-ng.herokuapp.com/chathub";

        public static HubConnection Build()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(herokuServerUrl)
                .Build();

            return connection;
        }
    }
}