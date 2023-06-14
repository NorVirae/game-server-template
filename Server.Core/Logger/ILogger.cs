using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core
{
    public interface ILogger
    {
        string Name { get; }
        string Path { get; set; }
        object sync { get; set; }
        string Write(object log);
    }
}
