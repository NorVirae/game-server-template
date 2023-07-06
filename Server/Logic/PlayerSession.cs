using Server.DataAccess;
using Server.DataAccess.Repositories;
using Server.Logic.Handlers;
using Server.Messages.Handlers;
using Server.Playfab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{

    public class PlayerSession : ISession
    {
        public string userID;
        public string playfabId;
        public Client client;
        public NetworkManager networkManager;

        public ChatsActionHandler chatsActionHandler;
        public PlayfabPlayerHandler playfabPlayerHandler;

        public bool dataSynced = false;
        private double createdTimeStamp;
        private double updatedTimeStamp;

        public Guid UserId
        {
            get
            {
                return Guid.Parse(userID);
            }
        }

        public double CreatedAt { get => createdTimeStamp; set => createdTimeStamp = value; }
        public double UpdatedAt { get => updatedTimeStamp; set => updatedTimeStamp = value; }


        public PlayerSession(NetworkManager _networkManager)
        {
            networkManager = _networkManager;
            chatsActionHandler = new ChatsActionHandler(this);
            playfabPlayerHandler = new PlayfabPlayerHandler(this);
        }

        ////We can perform data loading based on the needed information from the request
        //public async Task LoadPlayerData()
        //{
        //    //await squadHandler.Init();
        //    //await campaignHandler.Init();
        //    //await spawnHandler.Init();
        //    //await playerVipHandler.Init();
        //    //await ePadHandler.Init();
        //    //await characterHandler.Init();

        //    Console.WriteLine("Player Progress Loaded");
        //}
        

        public async Task GetStateOnCategory()
        {

        }

        #region WriteData
        public void Save()
        {

        }
        #endregion
    }
}
