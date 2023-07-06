using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public enum ClientStatus
    {
        Pending,
        Connectd,
        Disconnectd
    }

    public enum EventType
    {
        Connection,
        Ping,
        Pong,
        Login,
        Message,
        Disconnection,
        RPC
    }

    public enum SystemMessageType
    {
        Info,
        Warn,
        Error
    }
}
