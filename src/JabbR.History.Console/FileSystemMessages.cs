// <copyright file="FileSystemMessages.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. (adam@adamralph.com)
// </copyright>

namespace JabbR.History.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    public class FileSystemMessages : IGetMessages
    {
        private readonly string path;

        public FileSystemMessages(string path)
        {
            this.path = path;
        }

        public IEnumerable<Message> Get()
        {
            using (var file = File.OpenText(this.path))
            using (var reader = new JsonTextReader(file))
            {
                while (reader.Read())
                {
                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                    {
                        var message = new Message();
                        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                        {
                            switch ((string)reader.Value)
                            {
                                case "content":
                                    reader.ReadAsString();
                                    message.Content = (string)reader.Value;
                                    break;
                                case "username":
                                    reader.ReadAsString();
                                    message.UserName = (string)reader.Value;
                                    break;
                                case "when":
                                    reader.ReadAsDateTime();
                                    message.When = (DateTime)reader.Value;
                                    break;
                                case "htmlEncoded":
                                    reader.ReadAsString();
                                    message.HtmlEncoded =
                                        string.Equals((string)reader.Value, "TRUE", StringComparison.OrdinalIgnoreCase);
                                    break;
                                default:
                                    reader.Read();
                                    break;
                            }
                        }

                        yield return message;
                    }
                }
            }
        }
    }
}
