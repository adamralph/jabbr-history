// <copyright file="MessageExtensions.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. (adam@adamralph.com)
// </copyright>

namespace JabbR.History.Console
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using ColoredConsole;

    internal static class MessageExtensions
    {
        public static void WriteDark(this Message message, int userNameWidth)
        {
            message.Write(
                userNameWidth,
                ConsoleColor.DarkCyan,
                ConsoleColor.DarkYellow,
                ConsoleColor.DarkGreen,
                ConsoleColor.DarkGray);
        }

        public static void Write(this Message message, int userNameWidth)
        {
            message.Write(
                userNameWidth,
                ConsoleColor.Cyan,
                ConsoleColor.Yellow,
                ConsoleColor.Green,
                ConsoleColor.White);
        }

        private static void Write(
            this Message message,
            int userNameWidth,
            ConsoleColor whenColor,
            ConsoleColor userNameColor,
            ConsoleColor delimiterColor,
            ConsoleColor contentColor)
        {
            var prefix = new[]
            {
                message.When.ToString("u", CultureInfo.CurrentCulture).Color(whenColor),
                " ",
                message.UserName.PadRight(userNameWidth).Color(userNameColor),
                ": ".Color(delimiterColor),
            };

            var indentWidth = prefix.Sum(token => token.Text.Length);
            var contentWidth = Console.BufferWidth - indentWidth - 1;
            var contentLines = new List<string>();
            for (var index = 0; index < message.Content.Length; index += contentWidth)
            {
                contentLines.Add(message.Content.Substring(index, Math.Min(contentWidth, message.Content.Length - index)));
            }

            ColorConsole.WriteLine(prefix.Concat(new[] { contentLines.FirstOrDefault().Color(contentColor) }).ToArray());
            foreach (var contentLine in contentLines.Skip(1))
            {
                ColorConsole.WriteLine(string.Empty.PadRight(indentWidth), contentLine.Color(contentColor));
            }
        }
    }
}
