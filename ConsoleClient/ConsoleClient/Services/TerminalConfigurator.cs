using System;

namespace ConsoleClient.Services
{
    public class TerminalConfigurator
    {
        public static void Configure()
        {
            Console.SetCursorPosition(Console.WindowWidth/2, Console.WindowHeight /2);
        }
    }
}