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

        private const int MessageBoxUpperLineOffsetCount = 5;
        private const int MessageBoxOffsetCount = 4;
        
        private static int _messageBoxUpperLineOffset;
        private static int _messageBoxOffset;

        private const string CornerSymbol = "+";
        private const string HorizontalSymbol = "-";
        private const string LeftVerticalSymbol = "|  ";
        private const string RightVerticalSymbol = "  |";

        private static readonly List<string> MessageHistory;
        private static readonly StringBuilder MessageStringBuilder;
        private static readonly string EnterMessage;

        static TerminalConfigurator()
        {
            _currentPositionLeft = Console.WindowWidth / 2;
            _currentPositionTop = Console.WindowHeight / 3;

            MessageHistory = new List<string>();
            MessageStringBuilder = new StringBuilder();

            _messageBoxUpperLineOffset = Console.WindowHeight - MessageBoxUpperLineOffsetCount;
            _messageBoxOffset = Console.WindowHeight - MessageBoxOffsetCount;

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

            var authorizationWindowWidth = LeftVerticalSymbol.Length + userNameMessage.Length + maxInputSize +
                                           RightVerticalSymbol.Length;
            var leftCornerStart = (Console.WindowWidth - authorizationWindowWidth) / 2 + 1;
            var rightCornerStart = (Console.WindowWidth + authorizationWindowWidth) / 2;
            WriteLineWithCorners(leftCornerStart, rightCornerStart);

            _currentPositionLeft = oldPositionLeft;
            _currentPositionTop = oldPositionTop;

            var leftOffset = oldPositionLeft - userNameMessage.Length / 2 - LeftVerticalSymbol.Length - 3;
            SetNewCursorPosition(leftOffset, _currentPositionTop);
            Console.Write(LeftVerticalSymbol);
            Console.Write(userNameMessage);
            WriteSymbols(" ", maxInputSize); //var userName = Console.ReadLine();
            Console.Write(RightVerticalSymbol); //
            SetNewCursorPosition(leftOffset, ++_currentPositionTop);
            Console.Write(LeftVerticalSymbol);
            WriteSymbols(" ", userNameMessage.Length + maxInputSize);
            Console.Write(RightVerticalSymbol); //
            SetNewCursorPosition(leftOffset, ++_currentPositionTop);
            Console.Write(LeftVerticalSymbol);
            Console.Write(userPasswordMessage);
            WriteSymbols(" ", maxInputSize); //var password = Console.ReadLine();
            Console.Write(RightVerticalSymbol); //

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
            var pressedKeyInfo = new ConsoleKeyInfo();

            while (pressedKeyInfo.Key != ConsoleKey.Enter)
            {
                DisplayUpdate();
                
                Console.Clear();
                ShowAllMessages();
                UpdateMessageBoxOffset();
                SetNewCursorPosition(0, _messageBoxUpperLineOffset);
                WriteLineWithCorners();
                SetNewCursorPosition(0, _messageBoxOffset);
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
            SetNewCursorPosition(0, _messageBoxUpperLineOffset);
            WriteLineWithCorners();

            SetNewCursorPosition(0, _messageBoxOffset);
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
            DisplayUpdate();
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
                Console.Write(cornerPoints.Contains(i) ? CornerSymbol : HorizontalSymbol);
        }

        private static void WriteSymbols(string symbol, int count)
        {
            Console.Write(String.Concat(Enumerable.Repeat(symbol, count)));
        }

        private static void UpdateMessageBoxOffset()
        {
            _messageBoxUpperLineOffset = Console.WindowHeight - MessageBoxUpperLineOffsetCount;
            _messageBoxOffset = Console.WindowHeight - MessageBoxOffsetCount;
        }

        private static bool IsSymbol(char keyChar)
        {
            var acceptedSymbols = new char[] {'+', '=', '|', '/', '`', '~', '$', '^'};

            return char.IsLetterOrDigit(keyChar) || char.IsPunctuation(keyChar) || char.IsWhiteSpace(keyChar) ||
                   acceptedSymbols.Contains(keyChar);
        }
    }
}