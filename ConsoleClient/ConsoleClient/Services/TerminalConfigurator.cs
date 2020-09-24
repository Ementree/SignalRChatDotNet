using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleClient.Classes;
using SignalRServer.Models;

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
        
        

        private static readonly List<string> MessageHistory;
        private static readonly StringBuilder MessageStringBuilder;
        private static readonly int UpperLineOffset;
        private static readonly int MessageWindowOffset;
        private static readonly string EnterMessage;

        static TerminalConfigurator()
        {
            _currentPositionLeft = Console.WindowWidth / 2;
            _currentPositionTop = Console.WindowHeight / 3;

            MessageHistory = new List<string>();
            MessageStringBuilder = new StringBuilder();

            UpperLineOffset = Console.WindowHeight - 5;
            MessageWindowOffset = Console.WindowHeight - 4;

            EnterMessage = "Enter the message: ";
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

            var authorizationWindowWidth = _leftVerticalSymbol.Length + userNameMessage.Length + maxInputSize +
                                           _rightVerticalSymbol.Length;
            var leftCornerStart = (Console.WindowWidth - authorizationWindowWidth) / 2 + 1;
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

        public static async Task<string> GetUserMessage()
        {
            var pressedKeyInfo = new ConsoleKeyInfo();

            while (pressedKeyInfo.Key != ConsoleKey.Enter)
            {
                //DisplayUpdate();
                
                Console.Clear();
                ShowAllMessages();
                SetNewCursorPosition(0, UpperLineOffset);
                WriteLineWithCorners();

                SetNewCursorPosition(0, MessageWindowOffset);
                Console.Write(EnterMessage);

                for (int i = 0; i < MessageStringBuilder.Length; i++)
                {
                    Console.Write(MessageStringBuilder[i]);
                }

                pressedKeyInfo = Console.ReadKey();

                var keyChar = pressedKeyInfo.KeyChar;


                if (IsSymbol(keyChar))
                    MessageStringBuilder.Append(keyChar);
                if (pressedKeyInfo.Key == ConsoleKey.Backspace && MessageStringBuilder.Length != 0)
                    MessageStringBuilder.Remove(MessageStringBuilder.Length - 1, 1);
            }

            var message = MessageStringBuilder.ToString();
            MessageStringBuilder.Clear();

            return message;
        }

        private static void DisplayUpdate()
        {
            Console.Clear();
            ShowAllMessages();
            SetNewCursorPosition(0, UpperLineOffset);
            WriteLineWithCorners();

            SetNewCursorPosition(0, MessageWindowOffset);
            Console.Write(EnterMessage);

            for (int i = 0; i < MessageStringBuilder.Length; i++)
            {
                Console.Write(MessageStringBuilder[i]);
            }
        }

        private static void ShowAllMessages()
        {
            _currentPositionTop = 0;
            _currentPositionLeft = 0;
            SetNewCursorPosition(_currentPositionLeft, _currentPositionTop);

            foreach (var msg in MessageHistory)
            {
                Console.Write(msg);
                SetNewCursorPosition(_currentPositionLeft, ++_currentPositionTop);
            }
        }

        public static void Notify(UserMessage userMessage)
        {
            MessageHistory.Add($"{userMessage.UserName} : {userMessage.Text}");
            //DisplayUpdate();
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
            Console.SetBufferSize(newWidth, 1000);
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

        private static bool IsSymbol(char keyChar)
        {
            var acceptedSymbols = new char[] {'+', '=', '|', '/', '`', '~', '$', '^'};

            return char.IsLetterOrDigit(keyChar) || char.IsPunctuation(keyChar) || char.IsWhiteSpace(keyChar) ||
                   acceptedSymbols.Contains(keyChar);
        }
    }
}