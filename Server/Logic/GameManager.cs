using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Server.Context;
using Server.DataAccess;
using static System.Collections.Specialized.BitVector32;

namespace Server
{
    public class GameManager : Actor<GameManager>
    {
        public ConcurrentDictionary<string, PlayerSession> playerSessions;
        public ConcurrentBag<string> oldSessions = new ConcurrentBag<string>();
        private readonly NetworkManager _networkManager;
        public static GameManager instance = new GameManager();
        public GameManager()
        {
            Instance = this;

            playerSessions = new ConcurrentDictionary<string, PlayerSession>();
            PlayFab.PlayFabSettings.staticSettings.DeveloperSecretKey = ServerManager.Instance.configuration.GetConnectionString("PLAYFAB_DEVELOPER_SECRET");

        }

        public bool GetPlayerSeesion(string userId, out PlayerSession player)
        {
            return playerSessions.TryGetValue(userId, out player);
        }

        public async Task CreateSession(Client client, LoginMessage message)
        {
            
                PlayerSession playerSession = new PlayerSession(_networkManager)
                {
                    client = client,
                    userID = client.userID
                };
                playerSession.playfabPlayerHandler.LoginPlayfab(message.UserId);
                playerSession.playfabId = message.PlayfabId;

                //await playerSession.Init();
                Console.WriteLine("GOT INTO LOGIN HERE");

                playerSessions.AddOrUpdate(client.userID, playerSession, (key, old) => playerSession);
                await Task.FromResult<object>(null);
            
        }

        public void RemoveSession(Client client)
        {
            oldSessions.Add(client.userID);
        }

        public void Update()
        {
            //foreach (string id in oldSessions)
            //{

            //}
        }

       
    }
}
