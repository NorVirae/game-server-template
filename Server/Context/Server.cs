using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using Server.Core;
using QNetLib;

namespace Server
{
    public class Server 
    {
        private SimpleWebServer webServer;
        private NetworkManager networkManager;
        
        public Server(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        public void StartServer(string ip, int port)
        {
            webServer = new SimpleWebServer(ip, port);
            webServer.onClientConnected += OnClientConnectedCallback;
            webServer.onClientDisconnected += OnClientDisconnectedCallback;
            webServer.onConnectionMessage += OnConnectionMessageCallback;

            Logger.Log("Starting the game server");
            webServer.Start();
        }

        private void OnConnectionMessageCallback(byte[] payload, int lenght, EndPoint sender, DeliveryMethod deliveryMethod, Channel channel)
        {
            AppThread.Schedule(() =>
            {
                networkManager.OnReciveData(payload, lenght, sender);
            });
            
        }

        private void OnClientDisconnectedCallback(INetworkClient client, DisconnectionReasons disconnectionReasons)
        {
            Console.WriteLine("Disconnected");
            AppThread.Schedule(() =>
            {
                Console.WriteLine("Disconnected");
                networkManager.OnClientDisconnected(client.NetID, client.ClientEndpoint, disconnectionReasons);
            });
        }

        private void OnClientConnectedCallback(INetworkClient client)
        {
            AppThread.Schedule(() =>
            {
                Client connectedClient = new Client(client, networkManager);
                networkManager.OnClientConnected(connectedClient);
            });
        }



        public void StopServer()
        {
            if(webServer != null)
            {
                webServer.Stop();
            }
        }
    }
}
