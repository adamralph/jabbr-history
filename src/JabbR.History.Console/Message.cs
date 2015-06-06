// <copyright file="Message.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. (adam@adamralph.com)
// </copyright>

namespace JabbR.History.Console
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{When} {UserName}: {Content}")]
    public class Message
    {
        public string Content { get; set; }

        public string UserName { get; set; }

        public DateTime When { get; set; }

        public bool HtmlEncoded { get; set; }
    }
}
