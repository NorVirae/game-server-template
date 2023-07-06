using Server.Core;
using Server.Playfab;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.Logic.Handlers
{
    public class PlayfabPlayerHandler : BaseHandler<PlayerSession>
    {
        private PlayfabManager playfabManager;
        public PlayfabPlayerHandler(PlayerSession playerSession) : base(playerSession)
        {
            playfabManager = new PlayfabManager();
        }

        public void LoginPlayfab(string userId)
        {
            playfabManager.LoginPlayfab(userId);
        }
        public Task<PlayFabResult<AddFriendResult>> AddFriendHandler(string playfabId)
        {
           
           
            //Playfab call api
            Console.WriteLine(playfabId + " PLAYFAB ID " );
            Task <PlayFabResult<AddFriendResult>> result = playfabManager.AddFriend(PlayfabManager.FriendIdType.PlayFabId, playfabId);
            Console.WriteLine("FInished CALL");
            return result;
         
        }

        public async Task<PlayFabResult<PlayFab.AdminModels.GetUserDataResult>> GetUserData(string playfabId, params string[] key){
            return await playfabManager.GetPlayerPublicData(playfabId, key);
        }

        public void SetUserData(Dictionary<string, string> data, string playFabId)
        {
            playfabManager.SetPlayerPublicData(data, playFabId);
        }

        protected override Task LoadDataFromDB()
        {
            throw new NotImplementedException();
        }
    }
}
