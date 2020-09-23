using System;
using ConsoleClient.Classes;

namespace ConsoleClient.Services
{
    public static class TerminalConfigurator
    {
        private static int _currentPositionLeft;
        private static int _currentPositionTop;

        private static string _cornerSymbol = "+";
        private static string _horyzontalSymbol = "-";
        private static string _leftVerticalSymbol = "|  ";
        private static string _rightVerticalSymbol = "  |";
         
        static TerminalConfigurator()
        {
            _currentPositionLeft = Console.WindowWidth / 2;
            _currentPositionTop = Console.WindowHeight / 3;
        }

        public static void Configure()
        {
            SetConsoleWindowSize(1, 1);
            SetConsoleWindowBufferSize(60, 80);
            SetConsoleWindowSize(60, 30);
            Console.CursorVisible = false;
        }

        public static UserCredentials GetUserCredentials()
        {
            var userNameMessage = "User name:     ";
            var userPasswordMessage = "User password: ";

            var leftOffset = _currentPositionLeft - userNameMessage.Length/2;
            SetNewCursorPosition(leftOffset, _currentPositionTop );
            Console.Write(userNameMessage);
            var userName = Console.ReadLine();
            SetNewCursorPosition(leftOffset, ++_currentPositionTop);
            Console.Write(userPasswordMessage);
            var password = Console.ReadLine();

            return new UserCredentials
            {
                UserName = userName,
                Password = password
            };
        }

        private static void SetNewCursorPosition(int newPositionLeft, int newPositionTop)
        {
            Console.SetCursorPosition(newPositionLeft, newPositionTop);
        }

        private static void SetConsoleWindowSize(int newWidth, int newHeight)
        {
            Console.SetWindowSize(newWidth, newHeight);
        }

        private static void SetConsoleWindowBufferSize(int newWidth, int newHeight)
        {
            Console.SetBufferSize(newWidth, newHeight);
        }
    }
}