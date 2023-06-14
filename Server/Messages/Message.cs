using Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public abstract class Message
    {
        public string ToJSon()
        {
            return SerializationHelper.Serialize(this);
        }
    }
}
