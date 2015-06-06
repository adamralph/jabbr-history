// <copyright file="Program.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. (adam@adamralph.com)
// </copyright>

namespace JabbR.History.Console
{
    using System;
    using ConfigR;

    internal static class Program
    {
        public static void Main()
        {
            var messages = new FileSystemMessages(Config.Global.Get<string>("FileName"));
            var searcher = new MessageSearcher(messages);
            
            searcher.Search(
                Config.Global.Get<Func<Message, bool>>("Match"),
                Config.Global.Get<int>("NumberOfMessagesToShowBeforeAndAfter"));

            Console.WriteLine("Tickle a key to exit.");
            Console.ReadKey();
        }
    }
}
