using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CyberspawnsServer.Core
{
    //master class for handling server logs event
    public class Logger : Singliton<Logger>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="path">The path for saving logs.</param>
        public Logger(string path)
        {
            ToConsole = true;
            RootDir = path;
            loggers = new Dictionary<Type, ILogger>();
            Instance = this;
            RegisterLogger<InfoLogs>();
            RegisterLogger<ErrorLogs>();
            RegisterLogger<Warnlogs>();
        }

        public Logger()
        {
            ToConsole = true;
            RootDir = "";
            loggers = new Dictionary<Type, ILogger>();
            Instance = this;
            RegisterLogger<InfoLogs>();
            RegisterLogger<ErrorLogs>();
            RegisterLogger<Warnlogs>();
        }

        public static void LogInfo(object dataToLog, [CallerMemberName] string memberName = "",[CallerFilePath] string fileName = "",[CallerLineNumber] int lineNumber = 0)
        {
            ILogger logger = Instance.GetLogger<InfoLogs>();
            Instance.Log(dataToLog, logger, ConsoleColor.White, memberName, fileName, lineNumber);
        }

        public static void LogInfo(string format, params object[] obj)
        {
            object message = String.Format(format, obj);
            LogInfo((object)message);
        }

        public static void LogWarn(object dataToLog, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            ILogger logger = Instance.GetLogger<Warnlogs>();
            Instance.Log(dataToLog, logger, ConsoleColor.Yellow, memberName, fileName, lineNumber);
        }

        public static void LogWarn(string format, params object[] obj)
        {
            object message = String.Format(format, obj);
            LogWarn((object)message);
        }

        public static void LogError(object dataToLog, [CallerMemberName] string memberName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            ILogger logger = Instance.GetLogger<ErrorLogs>();
            Instance.Log(dataToLog, logger, ConsoleColor.Red, memberName, fileName, lineNumber);
        }

        public static void LogError(string format, params object[] obj)
        {
            object message = String.Format(format, obj);
            LogError((object)message);
        }

        public static void Log(string format, params object[] obj)
        {
            LogInfo(format, obj);
        }

        private void Log(object data, ILogger logger, ConsoleColor color, string memberName, string fileName, int lineNumber)
        {
            string message = $"[{fileName}:{memberName}:{lineNumber}] {data}";
            string log = logger.Write(message);

            lock (logger.sync)
            {
                if (ToConsole)
                    LogToConsole(log, color);

                if (RootDir != null && RootDir != "")
                {
                    var file = new FileStream(logger.Path, FileMode.Append);
                    using (var stream = new StreamWriter(file))
                        stream.WriteLine(log);
                }
                
            }
        }

        private void LogToConsole(object data ,ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(data);
        }

        private ILogger GetLogger<TLogger>() where TLogger : ILogger, new()
        {
            loggers.TryGetValue(typeof(TLogger), out ILogger logger);
            return logger;
        }

        private void RegisterLogger<TLogger>() where TLogger : ILogger, new()
        {
            var type = typeof(TLogger);
            if (loggers.ContainsKey(type))
                throw new Exception();

            var logger = new TLogger();
            logger.sync = new object();
            if(RootDir != null && RootDir != "")
            {
                Directory.CreateDirectory(RootDir);
                logger.Path = Path.Combine(RootDir, logger.Name + ".Log");
            }
           
            loggers.Add(type, logger);
        }


        //private static Logger instance;
                
        public virtual void Write(object dataToLog){}
        
        protected string RootDir { get; set; }
        public bool ToConsole { get; set;}
        public Dictionary<Type, ILogger> loggers { get; set; }
    }
}
