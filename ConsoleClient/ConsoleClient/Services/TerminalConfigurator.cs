using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleClient.Classes;

namespace ConsoleClient.Services
{
    public static class TerminalConfigurator
    {
        private static int _currentPositionLeft;
        private static int _currentPositionTop;

        private static string _cornerSymbol = "+";
        private static string _horizontalSymbol = "-";
        private static string _leftVerticalSymbol = "|  ";
        private static string _rightVerticalSymbol = "  |";

        private static List<string> messageHistory;

        static TerminalConfigurator()
        {
            _currentPositionLeft = Console.WindowWidth / 2;
            _currentPositionTop = Console.WindowHeight / 3;

            messageHistory = new List<string>();
        }

        public static void Configure()
        {
            SetConsoleWindowSize(1, 1);
            SetConsoleWindowBufferSize(60, 30);
            SetConsoleWindowSize(60, 30);
            Console.CursorVisible = false;

            _currentPositionLeft = Console.WindowWidth / 2;
            _currentPositionTop = Console.WindowHeight / 3;
        }

        public static UserCredentials GetUserCredentials()
        {
            Console.Clear();
            
            var userNameMessage = "User name:     ";
            var userPasswordMessage = "User password: ";

            var userName = "";
            var password = "";

            const int maxInputSize = 6;

            var oldPositionLeft = _currentPositionLeft;
            var oldPositionTop = _currentPositionTop;

            _currentPositionLeft = 0;
            _currentPositionTop = _currentPositionTop - 1;

            SetNewCursorPosition(_currentPositionLeft, _currentPositionTop);

            var authorizationWindowWidth = _leftVerticalSymbol.Length  + userNameMessage.Length + maxInputSize +
                                           _rightVerticalSymbol.Length;
            var leftCornerStart = (Console.WindowWidth -  authorizationWindowWidth) / 2 + 1;
            var rightCornerStart = (Console.WindowWidth + authorizationWindowWidth) / 2;
            WriteLineWithCorners(leftCornerStart, rightCornerStart);

            _currentPositionLeft = oldPositionLeft;
            _currentPositionTop = oldPositionTop;

            var leftOffset = oldPositionLeft - userNameMessage.Length / 2 - _leftVerticalSymbol.Length - 3;
            SetNewCursorPosition(leftOffset, _currentPositionTop);
            Console.Write(_leftVerticalSymbol);
            Console.Write(userNameMessage);
            WriteSymbols(" ", maxInputSize); //var userName = Console.ReadLine();
            Console.Write(_rightVerticalSymbol); //
            SetNewCursorPosition(leftOffset, ++_currentPositionTop);
            Console.Write(_leftVerticalSymbol);
            WriteSymbols(" ", userNameMessage.Length + maxInputSize);
            Console.Write(_rightVerticalSymbol); //
            SetNewCursorPosition(leftOffset, ++_currentPositionTop);
            Console.Write(_leftVerticalSymbol);
            Console.Write(userPasswordMessage);
            WriteSymbols(" ", maxInputSize); //var password = Console.ReadLine();
            Console.Write(_rightVerticalSymbol); //

            SetNewCursorPosition(0, ++_currentPositionTop);
            WriteLineWithCorners(leftCornerStart, rightCornerStart);

            _currentPositionLeft = oldPositionLeft;
            _currentPositionTop = oldPositionTop;
            
            userName = Console.ReadLine();
            password = Console.ReadLine();
            return new UserCredentials
            {
                UserName = userName,
                Password = password
            };
        }

        public static string GetUserMessage()
        {
            Console.Clear();

            _currentPositionTop = 0;
            _currentPositionLeft = 0;
            SetNewCursorPosition(_currentPositionLeft,_currentPositionTop);

            foreach (var msg in messageHistory)
            {
                Console.Write(msg);
                SetNewCursorPosition(_currentPositionLeft, ++_currentPositionTop);
            }
            
            
            SetNewCursorPosition(0, Console.WindowHeight - 5);
            WriteLineWithCorners();
            SetNewCursorPosition(0, Console.WindowHeight - 4);
            Console.Write("Enter the message: ");
            var message = Console.ReadLine();

            return message;
        }
        

        public static void AddMessageToStorage(string userName, string message)
        {
            messageHistory.Add($"{userName} : {message}");
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

        private static void WritAuthorizationUpperLine()
        {
        }

        private static void WriteLineWithCorners(params int[] cornerPoints)
        {
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(cornerPoints.Contains(i) ? _cornerSymbol : _horizontalSymbol);
        }

        private static void WriteSymbols(string symbol, int count)
        {
            Console.Write(String.Concat(Enumerable.Repeat(symbol, count)));
        }
    }
}