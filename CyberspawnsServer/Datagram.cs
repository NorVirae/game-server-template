using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CyberspawnsServer
{
    public class Datagram
    {
        public EventType type;
        public object key;
        public object body;
        public object clientCallabckId;

        public Datagram(EventType type, object body, object id = null)
        {
            this.type = type;
            this.body = body;
            this.clientCallabckId = id;
        }

        public override string ToString()
        {
            //object data = new { type = this.type, body = this.body };
            string sd = JsonConvert.SerializeObject(this);
            return sd;
        }
    }
}
