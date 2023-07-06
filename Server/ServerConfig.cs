using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class ServerConfig
    {
        public string Secret { get; set; }
        public string connectionString { get; set; }

        public ServerConfig()
        {
            
        }
    }
}
