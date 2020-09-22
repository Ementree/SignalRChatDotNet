using System;
using System.Threading.Tasks;
using ConsoleClient.Extensions;
using ConsoleClient.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient
{
    class Program
    {
        static HubConnection InitConnection()
        {
            return ChatHubConnectionBuilder
                .Build()
                .AddOnReceiveMethods();
        }

        static async Task Main(string[] args)
        {
            var connection = InitConnection();
            TerminalConfigurator.Configure();

            await connection.StartAsync();

            await connection.InvokeAsync("Authorize", connection.ConnectionId, "console", "pswd");

            while (true)
            {
                var msg = Console.ReadLine();
                await connection.InvokeAsync("SendMessage", connection.ConnectionId, "console", msg);
            }
        }
    }
}