using System;
using Microsoft.AspNet.SignalR.Client;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var hubConnection = new HubConnection("http://localhost:5000");
            var chat = hubConnection.CreateHubProxy("ChatHub");
            chat.On<string, string>("broadcastMessage", (name, message) => 
                { Console.Write(name + ": "); Console.WriteLine(message); });
            hubConnection.Start().Wait();
            string msg = null;

            while ((msg = Console.ReadLine()) != null)
            {
                chat.Invoke("Send", "Console app", msg).Wait();
            }
        }
    }
}