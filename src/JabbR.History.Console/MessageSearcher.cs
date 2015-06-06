// <copyright file="MessageSearcher.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. (adam@adamralph.com)
// </copyright>

namespace JabbR.History.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using ColoredConsole;
    using Humanizer;

    public class MessageSearcher
    {
        private readonly IGetMessages messages;

        public MessageSearcher(IGetMessages messages)
        {
            Guard.AgainstNullArgument("messages", messages);

            this.messages = messages;
        }

        public void Search(Func<Message, bool> match, int numberOfMessagesToShowBeforeAndAfter)
        {
            Guard.AgainstNullArgument("match", match);

            var spinner = new Spinner(
                8,
                "Searching JabbR history ({0:N0} messages)",
                "Searching JabbR history ({0:N0} messages).",
                "Searching JabbR history ({0:N0} messages) o",
                "Searching JabbR history ({0:N0} messages)  O",
                "Searching JabbR history ({0:N0} messages)   @",
                "Searching JabbR history ({0:N0} messages)    *");

            var context = new Queue<Message>();
            var maxUsernameLength = 0;
            var writeSeparator = false;
            var before = true;
            var messageCount = 0;
            var matchCount = 0;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Message firstMessage = null;
            Message lastMessage = null;
            Message firstMatch = null;
            Message lastMatch = null;
            foreach (var candidate in this.messages.Get())
            {
                firstMessage = firstMessage ?? candidate;
                lastMessage = candidate;
                spinner.Spin(messageCount++);

                var matches = match(candidate);
                if (before)
                {
                    if (matches)
                    {
                        spinner.Hide();
                        firstMatch = firstMatch ?? candidate;
                        lastMatch = candidate;
                        ++matchCount;
                        maxUsernameLength = Math.Max(
                            maxUsernameLength,
                            Math.Max(context.Any() ? context.Max(msg => msg.UserName.Length) : 0, candidate.UserName.Length));

                        if (writeSeparator)
                        {
                            Console.WriteLine();
                            ColorConsole.WriteLine("======".DarkGreen());
                            Console.WriteLine();
                            writeSeparator = false;
                        }

                        foreach (var message in context)
                        {
                            message.WriteDark(maxUsernameLength);
                        }

                        context.Clear();
                        candidate.Write(maxUsernameLength);
                        before = false;
                    }
                    else
                    {
                        if (context.Count == numberOfMessagesToShowBeforeAndAfter)
                        {
                            context.Dequeue();
                            writeSeparator = true;
                        }

                        context.Enqueue(candidate);
                    }
                }
                else
                {
                    if (matches)
                    {
                        spinner.Hide();
                        firstMatch = firstMatch ?? candidate;
                        lastMatch = candidate;
                        ++matchCount;
                        maxUsernameLength = Math.Max(
                            maxUsernameLength,
                            Math.Max(context.Any() ? context.Max(msg => msg.UserName.Length) : 0, candidate.UserName.Length));

                        foreach (var message in context)
                        {
                            message.WriteDark(maxUsernameLength);
                        }

                        context.Clear();
                        candidate.Write(maxUsernameLength);
                    }
                    else if (context.Count == numberOfMessagesToShowBeforeAndAfter)
                    {
                        spinner.Hide();
                        maxUsernameLength = Math.Max(maxUsernameLength, context.Max(msg => msg.UserName.Length));
                        foreach (var message in context)
                        {
                            message.WriteDark(maxUsernameLength);
                        }

                        context.Clear();
                        before = true;
                        context.Enqueue(candidate);
                    }
                    else
                    {
                        context.Enqueue(candidate);
                    }
                }
            }

            spinner.Hide();
            if (context.Any() && !before)
            {
                maxUsernameLength = Math.Max(maxUsernameLength, context.Max(msg => msg.UserName.Length));

                foreach (var message in context)
                {
                    message.WriteDark(maxUsernameLength);
                }

                context.Clear();
            }

            Console.WriteLine(
                "{0:N0} results from {1:N0} messages ({2}).",
                matchCount,
                messageCount,
                stopWatch.Elapsed.Humanize());

            if (firstMessage != null)
            {
                Console.WriteLine(
                    "The messages range from {0} to {1}.",
                    firstMessage.When.Humanize(),
                    lastMessage.When.Humanize());
            }

            if (firstMatch != null)
            {
                Console.WriteLine(
                    "The results range from {0} to {1}.",
                    firstMatch.When.Humanize(),
                    lastMatch.When.Humanize());
            }
        }
    }
}
