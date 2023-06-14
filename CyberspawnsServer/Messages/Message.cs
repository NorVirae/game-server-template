using CyberspawnsServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnsServer
{
    public abstract class Message
    {
        public string ToJSon()
        {
            return SerializationHelper.Serialize(this);
        }
    }
}
