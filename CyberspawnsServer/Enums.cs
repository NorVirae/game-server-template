using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnsServer
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
        Disconnection
    }

    public enum SystemMessageType
    {
        Info,
        Warn,
        Error
    }
}
