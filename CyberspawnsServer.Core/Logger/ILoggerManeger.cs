using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CyberspawnsServer.Core
{
    public abstract class LoggerManeger
    {
        public abstract void LogInfo(object data);
        public abstract void LogError(object data);
        public abstract void LogWarn(object data);
    }
}
