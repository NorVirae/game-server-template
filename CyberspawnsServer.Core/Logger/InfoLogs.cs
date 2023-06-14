using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnsServer.Core
{
    class InfoLogs : ILogger
    {
        public string Name => "Info";
        public string Path { get; set; }
        public object sync { get; set; }

        public string Write(object log)
        {
            return $"[{Name}]::{log}";
        }
    }
}
