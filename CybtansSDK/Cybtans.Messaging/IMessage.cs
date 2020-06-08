using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Messaging
{
    public interface IMessage
    {         
        string Exchange { get; }

        string Topic { get; }
    }
}
