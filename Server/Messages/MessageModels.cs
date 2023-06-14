using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
