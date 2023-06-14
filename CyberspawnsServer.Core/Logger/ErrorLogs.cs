using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnsServer.Core
{
    class ErrorLogs : ILogger
    {
        public string Name => "Error";
        public string Path { get; set; }
        public object sync { get; set; }

        public string Write(object log)
        {
            return $"[{Name}]::{log}";
        }
    }
}
