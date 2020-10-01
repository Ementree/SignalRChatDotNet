using System.Threading.Tasks;
using ConsoleClient.Clients;

namespace ConsoleClient
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            await TerminalChatClient.RunChat();
        }
    }
}