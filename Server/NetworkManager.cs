﻿using Server.Core;
using Server.Messages;
using ENet;
using Newtonsoft.Json;
using QNetLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IO.Ably;
using Server.DataAccess;

namespace Server
{
    public class NetworkManager
    {
        public static NetworkManager instance;
        ServerConfig config;
        public IDataService dataService;
        public GameManager gameManager;
        private static ConcurrentDictionary<Type, IActor> remoteActors = new ConcurrentDictionary<Type, IActor>();

        private Server server;
        public ConcurrentDictionary<EndPoint, Client> connectedClientsWithEndpoint;// Set of pending connections
        private static PongEvent pong = new();
        private MessageHandler messageHandler;

        public NetworkManager(IDataService _dataService)
        {
            connectedClientsWithEndpoint = new ConcurrentDictionary<EndPoint, Client>();
            gameManager = new GameManager();
            dataService = _dataService;
            instance = this;
            messageHandler = new MessageHandler(this);
            RegisterActors();
        }

        public void StartServer(string ip, int port)
        {
           
            server = new Server(this);
            server.StartServer(ip, port);

        }

        public void Update()
        {
            if (Core.Timer.CheckTick())
            {
                
                foreach (KeyValuePair<EndPoint, Client> item in connectedClientsWithEndpoint)
                {
                    double responsDifference = Core.Timer.TotalsecondsSinceStart - item.Value.lastPongTime;
                    if (responsDifference >= 5) //TODO : implement a better expire value
                    {
                        //Dont bother sending ping just kill the client
                        //item.Value.Disconnect();
                        continue;
                    }
                    item.Value.SendPing();
                }
            }
        }

        public void OnReciveData(byte[] payload, int lenght, EndPoint sender)
        {
            if (TryGetClient(sender, out Client client))
            {
                try
                {
                    string msg = Encoding.ASCII.GetString(payload, 0, lenght);
                    Datagram? datagram = JsonConvert.DeserializeObject<Datagram>(msg);
                    HandleIncommingData(client, datagram);
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }

            }
        }

        public void HandleIncommingData(Client client, Datagram datagram)
        {
            EventType type = (EventType)Convert.ToInt32(datagram.type);
            switch (type)
            {
                case EventType.Pong:
                case EventType.Ping:
                    client.lastPongTime = Core.Timer.TotalsecondsSinceStart;
                    break;
                case EventType.RPC:
                    //HandleRpc(@event);
                    Logger.LogWarn("RPC is not supported yet");
                    break;
                case EventType.Message:
                    Logger.Log("Message Was Received!");
                    MessageHandler.Instance.HandleMessageAsync(client, datagram);
                    break;
            }
        }
        private void RegisterActors()
        {
            remoteActors.TryAdd(typeof(GameManager), new GameManager()); //Register the Gamemanegr first to avoid Getactor Actor crash.
     
        }




        public bool TryGetClient(EndPoint ID, out Client client)
        {
            
            return connectedClientsWithEndpoint.TryGetValue(ID, out client);
        }


        public void OnClientConnected(Client client)
        {
            Console.WriteLine(client.ClientEndpoint + " CLIENT END POINT");
            connectedClientsWithEndpoint.TryAdd(client.ClientEndpoint, client);
            client.sessionId = Guid.NewGuid().ToString();
            client.SendDataGram(new Datagram(EventType.Connection, client.sessionId));
            //Logger.LogInfo("New Connection Waiting for logging");
        }

        public void OnClientDisconnected(int NetId, EndPoint endPoint, DisconnectionReasons disconnectionReasons)
        {
            //Logger.LogInfo(" DisConnection");
            if (connectedClientsWithEndpoint.ContainsKey(endPoint))
            {
                connectedClientsWithEndpoint.TryRemove(endPoint, out Client c);
                
            }
        }

        public static void PublishMessage(Client client, short messageId, Message message, object clientCallbackId)
        {
            Datagram gram = new(EventType.Message, MessageHandler.SerializeMessage(messageId, message), clientCallbackId);
            client.SendDataGram(gram);
        }

        public void Configure(ServerConfig config)
        {
            this.config = config;
            dataService = new DataService(config.connectionString);
            //Run test

            return;
        }

            public void Stop()
        {
            server.StopServer();
        }

    }
}
