using Server.Core;
using QNetLib;

namespace Server
{
    public class ServerManager : Singliton<ServerManager>
    {
        public ServerManager(string dbConnString) { 
            Instance= this;
            appThread = new AppThread();
            networkManager = new NetworkManager(dbConnString);
            Core.Timer.Init();
            appThread.Start(Update);
            logger = new Logger();
        }

        public NetworkManager networkManager;
        public Logger logger;
        public static AppThread appThread;
        

        public void Update()
        {
            networkManager.Update();
        }

        public void Stop()
        {
            networkManager.Stop();
            appThread.Stop();
        }

        public void NetLogCallback(string message, DebugLevel level)
        {
            Console.WriteLine(message);
        }

        public void SetconnectionString(string connectionString)
        {

        }
    }
}