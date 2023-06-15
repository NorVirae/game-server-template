using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Ably.Realtime;

namespace Server
{
    public class SystemMessage : Message
    {
        public SystemMessageType messageType;
        public string message;
        
    }

    public class LoginMessage : Message
    {
        public string userId;
        public string playfabId;
    }

    public class ChatMessage : Message
    {
        public string channelID;
        public string clientID;
        public string eventName;
        public object messageBody;
    }
}
