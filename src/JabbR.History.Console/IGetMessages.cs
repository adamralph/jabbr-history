// <copyright file="IGetMessages.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. (adam@adamralph.com)
// </copyright>

namespace JabbR.History.Console
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface IGetMessages
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "By design.")]
        IEnumerable<Message> Get();
    }
}
