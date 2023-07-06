using Server.Core;
using Server.DataAccess;
using QNetLib;
using System.Configuration.Internal;
using Microsoft.Extensions.Configuration;

namespace Server
{
    public class ServerManager : Singliton<ServerManager>
    {
        public ServerManager( IDataService dataService, IConfiguration config) { 
            Instance= this;
            configuration = config;
            appThread = new AppThread();
            networkManager = new NetworkManager( dataService);
            Core.Timer.Init();
            appThread.Start(Update);
            logger = new Logger();
        }

        public NetworkManager networkManager;
        public IConfiguration configuration;
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

    }
}